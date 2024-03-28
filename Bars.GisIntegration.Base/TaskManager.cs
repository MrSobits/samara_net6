namespace Bars.GisIntegration.Base
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Events.Listeners.RisTask;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Listeners;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.Gkh.Quartz.Scheduler;
    using Bars.Gkh.Quartz.Scheduler.Entities;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;
    using Quartz;
    using Quartz.Impl.Matchers;

    /// <summary>
    /// Менеджер задач
    /// </summary>
    public class TaskManager : ITaskManager
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Планировщик задач
        /// </summary>
        public IScheduler Scheduler { get; }

        /// <summary>
        /// Конструктор менеджера задач
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public TaskManager(IWindsorContainer container)
        {
            this.Container = container;
            this.Scheduler = this.Container.Resolve<IScheduler>("TaskScheduler");
        }

        /// <summary>
        /// Создать задачу экспорта
        /// </summary>
        /// <param name="exporter">Экспортер, инициировавший задачу</param>
        /// <param name="extractParams">Словарь идентификатор контрагента - параметры извлечения данных</param>
        public void CreateExportTask(IDataExporter exporter, IDictionary<long, DynamicDictionary> extractParams)
        {
            var task = this.CreateTask(exporter, extractParams);

            //если задача на отправку документа в ерп, убираем из extractParams ссылку на распоряжение (для корректного создания jobDataMap)
            if (extractParams.Any() && extractParams[0].Keys.Contains("documentGji"))
            {
                extractParams[0] = new DynamicDictionary { { "id", extractParams[0].GetAs<long>("id") } } ;
            }

            this.CreatePrepareDataSubTask(exporter, task, extractParams);
        }

        /// <summary>
        /// Создать подзадачу подготовки данных
        /// </summary>
        /// <param name="exporter">Экспортер, инициировавший задачу</param>
        /// <param name="task">Хранимая задача</param>
        /// <param name="extractParams">Словарь идентификатор контрагента - параметры извлечения данных</param>
        public void CreatePrepareDataSubTask(IDataExporter exporter, RisTask task, IDictionary<long, DynamicDictionary> extractParams)
        {
            var taskType = exporter.PrepareDataTaskType;

            var jobDataMap = this.CreatePrepareDataJobDataMap(extractParams, exporter.NeedSign, exporter.FileStorage);

            RisTaskStateCalculator.HandlePrepareDataStage(task);

            this.SchedulePrepareDataSubTask(taskType, jobDataMap, string.Format("Подготовка данных: {0}", exporter.Name), task);
        }

        /// <summary>
        /// Создать подзадачу отправки данных
        /// </summary>
        /// <param name="exporter">Экспортер, инициировавший задачу</param>
        /// <param name="task">Хранимая задача</param>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        /// <param name="contragent">Контрагент, от имени которого выполняется отправка данных</param>
        public void CreateSendDataSubTask(IDataExporter exporter, RisTask task, long[] packageIds)
        {
            var taskType = exporter.SendDataTaskType;

            var sendDataParams = exporter.GetSendDataParams();

            var jobDataMap = this.CreateSendDataJobDataMap(sendDataParams, packageIds, exporter.NeedSign);

            RisTaskStateCalculator.HandleSendDataStage(task);

            this.ScheduleSendDataSubTask(taskType, jobDataMap, string.Format("Отправка данных: {0}", exporter.Name), exporter.Interval, exporter.MaxRepeatCount + 1, task);
        }

        /// <summary>
        /// Получить триггеры, связанные с задачей
        /// </summary>
        /// <param name="taskId">Идентификатор хранимой задачи</param>
        /// <returns>Триггеры, связанные с задачей</returns>
        public List<RisTaskTrigger> GetRisTaskTriggers(long taskId)
        {
            var taskTriggerRepository = this.Container.ResolveDomain<RisTaskTrigger>();

            try
            {
                return taskTriggerRepository.GetAll().Where(x => x.Task.Id == taskId).OrderBy(x => x.ObjectCreateDate).ToList();
            }
            finally
            {
                this.Container.Release(taskTriggerRepository);
            }
        }

        /// <summary>
        /// Получить пакеты, связанные с триггером
        /// </summary>
        /// <param name="triggerId">Идентификатор хранимого триггера</param>
        /// <returns>Пакеты, связанные с триггером</returns>
        public List<RisPackageTrigger> GetTriggerPackages(long triggerId)
        {
            var packageTriggerRepository = this.Container.ResolveDomain<RisPackageTrigger>();

            try
            {
                return packageTriggerRepository.GetAll().Where(x => x.Trigger.Id == triggerId).OrderBy(x => x.ObjectCreateDate).ToList();
            }
            finally
            {
                this.Container.Release(packageTriggerRepository);
            }
        }

        /// <summary>
        /// Получить хранимый триггер
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Триггер</returns>
        public Trigger GetTrigger(long triggerId)
        {
            var triggerRepository = this.Container.ResolveDomain<Trigger>();

            try
            {
                return triggerRepository.Get(triggerId);
            }
            finally
            {
                this.Container.Release(triggerRepository);
            }
        }

        /// <summary>
        /// Получить объект, связывающий триггер и пакет по идентификатору
        /// </summary>
        /// <param name="triggerPackageId">Идентификатор связки</param>
        /// <returns>Хранимый объект, связывающий триггер и пакет</returns>
        public RisPackageTrigger GetTriggerPackage(long triggerPackageId)
        {
            var packageTriggerRepository = this.Container.ResolveDomain<RisPackageTrigger>();

            try
            {
                return packageTriggerRepository.Get(triggerPackageId);
            }
            finally
            {
                this.Container.Release(packageTriggerRepository);
            }
        }

        /// <summary>
        /// Получить хранимую задачу
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>Хранимая задача</returns>
        public RisTask GetTask(long taskId)
        {
            var taskRepository = this.Container.ResolveDomain<RisTask>();

            try
            {
                return taskRepository.Get(taskId);
            }
            finally
            {
                this.Container.Release(taskRepository);
            }
        }

        /// <summary>
        /// Получить результат триггера подготовки данных
        /// </summary>
        /// <param name="triggerId">Идентификатор хранимого триггера</param>
        /// <returns>Результат подготовки данных</returns>
        public PrepareDataResult GetPreparingDataTriggerResult(long triggerId)
        {
            var serializableResult = this.GetSerializablePreparingDataTriggerResult(triggerId);

            if (serializableResult != null)
            {
                var triggerPackages = this.GetTriggerPackages(triggerId);

                return this.CreatePrepareDataResult(serializableResult, triggerPackages);
            }

            return null;
        }

        /// <summary>
        /// Получить список результатов валидации
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера подготовки данных</param>
        /// <returns>Список результатов валидации</returns>
        public List<ValidateObjectResult> GetValidationResultList(long triggerId)
        {
            var prepareDataResult = this.GetSerializablePreparingDataTriggerResult(triggerId);

            return prepareDataResult.ValidateResult;
        }

        /// <summary>
        /// Получить список результатов загрузки вложений
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера подготовки данных</param>
        /// <returns>Список результатов загрузки вложений</returns>
        public List<UploadAttachmentResult> GetUploadAttachmentResultList(long triggerId)
        {
            var prepareDataResult = this.GetSerializablePreparingDataTriggerResult(triggerId);

            return prepareDataResult.UploadResult;
        }

        /// <summary>
        /// Получить протокол выполнения триггера
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Протокол выполнения триггера</returns>
        public List<ILogRecord> GetTriggerProtocol(long triggerId)
        {
            var triggerJournal = this.GetTriggerJournal(triggerId);

            var result = new List<ILogRecord>();

            foreach (var journalRecord in triggerJournal)
            {
                try
                {
                    result.AddRange(this.Deserialise<List<ILogRecord>>(journalRecord.Protocol));
                }
                catch (Exception)
                {
                    //обработать другие типы протокола
                }
            }

            var trigger = this.GetTrigger(triggerId);

            var currentlyExecutingTriggers = this.Scheduler.GetCurrentlyExecutingJobs();

            var currentlyExecutingTrigger = currentlyExecutingTriggers.FirstOrDefault(x => x.Trigger.Key.Name == trigger.QuartzTriggerKey);

            if (currentlyExecutingTrigger != null)
            {
                var task = (ITask)currentlyExecutingTrigger.JobInstance;

                result.AddRange(task.GetLog());
            }

            return result.OrderBy(x => x.DateTime).ToList();
        }

        /// <summary>
        /// Получить описание результата подготовки данных
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>Описание результата подготовки данных</returns>
        public PrepareDataResultDescription GetPrepareDataResultDescription(long taskId)
        {
            var prepareDataTrigger = this.GetTaskPreparingDataTrigger(taskId);

            if (prepareDataTrigger != null)
            {
                return this.GetPrepareDataResultDescription(prepareDataTrigger);
            }

            return null;
        }

        /// <summary>
        /// Получить триггер подготовки данных для задачи
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>Триггер подготовки данных</returns>
        public Trigger GetTaskPreparingDataTrigger(long taskId)
        {
            var taskTriggerRepository = this.Container.ResolveDomain<RisTaskTrigger>();

            try
            {
                var taskTrigger = taskTriggerRepository.GetAll().FirstOrDefault(x => x.Task.Id == taskId && x.TriggerType == TriggerType.PreparingData);
                return taskTrigger?.Trigger;
            }
            finally
            {
                this.Container.Release(taskTriggerRepository);
            }
        }

        /// <summary>
        /// Получить журнал выполнения триггера
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Список записей журнала выполнения триггера</returns>
        public List<JournalRecord> GetTriggerJournal(long triggerId)
        {
            var journalRepository = this.Container.ResolveDomain<JournalRecord>();

            try
            {
                return journalRepository.GetAll().Where(x => x.Trigger.Id == triggerId).ToList();
            }
            finally
            {
                this.Container.Release(journalRepository);
            }
        }

        /// <summary>
        /// Получить сериализуемый результат подготовки данных
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера подготовки данных</param>
        /// <returns>Сериализуемый результат подготовки данных</returns>
        public SerializablePrepareDataResult GetSerializablePreparingDataTriggerResult(long triggerId)
        {
            var journalRecord = this.GetPreparingDataTriggerJournalRecord(triggerId);

            return this.GetSerializablePreparingDataTriggerResult(journalRecord);
        }

        /// <summary>
        /// Получить сериализуемый результат подготовки данных
        /// </summary>
        /// <param name="journalRecord">Запись журнала подготовки данных</param>
        /// <returns>Сериализуемый результат подготовки данных</returns>
        public SerializablePrepareDataResult GetSerializablePreparingDataTriggerResult(JournalRecord journalRecord)
        {
            if (journalRecord != null)
            {
                try
                {
                    return this.Deserialise<SerializablePrepareDataResult>(journalRecord.Result);
                }
                catch (Exception)
                {
                    //обработать другие типы результата
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Получить описание результата подготовки данных
        /// </summary>
        /// <param name="trigger">Триггер подготовки данных</param>
        /// <returns>Описание результата подготовки данных</returns>
        private PrepareDataResultDescription GetPrepareDataResultDescription(Trigger trigger)
        {
            var prepareDataResult = this.GetSerializablePreparingDataTriggerResult(trigger.Id);

            return new PrepareDataResultDescription { UserName = trigger.UserName, StartPrepareTime = trigger.StartTime, PackagesCount = prepareDataResult?.PackageIds.Count ?? 0, ValidationMessagesCount = prepareDataResult?.ValidateResult.Count ?? 0 };
        }

        private JournalRecord GetPreparingDataTriggerJournalRecord(long triggerId)
        {
            var triggerJournal = this.GetTriggerJournal(triggerId);

            if (triggerJournal.Count == 1)
            {
                return triggerJournal[0];
            }

            throw new Exception("Не корректное количество записей в журнале триггера подготовки данных");
        }

        private PrepareDataResult CreatePrepareDataResult(SerializablePrepareDataResult serializableResult, List<RisPackageTrigger> triggerPackages)
        {
            var result = new PrepareDataResult();

            result.ValidateResult = serializableResult.ValidateResult;

            var packages = new List<RisPackage>();

            foreach (var packageId in serializableResult.PackageIds)
            {
                var packageTrigger = triggerPackages.FirstOrDefault(x => x.Package.Id == packageId);

                if (packageTrigger != null)
                {
                    packages.Add(packageTrigger.Package);
                }
                else
                {
                    throw new Exception(string.Format("Триггер не связан с пакетом Id = {0}", packageId));
                }
            }

            result.Packages = packages;

            result.UploadAttachmentsResult = serializableResult.UploadResult;

            return result;
        }

        private T Deserialise<T>(byte[] data) where T : class
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (data != null && data.Length > 0)
            {
                using (var memoryStream = new MemoryStream(data))
                {
                    return (T)formatter.Deserialize(memoryStream);
                }
            }

            return null;
        }

        private RisTask CreateTask(IDataExporter exporter, IDictionary<long, DynamicDictionary> extractParams)
        {
            var task = new RisTask
            {
                Description = exporter.Name,
                ClassName = exporter.GetType().Name,
                TaskState = TaskState.Waiting,
                DocumentGji = extractParams.Any() ? extractParams[0].GetAs<DocumentGji>("documentGji") : null
            };

            var taskDomain = this.Container.ResolveDomain<RisTask>();

            try
            {
                taskDomain.Save(task);
            }
            finally
            {
                this.Container.Release(taskDomain);
            }

            return task;
        }

        private void SchedulePrepareDataSubTask(Type taskType, JobDataMap jobDataMap, string taskDescription, RisTask task)
        {
            var triggerIdentity = this.GetTriggerIdentity();
            var jobIdentity = this.GetJobIdentity(taskType, triggerIdentity);

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity(triggerIdentity)
                .WithDescription(taskDescription)
                .ForJob(jobIdentity)
                .WithSimpleSchedule(x => x.WithRepeatCount(0).WithMisfireHandlingInstructionFireNow())
                .StartNow()
                .UsingJobData(jobDataMap)
                .Build();

            this.ScheduleSubTask(trigger, jobIdentity, taskType, task, TriggerType.PreparingData);
        }

        private void ScheduleSendDataSubTask(Type taskType, JobDataMap jobDataMap, string taskDescription, int interval, int maxRepeatCount, RisTask task)
        {
            var triggerIdentity = this.GetTriggerIdentity();
            var jobIdentity = this.GetJobIdentity(taskType, triggerIdentity);

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity(triggerIdentity)
                .WithDescription(taskDescription)
                .ForJob(jobIdentity)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(interval)
                .WithRepeatCount(maxRepeatCount)
                .WithMisfireHandlingInstructionFireNow())
                .StartNow()
                .UsingJobData(jobDataMap).Build();

            this.ScheduleSubTask(trigger, jobIdentity, taskType, task, TriggerType.SendingData);
        }

        private void ScheduleSubTask(ITrigger trigger, string jobIdentity, Type taskType, RisTask task, TriggerType triggerType)
        {
            var scheduler = this.Container.Resolve<IScheduler>("TaskScheduler");

            try
            {
                var jobDetail = scheduler.GetJobDetail(JobKey.Create(jobIdentity));

                if (jobDetail == null)
                {
                    jobDetail = JobBuilder.Create(taskType).WithIdentity(jobIdentity).Build();
                }

                var subTaskListener = new SubTaskListener(this.Container, task, triggerType);

                scheduler.ListenerManager.AddJobListener(subTaskListener, KeyMatcher<JobKey>.KeyEquals<JobKey>(jobDetail.Key));

                scheduler.ListenerManager.AddTriggerListener(subTaskListener, KeyMatcher<TriggerKey>.KeyEquals<TriggerKey>(trigger.Key));

                scheduler.ScheduleJob(jobDetail, trigger);
            }
            finally
            {
                this.Container.Release(scheduler);
            }
        }

        private string GetTriggerIdentity()
        {
            return Guid.NewGuid().ToStr();
        }

        private string GetJobIdentity(Type taskType, string triggerIdentity)
        {
            return string.Format("{0}_{1}", taskType.Name, triggerIdentity);
        }

        private JobDataMap CreatePrepareDataJobDataMap(IDictionary<long, DynamicDictionary> extractParams, bool dataForSigning, FileStorageName? fileStorageName)
        {
            var result = new JobDataMap();

            result.Put("ExtractParams", extractParams);
            result.Put("ExecutionOwner", this.GetExecutionOwner());
            result.Put("DataForSigning", dataForSigning);
            result.Put("FileStorageName", fileStorageName);

            return result;
        }

        private JobDataMap CreateSendDataJobDataMap(DynamicDictionary sendDataParams, long[] packageIds, bool sendSignedData)
        {
            var result = new JobDataMap();

            result.Put("SendDataParams", sendDataParams);
            result.Put("packageIds", packageIds);
            result.Put("SendSignedData", sendSignedData);
            result.Put("ExecutionOwner", this.GetExecutionOwner());

            return result;
        }

        private IExecutionOwner GetExecutionOwner()
        {
            try
            {
                var userInfo = this.Container.Resolve<RequestingUserInformation>();
                return new UserExecutionOwner
                {
                    UserId = userInfo.UserIdentity.UserId,
                    RequestIpAddress = userInfo.RequestIpAddress,
                    Name = userInfo.UserIdentity.Name
                };
            }
            catch
            {
                return new SystemExecutionOwner();
            }
        }
    }
}
