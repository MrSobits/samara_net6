namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Контролируемые лица
    /// </summary>
    public class DecisionControlSubjects : BaseGkhEntity
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Decision Decision { get; set; }

        /// <summary>
        /// Объект проверки
        /// </summary>
        public virtual PersonInspection PersonInspection { get; set; }

        /// <summary>
        /// Контрагент проверки
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPersonINN { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPersonPosition { get; set; }



    }
}