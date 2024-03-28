namespace Bars.GisIntegration.UI.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Delegacy;
    using Bars.GisIntegration.Base.Entities.GisRole;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Package;
    using Bars.GisIntegration.Base.Service;
    using Castle.Windsor;

    public class GisIntegrationService : IGisIntegrationService
    {
        public IWindsorContainer Container { get; set; }

        public IGisSettingsService GisSettingsService { get; set; }

        /// <summary>
        /// Менеджер пакетов
        /// </summary>
        public IPackageManager<RisPackage, long> PackageManager { get; set; }

        public IDataResult ListContragents(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var exporterId = baseParams.Params.GetAs<string>("exporter_Id");

            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();
            var contragentDomain = this.Container.ResolveDomain<RisContragent>();
            var delegacyDomain = this.Container.ResolveDomain<Delegacy>();

            try
            {
                var currentContragentIds = dataSupplierProvider.GetContragentIds();

                if (currentContragentIds == null || currentContragentIds.Count == 0)
                {
                    throw new Exception("К учетной записи не привязан контрагент");
                }

                currentContragentIds.AddRange(
                    delegacyDomain.GetAll()
                                  .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                                  .WhereContains(x => x.OperatorIS.Id, currentContragentIds)
                                  .Select(x => x.InformationProvider.Id));

                var suitableContragentIds = this.GetSuitableContragentIds(exporterId);

                var risContragents =
                    contragentDomain.GetAll()
                                    .WhereContains(x => x.Id, currentContragentIds.Intersect(suitableContragentIds))
                                    .Where(x => x.OrgPpaGuid != null && x.OrgPpaGuid.Length > 0)
                                    .Filter(loadParam, this.Container);

                return new ListDataResult(risContragents.Order(loadParam).Paging(loadParam).ToList(), risContragents.Count());
            }
            finally
            {
                this.Container.Release(dataSupplierProvider);
                this.Container.Release(contragentDomain);
                this.Container.Release(delegacyDomain);
            }
        }

        private List<long> GetSuitableContragentIds(string exporterId)
        {
            var risContragentRoleDomain = this.Container.ResolveDomain<RisContragentRole>();
            var gisRoleMethodDomain = this.Container.ResolveDomain<GisRoleMethod>();

            try
            {
                var roleIds = gisRoleMethodDomain.GetAll().Where(x => x.MethodId == exporterId).Select(x => x.Role.Id);
                return risContragentRoleDomain.GetAll().Where(x => roleIds.Contains(x.Role.Id)).Select(x => x.GisOperator.Contragent.Id).ToList();
            }
            finally
            {
                this.Container.Release(risContragentRoleDomain);
                this.Container.Release(gisRoleMethodDomain);
            }
        }

        private List<string> GetAvailableMethods()
        {
            var risContragentRoleDomain = this.Container.ResolveDomain<RisContragentRole>();
            var gisRoleMethodDomain = this.Container.ResolveDomain<GisRoleMethod>();

            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();
            var delegacyDomain = this.Container.ResolveDomain<Delegacy>();

            try
            {
                var currentContragentIds = dataSupplierProvider.GetContragentIds();

                if (currentContragentIds == null || currentContragentIds.Count == 0)
                {
                    throw new Exception("К учетной записи не привязан контрагент");
                }

                currentContragentIds.AddRange(
                    delegacyDomain.GetAll()
                                  .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                                  .WhereContains(x => x.OperatorIS.Id, currentContragentIds)
                                  .Select(x => x.InformationProvider.Id));

                var roles = risContragentRoleDomain.GetAll()
                                                   .WhereContains(x => x.GisOperator.Contragent.Id, currentContragentIds)
                                                   .Select(x => x.Role.Id);

                return gisRoleMethodDomain.GetAll().Where(x => roles.Contains(x.Role.Id)).Select(x => x.MethodId).ToList();
            }
            finally
            {
                this.Container.Release(risContragentRoleDomain);
                this.Container.Release(gisRoleMethodDomain);
                this.Container.Release(dataSupplierProvider);
                this.Container.Release(delegacyDomain);
            }
        }

        /// <summary>
        /// Получить список методов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetMethodList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var showAll = baseParams.Params.GetAs("showAll", true);

            var exporterList = this.Container.ResolveAll<IDataExporter>();

            List<string> availableMethods;
            try
            {
                availableMethods = this.GetAvailableMethods();
            }
            catch
            {
                availableMethods = new List<string>();
            }

            try
            {
                var exporters =
                    exporterList.WhereIf(!showAll, x => !x.DataSupplierIsRequired || availableMethods.Contains(x.GetType().Name))
                                .Select(
                                    x =>
                                        new MethodProxy
                                            {
                                                Id = x.GetType().Name,
                                                Name = x.Name,
                                                Order = x.Order,
                                                Description = x.Description,
                                                Type = "exporter",
                                                NeedSign = x.NeedSign,
                                                Dependencies = x.GetDependencies().AggregateWithSeparator(";")
                                            });

                var dataList = new List<MethodProxy>();
                dataList.AddRange(exporters);

                var data = dataList.AsQueryable().OrderBy(x => x.Order).Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(exporterList);
            }
        }


        /// <summary>
        /// Проверить выполнимость метода
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор метода</param>
        /// <returns>Результат проверки</returns>
        public IDataResult CheckMethodFeasibility(BaseParams baseParams)
        {
            var methodId = baseParams.Params.GetAs<string>("exporter_Id");

            var exporter = this.Container.Resolve<IDataExporter>(methodId);
            var refs = this.Container.Resolve<ExporterWizardRefs>();

            try
            {
                return new BaseDataResult(new
                {
                    PrepareDataWizardClassName = refs.Get(exporter),
                    exporter.DataSupplierIsRequired
                });
            }
            catch (Exception exception)
            {
                return new BaseDataResult(false, exception.Message);
            }
            finally
            {
                this.Container.Release(exporter);
            }
        }

        /// <summary>
        ///  Запланировать подготовку данных: извлечение, валидация, формирование пакетов
        /// </summary>
        /// <param name="baseParams">Параметры подготовки данных, содержащие фильтры, идентификатор метода</param>
        /// <returns>Результат планирования</returns>
        public IDataResult SchedulePrepareData(BaseParams baseParams)
        {
            var exporterId = baseParams.Params.GetAs("exporter_Id", string.Empty);

            if (string.IsNullOrEmpty(exporterId))
            {
                return new BaseDataResult(false, "Не удалось получить экспортер c пустым идентификатором.");
            }

            var exporter = this.Container.Resolve<IDataExporter>(exporterId);

            if (exporter == null)
            {
                return new BaseDataResult(false, $"Не удалось получить экспортер c идентификатором {exporterId}.");
            }

            try
            {
                IDictionary<long, DynamicDictionary> extractParams = ((DynamicDictionary)baseParams.Params["params"]).ToDictionary(
                    x => x.Key.ToLong(),
                    x => (DynamicDictionary)x.Value);

                if (exporter.DataSupplierIsRequired)
                {
                    if (extractParams.Any(x => x.Key == 0))
                    {
                        return new BaseDataResult(false, "Экспортер требует наличие поставщика данных");
                    }
                }

                var taskManager = this.Container.Resolve<ITaskManager>();

                try
                {
                    taskManager.CreateExportTask(exporter, extractParams);
                }
                finally
                {
                    this.Container.Release(taskManager);
                }

                return new BaseDataResult();
            }
            catch (Exception exception)
            {
                return new BaseDataResult(false, exception.Message);
            }
            finally
            {
                this.Container.Release(exporter);
            }
        }

        /// <summary>
        ///  Запланировать отправку данных
        /// </summary>
        /// <param name="baseParams">Параметры отправки данных, содержащие
        /// идентификатор задачи,
        /// идентификаторы пакетов к отправке</param>
        /// <returns>Результат планирования</returns>
        public IDataResult ScheduleSendData(BaseParams baseParams)
        {
            var taskId = baseParams.Params.GetAs<long>("taskId");
            var packageIds = baseParams.Params.GetAs("packageIds", string.Empty).ToLongArray();

            if (taskId == 0)
            {
                return new BaseDataResult(false, "Не удалось получить задачу c пустым идентификатором.");
            }

            if (packageIds == null || packageIds.Length == 0)
            {
                return new BaseDataResult(false, "Не определены пакеты к отправке.");
            }

            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                var task = taskManager.GetTask(taskId);

                var exporter = this.Container.Resolve<IDataExporter>(task.ClassName);

                try
                {
                    taskManager.CreateSendDataSubTask(exporter, task, packageIds);
                }
                finally
                {
                    this.Container.Release(exporter);
                }

                return new BaseDataResult();
            }
            catch (Exception exception)
            {
                return new BaseDataResult(false, exception.Message);
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }

        /// <summary>
        /// Получить параметры выполнения подписывания и отправки данных
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор задачи</param>
        /// <returns>Параметры выполнения подписывания и отправки данных</returns>
        public IDataResult GetSignAndSendDataParams(BaseParams baseParams)
        {
            var taskId = baseParams.Params.GetAs<long>("taskId");

            if (taskId == 0)
            {
                return new BaseDataResult(false, "Не удалось получить задачу c пустым идентификатором.");
            }

            var needSign = this.GetNeedSign(taskId);
            var prepareDataResultDescription = this.GetPrepareDataResultDescription(taskId);

            if (prepareDataResultDescription != null)
            {
                return new BaseDataResult(new
                                          {
                                              PrepareDataResultDescription = prepareDataResultDescription,
                                              NeedSign = needSign
                                          });
            }

            return new BaseDataResult(false, "Не получено описание результата подготовки данных");
        }

        private bool GetNeedSign(long taskId)
        {
            if (!this.SingXmlConfig())
            {
                return false;
            }

            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                var task = taskManager.GetTask(taskId);

                var exporter = this.Container.Resolve<IDataExporter>(task.ClassName);

                try
                {
                    return exporter.NeedSign;
                }
                finally
                {
                    this.Container.Release(exporter);
                }
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }

        private PrepareDataResultDescription GetPrepareDataResultDescription(long taskId)
        {
            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                return taskManager.GetPrepareDataResultDescription(taskId);
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }
              
        /// <summary>
        /// Метод получения настройки "подписывать сообщение"
        /// </summary>
        /// <returns>Значение настройки</returns>
        private bool SingXmlConfig()
        {
            var gisIntegrationConfig = this.Container.Resolve<GisIntegrationConfig>();

            try
            {
                return gisIntegrationConfig.SingXml;
            }
            finally
            {
                this.Container.Release(gisIntegrationConfig);
            }
        }

        private class MethodProxy
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public bool NeedSign { get; set; }
            public string Dependencies { get; set; }
        }
    }
}