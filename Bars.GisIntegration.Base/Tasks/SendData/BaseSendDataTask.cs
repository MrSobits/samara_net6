namespace Bars.GisIntegration.Base.Tasks.SendData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.Package;
    using Bars.Gkh.Quartz.Scheduler;
    using Bars.Gkh.Quartz.Scheduler.Entities;
    using Bars.Gkh.Quartz.Scheduler.Extensions;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using Quartz;

    /// <summary>
    /// Базовый класс задачи отправки данных и получения результатов экспорта пакетов данных
    /// </summary>
    /// <typeparam name="TRequestType">Тип запросов к сервису</typeparam>
    /// <typeparam name="TResponseType">Тип объектов, содержащих ответ от сервиса</typeparam>
    /// <typeparam name="TSoapClient">Тип soap клиента</typeparam>
    /// <typeparam name="THeaderType">Тип заголовка запроса</typeparam>
    public abstract class BaseSendDataTask<TRequestType, TResponseType, TSoapClient, THeaderType> : BaseTask
    {
        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Менеджер пакетов
        /// </summary>
        public IPackageManager<RisPackage, long> PackageManager { get; set; }

        /// <summary>
        /// Отправлять подписанные данные
        /// true - отправлять подписанные 
        /// false - отправлять неподписанные 
        /// </summary>
        public bool SendSignedData { get; set; }

        /// <summary>
        /// Параметры отправки данных - резолвится из контекста
        /// </summary>
        private DynamicDictionary SendDataParams { get; set; }

        /// <summary>
        /// Soap клиент
        /// </summary>
        public IGisServiceProvider<TSoapClient> ServiceProvider { get; set; }

        /// <summary>
        /// Список пакетов на обработку
        /// </summary>
        public List<RisPackageTrigger> Packages { get; set; }

        /// <summary>
        /// Отменить запланированное выполнение триггера при возникновении ошибки
        /// </summary>
        protected override bool UnscheduleOnError => true;

        /// <summary>
        /// Метод выполнения задачи. При выполнении проверять значение флага InterruptRequested.
        /// </summary>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <returns>Результат выполнения задачи - должен быть сериализуемый - сохраняется в BLOB</returns>
        protected override object DoWork(IJobExecutionContext context)
        {
            this.Initialize(context);

            var processedPackages = new List<RisPackageTrigger>();

            foreach (var package in this.Packages)
            {
                if (this.InterruptRequested)
                {
                    this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Запрошено прерывание выполнения задачи"));
                    break;
                }

                if (this.ProcessPackage(package))
                {
                    processedPackages.Add(package);
                }
            }

            var notProcessedPackages = this.Packages.Where(x => !processedPackages.Contains(x)).ToList();

            if (notProcessedPackages.Count == 0 || this.InterruptRequested)
            {
                context.Scheduler.UnscheduleJob(context.Trigger.Key);
            }
            else if (notProcessedPackages.Count > 0 && !context.Trigger.GetMayFireAgain())
            {
                var simpleTrigger = (ISimpleTrigger)context.Trigger;
                this.UpdateNotProcessedPackages(notProcessedPackages, simpleTrigger.RepeatCount - 1);
            }

            return processedPackages.Select(x => x.Package.Id).ToList();
        }

        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected abstract string ExecuteRequest(THeaderType header, TRequestType request);
        /// <summary>
        /// Получить результат экспорта пакета данных
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="ackMessageGuid">Идентификатор сообщения</param>
        /// <param name="result">Результат экспорта</param>
        /// <returns>Статус обработки запроса</returns>
        protected abstract sbyte GetStateResult(THeaderType header, string ackMessageGuid, out TResponseType result);

        /// <summary>
        /// Обработать результат экспорта пакета данных
        /// </summary>
        /// <param name="response">Ответ от сервиса</param>
        /// <param name="transportGuidDictByType">Словарь transportGuid-ов для типа</param>
        /// <returns>Результат обработки пакета</returns>
        protected abstract PackageProcessingResult ProcessResult(
            TResponseType response,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictByType);

        /// <summary>
        /// Получить заголовок запроса
        /// </summary>
        /// <param name="messageGuid">Идентификатор запроса</param>
        /// <param name="package"></param>
        /// <returns>Заголовок запроса</returns>>
        protected abstract THeaderType GetHeader(string messageGuid, RisPackage package);

        /// <summary>
        /// Инициализировать эксземпляр исполнителя задачи
        /// </summary>
        /// <param name="context">Контекст выполения задачи</param>
        private void Initialize(IJobExecutionContext context)
        {
            if (context.PreviousFireTimeUtc == null)
            {
                //триггер запустился первый раз

                long[] packageIds;

                try
                {
                    packageIds = (long[])context.GetPropertyValue("packageIds");
                }
                catch (Exception exception)
                {
                    throw new JobExecutionException(string.Format("Obtaining task error: {0}", exception.Message), exception, false);
                }

                var packages = this.GetPackages(packageIds);

                if (packages.Count == 0)
                {
                    throw new JobExecutionException("Obtaining task error: Нет данных для отправки (количество пакетов = 0)");
                }

                this.Packages = this.SetPackageTriggerRelations(this.StorableTrigger, packages);
            }
            else
            {
                this.Packages = this.GetTriggerPackagesForProcessing(this.StorableTrigger.Id);
            }
        }


        /// <summary>
        /// Обработать пакет
        /// </summary>
        /// <param name="package">Пакет</param>
        /// <returns>Флаг: true - пакет обработан, false - в противном случае</returns>
        private bool ProcessPackage(RisPackageTrigger package)
        {
            if (package.State == PackageState.Signed || package.State == PackageState.New)
            {
                return this.ProcessSendingPackage(package);
            }

            if (package.State == PackageState.Sent)
            {
                return this.ProcessGettingResult(package);
            }

            throw new ArgumentException("Некорректный статус пакета", nameof(package));
        }

        /// <summary>
        /// Обработать отправку пакета
        /// </summary>
        /// <param name="package">Пакет</param>
        /// <returns>Флаг: true - пакет обработан, false - в противном случае</returns>
        private bool ProcessSendingPackage(RisPackageTrigger package)
        {
            var packageSendingResult = this.SendPackage(package.Package);

            this.SaveSendingResult(package, packageSendingResult);

            return !packageSendingResult.Success;
        }

        /// <summary>
        /// Обработать получение результата
        /// </summary>
        /// <param name="package">Пакет</param>
        /// <returns>Признак: true - пакет обработан, false - в противном случае</returns>
        private bool ProcessGettingResult(RisPackageTrigger package)
        {
            TResponseType requestResult;

            sbyte state;

            try
            {
                var messageGuid = Guid.NewGuid().ToStr();

                var header = this.GetHeader(messageGuid, package.Package);

                this.AddLogRecord(new PackageSendingLogRecord(MessageType.Info, package.Package.Name, $"Получение результата, идентификатор запроса = {messageGuid}"));

                state = this.GetStateResult(header, package.AckMessageGuid, out requestResult);
            }
            catch (Exception exception)
            {
                this.AddLogRecord(new PackageSendingLogRecord(MessageType.Error, package.Package.Name, string.Format("Ошибка получения результата: {0}", exception.Message)));

                this.SaveProcessingResult(package, new PackageProcessingResult
                {
                    State = PackageState.GettingStateError,
                    Message = exception.Message
                });

                return true;
            }

            //Статус обработки сообщения в асинхронном обмене (1- получено; 2 - в обработке; 3- обработано)
            if (state == 3)
            {
                try
                {
                    this.AddLogRecord(new PackageSendingLogRecord(MessageType.Info, package.Package.Name, "Результат получен. Обработка результата."));

                    var packageProcessingResult = this.ProcessResult(requestResult, this.PackageManager.GetTransportGuidDictionary(package.Package));

                    if (!string.IsNullOrEmpty(packageProcessingResult.Message))
                    {
                        this.AddLogRecord(new PackageSendingLogRecord(MessageType.Info, package.Package.Name, packageProcessingResult.Message));
                    }
                    
                    if (packageProcessingResult.State == PackageState.SuccessProcessed && packageProcessingResult.Objects.Any( x => x.State == ObjectProcessingState.Error))
                    {
                        packageProcessingResult.State = PackageState.ProcessedWithErrors;
                    }

                    this.SaveProcessingResult(package, packageProcessingResult);
                }
                catch (Exception exception)
                {
                    this.AddLogRecord(new PackageSendingLogRecord(MessageType.Error, package.Package.Name, string.Format("Ошибка обработки результата: {0}", exception.Message)));

                    this.SaveProcessingResult(package, new PackageProcessingResult
                    {
                        State = PackageState.ProcessingResultError,
                        Message = exception.Message
                    });
                }

                return true;
            }

            this.AddLogRecord(new PackageSendingLogRecord(MessageType.Info, package.Package.Name, "Пакет еще не обработан на стороне ГИС"));

            return false;
        }

        private List<RisPackage> GetPackages(IEnumerable<long> packageIds)
        {
            var packageRepository = this.Container.ResolveDomain<RisPackage>();

            try
            {
                return packageRepository.GetAll().Where(x => packageIds.Contains(x.Id)).ToList();
            }
            finally
            {
                this.Container.Release(packageRepository);
            }
        }

        private List<RisPackageTrigger> SetPackageTriggerRelations(Trigger trigger, IEnumerable<RisPackage> packages)
        {
            var packageTriggerRepository = this.Container.ResolveDomain<RisPackageTrigger>();

            try
            {
                var packageTriggerList = new List<RisPackageTrigger>();

                foreach (var package in packages)
                {
                    packageTriggerList.Add(new RisPackageTrigger
                    {
                        Package = package,
                        Trigger = trigger,
                        State = package.Signed ? PackageState.Signed : PackageState.New
                    });
                }

                TransactionHelper.InsertInManyTransactions(this.Container, packageTriggerList, 1000, true, true);

                return packageTriggerList;
            }
            finally
            {
                this.Container.Release(packageTriggerRepository);
            }
        }

        /// <summary>
        /// Получить пакеты, связанные с триггером, ожидающие обработки
        /// </summary>
        /// <param name="triggerId">Идентификатор хранимого триггера</param>
        /// <returns>Пакеты, связанные с триггером, ожидающие обработки</returns>
        private List<RisPackageTrigger> GetTriggerPackagesForProcessing(long triggerId)
        {
            var packageTriggerRepository = this.Container.ResolveDomain<RisPackageTrigger>();

            try
            {
                return packageTriggerRepository.GetAll()
                    .Where(x => x.Trigger.Id == triggerId)
                    .Where(x => x.State == PackageState.New
                    || x.State == PackageState.Signed
                    || x.State == PackageState.Sent)
                    .OrderBy(x => x.ObjectCreateDate)
                    .ToList();
            }
            finally
            {
                this.Container.Release(packageTriggerRepository);
            }
        }

        private void UpdateNotProcessedPackages(List<RisPackageTrigger> packages, int repeatCount)
        {
            var errorMessage = string.Format("Превышено максимальное количество  = {0} попыток получения результата асинхронного запроса", repeatCount);

            foreach (var package in packages)
            {
                this.AddLogRecord(new PackageSendingLogRecord(MessageType.Error, package.Package.Name, errorMessage));

                this.SaveProcessingResult(package, new PackageProcessingResult
                {
                    State = PackageState.TimeoutError,
                    Message = errorMessage
                });
            }
        }

        /// <summary>
        /// Сохранить результаты обработки пакета ()
        /// </summary>
        /// <param name="package">Пакет</param>
        /// <param name="packageProcessingResult">Результат обработки пакета</param>
        private void SaveProcessingResult(RisPackageTrigger package, PackageProcessingResult packageProcessingResult)
        {
            this.SaveObjects(packageProcessingResult);

            this.UpdatePackageTrigger(package, packageProcessingResult);

            this.UpdatePackageState(package, packageProcessingResult.State);
        }

        private void SaveSendingResult(RisPackageTrigger package, PackageSendingResult packageSendingResult)
        {
            var newState = packageSendingResult.Success ? PackageState.Sent : PackageState.SendingError;

            this.UpdatePackageTrigger(package, packageSendingResult);

            this.UpdatePackageState(package, newState);
        }

        private void SaveObjects(PackageProcessingResult packageProcessingResult)
        {
            var objectsToSave = new List<PersistentObject>();

            if (packageProcessingResult.State == PackageState.SuccessProcessed || packageProcessingResult.State == PackageState.ProcessedWithErrors)
            {
                foreach (var objectProcessingResult in packageProcessingResult.Objects)
                {
                    if (objectProcessingResult.State == ObjectProcessingState.Success)
                    {
                        objectsToSave.AddRange(objectProcessingResult.ObjectsToSave);
                    }
                }

                this.SaveObjects(objectsToSave);
            }
        }

        /// <summary>
        /// Сохранить объекты
        /// </summary>
        /// <param name="objects">Список объектов</param>
        public virtual void SaveObjects(List<PersistentObject> objects)
        {
            var objectsDictionary = objects.GroupBy(x => x.GetType()).ToDictionary(group => group.Key, group => group.ToList());

            foreach (var objectList in objectsDictionary.Values)
            {
                TransactionHelper.InsertInManyTransactions(this.Container, objectList);
            }
        }

        private void UpdatePackageState(RisPackageTrigger package, PackageState newPackageState)
        {
            var packageDomain = this.Container.ResolveDomain<RisPackageTrigger>();

            try
            {
                package.State = newPackageState;
                packageDomain.Update(package);
            }
            finally
            {
                this.Container.Release(packageDomain);
            }
        }

        private void UpdatePackageTrigger(RisPackageTrigger packageTrigger, PackageProcessingResult packageProcessingResult)
        {
            var packageTriggerDomain = this.Container.ResolveDomain<RisPackageTrigger>();

            var processingResult = packageProcessingResult.Objects?.Select(x => new SerializableObjectProcessingResult(x)).ToList();

            try
            {
                if (processingResult != null)
                {
                    packageTrigger.SetProcessingResult(processingResult);
                }
                
                packageTrigger.Message = packageProcessingResult.Message;
                packageTriggerDomain.Update(packageTrigger);
            }
            finally
            {
                this.Container.Release(packageTriggerDomain);
            }
        }

        private void UpdatePackageTrigger(RisPackageTrigger packageTrigger, PackageSendingResult packageSendingResult)
        {
            var packageTriggerDomain = this.Container.ResolveDomain<RisPackageTrigger>();

            try
            {
                packageTrigger.AckMessageGuid = packageSendingResult.AckMessageGuid;
                packageTrigger.Message = packageSendingResult.Message;
                packageTriggerDomain.Update(packageTrigger);
            }
            finally
            {
                this.Container.Release(packageTriggerDomain);
            }
        }

        private PackageSendingResult SendPackage(RisPackage package)
        {
            var request = this.PackageManager.GetData<TRequestType>(package, this.SendSignedData);
            try
            {
                var messageGuid = Guid.NewGuid().ToStr();
                var header = this.GetHeader(messageGuid, package);

                this.AddLogRecord(new PackageSendingLogRecord(MessageType.Info, package.Name, $"Отправка, идентификатор запроса {messageGuid}"));

                var ackMessageGuid = this.ExecuteRequest(header, request);

                return new PackageSendingResult
                {
                    Success = true,
                    AckMessageGuid = ackMessageGuid
                };
            }
            catch (Exception exception)
            {
                this.AddLogRecord(new PackageSendingLogRecord(MessageType.Error, package.Name, string.Format("Ошибка отправки: {0}", exception.Message)));

                return new PackageSendingResult
                {
                    Success = false,
                    Message = exception.Message
                };
            }
        }
    }
}
