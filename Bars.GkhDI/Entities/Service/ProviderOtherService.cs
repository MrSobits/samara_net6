namespace Bars.GkhDi.Entities.Service
{
    using System;

    using Bars.Gkh.Entities;

    public class ProviderOtherService : BaseGkhEntity
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual OtherService OtherService { get; set; }

        /// <summary>
        /// Поставщик услуги
        /// </summary>
        public virtual Contragent Provider { get; set; }

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        public virtual DateTime? DateStartContract { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Активный
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string NumberContract { get; set; }

        /// <summary>
        /// Наименование поставщика (костыль для корректного отображение записей, добавленных до обновления сохранения прочих услуг в SectionImport3)
        /// </summary>
        public virtual string ProviderName { get; set; }
    }
}
