/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.ServiceSubsidyRegister
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.ServiceSubsidyRegister;
/// 
///     public class ServiceSubsidyRegisterMap : BaseEntityMap<ServiceSubsidyRegister>
///     {
///         public ServiceSubsidyRegisterMap()
///             : base("GIS_SERVICE_SUBSIDY_REGISTER")
///         {
///             Map(x => x.Pss, "PSS");
///             Map(x => x.CalculationMonth, "CALCULATIONMONTH");
///             Map(x => x.ManagementOrganizationAccount, "MANAGEMENTORGANIZATIONACCOUNT");
///             References(x => x.Service, "SERVICE");
///             Map(x => x.AccruedBenefitSum, "ACCRUEDBENEFITSUM");
///             Map(x => x.AccruedEdvSum, "ACCRUEDEDVSUM");
///             Map(x => x.RecalculatedBenefitSum, "RECALCULATEDBENEFITSUM");
///             Map(x => x.RecalculatedEdvSum, "RECALCULATEDEDVSUM");
///             Map(x => x.OrganizationUnit, "ORGANIZATIONUNIT");
///             References(x => x.LoadedFile, "LOADEDFILE");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.PersonalAccountId, "PERSONALACCOUNTID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.ServiceSubsidyRegister
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Register.ServiceSubsidyRegister;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.ServiceSubsidyRegister.ServiceSubsidyRegister"</summary>
    public class ServiceSubsidyRegisterMap : BaseEntityMap<ServiceSubsidyRegister>
    {
        
        public ServiceSubsidyRegisterMap() : 
                base("Bars.Gkh.Gis.Entities.Register.ServiceSubsidyRegister.ServiceSubsidyRegister", "GIS_SERVICE_SUBSIDY_REGISTER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Pss, "Pss").Column("PSS").Length(250);
            Property(x => x.CalculationMonth, "CalculationMonth").Column("CALCULATIONMONTH");
            Property(x => x.ManagementOrganizationAccount, "ManagementOrganizationAccount").Column("MANAGEMENTORGANIZATIONACCOUNT");
            Reference(x => x.Service, "Service").Column("SERVICE");
            Property(x => x.AccruedBenefitSum, "AccruedBenefitSum").Column("ACCRUEDBENEFITSUM");
            Property(x => x.AccruedEdvSum, "AccruedEdvSum").Column("ACCRUEDEDVSUM");
            Property(x => x.RecalculatedBenefitSum, "RecalculatedBenefitSum").Column("RECALCULATEDBENEFITSUM");
            Property(x => x.RecalculatedEdvSum, "RecalculatedEdvSum").Column("RECALCULATEDEDVSUM");
            Property(x => x.OrganizationUnit, "OrganizationUnit").Column("ORGANIZATIONUNIT").Length(250);
            Reference(x => x.LoadedFile, "LoadedFile").Column("LOADEDFILE");
            Property(x => x.Address, "Address").Column("ADDRESS").Length(250);
            Property(x => x.PersonalAccountId, "PersonalAccountId").Column("PERSONALACCOUNTID");
        }
    }
}
