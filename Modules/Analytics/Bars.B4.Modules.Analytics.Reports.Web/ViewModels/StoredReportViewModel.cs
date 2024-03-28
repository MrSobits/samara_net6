namespace Bars.B4.Modules.Analytics.Reports.Web.ViewModels
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;

    using NHibernate.Linq;

    public class StoredReportViewModel : BaseViewModel<StoredReport>
    {
        internal static string Salt = "CBBCC8C8-B911-4D4C-8919-116D083041BA"; // Не менять!!!

        public override IDataResult List(IDomainService<StoredReport> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var rolePermDomain = this.Container.ResolveDomain<RolePermission>();

            try
            {
                var data = domainService.GetAll().Fetch(x => x.DataSources).Filter(loadParam, this.Container);

                var reportIds = domainService.GetAll().Select(x => x.Id.ToString()).ToArray();
                var rolePermsByRepordDict = rolePermDomain.GetAll()
                    .Where(x => reportIds.Contains(x.PermissionId))
                    .Select(
                        x => new
                        {
                            PermissionId = x.PermissionId.ToLong(),
                            x.Role.Name
                        })
                    .ToList()
                    .GroupBy(x => x.PermissionId)
                    .ToDictionary(x => x.Key, x => x.AggregateWithSeparator(z => z.Name, ", "));

                var pageData = data
                    .AsEnumerable()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Name,
                            DataSourcesNames =
                                x.DataSources.AggregateWithSeparator(z => z.Name, ", "),
                            Roles = x.ForAll
                                ? "Для всех ролей"
                                : rolePermsByRepordDict.ContainsKey(x.Id)
                                    ? rolePermsByRepordDict[x.Id]
                                    : string.Empty,
                            HasParams = x.GetParams().Any(),
                            Token = MD5.GetHashString64(x.Id + StoredReportViewModel.Salt)
                        })
                    .AsQueryable()
                    .Order(loadParam)
                    .Paging(loadParam)
                    .ToList();

                return new ListDataResult(pageData, data.Count());
            }
            finally
            {
                this.Container.Release(rolePermDomain);
            }
        }
    }
}