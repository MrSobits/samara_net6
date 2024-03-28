namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;

    public class LocationCode: BaseImportableEntity
    {
        /// <summary>
        /// Код для первого уровня (муниципальный район)
        /// </summary>
        public virtual string CodeLevel1 { get; set; }

        /// <summary>
        /// Код для второго уровня (муниципальное образование)
        /// </summary>
        public virtual string CodeLevel2 { get; set; }

        /// <summary>
        /// Код для третьего уровня (поселение)
        /// </summary>
        public virtual string CodeLevel3 { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality FiasLevel1 { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality FiasLevel2 { get; set; }

        /// <summary>
        /// Наименование поселения
        /// </summary>
        public virtual string FiasLevel3 { get; set; }

        /// <summary>
        /// Гуид поселения
        /// </summary>
        public virtual string AOGuid { get; set; }
    }
}
