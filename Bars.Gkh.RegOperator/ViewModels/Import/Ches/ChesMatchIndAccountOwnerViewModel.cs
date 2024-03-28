namespace Bars.Gkh.RegOperator.ViewModels.Import
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.Utils;

    public class ChesMatchIndAccountOwnerViewModel : BaseViewModel<ChesMatchIndAccountOwner>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ChesMatchIndAccountOwner> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ExternalName = x.Name,
                    ExternalAccountNumber = x.PersonalAccountNumber,
                    x.AccountOwner.Name
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}