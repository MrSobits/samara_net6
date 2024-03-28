namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Linq;
    using B4.Modules.NH.Extentions;
    using NHibernate.Util;
    using B4;

    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using Domain.Repository;
    using Entities;
    using Entities.ValueObjects;

    /// <summary>
    /// Сервис для осуществления займов
    /// </summary>
    public class TakeLoanService : ITakeLoanService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private readonly IChargePeriodRepository periodRepo;
        private readonly IDomainService<Transfer> transferRepo;
        private readonly IDomainService<MoneyOperation> operationRepo;

        public TakeLoanService(
            IChargePeriodRepository periodRepo,
            IDomainService<Transfer> transferRepo,
            IDomainService<MoneyOperation> operatonRepo)
        {
            this.periodRepo = periodRepo;
            this.transferRepo = transferRepo;
            this.operationRepo = operatonRepo;
        }

        /// <summary>
        /// Утверждение займа
        /// </summary>
        /// <param name="loan"></param>
        public void TakeLoan(RealityObjectLoan loan)
        {
            var transfers = loan.TakeLoan(this.periodRepo.GetCurrentPeriod());
            var operations = transfers.Select(x => x.Operation);

            this.Container.InTransaction(() =>
            {
                operations.ForEach(this.operationRepo.Save);
                transfers.ForEach(this.transferRepo.Save);
            });
        }
    }
}