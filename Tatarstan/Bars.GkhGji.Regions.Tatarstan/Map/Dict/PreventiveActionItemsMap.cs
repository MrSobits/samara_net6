namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class PreventiveActionItemsMap : BaseGkhDictMap<PreventiveActionItems>
    {
        /// <inheritdoc />
        public PreventiveActionItemsMap()
            : base("Предметы профилактических мероприятий", "GJI_DICT_PREVENTIVE_ACTION_ITEMS")
        {
        }
    }
}