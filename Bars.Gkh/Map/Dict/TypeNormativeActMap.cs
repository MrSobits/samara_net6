namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Виды нормативных актов"</summary>
    public class TypeNormativeActMap : BaseEntityMap<TypeNormativeAct>
    {
        public TypeNormativeActMap() : 
                base("Виды нормативных актов", "GKH_DICT_TYPE_NORMATIVE_ACT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
            this.Property(x => x.ActionLevel, "Действует на").Column("ACTION_LEVEL").NotNull();
        }
    }
}
