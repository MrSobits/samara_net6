namespace Bars.GisIntegration.Base.DataExtractors
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using Castle.Windsor;

    /// <summary>
    /// Базовый тонкий экстрактор данных
    /// </summary>
    public abstract class BaseSlimDataExtractor<TRisEntity> : IDataExtractor<TRisEntity> where TRisEntity : BaseRisEntity
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Лог экстрактора
        /// </summary>
        public List<ILogRecord> Log { get; set; } = new List<ILogRecord>();

        private RisContragent contragent;

        /// <summary>
        /// Поставщик данных
        /// </summary>
        public RisContragent Contragent
        {
            get
            {
                if (this.contragent == null && this.ContragentIsRequired)
                {
                    this.contragent = this.GetCurrentContragent();
                }

                return this.contragent;
            }

            set
            {
                this.contragent = value;
            }
        }

        /// <summary>
        /// Контрагент обязателен
        /// </summary>
        protected virtual bool ContragentIsRequired => true;

        private RisContragent GetCurrentContragent()
        {
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                return dataSupplierProvider.GetCurrentDataSupplier();
            }
            finally
            {
                this.Container.Release(dataSupplierProvider);
            }
        }

        /// <summary>
        /// Добавление записей лога в коллекцию
        /// </summary>
        protected void AddLogRecord(ILogRecord logRecord)
        {
            this.Log.Add(logRecord);
        }

        /// <summary>
        /// Получить сущности внутренней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности внутренней системы</returns>
        public abstract List<TRisEntity> Extract(DynamicDictionary parameters);
    }
}
