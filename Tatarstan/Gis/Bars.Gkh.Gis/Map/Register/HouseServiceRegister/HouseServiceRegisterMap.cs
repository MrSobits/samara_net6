/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.HouseServiceRegister
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.HouseServiceRegister;
/// 
///     public class HouseServiceRegisterMap : BaseEntityMap<HouseServiceRegister>
///     {
///         public HouseServiceRegisterMap()
///             : base("GIS_HOUSE_SERVICE_REGISTER")
///         {
///             References(x => x.House, "HOUSEID");
///             References(x => x.Service, "SERVICEID");
///             Map(x => x.HouseAddress, "HOUSEADDRESS");
///             Map(x => x.CalculationDate, "CALCULATION_DATE");
///             Map(x => x.VolumeIndividualCounter, "VOLUMEINDIVIDUALCOUNTER");
///             Map(x => x.VolumeNormative, "VOLUMENORMATIVE");
///             Map(x => x.CoefOdn, "COEFODN");
///             Map(x => x.VolumeDistributed, "VOLUMEDISTRIBUTED");
///             Map(x => x.VolumeNotDistributed, "VOLUMENOTDISTRIBUTED");
///             Map(x => x.VolumeOdnIndividualCounter, "VOLUMEODNINDIVIDUALCOUNTER");
///             Map(x => x.VolumeOdnNormative, "VOLUMEODNNORMATIVE");
///             Map(x => x.Tariff, "TARIFF");
///             Map(x => x.TariffDate, "TARIFFDATE");
///             Map(x => x.Rso, "RSO");
///             Map(x => x.RsoInn, "RSOINN");
///             Map(x => x.Charge, "CHARGE");
///             Map(x => x.Payment, "PAYMENT");
///             Map(x => x.ManOrgs, "MANORGS");
///             Map(x => x.TotalVolume, "TOTALVOLUME");
///             Map(x => x.IsPublished, "ISPUBLISHED");
///             Map(x => x.InternalId, "INTERNAL_ID");
///             References(x => x.LoadedFile, "LOADEDFILE");
///             Map(x => x.AreaType, "TYPE_AREA");
///             Map(x => x.ContractType, "TYPE_CONTRACT");
///             Map(x => x.ServiceSum, "SERVICE_SUM");
///             Map(x => x.ServiceNds, "SERVICE_NDS");
///             Map(x => x.ServiceRecalculation, "SERVICE_RECALCULATION");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.HouseServiceRegister
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Register.HouseServiceRegister;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.HouseServiceRegister.HouseServiceRegister"</summary>
    public class HouseServiceRegisterMap : BaseEntityMap<HouseServiceRegister>
    {
        
        public HouseServiceRegisterMap() : 
                base("Bars.Gkh.Gis.Entities.Register.HouseServiceRegister.HouseServiceRegister", "GIS_HOUSE_SERVICE_REGISTER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.House, "House").Column("HOUSEID");
            Reference(x => x.Service, "Service").Column("SERVICEID");
            Property(x => x.HouseAddress, "HouseAddress").Column("HOUSEADDRESS").Length(250);
            Property(x => x.CalculationDate, "CalculationDate").Column("CALCULATION_DATE");
            Property(x => x.Charge, "Charge").Column("CHARGE");
            Property(x => x.Payment, "Payment").Column("PAYMENT");
            Property(x => x.ManOrgs, "ManOrgs").Column("MANORGS").Length(250);
            Property(x => x.VolumeIndividualCounter, "VolumeIndividualCounter").Column("VOLUMEINDIVIDUALCOUNTER");
            Property(x => x.VolumeNormative, "VolumeNormative").Column("VOLUMENORMATIVE");
            Property(x => x.CoefOdn, "CoefOdn").Column("COEFODN");
            Property(x => x.VolumeDistributed, "VolumeDistributed").Column("VOLUMEDISTRIBUTED");
            Property(x => x.VolumeNotDistributed, "VolumeNotDistributed").Column("VOLUMENOTDISTRIBUTED");
            Property(x => x.VolumeOdnIndividualCounter, "VolumeOdnIndividualCounter").Column("VOLUMEODNINDIVIDUALCOUNTER");
            Property(x => x.VolumeOdnNormative, "VolumeOdnNormative").Column("VOLUMEODNNORMATIVE");
            Property(x => x.Tariff, "Tariff").Column("TARIFF");
            Property(x => x.TariffDate, "TariffDate").Column("TARIFFDATE");
            Property(x => x.Rso, "Rso").Column("RSO").Length(250);
            Property(x => x.RsoInn, "RsoInn").Column("RSOINN");
            Property(x => x.TotalVolume, "TotalVolume").Column("TOTALVOLUME");
            Property(x => x.IsPublished, "IsPublished").Column("ISPUBLISHED");
            Property(x => x.InternalId, "InternalId").Column("INTERNAL_ID");
            Reference(x => x.LoadedFile, "LoadedFile").Column("LOADEDFILE");
            Property(x => x.AreaType, "AreaType").Column("TYPE_AREA");
            Property(x => x.ContractType, "ContractType").Column("TYPE_CONTRACT");
            Property(x => x.ServiceSum, "ServiceSum").Column("SERVICE_SUM");
            Property(x => x.ServiceNds, "ServiceNds").Column("SERVICE_NDS");
            Property(x => x.ServiceRecalculation, "ServiceRecalculation").Column("SERVICE_RECALCULATION");
        }
    }
}
