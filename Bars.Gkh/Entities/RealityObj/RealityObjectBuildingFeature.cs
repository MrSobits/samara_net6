namespace Bars.Gkh.Entities.RealityObj
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    public class RealityObjectBuildingFeature: BaseImportableEntity
    {
        public virtual RealityObject RealityObject { get; set; }

        public virtual BuildingFeature BuildingFeature { get; set; }
    }
}
