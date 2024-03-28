namespace Bars.Gkh.RegOperator.Domain.Repository.PersonalAccounts
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Entities;
    using Entities.ValueObjects;
    using NHibernate.Linq;

    public class PersonalAccountRepository : IPersonalAccountRepository
    {
        private readonly IDomainService<BasePersonalAccount> _personalAccDomain;
        private readonly IChargePeriodRepository _chargePeriodRepo;

        public PersonalAccountRepository(
            IDomainService<BasePersonalAccount> personalAccDomain,
            IChargePeriodRepository chargePeriodRepo)
        {
            _personalAccDomain = personalAccDomain;
            _chargePeriodRepo = chargePeriodRepo;
        }

        public IQueryable<BasePersonalAccount> GetParticipatedAsSourceIn(MoneyOperation operation)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<BasePersonalAccount> GetParticipatedAsTargetIn(MoneyOperation operation)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Получить ЛС, для обновления
        /// </summary>
        /// <param name="accountsIds">Фильтр по идентификаторам ЛС</param>
        /// <returns></returns>
        public IQueryable<BasePersonalAccount> GetForBalanceUpdateInOpenedPeriod(IEnumerable<long> accountsIds)
        {
            var period = _chargePeriodRepo.GetCurrentPeriod();

            var accounts = _personalAccDomain.GetAll()
                .Where(x => accountsIds.Contains(x.Id))
                .Where(x =>
                    x.AccumulatedFundWallet.InTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.AccumulatedFundWallet.OutTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.BaseTariffWallet.InTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.BaseTariffWallet.OutTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.DecisionTariffWallet.InTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.DecisionTariffWallet.OutTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.PenaltyWallet.InTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.PenaltyWallet.OutTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.PreviosWorkPaymentWallet.InTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.PreviosWorkPaymentWallet.OutTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.RentWallet.InTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.RentWallet.OutTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.SocialSupportWallet.InTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.SocialSupportWallet.OutTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.RestructAmicableAgreementWallet.InTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                    || x.RestructAmicableAgreementWallet.OutTransfers.Any(t => t.OperationDate.Date >= period.StartDate)
                )
                .FetchMany(x => x.Summaries)
                .ThenFetch(x => x.Period)
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .FetchAllWallets();

            return accounts;
        }

        public IQueryable<BasePersonalAccount> GetByIds(long[] ids)
        {
            ArgumentChecker.NotNull(ids, "ids");
            return _personalAccDomain.GetAll().Where(x => ids.Contains(x.Id));
        }
    }
}
