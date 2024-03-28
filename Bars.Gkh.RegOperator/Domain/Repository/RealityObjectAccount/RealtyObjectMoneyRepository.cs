namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using B4;
    using B4.Utils;
    using B4.Utils.Annotations;
    using System.Linq.Dynamic.Core;

    using Entities;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using Extensions;
    using FastMember;
    using Gkh.Entities;
    using NHibernate.Linq;

    public class RealtyObjectMoneyRepository : IRealtyObjectMoneyRepository
    {
        private readonly IDomainService<RealityObjectTransfer> transferRepo;
        private readonly IDomainService<RealityObjectPaymentAccount> accountRepo;
        private readonly IDomainService<RealityObjectLoan> loanDomain;

        public RealtyObjectMoneyRepository(
            IDomainService<RealityObjectTransfer> transferRepo, 
            IDomainService<RealityObjectPaymentAccount> accountRepo, 
            IDomainService<RealityObjectLoan> loanDomain)
        {
            this.transferRepo = transferRepo;
            this.accountRepo = accountRepo;
            this.loanDomain = loanDomain;
        }

        /// <inheritdoc />
        public IQueryable<TransferDto> GetDebtTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts)
        {
            var guids = this.GetWalletGuids(paymentAccounts);
            var ids = paymentAccounts.Select(x => x.Id).ToArray();

            return this.transferRepo.GetAll()
                .Where(x => ids.Contains(x.Owner.Id))
                .Where(x => (guids.Contains(x.TargetGuid) && (((long?) x.Operation.CanceledOperation.Id) == null || x.IsLoan))
                            || (guids.Contains(x.SourceGuid) && (((long?) x.Operation.CanceledOperation.Id) != null || x.IsReturnLoan)))
                .TranslateToDto();
        }

        /// <inheritdoc />
        public ICollection<TransferDto> GetDebtTransfers(ICollection<string> walletGuids)
        {
            return this.transferRepo.GetAll()
                .Where(x => (walletGuids.Contains(x.TargetGuid) && (((long?) x.Operation.CanceledOperation.Id) == null || x.IsLoan))
                    || (walletGuids.Contains(x.SourceGuid) && (((long?) x.Operation.CanceledOperation.Id) != null || x.IsReturnLoan)))
                .TranslateToDto()
                .ToList();
        }

        /// <inheritdoc />
        public IQueryable<TransferDto> GetCreditTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts)
        {
            var guids = this.GetWalletGuids(paymentAccounts);
            var ids = paymentAccounts.Select(x => x.Id).ToArray();

            return this.transferRepo.GetAll()
                .Where(x => ids.Contains(x.Owner.Id))
                .Where(x => !x.IsLoan && !x.IsReturnLoan)
                .Where(x => (guids.Contains(x.SourceGuid) && ((long?) x.Operation.CanceledOperation.Id) == null)
                            || (guids.Contains(x.TargetGuid) && ((long?) x.Operation.CanceledOperation.Id) != null ))
                .TranslateToDto();
        }

        /// <inheritdoc />
        public ICollection<TransferDto> GetCreditTransfers(ICollection<string> walletGuids)
        {
            return this.transferRepo.GetAll()
                .Where(x => !x.IsLoan && !x.IsReturnLoan)
                .Where(x => (walletGuids.Contains(x.TargetGuid) && ((long?) x.Operation.CanceledOperation.Id) == null)
                    || (walletGuids.Contains(x.SourceGuid) && ((long?) x.Operation.CanceledOperation.Id) != null))
                .TranslateToDto()
                .ToList();
        }

        /// <inheritdoc />
        public IQueryable<RealityObjectLoanDto> GetRealityObjectLoanSum(IQueryable<RealityObject> realityObjects, bool anyOperations = true)
        {
            return this.loanDomain.GetAll()
                .WhereIf(anyOperations, x => x.Operations.Any())
                .Where(x => realityObjects.Any(r => r == x.LoanTaker.RealityObject))
                .Select(
                    x => new RealityObjectLoanDto
                    {
                        RealityObjectId = x.LoanTaker.RealityObject.Id,
                        LoanSum = x.LoanSum,
                        LoanReturnedSum = x.LoanReturnedSum
                    });
        }

        /// <inheritdoc />
        public IQueryable<RealtyObjectBalanceDto> GetRealtyBalances(IQueryable<RealityObject> realityObjects)
        {
            return this.accountRepo.GetAll()
                .Where(x => realityObjects.Any(r => r == x.RealityObject))
                .Select(x => new RealtyObjectBalanceDto
                {
                    RealtyObjectId = x.RealityObject.Id,
                    Credit = x.CreditTotal,
                    Debt = x.DebtTotal,
                    Loan = x.Loan,
                    Lock = x.MoneyLocked
                });
        }

        /// <inheritdoc />
        public IQueryable<TransferDto> GetSubsidyTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts)
        {
            return this.transferRepo.GetAll()
                .Fetch(x => x.Operation)
                .Where(z => paymentAccounts.Any(x =>
                    x.FundSubsidyWallet.WalletGuid == z.SourceGuid
                    || x.FundSubsidyWallet.WalletGuid == z.TargetGuid
                    || x.RegionalSubsidyWallet.WalletGuid == z.SourceGuid
                    || x.RegionalSubsidyWallet.WalletGuid == z.TargetGuid
                    || x.StimulateSubsidyWallet.WalletGuid == z.SourceGuid
                    || x.StimulateSubsidyWallet.WalletGuid == z.TargetGuid
                    || x.TargetSubsidyWallet.WalletGuid == z.SourceGuid
                    || x.TargetSubsidyWallet.WalletGuid == z.TargetGuid))
                .TranslateToDto();
        }

        /// <inheritdoc />
        public IQueryable<TransferDto> GetSubsidyTransfers(RealityObjectPaymentAccount paymentAccount)
        {
            ArgumentChecker.NotNull(paymentAccount, "paymentAccount");

            var walletGuids = paymentAccount.GetSubsidyWalletGuids();

            return this.transferRepo.GetAll()
                .Where(x => walletGuids.Contains(x.SourceGuid) || walletGuids.Contains(x.TargetGuid))
                .Where(x =>
                        (!x.Reason.ToLower().Contains("займ")) || (x.Operation != null && !x.Operation.Reason.ToLower().Contains("займ")))
                .Where(x => // фильтруем "Оплата акта" и "Отмена оплаты акта"
                    ((!x.Reason.ToLower().Contains("оплата акта")) || (x.Operation != null && !x.Operation.Reason.ToLower().Contains("оплата акта"))) &&
                    ((!x.Reason.ToLower().Contains("оплаты акта")) || (x.Operation != null && !x.Operation.Reason.ToLower().Contains("оплаты акта"))))
                .Select(x => new TransferDto
                {
                    Id = x.Id,
                    Reason = x.Reason ?? x.Originator.Reason,
                    OperationDate = x.PaymentDate,
                    Amount = walletGuids.Contains(x.TargetGuid) ? x.Amount : -1 * x.Amount
                });
        }

        private List<string> GetWalletGuids(IQueryable<RealityObjectPaymentAccount> paymentAccounts)
        {
            var walletProps =
                typeof (RealityObjectPaymentAccount).GetProperties()
                    .Where(x => x.PropertyType == typeof (Wallet))
                    .Select(x => x.Name);

            var query = new StringBuilder("new(");

            var counter = 0;
            foreach (var walletProp in walletProps)
            {
                counter++;

                query.Append(walletProp).Append(".WalletGuid as ").Append("w_" + walletProp);
                if (counter != walletProps.Count())
                {
                    query.Append(", ");
                }
            }

            query.Append(")");

            var guids = new List<string>();

            var queryString = query.ToString().TrimEnd(',');
            var result = paymentAccounts.Select(queryString);

            foreach (var item in result)
            {
                var accessor = ObjectAccessor.Create(item);

                foreach (var walletProp in walletProps)
                {
                    guids.Add(accessor["w_" + walletProp].ToStr());
                }
            }

            return guids;
        }

        public ISet<string> GetWalletGuids(ICollection<RealityObjectPaymentAccount> paymentAccounts)
        {
            var walletProps = typeof(RealityObjectPaymentAccount).GetProperties()
                .Where(x => x.PropertyType == typeof(Wallet));

            var guids = new HashSet<string>();
            foreach (var account in paymentAccounts)
            {
                foreach (var walletProp in walletProps)
                {
                    var wallet = walletProp.GetValue(account) as Wallet;
                    guids.Add(wallet?.WalletGuid);
                }
            }

            return guids;
        }

        private Tuple<Expression, Expression> GetWalletContainsQuery(ParameterExpression transferParam, ParameterExpression accountParam)
        {
            var accountType = typeof(RealityObjectPaymentAccount);
            var walletType = typeof(Wallet);
            var wallets = accountType.GetProperties().Where(x => x.PropertyType == walletType);
            var walletGuid = walletType.GetProperty("WalletGuid", typeof(string));

            var sourceGuidExpression = Expression.Property(transferParam, "SourceGuid");
            var targetGuidExpression = Expression.Property(transferParam, "TargetGuid");

            var srcGuidContains = new List<Expression>();
            var dstGuidContains = new List<Expression>();
            foreach (var wallet in wallets)
            {
                var walletGuidExpression = Expression.Property(Expression.Property(accountParam, wallet), walletGuid);
                var eqSourceGuid = Expression.Equal(walletGuidExpression, sourceGuidExpression);
                var eqTargetGuid = Expression.Equal(walletGuidExpression, targetGuidExpression);
                srcGuidContains.Add(eqSourceGuid);
                dstGuidContains.Add(eqTargetGuid);
            }

            var srcContainsExpression = srcGuidContains.Aggregate(Expression.OrElse);
            var dstContainsExpression = dstGuidContains.Aggregate(Expression.OrElse);

            return Tuple.Create(srcContainsExpression, dstContainsExpression);
        }

        private Expression GetTransferFilterExpression(bool isDebt,
            IQueryable<RealityObjectPaymentAccount> accountQuery,
            ParameterExpression transferParam,
            ParameterExpression accountParam)
        {
            var operationType = typeof(MoneyOperation);
            var trueExpression = Expression.Constant(true, typeof(bool));
            var nullOperationExpression = Expression.Constant(null, operationType);

            var isLoan = Expression.Property(transferParam, "IsLoan");
            var isReturnLoan = Expression.Property(transferParam, "IsReturnLoan");
            var canceledOperation = Expression.Property(Expression.Property(transferParam, "Operation"), "CanceledOperation");

            var eqNullOperation = Expression.Equal(canceledOperation, nullOperationExpression);
            var notNullOperation = Expression.NotEqual(canceledOperation, nullOperationExpression);
            var isLoanExpression = Expression.Equal(isLoan, trueExpression);
            var isReturnLoanExpression = Expression.Equal(isReturnLoan, trueExpression);

            var guidContainsQuery = this.GetAnyWallet(accountQuery, transferParam, accountParam);
            var sourceGuidExpression = Expression.Equal(guidContainsQuery.Item1, trueExpression);
            var targetGuidExpression = Expression.Equal(guidContainsQuery.Item2, trueExpression);

            Expression left;
            Expression right;
            if (isDebt)
            {
                left = Expression.AndAlso(targetGuidExpression, Expression.OrElse(eqNullOperation, isLoanExpression));
                right = Expression.AndAlso(sourceGuidExpression, Expression.OrElse(notNullOperation, isReturnLoanExpression));
            }
            else
            {
                left = Expression.AndAlso(targetGuidExpression, eqNullOperation);
                right = Expression.AndAlso(sourceGuidExpression, notNullOperation);
            }

            return Expression.OrElse(left, right);
        }

        private Tuple<MethodCallExpression, MethodCallExpression> GetAnyWallet(IQueryable<RealityObjectPaymentAccount> accountQuery, ParameterExpression transferParam, ParameterExpression accountParam)
        {
            var guidContainsQuery = this.GetWalletContainsQuery(transferParam, accountParam);
            var sourceGuidExpression = guidContainsQuery.Item1;
            var targetGuidExpression = guidContainsQuery.Item2;

            var sourceAny = Expression.Call(typeof(Queryable),
                "Any",
                new[] { accountQuery.ElementType },
                accountQuery.Expression,
                Expression.Lambda<Func<RealityObjectPaymentAccount, bool>>(sourceGuidExpression, accountParam));
            var targetAny = Expression.Call(typeof(Queryable),
                "Any",
                new[] { accountQuery.ElementType },
                accountQuery.Expression,
                Expression.Lambda<Func<RealityObjectPaymentAccount, bool>>(targetGuidExpression, accountParam));

            return Tuple.Create(sourceAny, targetAny);
        }

        /// <summary>
        /// Собирает запрос:
        /// <code>
        /// transferQuery.Where(t => (accountQuery.Any(a => a.*Wallet == t.TargetGuid || a.*Wallet == t.TargetGuid ||...)
        ///                         && (t.Operation.CanceledOperation == null || t.IsLoan))
        ///                     || (accountQuery.Any(a => a.*Wallet == t.SourceGuid || a.*Wallet == t.SourceGuid ||...)
        ///                         && (t.Operation.CanceledOperation != null || t.IsReturnLoan));
        /// </code>
        /// </summary>
        private IQueryable<RealityObjectTransfer> GetDebtQuery(IQueryable<RealityObjectTransfer> transferQuery,
            IQueryable<RealityObjectPaymentAccount> accountQuery)
        {
            var transferParam = Expression.Parameter(typeof(RealityObjectTransfer), "transferParam");
            var accountParam = Expression.Parameter(typeof(RealityObjectPaymentAccount), "accountParam");
            var filterExpression = this.GetTransferFilterExpression(true, accountQuery, transferParam, accountParam);
            var lambda = Expression.Lambda<Func<RealityObjectTransfer, bool>>(filterExpression, transferParam);
            return transferQuery.Where(lambda);
        }
    }
}