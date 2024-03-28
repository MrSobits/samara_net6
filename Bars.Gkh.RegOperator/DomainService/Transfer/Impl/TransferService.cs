namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities.ValueObjects;
    using Castle.Windsor;
    using Domain.Repository.RealityObjectAccount;
    using Entities;
    using Gkh.Domain;
    using NHibernate.Linq;

    public class TransferService : ITransferService
    {
        private readonly IWindsorContainer _container;
        private readonly IRealtyObjectMoneyRepository _moneyRepo;

        public TransferService(IWindsorContainer container,
            IRealtyObjectMoneyRepository moneyRepo)
        {
            _container = container;
            _moneyRepo = moneyRepo;
        }

        public IDataResult ListTransferForPaymentAccount(BaseParams baseParams)
        {
            var transferDomain = _container.ResolveDomain<Transfer>();

            var roPaymentAccDomain = _container.ResolveDomain<RealityObjectPaymentAccount>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

               var paymentAccountId = loadParams.Filter.GetAsId("paymentAccountId");
                var isCredit = loadParams.Filter.GetAs<bool>("isCredit");

                var roPaymentAccounts =
                    roPaymentAccDomain.GetAll()
                        .Where(x => x.Id == paymentAccountId);

                var data =
                    (isCredit
                        ? _moneyRepo.GetCreditTransfers(roPaymentAccounts)
                        : _moneyRepo.GetDebtTransfers(roPaymentAccounts));

                var result = data
                    .Filter(loadParams, _container);

                return new ListDataResult(result.Order(loadParams).Paging(loadParams), result.Count());
            }
            finally
            {
                _container.Release(transferDomain);
                _container.Release(roPaymentAccDomain);
            }
        }

        public IDataResult ListTransferForSubsidyAccount(BaseParams baseParams)
        {
            var roSubsidyAccDomain = _container.ResolveDomain<RealityObjectSubsidyAccount>();
            var roPaymentAccDomain = _container.ResolveDomain<RealityObjectPaymentAccount>();
            var roMoneyRepo = _container.Resolve<IRealtyObjectMoneyRepository>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var subsidyAccountId = loadParams.Filter.GetAsId("accId");

                var paymentAcc = roPaymentAccDomain.GetAll()
                    .FirstOrDefault(x => roSubsidyAccDomain.GetAll()
                        .Where(y => y.Id == subsidyAccountId)
                        .Any(y => y.RealityObject.Id == x.RealityObject.Id));

                if (paymentAcc == null)
                {
                    return null;
                }

                var result =
                    roMoneyRepo
                        .GetSubsidyTransfers(paymentAcc)
                        .Filter(loadParams, _container);

                return new ListDataResult(result.Order(loadParams).Paging(loadParams), result.Count());
            }
            finally
            {
                _container.Release(roPaymentAccDomain);
                _container.Release(roSubsidyAccDomain);
                _container.Release(roMoneyRepo);
            }
        }
    }
}