namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public class ViolationGjiViewModel : ViolationGjiViewModel<ViolationGji>
    {
    }

    public class ViolationGjiViewModel<T> : BaseViewModel<T>
        where T : ViolationGji
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var normativeDocId = baseParams.Params.GetAsId("NormativeDocId");
            var isForTatarstanGjiSelect = baseParams.Params.GetAs<bool>("isForTatarstanGjiSelect");
            var violFeatureDomain = this.Container.ResolveDomain<ViolationFeatureGji>();
            var violActionsRemovDomain = this.Container.ResolveDomain<ViolationActionsRemovGji>();
            var violNormativeDocItemDomain = this.Container.ResolveDomain<ViolationNormativeDocItemGji>();

            using (this.Container.Using(violFeatureDomain, violActionsRemovDomain, violNormativeDocItemDomain))
            {
                var featDict = violFeatureDomain
                    .GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        Name = x.FeatureViolGji.FullName ?? x.FeatureViolGji.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

                var actRemViolDict = violActionsRemovDomain
                    .GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        x.ActionsRemovViol.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

                IQueryable<long> violationsQuery = null;
                if (normativeDocId != default(long))
                {
                    violationsQuery = violNormativeDocItemDomain.GetAll()
                        .Where(x => x.NormativeDocItem.NormativeDoc.Id == normativeDocId)
                        .Select(x => x.ViolationGji.Id);
                }

                return domainService.GetAll()
                    .WhereIf(violationsQuery != null, x => violationsQuery.Any(v => v == x.Id))
                    .WhereIf(isForTatarstanGjiSelect, x => x.NormativeDocNames.Contains("ст.20.6.1 КоАП РФ"))
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
                        x.NormativeDocNames,
                        x.IsActual
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
                        NormDocNum = x.NormativeDocNames ?? string.Empty,
                        x.IsActual
                    })
                    .ToListDataResult(loadParam);
            }
        }
    }
}