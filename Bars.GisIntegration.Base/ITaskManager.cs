namespace Bars.GisIntegration.Base
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.Gkh.Quartz.Scheduler.Entities;
    using Bars.Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Интерфейс менеджера задач
    /// </summary>
    public interface ITaskManager
    {
        /// <summary>
        /// Создать задачу экспорта
        /// </summary>
        /// <param name="exporter">Экспортер, инициировавший задачу</param>
        /// <param name="extractParams">Словарь идентификатор контрагента - параметры извлечения данных</param>
        void CreateExportTask(IDataExporter exporter, IDictionary<long, DynamicDictionary> extractParams);

        /// <summary>
        /// Создать подзадачу подготовки данных
        /// </summary>
        /// <param name="exporter">Экспортер, инициировавший задачу</param>
        /// <param name="task">Хранимая задача</param>
        /// <param name="extractParams">Словарь идентификатор контрагента - параметры извлечения данных</param>
        void CreatePrepareDataSubTask(IDataExporter exporter, RisTask task, IDictionary<long, DynamicDictionary> extractParams);

        /// <summary>
        /// Создать подзадачу отправки данных
        /// </summary>
        /// <param name="exporter">Экспортер, инициировавший задачу</param>
        /// <param name="task">Хранимая задача</param>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        void CreateSendDataSubTask(IDataExporter exporter, RisTask task, long[] packageIds);
       
        /// <summary>
        /// Получить триггеры, связанные с задачей
        /// </summary>
        /// <param name="taskId">Идентификатор хранимой задачи</param>
        /// <returns>Триггеры, связанные с задачей</returns>
        List<RisTaskTrigger> GetRisTaskTriggers(long taskId);

        /// <summary>
        /// Получить пакеты, связанные с триггером
        /// </summary>
        /// <param name="triggerId">Идентификатор хранимого триггера</param>
        /// <returns>Пакеты, связанные с триггером</returns>
        List<RisPackageTrigger> GetTriggerPackages(long triggerId);

        /// <summary>
        /// Получить хранимый триггер
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Триггер</returns>
        Trigger GetTrigger(long triggerId);

        /// <summary>
        /// Получить объект, связывающий триггер и пакет по идентификатору
        /// </summary>
        /// <param name="triggerPackageId">Идентификатор связки</param>
        /// <returns>Хранимый объект, связывающий триггер и пакет</returns>
        RisPackageTrigger GetTriggerPackage(long triggerPackageId);

        /// <summary>
        /// Получить хранимую задачу
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>Хранимая задача</returns>
        RisTask GetTask(long taskId);

        /// <summary>
        /// Получить триггер подготовки данных для задачи
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>Триггер подготовки данных</returns>
        Trigger GetTaskPreparingDataTrigger(long taskId);

        /// <summary>
        /// Получить результат триггера подготовки данных
        /// </summary>
        /// <param name="triggerId">Идентификатор хранимого триггера</param>
        /// <returns>Результат подготовки данных</returns>
        PrepareDataResult GetPreparingDataTriggerResult(long triggerId);

        /// <summary>
        /// Получить список результатов валидации
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера подготовки данных</param>
        /// <returns>Список результатов валидации</returns>
        List<ValidateObjectResult> GetValidationResultList(long triggerId);

        /// <summary>
        /// Получить список результатов загрузки вложений
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера подготовки данных</param>
        /// <returns>Список результатов загрузки вложений</returns>
        List<UploadAttachmentResult> GetUploadAttachmentResultList(long triggerId);

        /// <summary>
        /// Получить протокол выполнения триггера
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Протокол выполнения триггера</returns>
        List<ILogRecord> GetTriggerProtocol(long triggerId);

        /// <summary>
        /// Получить описание результата подготовки данных
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>Описание результата подготовки данных</returns>
        PrepareDataResultDescription GetPrepareDataResultDescription(long taskId);

        /// <summary>
        /// Получить журнал выполнения триггера
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Список записей журнала выполнения триггера</returns>
        List<JournalRecord> GetTriggerJournal(long triggerId);

        /// <summary>
        /// Получить сериализуемый результат подготовки данных
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера подготовки данных</param>
        /// <returns>Сериализуемый результат подготовки данных</returns>
        SerializablePrepareDataResult GetSerializablePreparingDataTriggerResult(long triggerId);

        /// <summary>
        /// Получить сериализуемый результат подготовки данных
        /// </summary>
        /// <param name="journalRecord">Запись журнала подготовки данных</param>
        /// <returns>Сериализуемый результат подготовки данных</returns>
        SerializablePrepareDataResult GetSerializablePreparingDataTriggerResult(JournalRecord journalRecord);
    }
}