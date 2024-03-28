namespace Bars.Gkh.Map.TechnicalPassport
{
    using Bars.Gkh.Entities.TechnicalPassport;
    using Bars.Gkh.TechnicalPassport.Impl;

    public class TechnicalPassportMap : GkhBaseEntityMap<TechnicalPassport>
    {
        public TechnicalPassportMap()
            : base(TechnicalPassportTransformer.TechnicalPassportTableName)
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID");
        }
    }
}