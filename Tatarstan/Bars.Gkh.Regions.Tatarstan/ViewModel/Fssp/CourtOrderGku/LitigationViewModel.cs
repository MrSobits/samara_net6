namespace Bars.Gkh.Regions.Tatarstan.ViewModel.Fssp.CourtOrderGku
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Вьюмодель для Делопроизводств
    /// </summary>
    public class LitigationViewModel : BaseViewModel<Litigation>
    {
        public override IDataResult List(IDomainService<Litigation> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var showAll = baseParams.Params.GetAs<bool>("showAll");
            
            var inactiveLitigationStateList = new List<string>
            {
                "окончено",
                "прекращено",
                "уничтожено",
                "отказан"
            };
            
            return domainService.GetAll()
                .WhereIf(!showAll, x => !inactiveLitigationStateList.Contains(x.State.ToLower()))
                .Select(x => new
                {
                    x.Id,
                    x.JurInstitution,
                    x.State,
                    x.IndEntrRegistrationNumber,
                    x.Debtor,
                    DebtorAddress = x.DebtorFsspAddress.PgmuAddress == null
                        ? x.DebtorFsspAddress.Address ?? ""
                        : x.DebtorFsspAddress.PgmuAddress.District + ", " +
                        x.DebtorFsspAddress.PgmuAddress.Town + ", " + 
                        x.DebtorFsspAddress.PgmuAddress.Street + ", " +
                        x.DebtorFsspAddress.PgmuAddress.House + ", " +
                        x.DebtorFsspAddress.PgmuAddress.Building + ", " +
                        x.DebtorFsspAddress.PgmuAddress.Apartment + ", " +
                        x.DebtorFsspAddress.PgmuAddress.Room,
                    x.DebtorFsspAddress,
                    IsMatchAddress = x.DebtorFsspAddress.PgmuAddress != null,
                    x.EntrepreneurCreateDate,
                    EntrepreneurDebtSum = x.EntrepreneurDebtSum.ToString()
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}