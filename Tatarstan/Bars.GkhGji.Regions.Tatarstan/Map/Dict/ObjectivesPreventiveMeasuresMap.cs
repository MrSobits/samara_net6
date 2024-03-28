namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ObjectivesPreventiveMeasuresMap : BaseGkhDictMap<ObjectivesPreventiveMeasure>
    {
        /// <inheritdoc />
        public ObjectivesPreventiveMeasuresMap()
            : base("Цели профилактических мероприятий", "GJI_DICT_OBJECTIVES_PREVENTIVE_MEASURES")
        {
        }
    }
}