namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    /// <summary>
    ///  Связь записи первого этапа версии и видов работ
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