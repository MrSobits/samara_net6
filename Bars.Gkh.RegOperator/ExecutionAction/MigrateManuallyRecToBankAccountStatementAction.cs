namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Миграция данных в реестр оплат платежных агентов
    /// </summary>
    public class MigrateManuallyRecToBankAccountStatementAction : BaseExecutionAction
    {
        /// <summary>
        /// Container
        /// </summary>
        /// <summary>
        /// Код
        /// </summary>
        public override string Description => "Миграция данных из раздела \"Обновленный счет НВС\"(ручные записи) в раздел \"Банковские операции\"";

        public override string Name => "Миграция данных из раздела \"Обновленный счет НВС\"(ручные записи) в раздел \"Банковские операции\"";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var suspenseAccRepo = this.Container.ResolveRepository<SuspenseAccount>();
            var bankAccStatementRepo = this.Container.ResolveRepository<BankAccountStatement>();
            var importedPaymentRepo = this.Container.ResolveRepository<ImportedPayment>();
            var regopCalcAccountRepo = this.Container.ResolveRepository<RegopCalcAccount>();
            var transferDomain = this.Container.ResolveRepository<PersonalAccountPaymentTransfer>();
            var operationDomain = this.Container.ResolveRepository<MoneyOperation>();
            var persAccDomain = this.Container.ResolveRepository<BasePersonalAccount>();
            var roPayAccDomain = this.Container.ResolveRepository<RealityObjectPaymentAccount>();

            try
            {
                var regopDict = regopCalcAccountRepo.GetAll()
                    .Where(x => x.AccountNumber != null && x.AccountNumber != "")
                    .GroupBy(x => x.AccountNumber)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.AccountOwner).First());

                var suspAccQuery = suspenseAccRepo.GetAll()
                    .Where(x => !bankAccStatementRepo.GetAll().Any(y => x.Id == y.SuspenseAccount.Id))
                    .Where(
                        x =>
                            !importedPaymentRepo.GetAll()
                                .Any(y => x.Id == y.PaymentId && y.PaymentState == ImportedPaymentState.Rns));

                var suspAccTransferGuids =
                    suspAccQuery.Where(
                        x =>
                            x.DistributeState == DistributionState.Distributed ||
                                x.DistributeState == DistributionState.PartiallyDistributed)
                        .Select(x => x.TransferGuid)
                        .ToList();

                var sourceNameByWalletGuid = new Dictionary<string, string>();
                var transfers = new List<Transfer>();

                var portion = 1000;

                for (int i = 0; i < suspAccTransferGuids.Count; i += portion)
                {
                    var tempGuids = suspAccTransferGuids.Skip(i).Take(portion).ToList();

                    var transfersQuery = transferDomain.GetAll()
                        .Where(x => !operationDomain.GetAll().Any(y => y.CanceledOperation.Id == x.Id))
                        .Where(x => tempGuids.Contains(x.Operation.OriginatorGuid));

                    var tempTransfers = transfersQuery.ToList();

                    var tempTransfersIds = tempTransfers.Select(x => x.Id).ToList();

                    transfers.AddRange(tempTransfers);

                    persAccDomain.GetAll()
                        .Where(
                            x => transferDomain.GetAll().Where(y => tempTransfersIds.Contains(y.Id)).
                                Any(
                                    y => y.TargetGuid == x.BaseTariffWallet.WalletGuid
                                        || y.SourceGuid == x.BaseTariffWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.BaseTariffWallet.WalletGuid,
                                x.PersonalAccountNum
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.PersonalAccountNum);
                                }
                            });

                    persAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.DecisionTariffWallet.WalletGuid
                                            || y.SourceGuid == x.DecisionTariffWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.DecisionTariffWallet.WalletGuid,
                                x.PersonalAccountNum
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.PersonalAccountNum);
                                }
                            });

                    persAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.PenaltyWallet.WalletGuid
                                            || y.SourceGuid == x.PenaltyWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.PenaltyWallet.WalletGuid,
                                x.PersonalAccountNum
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.PersonalAccountNum);
                                }
                            });

                    persAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.RentWallet.WalletGuid
                                            || y.SourceGuid == x.RentWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.RentWallet.WalletGuid,
                                x.PersonalAccountNum
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.PersonalAccountNum);
                                }
                            });

                    persAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.PreviosWorkPaymentWallet.WalletGuid
                                            || y.SourceGuid == x.PreviosWorkPaymentWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.PreviosWorkPaymentWallet.WalletGuid,
                                x.PersonalAccountNum
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.PersonalAccountNum);
                                }
                            });

                    persAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.AccumulatedFundWallet.WalletGuid
                                            || y.SourceGuid == x.AccumulatedFundWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.AccumulatedFundWallet.WalletGuid,
                                x.PersonalAccountNum
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.PersonalAccountNum);
                                }
                            });

                    persAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.RestructAmicableAgreementWallet.WalletGuid
                                            || y.SourceGuid == x.RestructAmicableAgreementWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.RestructAmicableAgreementWallet.WalletGuid,
                                x.PersonalAccountNum
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.PersonalAccountNum);
                                }
                            });

                    persAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.SocialSupportWallet.WalletGuid
                                            || y.SourceGuid == x.SocialSupportWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.SocialSupportWallet.WalletGuid,
                                x.PersonalAccountNum
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.PersonalAccountNum);
                                }
                            });

                    roPayAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.TargetSubsidyWallet.WalletGuid
                                            || y.SourceGuid == x.TargetSubsidyWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.TargetSubsidyWallet.WalletGuid,
                                x.RealityObject.Address
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.Address);
                                }
                            });

                    roPayAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.FundSubsidyWallet.WalletGuid
                                            || y.SourceGuid == x.FundSubsidyWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.FundSubsidyWallet.WalletGuid,
                                x.RealityObject.Address
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.Address);
                                }
                            });

                    roPayAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.RegionalSubsidyWallet.WalletGuid
                                            || y.SourceGuid == x.RegionalSubsidyWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.RegionalSubsidyWallet.WalletGuid,
                                x.RealityObject.Address
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.Address);
                                }
                            });

                    roPayAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.StimulateSubsidyWallet.WalletGuid
                                            || y.SourceGuid == x.StimulateSubsidyWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.StimulateSubsidyWallet.WalletGuid,
                                x.RealityObject.Address
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.Address);
                                }
                            });

                    roPayAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.OtherSourcesWallet.WalletGuid
                                            || y.SourceGuid == x.OtherSourcesWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.OtherSourcesWallet.WalletGuid,
                                x.RealityObject.Address
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.Address);
                                }
                            });

                    roPayAccDomain.GetAll()
                        .Where(
                            x =>
                                transferDomain.GetAll()
                                    .Where(y => tempTransfersIds.Contains(y.Id))
                                    .Any(
                                        y => y.TargetGuid == x.BankPercentWallet.WalletGuid
                                            || y.SourceGuid == x.BankPercentWallet.WalletGuid))
                        .Select(
                            x => new
                            {
                                x.BankPercentWallet.WalletGuid,
                                x.RealityObject.Address
                            })
                        .AsEnumerable()
                        .ForEach(
                            x =>
                            {
                                if (!sourceNameByWalletGuid.ContainsKey(x.WalletGuid))
                                {
                                    sourceNameByWalletGuid.Add(x.WalletGuid, x.Address);
                                }
                            });
                }

                var transfersDict = transfers
                    .GroupBy(x => x.Operation.OriginatorGuid)
                    .ToDictionary(x => x.Key, y => y.ToList());

                var details = new List<DistributionDetail>();

                this.Container.InTransaction(
                    () =>
                    {
                        suspAccQuery.ToList()
                            .ForEach(
                                x =>
                                {
                                    var regop = regopDict.Get(x.AccountBeneficiary);

                                    var newBankAccountStatement = new BankAccountStatement
                                    {
                                        OperationDate = x.ObjectCreateDate,
                                        DocumentDate = x.DateReceipt,
                                        Sum = x.Sum,
                                        RemainSum = x.RemainSum,
                                        DistributionCode = x.DistributionCode,
                                        MoneyDirection = x.MoneyDirection,
                                        DateReceipt = x.DateReceipt,
                                        DistributionDate = x.DistributionDate,
                                        PaymentDetails = "Запись перенесена из раздела \"Обновленный счет НВС\"",
                                        DistributeState = x.DistributeState,
                                        RecipientAccountNum = x.AccountBeneficiary,
                                        RecipientName = regop.Return(z => z.Name),
                                        SuspenseAccount = x,
                                        IsDistributable = YesNo.Yes
                                    };

                                    newBankAccountStatement.SetGuid(x.TransferGuid);

                                    bankAccStatementRepo.Save(newBankAccountStatement);

                                    var tempTransfers = transfersDict.Get(x.TransferGuid);

                                    if (tempTransfers != null)
                                    {
                                        foreach (var transfer in tempTransfers)
                                        {
                                    var Object = sourceNameByWalletGuid.Get
                                        (transfer.SourceGuid == x.TransferGuid 
                                            ? transfer.TargetGuid
                                            : transfer.SourceGuid);

                                    var newDetail = new DistributionDetail(newBankAccountStatement.Id, Object, transfer.Amount)
                                            {
                                                Source = DistributionSource.BankStatement,

                                                Destination = "Распределение оплаты"
                                            };

                                            details.Add(newDetail);
                                        }
                                    }
                                });
                    });

                TransactionHelper.InsertInManyTransactions(this.Container, details, 10000, true, true);
            }
            finally
            {
                this.Container.Release(suspenseAccRepo);
                this.Container.Release(bankAccStatementRepo);
                this.Container.Release(importedPaymentRepo);
                this.Container.Release(regopCalcAccountRepo);
                this.Container.Release(transferDomain);
                this.Container.Release(operationDomain);
                this.Container.Release(persAccDomain);
                this.Container.Release(roPayAccDomain);
            }

            return new BaseDataResult();
        }
    }
}