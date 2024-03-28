namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.KnmActions
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;

    public class KnmActionMap : BaseEntityMap<KnmAction>
    {
        public KnmActionMap()
            : base("Действие в рамках КНМ", "GJI_DICT_KNM_ACTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ActCheckActionType, "Вид действия").Column("ACT_CHECK_ACTION_TYPE");
            this.Property(x => x.ErvkId, "Идентификатор в ЕРВК").Column("ERVK_ID");
        }
    }
}
