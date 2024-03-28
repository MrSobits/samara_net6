namespace Bars.Gkh.RegOperator.Modules.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class DocumentClwAccountDetailViewModel : BaseViewModel<DocumentClwAccountDetail>
    {
        public override IDataResult List(IDomainService<DocumentClwAccountDetail> domainService, BaseParams baseParams)
        {
            var docClwId = baseParams.Params.GetAsId();

            return domainService.GetAll()
                .Where(x => x.Document.Id == docClwId)
                .Select(x => new
                {
                    x.Id,
                    x.PersonalAccount.Room.RoomNum,
                    x.PersonalAccount.Room.ChamberNum,
                    x.PersonalAccount.Room.RealityObject.Address,
                    x.PersonalAccount.PersonalAccountNum,
                    x.DebtBaseTariffSum,
                    x.DebtDecisionTariffSum,
                    x.DebtSum,
                    x.PenaltyDebtSum,
                    x.PenaltyCalcFormula
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    Address = !string.IsNullOrEmpty(x.ChamberNum)
                        ? $"{x.Address}, кв. {x.RoomNum}, ком. {x.ChamberNum}"
                        : $"{x.Address}, кв. {x.RoomNum}",
                    AccountNumber = x.PersonalAccountNum,
                    x.DebtBaseTariffSum,
                    x.DebtDecisionTariffSum,
                    x.DebtSum,
                    x.PenaltyDebtSum,
                    x.PenaltyCalcFormula
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}