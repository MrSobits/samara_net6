namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    public class ConstructiveElementDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<ConstructiveElement>>().GetAll()
                .Select(x => new
                    {
                        x.Id,
                        GroupName = x.Group.Name,
                        x.Name,
                        x.Code,
                        x.Lifetime,
                        NormativeDocName = x.NormativeDoc.Name,
                        UnitMeasure = x.UnitMeasure.Name,
                        x.RepairCost
                    })
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}