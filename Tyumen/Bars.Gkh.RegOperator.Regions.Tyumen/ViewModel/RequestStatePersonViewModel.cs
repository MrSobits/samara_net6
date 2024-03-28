namespace Bars.Gkh.RegOperator.Regions.Tyumen.ViewModel
{
    using Bars.B4;
    using Entities;
    using Bars.Gkh.Utils;
    using System.Linq;

    public class RequestStatePersonViewModel : BaseViewModel<RequestStatePerson>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<RequestStatePerson> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Email,
                    x.Name,
                    x.Description,
                    x.Position,
                    x.Status
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}