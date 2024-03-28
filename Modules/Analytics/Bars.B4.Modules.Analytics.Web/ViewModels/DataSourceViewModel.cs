namespace Bars.B4.Modules.Analytics.Web.ViewModels
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Entities;

    public class DataSourceViewModel : BaseViewModel<DataSource>
    {
        public override IDataResult List(IDomainService<DataSource> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var data = domainService.GetAll().
                Select(x => new
                {
                    x.Id,
                    x.Description,
                    DataSourceType = x.OwnerType,
                    x.Name
                }).Filter(loadParam, Container);
            var pageData = data.Order(loadParam).Paging(loadParam).ToArray();


            return new ListDataResult(pageData, data.Count());
        }
    }
}
