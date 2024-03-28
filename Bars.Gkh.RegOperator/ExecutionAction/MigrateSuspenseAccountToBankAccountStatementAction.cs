namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    public class MigrateSuspenseAccountToBankAccountStatementAction : BaseExecutionAction
    {
        public override string Description => "Миграция данных из реестра счетов невыясненных сумм в реестр банковских выписок";

        public override string Name => "Миграция данных из реестра счетов невыясненных сумм в реестр банковских выписок";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var suspenseAccRepo = this.Container.ResolveRepository<SuspenseAccount>();
            var bankAccStatementRepo = this.Container.ResolveRepository<BankAccountStatement>();

            using (this.Container.Using(suspenseAccRepo, bankAccStatementRepo))
            {
                var bankAccStatementQuery = bankAccStatementRepo.GetAll()
                    .Where(x => x.DocumentDate != DateTime.MinValue)
                    .Where(x => x.Sum != 0M)
                    .Where(x => x.RecipientAccountNum != null);

                var bankAccStatementList = bankAccStatementQuery
                    .Where(x => x.SuspenseAccount == null)
                    .ToList();

                // id счетов невыясненных сумм уже связанные с банк. выписками
                var bankAccStatementHaveSuspAccIdsField = bankAccStatementQuery
                    .Where(x => x.SuspenseAccount != null)
                    .Select(x => x.SuspenseAccount.Id)
                    .ToArray();

                var suspenseAccDictByCombinedKey = suspenseAccRepo.GetAll()
                    .WhereIf(bankAccStatementHaveSuspAccIdsField.Length > 0, x => !bankAccStatementHaveSuspAccIdsField.Contains(x.Id))
                    .Where(x => x.DateReceipt != DateTime.MinValue)
                    .Where(x => x.Sum != 0M)
                    .Where(x => x.AccountBeneficiary != null)
                    .Select(
                        x => new
                        {
                            x.DateReceipt,
                            x.Sum,
                            x.AccountBeneficiary,
                            x.MoneyDirection,
                            x.DistributeState,
                            x.DistributionCode,
                            x.Id
                        })
                    .ToList()
                    .Select(
                        x => new
                        {
                            Key = string.Format(
                                "{0}#{1}#{2}",
                                x.DateReceipt.ToShortDateString(),
                                x.Sum,
                                x.AccountBeneficiary),
                            x.MoneyDirection,
                            x.DistributeState,
                            x.DistributionCode,
                            x.Id
                        })
                    .GroupBy(x => x.Key)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(
                            y => new
                            {
                                y.MoneyDirection,
                                y.DistributeState,
                                y.DistributionCode,
                                y.Id
                            })
                            .ToList());

                var bankAccStatementListToSave = new List<BankAccountStatement>();

                foreach (var bankAccountStatement in bankAccStatementList)
                {
                    var key = string.Format(
                        "{0}#{1}#{2}",
                        bankAccountStatement.DocumentDate.ToShortDateString(),
                        bankAccountStatement.Sum,
                        bankAccountStatement.RecipientAccountNum);

                    if (suspenseAccDictByCombinedKey.ContainsKey(key))
                    {
                        var suspenseAcc = suspenseAccDictByCombinedKey[key].FirstOrDefault();

                        if (suspenseAcc != null)
                        {
                            bankAccountStatement.MoneyDirection = suspenseAcc.MoneyDirection;
                            bankAccountStatement.DistributeState = suspenseAcc.DistributeState;
                            bankAccountStatement.DistributionCode = suspenseAcc.DistributionCode;
                            bankAccountStatement.SuspenseAccount = new SuspenseAccount {Id = suspenseAcc.Id};

                            bankAccStatementListToSave.Add(bankAccountStatement);
                            suspenseAccDictByCombinedKey[key].Remove(suspenseAcc);
                        }
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, bankAccStatementListToSave);
            }

            return new BaseDataResult();
        }
    }
}