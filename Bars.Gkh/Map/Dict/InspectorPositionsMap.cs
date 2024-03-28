namespace Bars.Gkh.Map.Dict
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Map;

    /// <summary>
    /// Маппинг полей сущности <see cref="InspectorPositions"/>
    /// </summary>
    public class InspectorPositionsMap : BaseGkhDictMap<InspectorPositions>
    {
        public InspectorPositionsMap()
            : base("Должности инспекторов", "GJI_DICT_INSPECTOR_POSITIONS")
        {
        }
    }
}