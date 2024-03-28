/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.ServiceRecalculationRegister
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.ServiceRecalculationRegister;
///     
///     public class ServiceRecalculationRegisterMap : BaseEntityMap<ServiceRecalculationRegister>
///     {
///         public ServiceRecalculationRegisterMap()
///             : base("GIS_SERVICE_RECALCULATION_REGISTER")
///         {
///             References(x => x.Service, "SERVICE_ID");
/// 
///             Map(x => x.RecalculationMonth, "RECALCULATION_MONTH");
///             Map(x => x.RecalculationSum, "RECALCULATION_SUM");
///             Map(x => x.RecalculationNds, "RECALCULATION_NDS");
///             Map(x => x.RecalculationVolume, "RECALCULATION_VOLUME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.ServiceRecalculationRegister
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Register.ServiceRecalculationRegister;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.ServiceRecalculationRegister.ServiceRecalculationRegister"</summary>
    public class ServiceRecalculationRegisterMap : BaseEntityMap<ServiceRecalculationRegister>
    {
        
        public ServiceRecalculationRegisterMap() : 
                base("Bars.Gkh.Gis.Entities.Register.ServiceRecalculationRegister.ServiceRecalculationR" +
                        "egister", "GIS_SERVICE_RECALCULATION_REGISTER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Service, "Service").Column("SERVICE_ID");
            Property(x => x.RecalculationMonth, "RecalculationMonth").Column("RECALCULATION_MONTH");
            Property(x => x.RecalculationSum, "RecalculationSum").Column("RECALCULATION_SUM");
            Property(x => x.RecalculationNds, "RecalculationNds").Column("RECALCULATION_NDS");
            Property(x => x.RecalculationVolume, "RecalculationVolume").Column("RECALCULATION_VOLUME");
        }
    }
}
