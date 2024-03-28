namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.GkhCr.Entities;

    public class BankPercentDistribution : AbstractRealtyAccountDistribution
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BankPercentDistribution(
            IDomainService<RealityObjectPaymentAccount> ropayAccDomain,
            IDomainService<RealityObjectTransfer> transferDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<CalcAccountRealityObject> calcAccountRoDomain,
            IDomainService<ObjectCr> objectCrDomain,
            IDomainService<Room> roomDomain)
            : base(ropayAccDomain, transferDomain, transferRepo, detailDomain, moneyOperationDomain, calcAccountRoDomain, objectCrDomain, roomDomain)
        {
        }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.BankPercentDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.BankPercent";

        /// <summary>
        /// Селектор кошелька
        /// </summary>
        protected override Wallet GetWallet(RealityObjectPaymentAccount realityObjectPaymentAccount)
        {
            return realityObjectPaymentAccount.BankPercentWallet;
        }

        /// <summary>
        /// Получить счета оплат домов, связанных с трансферами
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override IEnumerable<RealityObjectPaymentAccount> GetPaymentAccounts(IQueryable<Transfer> query)
        {
            return this.RopayAccDomain.GetAll()
                .Where(y => query.Any(x => x.Owner.Id == y.Id))
                .ToArray();
        }

        /// <inheritdoc />
        public override IDataResult GetOriginatorName(BaseParams baseParams)
        {
            var distributable = DistributionProviderImpl.GetDistributables(baseParams).FirstOrDefault() as BankAccountStatement;
            RealityObject originator = null;

            if (distributable.IsNotNull())
            {
                var realityObjectFilter = distributable.PaymentDetails?.Split(";").FirstOrDefault();
                originator = this.RealityObjectDomain.GetAll().FirstOrDefault(x => x.FiasAddress.AddressName == realityObjectFilter);
            }

            return new BaseDataResult(originator);
        }
    }
}