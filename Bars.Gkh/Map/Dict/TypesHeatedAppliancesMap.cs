namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Типы отопительных приборов"</summary>
    public class TypesHeatedAppliancesMap : BaseEntityMap<TypesHeatedAppliances>
    {
        public TypesHeatedAppliancesMap() :
            base("Типы отопительных приборов", "GKH_DICT_TYPES_HEATED_APPLIANCES")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
