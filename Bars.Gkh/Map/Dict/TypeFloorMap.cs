namespace Bars.Gkh.Map.Dict
{
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для сущности <see cref="TypeFloor"/>
    /// </summary>
    public class TypeFloorMap : BaseGkhDictMap<TypeFloor>
    {
        public TypeFloorMap()
            : base("Тип перекрытия", "GKH_DICT_TYPE_FLOOR")
        {

        }
    }
}