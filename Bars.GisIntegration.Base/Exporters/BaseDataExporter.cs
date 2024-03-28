namespace Bars.GisIntegration.Base.Exporters
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Базовый экспортер данных
    /// </summary>
    public abstract class BaseDataExporter : IDataExporter
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Поставщик данных
        /// </summary>
        public RisContragent Contragent { get; set; }

        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public abstract int Order { get; }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public abstract Type SendDataTaskType { get; }

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public abstract Type PrepareDataTaskType { get; }

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public virtual FileStorageName? FileStorage => null;

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public virtual string Description => this.Name;

        /// <summary>
        /// Необходимо подписывать данные
        /// </summary>
        public virtual bool NeedSign => true;

        /// <summary>
        /// Метод может выполняться только от имени поставщика данных
        /// </summary>
        public virtual bool DataSupplierIsRequired => true;

        /// <summary>
        /// Максимальное количество повторов триггера получения результатов экспорта
        /// </summary>
        public virtual int MaxRepeatCount => 10;

        /// <summary>
        /// Интервал запуска триггера получения результатов экспорта - в секундах
        /// </summary>
        public virtual int Interval => 30;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public virtual List<string> GetDependencies()
        {
            return new List<string>();
        }

        /// <summary>
        /// Отправить данные
        /// </summary>
        /// <param name="task">Хранимая задача, в рамках которой отправляются данные</param>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        public void SendData(RisTask task, long[] packageIds)
        {
            if (packageIds.Length == 0)
            {
                throw new Exception("Нет данных для выполнения экспорта");
            }

            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateSendDataSubTask(this, task, packageIds);
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }
       
        /// <summary>
        /// Получить дополнительные параметры отправки данных
        /// Передаются в контекст задачи отправки данных
        /// </summary>
        /// <returns></returns>
        public virtual DynamicDictionary GetSendDataParams()
        {
            return new DynamicDictionary();
        }
    }
}
