namespace Bars.Gkh.SystemDataTransfer.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.SystemDataTransfer;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.DataTransfer;
    using Bars.Gkh.Services.ServiceContracts.DataTransfer;
    using Bars.Gkh.SystemDataTransfer.Enums;
    using Bars.Gkh.SystemDataTransfer.Meta.Services;
    using Bars.Gkh.SystemDataTransfer.Tasks;
    using Bars.Gkh.SystemDataTransfer.Utils;

    using Castle.Windsor;

    using global::Quartz;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Сервис работы с импортом/экспортом системы
    /// </summary>
    public class SystemIntegrationService : ISystemIntegrationService
    {
        private IDataTransferService soapClient;

        public IDataTransferProvider DataTransferProvider { get; set; }

        public IWindsorContainer Container { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public IDomainService<DataTransferIntegrationSession> DataTransferIntegrationSessionDomain { get; set; }

        /// <inheritdoc />
        public IDataResult RunIntegration(BaseParams baseParams)
        {
            if (!this.IsAccessGranted(DataTransferOperationType.Import))
            {
                return BaseDataResult.Error("Недостаточно прав для выполнения данной операции");
            }

            var typeNames = baseParams.Params.GetAs<string[]>("typeNames");
            var exportDependencies = baseParams.Params.GetAs("exportDependencies", true);

            IDataResult errorResult = null;

            var sessionEntries = this.DataTransferProvider.GetLayers(typeNames, exportDependencies);
            foreach (var sessionEntry in sessionEntries)
            {
                // состояние "В очереди"
                var session = new DataTransferIntegrationSession(DataTransferOperationType.Import)
                {
                    TypesNames = sessionEntry.Select(x => x.FullName).ToDictionary(x => x, x => (bool?)null)
                };
                this.DataTransferIntegrationSessionDomain.Save(session);

                try
                {
                    var result = this.GetClient().CreateExportTask(new CreateExportTaskParams
                    {
                        Guid = session.Guid,
                        TypeNames = sessionEntry.Select(x => x.FullName).ToArray(),
                        ExportDependencies = false
                    });

                    if (result.Code != Result.NoErrors.Code)
                    {
                        session.SetErrorState(null, result.Code);
                        errorResult = BaseDataResult.Error(result.Name);
                    }
                }
                catch (Exception exception)
                {
                    session.SetErrorState(exception.Message);
                    session.UpdateStateless();

                    errorResult = BaseDataResult.Error(exception.Message);
                }

                if (errorResult.IsNotNull())
                {
                    break;
                }
            }
           
            return errorResult ?? new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult CreateExportTask(Guid guid, IEnumerable<string> typeNames = null, bool exportDependencies = true)
        {
            // состояние "Отправка запроса на экспорт"
            var session = new DataTransferIntegrationSession(guid, DataTransferOperationType.Export)
            {
                TypesNames = typeNames.ToDictionary(x => x, x => (bool?)null)
            };
            this.DataTransferIntegrationSessionDomain.Save(session);

            var scheduler = this.GetScheduler();

            var parameterDict = new DynamicDictionary
            {
                { "typeNames", typeNames },
                { "exportDependencies", exportDependencies },
                { "guid", guid }
            };
            var job = JobBuilder.Create<TaskJob<DataTransferExportTask>>().UsingJobData(new JobDataMap().Apply(parameterDict)).Build();
            var trigger = TriggerBuilder.Create().WithIdentity(Guid.NewGuid().ToStr()).StartNow().Build();

            try
            {
                // В очереди на экспорт
                session.SetSuccessState();
                session.UpdateStateless();

                this.GetClient().Notify(new NotificationParams
                {
                    Guid = guid,
                    Success = true,
                    OperationType = DataTransferOperationType.Export
                });

                scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception exception)
            {
                session.SetErrorState(exception.Message);
                session.UpdateStateless();

                this.GetClient().Notify(new NotificationParams
                {
                    Guid = guid,
                    Success = false,
                    OperationType = DataTransferOperationType.Export,
                    Message = exception.Message
                });
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult NotifyStartExport(Guid guid)
        {
            var session = this.DataTransferIntegrationSessionDomain.GetAll().FirstOrDefault(x => x.Guid == guid);
            if (session.IsNull())
            {
                this.GetClient().Notify(new NotificationParams
                {
                    Guid = guid,
                    OperationType = DataTransferOperationType.Export,
                    Success = false,
                    Message = "Сессия с указанным идентификатором не найдена"
                });

                return BaseDataResult.Error("Сессия с указанным идентификатором не найдена");
            }

            // Формирование экспортируемых данных
            session.SetSuccessState();
            session.UpdateStateless();

            this.GetClient().Notify(new NotificationParams
            {
                Guid = guid,
                OperationType = DataTransferOperationType.Export,
                Success = true
            });

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult SendExportResult(Guid guid, IDataResult<Stream> streamResult)
        {
            var session = this.DataTransferIntegrationSessionDomain.GetAll().FirstOrDefault(x => x.Guid == guid);
            if (session.IsNull())
            {
                return BaseDataResult.Error("Сессия с указанным идентификатором не найдена");
            }

            IDataResult errorResult = null;
            if (streamResult.Success)
            {

                // состояние "Отправка файла"
                session.SetSuccessState();
                session.UpdateStateless();

                this.GetClient().Notify(new NotificationParams
                {
                    Guid = guid,
                    Success = true,
                    OperationType = DataTransferOperationType.Export
                });

                try
                {
                    byte[] data;
                    using (streamResult.Data)
                    {
                        var stream = streamResult.Data as MemoryStream;
                        if (stream != null)
                        {
                            data = stream.ToArray();
                        }
                        else
                        {
                            using (var ms = new MemoryStream())
                            {
                                streamResult.Data.CopyTo(ms);
                                data = ms.ToArray();
                            }
                        }
                    }

                    var result = this.GetClient().CreateImportTask(new CreateImportTaskParams
                    {
                        Guid = guid,
                        Data = data
                    });

                    if (result.Code != Result.NoErrors.Code)
                    {
                        session.SetErrorState(null, result.Code);
                        session.UpdateStateless();
                        errorResult = BaseDataResult.Error(result.Name);
                    }
                }
                catch (Exception exception)
                {
                    session.SetErrorState(exception.Message);
                    session.UpdateStateless();
                    errorResult = BaseDataResult.Error(exception.Message);
                }
            }
            else
            {
                session.SetErrorState();
                session.UpdateStateless();

                this.GetClient().Notify(new NotificationParams
                {
                    Guid = guid,
                    Success = false,
                    OperationType = DataTransferOperationType.Export,
                    Message = streamResult.Message
                });
            }

            return errorResult ?? new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult CreateImportTask(Guid guid, Stream stream)
        {
            var session = this.DataTransferIntegrationSessionDomain.GetAll().FirstOrDefault(x => x.Guid == guid);
            if (session.IsNull())
            {
                return BaseDataResult.Error("Сессия с указанным идентификатором не найдена");
            }

            var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;

            var scheduler = this.GetScheduler();
            var job = JobBuilder.Create<TaskJob<DataTransferImportTask>>()
                .UsingJobData(new JobDataMap().Apply(new DynamicDictionary
                {
                    { "stream", ms },
                    { "guid", guid }
                }))
                .Build();

            var trigger = TriggerBuilder.Create().WithIdentity(Guid.NewGuid().ToStr()).StartNow().Build();

            try
            {
                // состояние "В очереди на импорт"
                session.SetSuccessState();
                session.UpdateStateless();

                this.GetClient().Notify(new NotificationParams
                {
                    Guid = guid,
                    Success = true,
                    OperationType = DataTransferOperationType.Import
                });

                scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception exception)
            {
                session.SetErrorState(exception.Message);
                session.UpdateStateless();

                this.GetClient().Notify(new NotificationParams
                {
                    Guid = guid,
                    Success = false,
                    OperationType = DataTransferOperationType.Import,
                    Message = exception.Message
                });

                return BaseDataResult.Error(exception.Message);
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult NotifyStartImport(Guid guid)
        {
            var session = this.DataTransferIntegrationSessionDomain.GetAll().FirstOrDefault(x => x.Guid == guid);
            if (session.IsNull())
            {
                this.GetClient().Notify(new NotificationParams
                {
                    Guid = guid,
                    OperationType = DataTransferOperationType.Import,
                    Success = false,
                    Message = "Сессия с указанным идентификатором не найдена"
                });
                return BaseDataResult.Error("Сессия с указанным идентификатором не найдена");
            }

            // Импорт файла
            session.SetSuccessState();
            session.UpdateStateless();

            this.GetClient().Notify(new NotificationParams
            {
                Guid = guid,
                OperationType = DataTransferOperationType.Import,
                Success = true
            });

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult SendImportResult(Guid guid, IDataResult result)
        {
            var session = this.DataTransferIntegrationSessionDomain.GetAll().FirstOrDefault(x => x.Guid == guid);
            if (session.IsNull())
            {
                return BaseDataResult.Error("Сессия с указанным идентификатором не найдена");
            }

            this.GetClient().Notify(new NotificationParams
            {
                Guid = guid,
                OperationType = DataTransferOperationType.Import,
                Success = result.Success,
                Message = result.Message
            });

            if (result.Success)
            {
                // состояние "Интеграция завершена"
                session.SetSuccessState();
            }
            else
            {
                session.SetErrorState(result.Message);
            }

            session.UpdateStateless();

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult ProcessNotify(Guid guid, DataTransferOperationType operationType, IDataResult result)
        {
            var session = this.DataTransferIntegrationSessionDomain.GetAll().FirstOrDefault(x => x.Guid == guid);
            if (session.IsNull())
            {
                return BaseDataResult.Error("Сессия с указанным идентификатором не найдена");
            }

            this.Container.Resolve<ILogger>()
                .LogInformation("Got: {0}, success: {1}, message: {2}", operationType, result.Success, result.Message);

            if (result.Success)
            {
                session.SetSuccessState();
            }
            else
            {
                session.SetErrorState(result.Message);
            }

            session.UpdateStateless();

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult HandleSectionImportState(Guid guid, string name, bool success, bool notifyExternalSystem)
        {
            var session = this.DataTransferIntegrationSessionDomain.GetAll().FirstOrDefault(x => x.Guid == guid);
            if (session.IsNull())
            {
                return BaseDataResult.Error("Сессия с указанным идентификатором не найдена");
            }

            if (session.TypesNames.ContainsKey(name))
            {
                session.TypesNames[name] = success;
                session.UpdateStateless();

                if (notifyExternalSystem)
                {
                    this.GetClient().SetSuccessSectionImport(new SectionProgressParams
                    {
                        Guid = guid,
                        Name = name,
                        Success = success
                    });
                }
            }

            return new BaseDataResult();
        }

        private IDataTransferService GetClient()
        {
            if (this.soapClient.IsNull())
            {
                var configuration = new DataTransferIntegrationConfigs(this.Container.Resolve<IConfigProvider>());
                var isSecurityConnection = configuration.RemoteAddress.StartsWith("https");

                var binding = isSecurityConnection
                    ? (Binding)new BasicHttpsBinding
                    {
                        // TODO wcf
                        // Security = new BasicHttpsSecurity
                        // {
                        //     Mode = BasicHttpsSecurityMode.Transport,
                        //     Transport =
                        //         new HttpTransportSecurity
                        //         {
                        //             ClientCredentialType = HttpClientCredentialType.None,
                        //             ProxyCredentialType = HttpProxyCredentialType.None
                        //         }
                        // }
                    }
                    : (Binding)new BasicHttpBinding();

                var client = new DataTransferIntegrationClient(binding, new EndpointAddress(configuration.RemoteAddress));

                // проверка и проставление токена
                // TODO wcf
                // client.Endpoint.Behaviors.Add(new AuthHeaderEndpointBehavior(this.Container.Resolve<IConfigProvider>()));

                this.soapClient = client;
            }

            return this.soapClient;
        }

        private IScheduler GetScheduler()
        {
            return this.Container.Resolve<IScheduler>("TransferEntityScheduler");
        }

        private bool IsAccessGranted(DataTransferOperationType operationType)
        {
            var permssionKey = $"Administration.DataTransferIntegration.{operationType.ToString()}";
            return this.Container.Resolve<IAuthorizationService>().Grant(this.Container.Resolve<IUserIdentity>(), permssionKey);
        }
    }
}