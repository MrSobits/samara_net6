namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха прденазначена для виджета проверки исполнения предписаний
     * чтобы получать данные:
     * срок исполнения, номер предписания, дата предписания, контрагент
     */
    public class ViewPrescriptionWidget : PersistentObject
    {
        /// <summary>
        /// Последний срок нарушения
        /// </summary>
        public virtual DateTime? LastDateViolation { get; set; }

        /// <summary>
        /// Дата предписания
        /// </summary>
        public virtual DateTime? DatePrescription { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Оператор
        /// </summary>
        public virtual long OperatorId { get; set; }
    }
}