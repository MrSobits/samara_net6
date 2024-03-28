namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.Loan
{
    using B4;

    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using Domain;
    using Domain.Repository;
    using Entities;
    using Entities.ValueObjects;
    using Gkh.Domain;

    public class RealityObjectLoanReserver : IRealityObjectLoanReserver
    {
        private readonly IWindsorContainer _container;
        private readonly IDatabaseMutexManager _mutexManager;
        private readonly IDomainService<Transfer> _transferDomain;
        private readonly IChargePeriodRepository _periodRepo;
        private readonly IRealtyObjectPaymentSession _session;

        public RealityObjectLoanReserver(
            IWindsorContainer container,
            IDatabaseMutexManager mutexManager,
            IDomainService<Transfer> transferDomain,
            IChargePeriodRepository periodRepo,
            IRealtyObjectPaymentSession session)
        {
            _container = container;
            _mutexManager = mutexManager;
            _transferDomain = transferDomain;
            _periodRepo = periodRepo;
            _session = session;
        }

        public void TakePreviouslyReservedLoan(RealityObjectLoan loan)
        {
            _container.InTransaction(() =>
            {
                using (new DatabaseMutexContext(loan.Source))
                {
                    loan.TakeLoan(_periodRepo.GetCurrentPeriod()).ForEach(_transferDomain.Save);

                    _session.Complete();
                }

                /*IDatabaseLockedMutexHandle handle = null;
                try
                {
                    if (!_mutexManager.TryLock(
                        string.Format("RealityObjectLoan_{0}", loan.TransferGuid),
                        "Проводится операция займа",
                        out handle))
                    {
                        throw new ValidationException("Операция займа уже производится");
                    }

                    loan.TakeLoan(_periodRepo.GetCurrentPeriod()).ForEach(_transferDomain.Save);

                    _session.Complete();
                }
                finally
                {
                    if (handle != null)
                    {
                        handle.Dispose();
                    }
                }*/
            });
        }
    }
}