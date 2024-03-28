namespace Bars.GkhDi.Entities
{
    using System;

    public class AdditionalService : BaseService
    {
        /// <summary>
        /// Периодичность выполнения
        /// </summary>
        public virtual PeriodicityTemplateService Periodicity { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string Document { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Документ От
        /// </summary>
        public virtual DateTime? DocumentFrom { get; set; }

        /// <summary>
        /// Дата начала действия договора
        /// </summary>
        public virtual DateTime? DateStart { get; set; }


        /// <summary>
        /// Дата окончания действия договора
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Сумма договора
        /// </summary>
        public virtual decimal? Total { get; set; }
    }
}