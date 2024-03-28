namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.LocationCode"</summary>
    public class LocationCodeMap : BaseImportableEntityMap<LocationCode>
    {
        public LocationCodeMap() :
                base("Bars.Gkh.RegOperator.Entities.LocationCode", "REGOP_LOC_CODE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.CodeLevel1, "CodeLevel1").Column("CODE_L1").Length(250);
            this.Property(x => x.CodeLevel2, "CodeLevel2").Column("CODE_L2").Length(250);
            this.Property(x => x.CodeLevel3, "CodeLevel3").Column("CODE_L3").Length(250);
            this.Reference(x => x.FiasLevel1, "FiasLevel1").Column("FIAS_ID_L1");
            this.Reference(x => x.FiasLevel2, "FiasLevel2").Column("FIAS_ID_L2");
            this.Property(x => x.FiasLevel3, "FiasLevel3").Column("FIAS_NAME_L3");
            this.Property(x => x.AOGuid, "AOGuid").Column("AOGUID").Length(250);
        }
    }
}
