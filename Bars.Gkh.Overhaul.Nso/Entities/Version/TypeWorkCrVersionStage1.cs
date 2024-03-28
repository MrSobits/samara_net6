namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.GkhCr.Entities;
    using Enums;
    using Gkh.Entities;

    /// <summary>
    ///  Связь версии и видов работ
    /// </summary>
    public class TypeWorkCrVersionStage1 : BaseEntity
    {
        public virtual VersionRecordStage1 Stage1Version { get; set; }

        public virtual TypeWorkCr TypeWorkCr { get; set; }

        public virtual UnitMeasure UnitMeasure { get; set; }

        public virtual PriceCalculateBy CalcBy { get; set; }

        public virtual decimal Volume { get; set; }

        public virtual decimal Sum { get; set; }
    }
}