namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха прденазначена для виджета первичные проверки
     * чтобы получать данные:
     * дата начала, дата окончания, номер, тип
     */
    public class ViewDisposalWidget : PersistentObject
    {
        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Оператор
        /// </summary>
        public virtual long OperatorId { get; set; }
    }
}