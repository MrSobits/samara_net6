namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Enums;
    using B4.Modules.States;

    /// <summary>
    /// Предписание для расчета категории риска по коэффициенту Vpr
    /// </summary>
    public class VprPrescription : BaseGkhEntity

    {
        /// <summary>
        /// Расчет категории для контрагента
        /// </summary>
        public virtual ROMCategory ROMCategory { get; set; }

        /// <summary>
        /// Постановление
        /// </summary>
        public virtual Prescription Prescription { get; set; }

        /// <summary>
        ///Статус предписания
        /// </summary>
        public virtual string StateName { get; set; }

    }
}