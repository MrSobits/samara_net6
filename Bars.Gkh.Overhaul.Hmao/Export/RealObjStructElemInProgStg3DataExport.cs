namespace Bars.Gkh.Overhaul.Hmao.Export
{
    using System.Collections;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class RealObjStructElemInProgStg3DataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>().GetAll()
                .Select(x =>
                    new
                    {
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        x.CommonEstateObjects,
                        x.Year,
                        x.IndexNumber,
                        x.Point,
                        x.Sum
                    })
                .Filter(loadParam, Container)
                .OrderIf(loadParam.Order.Length == 0, true, x => x.IndexNumber)
                .Order(loadParam)
                .ToList();
        }
    }
}