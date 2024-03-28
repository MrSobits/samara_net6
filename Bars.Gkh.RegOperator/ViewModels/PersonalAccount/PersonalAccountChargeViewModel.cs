namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Представление для <see cref="PersonalAccountCharge"/>
    /// </summary>
    public class PersonalAccountChargeViewModel: BaseViewModel<PersonalAccountCharge>
    {
        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Интерфейс для получения данных банка
        /// </summary>
        public IBankAccountDataProvider BankAccountDataProvider { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<PersonalAccountCharge> domainService, BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAs<long>("accountId", ignoreCase: true);
            var packetId = baseParams.Params.GetAs<long>("packetId", ignoreCase: true);
            var loadParams = baseParams.GetLoadParam();

            var currentPeriod = this.ChargePeriodRepository.GetCurrentPeriod();

            if (packetId == 0)
            {
                packetId = loadParams.Filter.GetAsId("packetId");
            }

            var data = domainService.GetAll()
                .WhereIf(packetId > 0, x => x.Packet.Id == packetId && x.ChargePeriod.Id == currentPeriod.Id)
                .WhereIf(accountId > 0, x => x.BasePersonalAccount.Id == accountId)
                .Select(x => new
                {
                    x.Id,
                    x.ChargeDate,
                    PeriodId = x.ChargePeriod.Id,
                    x.Guid,
                    x.Charge,
                    x.ChargeTariff,
                    ChargeBaseTariff = x.ChargeTariff - x.OverPlus,
                    x.OverPlus,
                    x.Penalty,
                    x.RecalcByBaseTariff,
                    x.RecalcByDecisionTariff,
                    x.RecalcPenalty,

                    AccountNum = x.BasePersonalAccount.PersonalAccountNum,
                    AccountState = x.BasePersonalAccount.State,
                    AccountId = x.BasePersonalAccount.Id,
                    x.BasePersonalAccount.Room.RealityObject
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();
            var resultData = data.Order(loadParams).Paging(loadParams).ToList();

            var bankAccounts = this.BankAccountDataProvider.GetBankNumbersForCollection(resultData.Select(x => x.RealityObject).Distinct().ToArray());
            var resultList = resultData
                .Select(x => new
                {
                    x.Id,
                    x.ChargeDate,
                    x.AccountState,
                    x.PeriodId,
                    x.Guid,
                    x.Charge,
                    x.ChargeTariff,
                    x.ChargeBaseTariff,
                    x.OverPlus,
                    x.Penalty,
                    x.RecalcByBaseTariff,
                    x.RecalcByDecisionTariff,
                    x.RecalcPenalty,
                    x.AccountNum,
                    x.AccountId,
                    ContragentAccountNumber = bankAccounts.Get(x.RealityObject.Id),
                    Description = string.Empty,
                    x.RealityObject.AccountFormationVariant
                })
                .ToList();

            return new ListSummaryResult(resultList, totalCount, new
            {
                Charge = resultList.SafeSum(x => x.Charge),
                ChargeTariff = resultList.SafeSum(x => x.ChargeTariff),
                ChargeBaseTariff = resultList.SafeSum(x => x.ChargeBaseTariff),
                Penalty = resultList.SafeSum(x => x.Penalty),
                OverPlus = resultList.SafeSum(x => x.OverPlus),
                RecalcByBaseTariff = resultList.SafeSum(x => x.RecalcByBaseTariff),
                RecalcByDecisionTariff = resultList.SafeSum(x => x.RecalcByDecisionTariff),
                RecalcPenalty = resultList.SafeSum(x => x.RecalcPenalty)
            });
        }
    }
}
