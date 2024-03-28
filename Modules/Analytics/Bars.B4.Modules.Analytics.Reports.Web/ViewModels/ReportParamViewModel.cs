namespace Bars.B4.Modules.Analytics.Reports.Web.ViewModels
{
    using System.Linq;
    using Analytics.Enums;
    using Entities;
    using Params;
    using B4.Utils;
    using NHibernate.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class ReportParamGkhViewModel : BaseViewModel<ReportParamGkh>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override IDataResult List(IDomainService<ReportParamGkh> domainService, BaseParams baseParams)
        {
            var reportDomain = Container.Resolve<IDomainService<StoredReport>>();
            try
            {
                var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);

                var loadParam = GetLoadParam(baseParams);

                //Почему-то при явном селекте not-null свойста Multiselect падает с ошибкой, если в базе ни true, ни false, а пусто (фишка postgres'a).
                //При простом GetAll получает данные без ошибок.
                var data = domainService.GetAll()
                    .Fetch(x => x.StoredReport)
                    .ThenFetch(x => x.Category)
                    .WhereIf(reportId > 0, param => param.StoredReport.Id == reportId)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.ParamType,
                        x.Label,
                        x.Name,
                        x.Required,
                        x.Multiselect,
                        Id = x.Id.ToString(),
                        x.Additional,
                        x.OwnerType,
                        x.SqlQuery
                    })
                    .AsQueryable();

                if (reportId > 0)
                {
                    var report = reportDomain.GetAll().FirstOrDefault(x => x.Id == reportId);
                    if (report != null && report.DataSources != null)
                    {
                        var dsParams = report.DataSources
                            .SelectMany(x => x.Params, (ds, dsParam) => new
                            {
                                dsParam.ParamType,
                                dsParam.Label,
                                dsParam.Name,
                                dsParam.Required,
                                dsParam.Multiselect,
                                Id = ds.Id + "_" + dsParam.Name,
                                dsParam.Additional,
                                dsParam.OwnerType,
                                dsParam.SqlQuery
                            })
                            .AsQueryable();
                        data = dsParams.Union(data);
                    }
                }
                var pageData = data.Order(loadParam).Paging(loadParam).ToArray();
                var result = pageData
                    .Select(x => new
                    {
                        x.ParamType,
                        x.Label,
                        x.Name,
                        x.Required,
                        x.Multiselect,
                        x.Id,
                        x.Additional,
                        x.OwnerType,
                        Catalog = x.ParamType == ParamType.Catalog ? CatalogRegistry.Get(x.Additional) : null,
                        Enum = x.ParamType == ParamType.Enum ? EnumRegistry.Get(x.Additional) : null,
                        x.SqlQuery
                    });

                return new ListDataResult(result, data.Count());
            }
            finally
            {
                Container.Release(reportDomain);
            }

        }
    }
}