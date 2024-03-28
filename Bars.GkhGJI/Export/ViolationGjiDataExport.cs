namespace Bars.GkhGji.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Utils;

    public class ViolationGjiDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var featDict = Container.Resolve<IDomainService<ViolationFeatureGji>>().GetAll()
                .Select(x => new
                {
                    x.ViolationGji.Id,
                    Name = x.FeatureViolGji.FullName ?? x.FeatureViolGji.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

            var actRemViolDict = Container.Resolve<IDomainService<ViolationActionsRemovGji>>()
                    .GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        x.ActionsRemovViol.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

            return Container.Resolve<IDomainService<ViolationGji>>()
                .GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.PpRf25,
                    x.PpRf307,
                    x.PpRf491,
                    x.CodePin,
                    x.PpRf170,
                    x.OtherNormativeDocs,
                    x.NormativeDocNames
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    Name = x.Name ?? string.Empty,
                    PpRf25 = x.PpRf25 ?? string.Empty,
                    PpRf307 = x.PpRf307 ?? string.Empty,
                    PpRf491 = x.PpRf491 ?? string.Empty,
                    CodePin = x.CodePin ?? string.Empty,
                    FeatViol = featDict.Get(x.Id) ?? string.Empty,
                    ActRemViol = actRemViolDict.Get(x.Id) ?? string.Empty,
                    NormDocNum = x.NormativeDocNames ?? string.Empty
                })
                .AsQueryable()
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}