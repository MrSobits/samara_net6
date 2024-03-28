namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Роли контрагента"</summary>
    public class ContragentRoleMap : BaseEntityMap<ContragentRole>
    {
        public ContragentRoleMap() : 
                base("Роли контрагента", "GKH_DICT_CONTRAGENT_ROLE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
            this.Property(x => x.Name, "Полное наименование").Column("NAME").Length(400).NotNull();
            this.Property(x => x.ShortName, "Краткое наименование").Column("SHORT_NAME").Length(400).NotNull();
        }
    }
}
