namespace Bars.Gkh.Export
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService.RealityObjectOutdoor;
    using Bars.Gkh.Entities.RealityObj;
    using System.Collections;

    public class RealityObjectOutdoorDataExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var roOutdoorDomain = this.Container.ResolveDomain<RealityObjectOutdoor>();
            var roOutdoorService = this.Container.Resolve<IRealityObjectOutdoorService>();

            using (this.Container.Using(roOutdoorDomain, roOutdoorService))
            {
                baseParams.Params.SetValue("limit", 0);

                baseParams.Params.SetValue("headers",
                    "Муниципальное образование,Населенный пункт,Наименование двора,Код двора," +
                    "Жилые дома двора,Площадь двора (кв. м.),Площадь асфальта (кв. м.),Плановый год ремонта");

                baseParams.Params.SetValue("dataIndexes",
                    "Municipality,Locality,Name,Code,RealityObjects," +
                    "Area,AsphaltArea,RepairPlanYear");

                var result = roOutdoorService.GetList(roOutdoorDomain, baseParams);

                return (IList)result.Data;
            }
        }
    }
}
