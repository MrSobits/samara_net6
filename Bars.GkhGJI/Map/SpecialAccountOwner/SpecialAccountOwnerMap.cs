namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Решение судебных участков"</summary>
    public class SpecialAccountOwnerMap : BaseEntityMap<SpecialAccountOwner>
    {
        
        public SpecialAccountOwnerMap() : 
                base("Владелец спецсчета", "GJI_SPECIAL_ACCOUNT_OWNER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Property(x => x.ActivityDateEnd, "Дата прекращения деятельности").Column("ACTIVITY_DATE_END");
            Property(x => x.ActivityGroundsTermination, "Причина прекращения деятельности").Column("TERM_REASON");
            Property(x => x.Description, "Описание").Column("DESCRIPTION");
            Property(x => x.OrgStateRole, "Статус").Column("STATE_ROLE").NotNull();
        }
    }
}
