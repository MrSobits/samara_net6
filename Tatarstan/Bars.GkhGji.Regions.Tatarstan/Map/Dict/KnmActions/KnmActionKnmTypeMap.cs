namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.KnmActions
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;

    public class KnmActionKnmTypeMap : BaseEntityMap<KnmActionKnmType>
    {
        public KnmActionKnmTypeMap()
            : base("Связь действия КНМ с видом КНМ", "GJI_DICT_KNM_ACTION_KNM_TYPE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.KnmAction, "Действие в рамках КНМ").Column("KNM_ACTION_ID").Fetch();
            this.Reference(x => x.KnmTypes, "Вид КНМ").Column("KNM_TYPE_ID").Fetch();
        }
    }
}
