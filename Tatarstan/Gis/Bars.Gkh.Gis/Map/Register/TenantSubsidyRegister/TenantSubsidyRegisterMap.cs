/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.TenantSubsidyRegister
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.TenantSubsidyRegister;
/// 
///     public class TenantSubsidyRegisterMap : BaseEntityMap<TenantSubsidyRegister>
///     {
///         public TenantSubsidyRegisterMap()
///             : base("GIS_TENANT_SUBSIDY_REGISTER")
///         {
///             Map(x => x.Pss, "PSS");
///             Map(x => x.CalculationMonth, "CALCULATIONMONTH");
///             Map(x => x.ManagementOrganizationAccount, "MANAGEMENTORGANIZATIONACCOUNT");
///             Map(x => x.Name, "NAME");
///             Map(x => x.Surname, "SURNAME");
///             Map(x => x.Patronymic, "PATRONYMIC");
///             Map(x => x.DateOfBirth, "DATEOFBIRTH");
///             Map(x => x.ArticleCode, "ARTICLECODE");
///             References(x => x.Service, "SERVICE");
///             Map(x => x.BankName, "BANKNAME");
///             Map(x => x.BeginDate, "BEGINDATE");
///             Map(x => x.IncomingSaldo, "INCOMINGSALDO");
///             Map(x => x.AccruedSum, "ACCRUEDSUM");
///             Map(x => x.RecalculatedSum, "RECALCULATEDSUM");
///             Map(x => x.AdvancedPayment, "ADVANCEDPAYMENT");
///             Map(x => x.PaymentSum, "PAYMENTSUM");
///             Map(x => x.SmoSum, "SMOSUM");
///             Map(x => x.SmoRecalculatedSum, "SMORECALCULATEDSUM");
///             Map(x => x.ChangesSum, "CHANGESSUM");
///             Map(x => x.EndDate, "ENDDATE");
///             Map(x => x.OrganizationUnit, "ORGANIZATIONUNIT");
///             References(x => x.LoadedFile, "LOADEDFILE");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.PersonalAccountId, "PERSONALACCOUNTID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.TenantSubsidyRegister
{
    using B4.Modules.Mapping.Mappers;
    using Entities.Register.TenantSubsidyRegister;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.TenantSubsidyRegister.TenantSubsidyRegister"</summary>
    public class TenantSubsidyRegisterMap : BaseEntityMap<TenantSubsidyRegister>
    {
        
        public TenantSubsidyRegisterMap() : 
                base("Bars.Gkh.Gis.Entities.Register.TenantSubsidyRegister.TenantSubsidyRegister", "GIS_TENANT_SUBSIDY_REGISTER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Pss, "Pss").Column("PSS").Length(250);
            Property(x => x.CalculationMonth, "CalculationMonth").Column("CALCULATIONMONTH");
            Property(x => x.ManagementOrganizationAccount, "ManagementOrganizationAccount").Column("MANAGEMENTORGANIZATIONACCOUNT");
            Property(x => x.Surname, "Surname").Column("SURNAME").Length(250);
            Property(x => x.Name, "Name").Column("NAME").Length(250);
            Property(x => x.Patronymic, "Patronymic").Column("PATRONYMIC").Length(250);
            Property(x => x.DateOfBirth, "DateOfBirth").Column("DATEOFBIRTH");
            Property(x => x.ArticleCode, "ArticleCode").Column("ARTICLECODE");
            Reference(x => x.Service, "Service").Column("SERVICE");
            Property(x => x.BankName, "BankName").Column("BANKNAME").Length(250);
            Property(x => x.BeginDate, "BeginDate").Column("BEGINDATE");
            Property(x => x.IncomingSaldo, "IncomingSaldo").Column("INCOMINGSALDO");
            Property(x => x.AccruedSum, "AccruedSum").Column("ACCRUEDSUM");
            Property(x => x.RecalculatedSum, "RecalculatedSum").Column("RECALCULATEDSUM");
            Property(x => x.AdvancedPayment, "AdvancedPayment").Column("ADVANCEDPAYMENT");
            Property(x => x.PaymentSum, "PaymentSum").Column("PAYMENTSUM");
            Property(x => x.SmoSum, "SmoSum").Column("SMOSUM");
            Property(x => x.SmoRecalculatedSum, "SmoRecalculatedSum").Column("SMORECALCULATEDSUM");
            Property(x => x.ChangesSum, "ChangesSum").Column("CHANGESSUM");
            Property(x => x.EndDate, "EndDate").Column("ENDDATE");
            Property(x => x.OrganizationUnit, "OrganizationUnit").Column("ORGANIZATIONUNIT").Length(250);
            Reference(x => x.LoadedFile, "LoadedFile").Column("LOADEDFILE");
            Property(x => x.Address, "Address").Column("ADDRESS").Length(250);
            Property(x => x.PersonalAccountId, "PersonalAccountId").Column("PERSONALACCOUNTID");
        }
    }
}
