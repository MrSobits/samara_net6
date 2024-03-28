namespace Bars.Gkh.Entities.RealityObj
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    public class RealityObjectOutdoor : BaseEntity
    {
        /// <summary>
        /// Населенный пункт.
        /// </summary>
        public virtual MunicipalityFiasOktmo MunicipalityFiasOktmo { get; set; }

        /// <summary>
        /// Наименование двора.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код двора.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Площадь.
        /// </summary>
        public virtual decimal? Area { get; set; }

        /// <summary>
        /// Площадь асфальта.
        /// </summary>
        public virtual decimal? AsphaltArea { get; set; }

        /// <summary>
        /// Примечание.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Плановый год ремонта.
        /// </summary>
        public virtual int? RepairPlanYear { get; set; }
    }
}
