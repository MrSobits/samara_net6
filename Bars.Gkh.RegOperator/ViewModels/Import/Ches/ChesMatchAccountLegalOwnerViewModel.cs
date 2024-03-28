namespace Bars.Gkh.RegOperator.ViewModels.Import
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.Utils;

    public class ChesMatchLegalAccountOwnerViewModel : BaseViewModel<ChesMatchLegalAccountOwner>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ChesMatchLegalAccountOwner> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.PersonalAccountNumber,
                    ExternalName = x.Name,
                    ExternalInn = x.Inn,
                    ExternalKpp = x.Kpp,
                    x.AccountOwner.Name,
                    (x.AccountOwner as LegalAccountOwner).Contragent.Inn,
                    (x.AccountOwner as LegalAccountOwner).Contragent.Kpp,
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}