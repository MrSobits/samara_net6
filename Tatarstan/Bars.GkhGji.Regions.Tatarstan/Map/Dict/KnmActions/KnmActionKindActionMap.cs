namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.KnmActions
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;

    public class KnmActionKindActionMap : BaseEntityMap<KnmActionKindAction>
    {
        /// <inheritdoc />
        public KnmActionKindActionMap()
            : base("KnmActionKindAction", "GJI_DICT_KNM_ACTION_KIND_ACTION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.KindAction, "KindAction").Column("KIND_ACTION");
            this.Reference(x => x.KnmAction, "KnmAction").Column("KNM_ACTION_ID").Fetch();
        }
    }
}