namespace Bars.GkhGji.ViewModel
{
    using Bars.B4;
    using Entities;
    using System.Linq;

    public class OrganMvdViewModel : BaseViewModel<OrganMvd>
    {
        public override IDataResult List(IDomainService<OrganMvd> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domain.GetAll().Select(
                x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                });

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}