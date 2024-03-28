namespace Bars.B4.Modules.Analytics.Web.ViewModels
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Entities;

    public class StoredFilterViewModel : BaseViewModel<StoredFilter>
    {
        public override IDataResult List(IDomainService<StoredFilter> domainService, BaseParams baseParams)
        {
            var providers = Container.ResolveAll<IDataProvider>().ToDictionary(x => x.Key, x => x.Name);
            var loadParam = GetLoadParam(baseParams);

            var data = domainService.GetAll().
                Select(x => new
                {
                    x.Id,
                    x.Description,
                    ProviderName = providers.ContainsKey(x.ProviderKey) ? providers[x.ProviderKey] : string.Empty,
                    x.Name
                }).Filter(loadParam, Container);
            var pageData = data.Order(loadParam).Paging(loadParam).ToArray();


            return new ListDataResult(pageData, data.Count());
        }
    }
}
