namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Разрез финансирования муниципального образования
    /// </summary>
    public class MunicipalitySourceFinancing : BaseGkhEntity
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Доп. КР
        /// </summary>
        public virtual string AddKr { get; set; }

        /// <summary>
        /// Доп. ФК
        /// </summary>
        public virtual string AddFk { get; set; }

        /// <summary>
        /// Доп. ЭК
        /// </summary>
        public virtual string AddEk { get; set; }

        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public virtual string SourceFinancing { get; set; }

        /// <summary>
        /// КВР
        /// </summary>
        public virtual string Kvr { get; set; }

        /// <summary>
        /// КВСР
        /// </summary>
        public virtual string Kvsr { get; set; }

        /// <summary>
        /// КИФ
        /// </summary>
        public virtual string Kif { get; set; }

        /// <summary>
        /// КФСР
        /// </summary>
        public virtual string Kfsr { get; set; }

        /// <summary>
        /// КЦСР
        /// </summary>
        public virtual string Kcsr { get; set; }

        /// <summary>
        /// КЭС
        /// </summary>
        public virtual string Kes { get; set; }
    }
}
