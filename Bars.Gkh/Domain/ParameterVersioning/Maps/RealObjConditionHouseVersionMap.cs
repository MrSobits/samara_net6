namespace Bars.Gkh.Domain.ParameterVersioning.Maps
{
    using ParameterVersioning;
    using Entities;

    public class RealObjConditionHouseVersionMap : VersionedEntity<RealityObject>
    {
        public RealObjConditionHouseVersionMap()
            : base("real_obj_condition_house")
        {
            Map(x => x.ConditionHouse, null, "Состояние дома");
        }
    }
}
