namespace Bars.GkhCr.Regions.Tatarstan.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhCr.Regions.Tatarstan.DomainService;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class ObjectOutdoorCrDataExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var objectOutdoorCrDomain = this.Container.ResolveDomain<ObjectOutdoorCr>();
            var objectOutdoorCrService = this.Container.Resolve<IObjectOutdoorCrService>();
            using (this.Container.Using(objectOutdoorCrDomain, objectOutdoorCrService))
            {
                baseParams.Params.SetValue("limit", 0);
                baseParams.Params.SetValue("headers",
                    "Статус,Муниципальный район,Программа,Наименование двора,Код двора,Принят ГЖИ");
                baseParams.Params.SetValue("dataIndexes",
                    "State,MunicipalityName,OutdoorProgramName,RealityObjectOutdoorName,RealityObjectOutdoorCode,DateAcceptGji");

                return (IList)objectOutdoorCrService.GetList(objectOutdoorCrDomain, baseParams).Data;
            }
        }
    }
}
