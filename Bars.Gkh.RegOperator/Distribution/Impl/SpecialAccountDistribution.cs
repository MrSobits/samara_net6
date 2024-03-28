namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.GkhCr.Entities;

    using NHibernate.Linq;

    /// <summary>
    /// Средства собственников специального счета
    /// </summary>
    public class SpecialAccountDistribution : TargetSubsidyDistribution, IDistributionAutomatically
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SpecialAccountDistribution(
            IDomainService<RealityObjectPaymentAccount> ropayAccDomain,
            IDomainService<RealityObjectTransfer> transferDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<BasePersonalAccount> personalAccountDomain,
            IDomainService<CalcAccountRealityObject> calcAccountRoDomain,
            IDomainService<ObjectCr> objectCrDomain,
            IDomainService<Room> roomDomain)
            : base(ropayAccDomain,
                transferDomain,
                transferRepo,
                periodRepo,
                detailDomain,
                moneyOperationDomain,
                personalAccountDomain,
                calcAccountRoDomain,
                objectCrDomain,
                roomDomain)
        {
        }

        /// <summary>
        /// Маркер распределяемости объекта множественно
        /// </summary>
        public override bool DistributableAutomatically => true;

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.SpecialAccountDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.SpecialAccountDistribution";

        /// <summary>
        /// Роут клиентского контроллера
        /// </summary>
        public override string Route => "specialaccountdistributiongrid";

        /// <summary>
        /// Вернуть объекты для автораспределения
        /// </summary>
        /// <param name="distributables">Распределяемые объекты</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список</returns>
        public IDataResult ListAutoDistributionObjs(IEnumerable<IDistributable> distributables, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var distributionDict = distributables
                .Select(x => (BankAccountStatement)x)
                .GroupBy(x => x.RecipientAccountNum)
                .ToDictionary(x => x.Key,
                    x => x.SafeSum(y => y.RemainSum == 0 && y.DistributeState == DistributionState.NotDistributed ? y.Sum : y.RemainSum));

            var paNumbers = distributionDict.Keys.ToList();
            var dictResult = this.CalcAccountRoDomain.GetAll()
                .Fetch(x => x.RealityObject)
                .ThenFetch(x => x.Municipality)
                .Where(x => paNumbers.Contains(x.Account.AccountNumber) && x.Account.TypeAccount == TypeCalcAccount.Special)
                .Where(x => !x.Account.DateClose.HasValue)
                .Select(
                    x => new
                    {
                        RealtyAccountId = this.RopayAccDomain.GetAll().Where(y => y.RealityObject.Id == x.RealityObject.Id).Select(y => y.Id)
                            .FirstOrDefault(),
                        AccountNumber = x.Account.AccountNumber,
                        RealityObject = x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name
                    })
                .AsEnumerable()
                .ToDictionary(x => x.AccountNumber);

            var listData = distributionDict.Select(
                    x =>
                    {
                        var account = dictResult.Get(x.Key);
                        return new Proxy
                        {
                            AccountNumber = x.Key,
                            Sum = x.Value,

                            RealityObject = account?.RealityObject,
                            Municipality = account?.Municipality,
                            RealtyAccountId = account?.RealtyAccountId
                        };
                    })
                .AsQueryable()
                .Filter(loadParams, this.Container)
                .Order(loadParams)
                .Paging(loadParams);

            return new ListDataResult(listData.ToList(), distributionDict.Count);
        }

        /// <summary>
        /// Вернуть объект для автораспределения по текущему распределению
        /// </summary>
        /// <param name="distributable">Распределяемая выписка</param>
        /// <returns>Список</returns>
        public IDistributionArgs GetDistributionArgs(IDistributable distributable)
        {
            var bankAccountStatement = distributable as BankAccountStatement;
            if (bankAccountStatement == null)
            {
                throw new InvalidCastException("IDistributable must be convertible to BankAccountStatement");
            }

            var realityAccountId = this.CalcAccountRoDomain.GetAll()
                .Where(x => x.Account.AccountNumber == bankAccountStatement.RecipientAccountNum && !x.Account.DateClose.HasValue)
                .Select(x => x.RealityObject.Id)
                .FirstOrDefault();

            var realityObjectPaymentAccount = this.RopayAccDomain.GetAll().FirstOrDefault(y => y.RealityObject.Id == realityAccountId);
            var data = new[]
            {
                new DistributionByRealtyAccountArgs.ByRealtyRecord(
                    realityObjectPaymentAccount,
                    bankAccountStatement.RemainSum == 0
                    && (bankAccountStatement.DistributeState == DistributionState.NotDistributed
                        || bankAccountStatement.DistributeState == DistributionState.WaitingDistribution)
                        ? bankAccountStatement.Sum
                        : bankAccountStatement.RemainSum)
            };

            return new DistributionByRealtyAccountArgs(distributable, data);
        }

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="suspenseAccount">Распределяемый объект</param>
        /// <returns></returns>
        public override bool CanApply(IDistributable suspenseAccount)
        {
            return suspenseAccount.MoneyDirection == MoneyDirection.Income;
        }

        /// <summary>
        /// Результат выполнения запроса.
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <returns></returns>
        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            throw new NotSupportedException("Данный вид распределения не поддерживает выборочное распределение");
        }

        private class Proxy
        {
            public long? RealtyAccountId { get; set; }

            public string AccountNumber { get; set; }

            public string RealityObject { get; set; }

            public decimal Sum { get; set; }

            public string Municipality { get; set; }
        }
    }
}