namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    public class MKDLicRequestHeadInspectorMap : BaseEntityMap<MKDLicRequestHeadInspector>
    {
        public MKDLicRequestHeadInspectorMap()
            : base("Руководитель заявки", "GJI_MKD_LIC_REQUEST_HEADINSP")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.MKDLicRequest, "Заявка").Column("MKD_LIC_REQUEST_ID").NotNull();
            this.Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
        }
    }
}