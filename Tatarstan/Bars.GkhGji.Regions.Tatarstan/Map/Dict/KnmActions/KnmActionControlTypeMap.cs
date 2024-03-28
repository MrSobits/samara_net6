namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.KnmActions
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;

    public class KnmActionControlTypeMap : BaseEntityMap<KnmActionControlType>
    {
        public KnmActionControlTypeMap()
            : base("Связь с видом контроля", "GJI_DICT_KNM_ACTION_CONTROL_TYPE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.KnmAction, "Действие в рамках КНМ").Column("KNM_ACTION_ID").Fetch();
            this.Reference(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE_ID").Fetch();
        }
    }
}
