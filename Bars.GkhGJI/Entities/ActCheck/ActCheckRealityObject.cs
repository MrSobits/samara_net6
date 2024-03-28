namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Дом акта проверки
    /// Данная таблица хранит всебе все дома которые необходимо проверить
    /// </summary>
    public class ActCheckRealityObject : BaseGkhEntity, IEntityUsedInErp
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Не выявленные нарушения
        /// </summary>
        public virtual string NotRevealedViolations { get; set; }

        /// <summary>
        /// Признак выявлено или невыявлено нарушение
        /// </summary>
        public virtual YesNoNotSet HaveViolation { get; set; }

        /// <summary>
        /// Сведения о лицах, допустивших нарушения
        /// </summary>
        public virtual string PersonsWhoHaveViolated { get; set; }

        /// <summary>
        /// Сведения, свидетельствующие, что нарушения допущены в результате виновных действий (бездействия)
        /// должностных лиц и (или) работников проверяемого лица
        /// </summary>
        public virtual string OfficialsGuiltyActions { get; set; }

        /// <summary>
        /// Гуид ЕРП
        /// </summary>
        public virtual string ErpGuid { get; set; }
    }
}