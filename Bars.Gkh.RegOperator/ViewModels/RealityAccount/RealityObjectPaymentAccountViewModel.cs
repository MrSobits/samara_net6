namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Enums;

    using Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Domain.Repository.RealityObjectAccount;
    using DomainService.RealityObjectAccount;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using GkhCr.Entities;

    /// <summary>
    /// Представление <see cref="RealityObjectPaymentAccount"/>
    /// </summary>
    public class RealityObjectPaymentAccountViewModel : BaseViewModel<RealityObjectPaymentAccount>
    {
        /// <summary>
        /// Репозиторий денежных средств дома
        /// </summary>
        public IRealtyObjectMoneyRepository MoneyRepo { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ObjectCr"/>
        /// </summary>
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Room"/>
        /// </summary>
        public IDomainService<Room> RoomDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectLoan"/>
        /// </summary>
        public IDomainService<RealityObjectLoan> RealityObjectLoanSummLoan { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="CalcAccountRealityObject"/>
        /// </summary>
        public IDomainService<CalcAccountRealityObject> CalcAccountRoDomain { get; set; }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<RealityObjectPaymentAccount> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var programCrId = baseParams.Params.GetAs<long>("programCr");
            var crFundTypes = baseParams.Params.GetAs<int[]>("crFundTypes");
            var bankAccNum = baseParams.Params.GetAs<bool>("bankAccNum");

            long[] roIds = null;
            var roIdsStr = baseParams.Params.GetAs<string>("roIds");
            if (roIdsStr != null && !roIdsStr.Contains("All"))
            {
                roIds = roIdsStr.ToLongArray();
            }

            var roIdsByProgramCr = this.ObjectCrDomain.GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
                .Select(x => x.RealityObject.Id)
                .ToArray();

            var data = domainService.GetAll()
                .WhereIf(programCrId != 0, x => roIdsByProgramCr.Contains(x.RealityObject.Id))
                .WhereIf(crFundTypes.IsNotEmpty(), x => crFundTypes.Contains((int)x.RealityObject.AccountFormationVariant.Value))
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    AccountNum = x.AccountNumber,
                    RealityId = x.RealityObject.Id,
                    CrFundType = x.RealityObject.AccountFormationVariant.HasValue
                        ? x.RealityObject.AccountFormationVariant.Value
                        : CrFundFormationType.NotSelected,
                    BankAccountNumber = this.CalcAccountRoDomain.GetAll()
                            .Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special && ((SpecialCalcAccount)y.Account).IsActive) || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                            .Where(y => y.RealityObject.Id == x.RealityObject.Id)
                            .Where(y => y.Account.DateOpen <= DateTime.Today)
                            .Where(y => !y.Account.DateClose.HasValue || y.Account.DateClose.Value >= DateTime.Today)
                            .Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special)
                                        || (y.Account.TypeAccount == TypeCalcAccount.Regoperator && y.DateStart <= DateTime.Today && (!y.DateEnd.HasValue || y.DateEnd >= DateTime.Today)))
                            .OrderByDescending(y => y.Account.DateOpen)
                            .Select(y => y.Account.AccountNumber ?? ((RegopCalcAccount)y.Account).ContragentCreditOrg.SettlementAccount)
                            .FirstOrDefault()
                })
                .WhereIf(roIds.IsNotEmpty(), x => roIds.Contains(x.RealityId))
                .WhereIf(roIds.IsEmpty() && bankAccNum, x => x.BankAccountNumber != null && x.BankAccountNumber != string.Empty)
                .Filter(loadParams, this.Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObject);

            var xdata = data.Order(loadParams).Paging(loadParams).ToList();

            // Дополнительный запрос для выборки площадей (NH не позволил добавить агрегат)
            var realityIds = (from v in xdata select v.RealityId).ToArray();

            var realAreaMap = (
                from v in this.RoomDomain.GetAll()
                where realityIds.Contains(v.RealityObject.Id)
                group v by v.RealityObject.Id
                into z
                select new { RealityObjectId = z.Key, RealArea = z.Sum(v => v.Area) }
                ).ToList();

            var result = from v in xdata
                         select new
                         {
                             v.Id,
                             v.Municipality,
                             v.RealityObject,
                             v.AccountNum,
                             v.RealityId,
                             v.BankAccountNumber,
                             v.CrFundType,
                             RealArea = (
                                 from x in realAreaMap
                                 where x.RealityObjectId == v.RealityId
                                 select x.RealArea
                                 ).FirstOrDefault()
                         };

            return new ListDataResult(result, data.Count());
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<RealityObjectPaymentAccount> domainService, BaseParams baseParams)
        {
            var bankProvider = this.Container.Resolve<IBankAccountDataProvider>();
            var realityObjectLoanSummLoan = this.Container.ResolveDomain<RealityObjectLoan>();
            var roTransferDomain = this.Container.ResolveDomain<RealityObjectTransfer>();

            try
            {
                var roId = baseParams.Params.GetAsId("Id");

                var entity = domainService.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId);

                if (entity == null)
                {
                    return new BaseDataResult();
                }

                var regopAccount = bankProvider.GetBankAccountInfo(entity.RealityObject.Id);

                var debt = this.MoneyRepo.GetDebtTransfers(new[] { entity }.AsQueryable()).Select(x => x.Amount).ToArray().SafeSum();
                var credit = this.MoneyRepo.GetCreditTransfers(new[] { entity }.AsQueryable()).Select(x => x.Amount).ToArray().SafeSum();
                var loan = realityObjectLoanSummLoan.GetAll()
                    .Where(x => !x.State.StartState) // учитываем только фактически взятые займы
                    .Where(x => x.LoanTaker.RealityObject.Id == roId)
                    .Sum(x => (decimal?)(x.LoanSum - x.LoanReturnedSum));

                return new BaseDataResult(new
                {
                    entity.Id,
                    DateClose = regopAccount.Return(x => x.CloseDate),
                    DateOpen = regopAccount.Return(x => x.OpenDate),
                    BankAccountNum = regopAccount.Return(x => x.BankAccountNum),
                    AccountNum = entity.AccountNumber,
                    LastOperationDate = roTransferDomain.GetAll().Where(x => x.Owner.Id == entity.Id).SafeMax(x => x.PaymentDate),
                    DebtTotal = debt,
                    CreditTotal = credit,
                    entity.MoneyLocked,
                    CurrentBalance = debt - entity.MoneyLocked - credit,
                    OverdraftLimit = 0m,
                    Loan = loan.GetValueOrDefault()
                });
            }
            finally
            {
                this.Container.Release(bankProvider);
                this.Container.Release(roTransferDomain);
                this.Container.Release(realityObjectLoanSummLoan);
            }
        }
    }
}