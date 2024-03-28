namespace Bars.Gkh.RegOperator.Regions.Tyumen.ViewModel
{
    using Bars.B4;
    using Entities;
    using Bars.Gkh.Utils;
    using System.Linq;

    public class RequestStateViewModel : BaseViewModel<RequestState>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<RequestState> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.File,
                    RealityObject = x.RealityObject.Address,
                    x.UserName,
                    x.ObjectCreateDate
                }).OrderByDescending(x=> x.Id)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}