namespace Bars.GkhCr.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class OfficialViewModel : BaseViewModel<Official>
    {
        public override IDataResult List(IDomainService<Official> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Operator = x.Operator.User.Name,
                    x.Fio,
                    x.Code
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}