namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тарифы для потребителей
    /// </summary>
    public class ConsumptionNormsNpa : BaseGkhEntity
    {
        /// <summary>
        /// Базовая услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? NpaDate { get; set; }

        /// <summary>
        /// Номер нормативного правового акта
        /// </summary>
        public virtual string NpaNumber { get; set; }

        /// <summary>
        /// Наименование принявшего акт органа
        /// </summary>
        public virtual string NpaAcceptor { get; set; }

    }
}