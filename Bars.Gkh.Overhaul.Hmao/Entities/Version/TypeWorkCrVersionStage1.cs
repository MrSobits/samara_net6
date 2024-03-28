namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.GkhCr.Entities;
    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    ///  Связь записи первого этапа версии и видов работ
    /// </summary>
    public class TypeWorkCrVersionStage1 : BaseImportableEntity
    {
        public virtual VersionRecordStage1 Stage1Version { get; set; }

        public virtual TypeWorkCr TypeWorkCr { get; set; }

        public virtual UnitMeasure UnitMeasure { get; set; }

        public virtual PriceCalculateBy CalcBy { get; set; }

        public virtual decimal Volume { get; set; }

        public virtual decimal Sum { get; set; }
    }
}