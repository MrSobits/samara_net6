namespace Bars.Gkh.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using System.Linq;

    public class AddressMatchViewModel : BaseViewModel<AddressMatch>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<AddressMatch> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.ExternalAddress,
                    RealityObject = x.RealityObject.Address
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}