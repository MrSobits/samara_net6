namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using B4.IoC;

    using Bars.Gkh.RegOperator.Extenstions;

    using Domain.Repository.RealityObjectAccount;
    using Castle.Windsor;
    using Entities;
    using Entities.ValueObjects;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;

    /// <summary>
    /// Сервис счета жилого дома 
    /// </summary>
    public class RealityObjectAccountService : IRealityObjectAccountService
    {
        private readonly IWindsorContainer container;
        private readonly IDomainService<RealityObjectChargeAccount> chargeAccDomain;
        private readonly IDomainService<RealityObjectPaymentAccount> payAccDomain;
        private readonly IDomainService<RealityObjectSupplierAccount> suppAccDomain;
        private readonly IDomainService<RealityObjectSupplierAccountOperation> suppAccOperDomain;
        private readonly IDomainService<PersonalAccountPayment> pAccPayDomain;
        private readonly IDomainService<RealityObjectChargeAccountOperation> chargeOperationDomain;
        private readonly IBankAccountDataProvider bankProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="chargeAccDomain"></param>
        /// <param name="payAccDomain"></param>
        /// <param name="suppAccDomain"></param>
        /// <param name="suppAccOperDomain"></param>
        /// <param name="pAccPayDomain"></param>
        /// <param name="bankProvider"></param>
        /// <param name="chargeOperationDomain"></param>
        public RealityObjectAccountService(
            IWindsorContainer container,
            IDomainService<RealityObjectChargeAccount> chargeAccDomain,
            IDomainService<RealityObjectPaymentAccount> payAccDomain,
            IDomainService<RealityObjectSupplierAccount> suppAccDomain,
            IDomainService<RealityObjectSupplierAccountOperation> suppAccOperDomain,
            IDomainService<PersonalAccountPayment> pAccPayDomain,
            IBankAccountDataProvider bankProvider,
            IDomainService<RealityObjectChargeAccountOperation> chargeOperationDomain)
        {
            this.container = container;
            this.chargeAccDomain = chargeAccDomain;
            this.payAccDomain = payAccDomain;
            this.suppAccDomain = suppAccDomain;
            this.suppAccOperDomain = suppAccOperDomain;
            this.pAccPayDomain = pAccPayDomain;
            this.bankProvider = bankProvider;
            this.chargeOperationDomain = chargeOperationDomain;
        }

        public IDataResult GetChargeAccountResult(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("Id");
            var entity = this.GetChargeAccount(new RealityObject { Id = roId });
            var paymentAcc = this.GetPaymentAccount(new RealityObject { Id = roId });

            if (entity == null)
            {
                return BaseDataResult.Error("Не найден Счет начислений");
            }

            return new BaseDataResult(
                new
                {
                    Id = entity.Return(x => x.Id),
                    AccountNum = entity.AccountNumber,
                    DateOpen = entity.Return(x => x.ObjectCreateDate),
                    BankAccountNumber = string.Empty,
                    ChargeTotal = entity.Operations.SafeSum(y => y.ChargedTotal),
                    PaidTotal = entity.Operations.SafeSum(y => y.PaidTotal + y.PaidPenalty),
                    LastOperationDate = this.GetMaxOperationDate(paymentAcc)
                });
        }

        public IDataResult GetPaymentAccountResult(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("Id");
            var ro = new RealityObject { Id = roId };

            var entity = this.GetPaymentAccount(ro);

            if (entity == null)
            {
                return BaseDataResult.Error("Не найден Счет оплат");
            }

            var maxPaymentDay = this.pAccPayDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Room.RealityObject.Id == roId)
                .SafeMax(x => x.PaymentDate);

            var overdraft = this.container.Resolve<ICalcAccountOverdraftService>().GetLastOverdraft(ro);

            return new BaseDataResult(
                new
                {
                    entity.Id,
                    entity.DateClose,
                    entity.DateOpen,
                    BankAccountNum = this.bankProvider.GetBankAccountNumber(entity.RealityObject),
                    AccountNum = entity.AccountNumber,
                    LastOperationDate = maxPaymentDay,
                    entity.DebtTotal,
                    entity.CreditTotal,
                    entity.Loan,
                    CurrentBalance = entity.DebtTotal - entity.CreditTotal,
                    OverdraftLimit = overdraft.Return(x => x.AvailableSum)
                });
        }

        public IDataResult GetSupplierAccountResult(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("Id", ignoreCase: true);

            var entity = this.GetSupplierAccount(new RealityObject { Id = roId });

            if (entity == null)
            {
                return BaseDataResult.Error("Не найден Счет расчета с поставщиками");
            }

            var suppOperations = this.suppAccOperDomain.GetAll()
                    .Where(x => x.Account == entity)
                    .Select(
                        x => new
                        {
                            x.Credit,
                            x.Debt
                        })
                    .ToList();

            var saldo = suppOperations.SafeSum(x => x.Debt) - suppOperations.SafeSum(x => x.Credit);

            return new BaseDataResult(
                new
                {
                    entity.Id,
                    entity.OpenDate,
                    entity.CloseDate,
                    Saldo = saldo,
                    AccountNum = entity.AccountNumber,
                    BankAccountNum = this.bankProvider.GetBankAccountNumber(entity.RealityObject),
                    LastOperationDate = this.suppAccOperDomain.GetAll().Where(x => x.Account.Id == entity.Id).SafeMax(x => x.Date)
                });
        }

        /// <summary>
        /// GetLastClosedOperation
        /// </summary>
        /// <param name="robject"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RealityObjectChargeAccountOperation GetLastClosedOperation(RealityObject robject)
        {
            var operation = this.chargeOperationDomain.GetAll()
                .Where(x => x.Account.RealityObject.Id == robject.Id)
                .Where(x => x.Period.IsClosed)
                .OrderByDescending(x => x.Date)
                .FirstOrDefault();
            if (operation == null)
                throw new InvalidOperationException("Operation does not exists.");
            return operation;
        }

        public IDataResult GetSubsidyAccountResult(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("Id");

            var subsidyAccDomain = this.container.ResolveDomain<RealityObjectSubsidyAccount>();
            var subsidyAccOperDomain = this.container.ResolveDomain<RealityObjectSubsidyAccountOperation>();
            var realityObjectSubsidyService = this.container.Resolve<IRealityObjectSubsidyService>();
            var roMoneyRepo = this.container.Resolve<IRealtyObjectMoneyRepository>();
            var ropayaccDomain = this.container.ResolveDomain<RealityObjectPaymentAccount>();

            try
            {
                var entity = subsidyAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId);

                var paymentAccount = ropayaccDomain.GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == roId);

                if (entity == null)
                {
                    return new BaseDataResult();
                }

                DateTime? lastOperationDate = null;
                decimal factOperationTotal = 0m;

                if (paymentAccount != null)
                {
                    var transfers = roMoneyRepo.GetSubsidyTransfers(paymentAccount);

                    lastOperationDate = transfers.Max(x => (DateTime?)x.OperationDate);
                    factOperationTotal = transfers.SafeSum(x => x.Amount);
                }

                var planSubsidySum = realityObjectSubsidyService.GetListPlanSubsidyOperations(entity)
                    .SafeSum(x => x.Sum);

                return new BaseDataResult(
                    new
                    {
                        entity.Id,
                        entity.DateOpen,
                        AccountNum = entity.AccountNumber,
                        LastOperationDate = lastOperationDate,
                        FactOperTotal = factOperationTotal,
                        PlanOperTotal = planSubsidySum,
                        CurrentBalance = planSubsidySum - factOperationTotal
                    });
            }
            finally
            {
                this.container.Release(subsidyAccDomain);
                this.container.Release(subsidyAccOperDomain);
                this.container.Release(roMoneyRepo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="robject"></param>
        /// <returns></returns>
        public RealityObjectPaymentAccount GetPaymentAccount(RealityObject robject)
        {
            return this.payAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == robject.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="robject"></param>
        /// <returns></returns>
        public RealityObjectChargeAccount GetChargeAccount(RealityObject robject)
        {
            return this.chargeAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == robject.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="robject"></param>
        /// <returns></returns>
        public RealityObjectSupplierAccount GetSupplierAccount(RealityObject robject)
        {
            return this.suppAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == robject.Id);
        }

        //TODO Get RID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetPaymentAccountBySources(BaseParams baseParams)
        {
            //var roPaymentAccOperId = baseParams.Params.GetAs<long>("roPaymentAccOperId");
            //var typePaymentOperation = baseParams.Params.GetAs<PaymentOperationType>("typePaymentOperation");

            //// В зависимости от типа операции источники этой операции находятся по-разному
            ////  (на мой взгляд костыльно - через таблицы соответствий)
            //IQueryable<CreditPaymentSourceProxy> data;
            //switch (typePaymentOperation)
            //{
            //    case PaymentOperationType.OutcomeLoan: // У дома заняли деньжат - смотрим в займах
            //        var loanService = _container.Resolve<IDomainService<RealityObjectLoan>>();
            //        var loanPayAccOperationService = _container.Resolve<IDomainService<RealObjLoanPaymentAccOper>>();
            //        using (_container.Using(loanService, loanPayAccOperationService))
            //        {
            //            var loan = loanPayAccOperationService.GetAll()
            //                .FirstOrDefault(x => x.PayAccOperation.Id == roPaymentAccOperId)
            //                .Return(x => x.RealityObjectLoan);
            //            if (loan != null)
            //            {
            //                data = loanService.GetAll()
            //                    .Where(x => x.Id == loan.Id)
            //                    .Select(x => new CreditPaymentSourceProxy
            //                    {
            //                        Id = x.Id,
            //                        SrcFinanceType = GetActPaymentSrcByLoanSrc(loan.TypeSourceLoan),
            //                        Sum = x.LoanSum
            //                    });

            //                return new ListDataResult(data, data.Count());
            //            }
            //        }
            //        return new BaseDataResult();

            //    case PaymentOperationType.ExpenseLoan:
            //        // Дом платит по займу. Причем платить по займу он может только из тех же источников,
            //        //  из которых он этот займ от друго дома получил
            //        var roLoanPaymentService = _container.Resolve<IDomainService<RealityObjectLoanPayment>>();
            //        using (_container.Using(roLoanPaymentService))
            //        {
            //            var loanPayment = roLoanPaymentService.GetAll()
            //                .FirstOrDefault(x => x.OutcomeOperation.Id == roPaymentAccOperId);

            //            if (loanPayment != null)
            //            {
            //                return new BaseDataResult(new CreditPaymentSourceProxy
            //                {
            //                    Id = loanPayment.Loan.Id,
            //                    SrcFinanceType =
            //                        GetActPaymentSrcByLoanSrc(loanPayment.Loan.TypeSourceLoan),
            //                    Sum = loanPayment.OutcomeOperation.OperationSum
            //                });
            //            }
            //        }

            //        return new BaseDataResult();

            //    default:
            //        var roPayAccPerfActService = _container.Resolve<IDomainService<RealObjPaymentAccOperPerfAct>>();
            //        var paymentDetailsService = _container.Resolve<IDomainService<PaymentSrcFinanceDetails>>();

            //        using (_container.Using(roPayAccPerfActService))
            //        {
            //            var roPayAccPerfActId = roPayAccPerfActService.GetAll()
            //                .FirstOrDefault(x => x.PayAccOperation.Id == roPaymentAccOperId)
            //                .Return(x => x.PerformedWorkActPayment.Id);

            //            data = paymentDetailsService.GetAll()
            //                .Where(x => x.ActPayment.Id == roPayAccPerfActId)
            //                .Select(x => new CreditPaymentSourceProxy
            //                {
            //                    Id = x.Id,
            //                    SrcFinanceType = x.SrcFinanceType,
            //                    Sum = x.Payment
            //                });

            //            return new ListDataResult(data, data.Count());
            //        }
            //}
            return new BaseDataResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="realties"></param>
        /// <returns></returns>
        public IEnumerable<RealityObjectChargeAccount> GetChargeAccounts(IEnumerable<RealityObject> realties)
        {
            var roIds = realties.Select(x => x.Id);
            return this.chargeAccDomain.GetAll().Where(x => roIds.Contains(x.RealityObject.Id)).ToList();
        }

        private DateTime GetMaxOperationDate(RealityObjectPaymentAccount paymentAcc)
        {
            var transferRepo = this.container.ResolveRepository<Transfer>();
            var guids = paymentAcc.GetWallets().Select(x => x.WalletGuid).ToList();

            using (this.container.Using(transferRepo))
            {
                return
                    transferRepo.GetAll()
                        .Where(x => guids.Contains(x.TargetGuid) || guids.Contains(x.SourceGuid))
                        .SafeMax(x => x.OperationDate);
            }
        }
    }
}