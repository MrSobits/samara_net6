namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
    using System;
    using System.Linq;
    using B4;

    using Bars.Gkh.Repositories.ChargePeriod;

    using Entities;
    using Gkh.Entities;

    public class RealityObjectChargeAccountRepository : IRealityObjectChargeAccountRepository
    {
        private readonly IDomainService<RealityObjectChargeAccountOperation> _operationDomain;
        private readonly IChargePeriodRepository _chargePeriodRepo;

        public RealityObjectChargeAccountRepository(
            IDomainService<RealityObjectChargeAccountOperation> operationDomain,
            IChargePeriodRepository chargePeriodRepo)
        {
            _operationDomain = operationDomain;
            _chargePeriodRepo = chargePeriodRepo;
        }

        public RealityObjectChargeAccountOperation GetOperationInOpenedPeriod(RealityObject realityObject)
        {
            var period = _chargePeriodRepo.GetCurrentPeriod();

            if (period == null)
                throw new InvalidOperationException("Невозможно получить начисление по дому, т.к. в системе нет периодов");

            return _operationDomain.GetAll()
                .Where(x => x.Account.RealityObject.Id == realityObject.Id)
                .FirstOrDefault(x => x.Date >= period.StartDate && (!period.EndDate.HasValue || period.EndDate >= x.Date));
        }
    }
}