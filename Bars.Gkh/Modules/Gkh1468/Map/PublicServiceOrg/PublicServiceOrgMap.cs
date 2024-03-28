namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Поставщик ресурсов"</summary>
    public class PublicServiceOrgMap : BaseImportableEntityMap<Entities.PublicServiceOrg>
    {
        
        public PublicServiceOrgMap() : 
                base("Поставщик ресурсов", "GKH_PUBLIC_SERVORG")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.OrgStateRole, "Статус").Column("ORG_STATE_ROLE").NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.ActivityGroundsTermination, "Основание прекращения деятельности").Column("ACTIVITY_TERMINATION").NotNull();
            this.Property(x => x.DescriptionTermination, "Примечание прекращения деятельности").Column("DESCRIPTION_TERM").Length(500);
            this.Property(x => x.DateTermination, "Дата прекращения деятельности").Column("DATE_TERMINATION");
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
