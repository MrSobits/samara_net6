namespace Bars.Gkh.Quartz.Scheduler.Listeners
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Quartz.Scheduler.Entities;
    using Bars.Gkh.Quartz.Scheduler.Extensions;

    using Castle.Windsor;

    using NLog;

    using global::Quartz;

    /// <summary>
    /// Обработчик событий триггеров
    /// </summary>
    public class TriggerEventsHandler : ITriggerListener
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private Logger logger = LogManager.GetLogger(typeof(TriggerEventsHandler).FullName);

        /// <summary>
        /// Имя слушателя
        /// </summary>
        public string Name
        {
            get
            {
                return "TriggerEventsHandler";
            }
        }

        /// <summary>
        /// Метод обработки события, когда сработал триггер.
        /// </summary>
        /// <param name="trigger">Объект триггера</param>
        /// <param name="context">Контекст выполнения задачи</param>
        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            ExecutionOwnerScope.CallInNewScope(
                context,
                () =>
                {
                    var storableTriggerId = (long)context.GetPropertyValue("storableTriggerId");

                    var storableTrigger = this.GetTrigger(storableTriggerId);

                    context.MergedJobDataMap.Put("StorableTrigger", storableTrigger);

                    if (context.PreviousFireTimeUtc == null)
                    {
                        this.UpdateTrigger(storableTrigger, true);
                    }
                });
        }

        /// <summary>
        /// Метод обработки события отмены выполнения задачи триггером
        /// </summary>
        /// <param name="trigger">Объект триггера</param>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <returns>true - если выполнение задачи должно быть отменено, false - в противном случае</returns>
        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            return false;
        }

        /// <summary>
        /// Метод обработки события, когда произошла осечка триггера
        /// </summary>
        /// <param name="trigger">Объект триггера</param>
        public void TriggerMisfired(ITrigger trigger)
        {
        }

        /// <summary>
        /// Метод обработки события, когда задача выполнена
        /// </summary>
        /// <param name="trigger">Объект триггера</param>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <param name="triggerInstructionCode">Инструкция планировщика</param>
        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            try
            {
                ExecutionOwnerScope.CallInNewScope(
                    context,
                    () => { this.SaveResult(context); });              
            }
            catch (Exception exception)
            {
                this.logger.Error(exception, string.Format("Trigger '{0}' saving result error.", trigger.Key.Name));
            }
        }

        /// <summary>
        /// Получить хранимый триггер
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Триггер</returns>
        private Trigger GetTrigger(long triggerId)
        {
            var triggerRepository = this.Container.ResolveDomain<Trigger>();

            try
            {
                var result = triggerRepository.Get(triggerId);

                if (result != null)
                {
                    return result;
                }

                throw new Exception($"Storable trigger {triggerId} is not found.");
            }
            finally
            {
                this.Container.Release(triggerRepository);
            }
        }

        private void SaveResult(IJobExecutionContext context)
        {
            var storableTrigger = (Trigger)context.GetPropertyValue("StorableTrigger");

            var triggerState = context.Scheduler.GetTriggerState(context.Trigger.Key);

            if (triggerState == TriggerState.None || triggerState == TriggerState.Complete || !context.Trigger.GetMayFireAgain())
            {
                this.UpdateTrigger(storableTrigger, false);
            }

            var journalRecord = this.CreateJournalRecord(storableTrigger, context);
            this.SaveJournalRecord(journalRecord);
        }

        /// <summary>
        /// Создает новую запись журнала на основании переданого контекста выполнения
        /// </summary>
        /// <param name="storableTrigger">Хранимый триггер</param>
        /// <param name="context">Котекст выполнения</param>
        /// <returns>Новый объект Запись журнала</returns>
        private JournalRecord CreateJournalRecord(Trigger storableTrigger, IJobExecutionContext context)
        {
            var startExecutionTime = context.FireTimeUtc ?? SystemTime.UtcNow();
            var finishExecutionTime = SystemTime.UtcNow();

            var journalRecord = new JournalRecord
            {
                Trigger = storableTrigger,
                StartTime = new DateTime(startExecutionTime.Ticks).ToLocalTime(),
                EndTime = new DateTime(finishExecutionTime.Ticks).ToLocalTime()
            };

            var worker = (ITask)context.JobInstance;

            if (worker.ExecutionException != null)
            {
                journalRecord.Message = worker.ExecutionException.Message;
            }

            journalRecord.Interrupted = worker.InterruptRequested;

            if (context.Result != null)
            {
                journalRecord.Result = this.GetBytes(context.Result);
            }

            var log = worker.GetLog();

            if (log.Any())
            {
                journalRecord.Protocol = this.GetBytes(log);
            }

            return journalRecord;
        }

        /// <summary>
        /// Сохраняет в БД запись журнала выполнения тригера
        /// </summary>
        /// <param name="journalRecord">запись журнала выполнения тригера</param>
        private void SaveJournalRecord(JournalRecord journalRecord)
        {
            var journalRecordRepository = this.Container.ResolveDomain<JournalRecord>();

            try
            {
                journalRecordRepository.Save(journalRecord);
            }
            finally 
            {
                this.Container.Release(journalRecordRepository);
            }
        }

        private void UpdateTrigger(Trigger storableTrigger, bool start)
        {
            var triggerRepository = this.Container.ResolveDomain<Trigger>();

            try
            {
                if (start)
                {
                    storableTrigger.StartTime = DateTime.Now;
                }
                else
                {
                    storableTrigger.EndTime = DateTime.Now;
                }
                
                triggerRepository.Update(storableTrigger);
            }
            finally
            {
                this.Container.Release(triggerRepository);
            }
        }

        private byte[] GetBytes(object inputObject)
        {
            byte[] result;

            var stream = inputObject as Stream;

            if (stream != null)
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    result = binaryReader.ReadBytes((int)stream.Length);
                }
            }
            else
            {
                var fileInfo = inputObject as FileInfo;

                if (fileInfo != null)
                {
                    using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
                    {
                        using (var binaryReader = new BinaryReader(fileStream))
                        {
                            result = binaryReader.ReadBytes((int)fileStream.Length);
                        }
                    }     
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(memoryStream, inputObject);

                        result = memoryStream.ToArray();
                    }
                }
            }

            return result;
        }
    }
}
