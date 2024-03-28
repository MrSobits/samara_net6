/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Regions.Msk.Entities;
/// 
///     public class RealityObjectInfoMap : BaseEntityMap<RealityObjectInfo>
///     {
///         public RealityObjectInfoMap()
///             : base("MSK_RO_INFO")
///         {
///             Map(x => x.Uid, "UID");
///             Map(x => x.Okrug, "OKRUG");
///             Map(x => x.Raion, "RAION");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.UnomCode, "UNOM_CODE");
///             Map(x => x.MziCode, "MZI_CODE");
///             Map(x => x.Serial, "SERIAL");
///             Map(x => x.BuildingYear, "BUILDING_YEAR");
///             Map(x => x.TotalArea, "TOTAL_AREA");
///             Map(x => x.LivingArea, "LIVING_AREA");
///             Map(x => x.NoLivingArea, "NOLIVING_AREA");
///             Map(x => x.FlatCount, "FLOOR_COUNT");
///             Map(x => x.FloorCount, "PORCH_COUNT");
///             Map(x => x.PorchCount, "FLAT_COUNT");
///             Map(x => x.AllDelay, "ALL_DELAY");
///             Map(x => x.Points, "POINTS");
///             Map(x => x.IndexNumber, "INDEX_NUMBER");
/// 
///             Map(x => x.EsPeriod, "ES_PERIOD");
///             Map(x => x.GasPeriod, "GAS_PERIOD");
///             Map(x => x.HvsPeriod, "HVS_PERIOD");
///             Map(x => x.HvsmPeriod, "HVSM_PERIOD");
///             Map(x => x.GvsPeriod, "GVS_PERIOD");
///             Map(x => x.GvsmPeriod, "GVSM_PERIOD");
///             Map(x => x.KanPeriod, "KAN_PERIOD");
///             Map(x => x.KanmPeriod, "KANM_PERIOD");
///             Map(x => x.OtopPeriod, "OTOP_PERIOD");
///             Map(x => x.OtopmPeriod, "OTOPM_PERIOD");
///             Map(x => x.MusPeriod, "MUS_PERIOD");
///             Map(x => x.PpiaduPeriod, "PPIADU_PERIOD");
///             Map(x => x.PvPeriod, "PV_PERIOD");
///             Map(x => x.FasPeriod, "FAS_PERIOD");
///             Map(x => x.KrovPeriod, "KROV_PERIOD");
///             Map(x => x.VdskPeriod, "VDSK_PERIOD");
///             Map(x => x.LiftPeriod, "LIFT_PERIOD");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Msk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Msk.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Regions.Msk.Entities.RealityObjectInfo"</summary>
    public class RealityObjectInfoMap : BaseEntityMap<RealityObjectInfo>
    {
        
        public RealityObjectInfoMap() : 
                base("Bars.Gkh.Regions.Msk.Entities.RealityObjectInfo", "MSK_RO_INFO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Uid, "Uid").Column("UID").Length(250);
            Property(x => x.Okrug, "Okrug").Column("OKRUG").Length(250);
            Property(x => x.Raion, "Raion").Column("RAION").Length(250);
            Property(x => x.Address, "Address").Column("ADDRESS").Length(250);
            Property(x => x.UnomCode, "UnomCode").Column("UNOM_CODE").Length(250);
            Property(x => x.MziCode, "MziCode").Column("MZI_CODE").Length(250);
            Property(x => x.Serial, "Serial").Column("SERIAL").Length(250);
            Property(x => x.BuildingYear, "BuildingYear").Column("BUILDING_YEAR");
            Property(x => x.TotalArea, "TotalArea").Column("TOTAL_AREA");
            Property(x => x.LivingArea, "LivingArea").Column("LIVING_AREA");
            Property(x => x.NoLivingArea, "NoLivingArea").Column("NOLIVING_AREA");
            Property(x => x.FloorCount, "FloorCount").Column("PORCH_COUNT");
            Property(x => x.PorchCount, "PorchCount").Column("FLAT_COUNT");
            Property(x => x.FlatCount, "FlatCount").Column("FLOOR_COUNT");
            Property(x => x.AllDelay, "AllDelay").Column("ALL_DELAY");
            Property(x => x.Points, "Points").Column("POINTS");
            Property(x => x.IndexNumber, "IndexNumber").Column("INDEX_NUMBER");
            Property(x => x.EsPeriod, "EsPeriod").Column("ES_PERIOD").Length(250);
            Property(x => x.GasPeriod, "GasPeriod").Column("GAS_PERIOD").Length(250);
            Property(x => x.HvsPeriod, "HvsPeriod").Column("HVS_PERIOD").Length(250);
            Property(x => x.HvsmPeriod, "HvsmPeriod").Column("HVSM_PERIOD").Length(250);
            Property(x => x.GvsPeriod, "GvsPeriod").Column("GVS_PERIOD").Length(250);
            Property(x => x.GvsmPeriod, "GvsmPeriod").Column("GVSM_PERIOD").Length(250);
            Property(x => x.KanPeriod, "KanPeriod").Column("KAN_PERIOD").Length(250);
            Property(x => x.KanmPeriod, "KanmPeriod").Column("KANM_PERIOD").Length(250);
            Property(x => x.OtopPeriod, "OtopPeriod").Column("OTOP_PERIOD").Length(250);
            Property(x => x.OtopmPeriod, "OtopmPeriod").Column("OTOPM_PERIOD").Length(250);
            Property(x => x.MusPeriod, "MusPeriod").Column("MUS_PERIOD").Length(250);
            Property(x => x.PpiaduPeriod, "PpiaduPeriod").Column("PPIADU_PERIOD").Length(250);
            Property(x => x.PvPeriod, "PvPeriod").Column("PV_PERIOD").Length(250);
            Property(x => x.FasPeriod, "FasPeriod").Column("FAS_PERIOD").Length(250);
            Property(x => x.KrovPeriod, "KrovPeriod").Column("KROV_PERIOD").Length(250);
            Property(x => x.VdskPeriod, "VdskPeriod").Column("VDSK_PERIOD").Length(250);
            Property(x => x.LiftPeriod, "LiftPeriod").Column("LIFT_PERIOD").Length(250);
        }
    }
}
