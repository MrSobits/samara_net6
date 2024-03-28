namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Base.Entities.RisContragent"
    /// </summary>
    public class RisContragentMap : BaseEntityMap<RisContragent>
    {
        public RisContragentMap() : 
                base("Bars.GisIntegration.Base.Entities.RisContragent", "GI_CONTRAGENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.GkhId, "GkhId").Column("GKHID");
            Property(x => x.FullName, "FullName").Column("FULLNAME").Length(1000);
            Property(x => x.Ogrn, "Ogrn").Column("OGRN").Length(50);
            Property(x => x.OrgRootEntityGuid, "OrgRootEntityGuid").Column("ORGROOTENTITYGUID").Length(50);
            Property(x => x.OrgVersionGuid, "OrgVersionGuid").Column("ORGVERSIONGUID").Length(50);          
            Property(x => x.FactAddress, "FactAddress").Column("FACT_ADDRESS").Length(500);
            Property(x => x.JuridicalAddress, "JuridicalAddress").Column("JUR_ADDRESS").Length(500);
            Property(x => x.OrganizationType, "OrganizationType").Column("ORGTYPE");
            Property(x => x.OrgPpaGuid, "OrgPpaGuid").Column("ORGPPAGUID");
            Property(x => x.AccreditationRecordNumber, "AccreditationRecordNumber").Column("ACCREDITATION_RECORD_NUMBER");
        }
    }
}