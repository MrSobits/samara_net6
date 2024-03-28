namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    using NHibernate.Linq;

    /// <summary>
    /// Миграция данных в реестр оплат платежных агентов
    /// </summary>
    public class MigrateInfoToBankDocumentImport : BaseExecutionAction
    {


        public override string Description => "Миграция данных в реестр оплат платежных агентов";

        public override string Name => "Миграция данных в реестр оплат платежных агентов";

        public override Func<IDataResult> Action => this.Execute;


        private BaseDataResult Execute()
        {
            var unacceptedPaymentDomain = this.Container.ResolveDomain<UnacceptedPayment>();
            var bankDocumentImportDomain = this.Container.ResolveDomain<BankDocumentImport>();
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();
            var suspenseAccountDomain = this.Container.ResolveDomain<SuspenseAccount>();
            var transferDomain = this.Container.ResolveDomain<PersonalAccountPaymentTransfer>();
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var operationDomain = this.Container.ResolveDomain<MoneyOperation>();
            var unacceptedPaymentPacketDomain = this.Container.ResolveDomain<UnacceptedPaymentPacket>();

            try
            {
                var packetIds = unacceptedPaymentPacketDomain.GetAll().Select(x => x.Id).ToList();

                var portion = 1000;

                for (int i = 0; i < packetIds.Count; i += portion)
                {
                    this.LinkUnacceptedPayments(
                        unacceptedPaymentDomain,
                        importedPaymentDomain,
                        personalAccountDomain,
                        packetIds.Skip(i).Take(portion).ToList());
                }

                var transfersQuery = transferDomain.GetAll()
                    .Where(x => !operationDomain.GetAll().Any(y => y.CanceledOperation.Id == x.Id))
                    .Where(
                        x => suspenseAccountDomain.GetAll()
                            .Where(
                                y => importedPaymentDomain.GetAll()
                                    .Any(z => z.PaymentId == y.Id && z.PaymentState == ImportedPaymentState.Rns))
                            .Any(y => y.TransferGuid == x.Operation.OriginatorGuid));

                var persAccByWalletGuid = new Dictionary<string, BasePersonalAccount>();

                personalAccountDomain.GetAll()
                    .Where(
                        x => transfersQuery.Any(
                            y => y.TargetGuid == x.BaseTariffWallet.WalletGuid
                                || y.TargetGuid == x.DecisionTariffWallet.WalletGuid
                                || y.TargetGuid == x.PenaltyWallet.WalletGuid))
                    .AsEnumerable()
                    .ForEach(
                        x =>
                        {
                            persAccByWalletGuid.Add(x.BaseTariffWallet.WalletGuid, x);
                            persAccByWalletGuid.Add(x.DecisionTariffWallet.WalletGuid, x);
                            persAccByWalletGuid.Add(x.PenaltyWallet.WalletGuid, x);
                        });

                var details = transfersQuery
                    .GroupBy(x => x.Operation.OriginatorGuid)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Select(
                            x => new
                            {
                                x.Amount,
                                Acc = persAccByWalletGuid.Get(x.TargetGuid),
                                Transfer = x
                            })
                            .GroupBy(x => x.Acc)
                            .Select(
                                x => new
                                {
                                    Acc = x.Key,
                                    Amount = x.SafeSum(z => z.Amount),
                                    Transfers = x.Select(z => z.Transfer).ToArray()
                                }).Where(x => x.Acc != null).ToArray());

                var importedPaymentToSave = new List<ImportedPayment>();
                var operationToUpd = new HashSet<MoneyOperation>();
                var transferToUpd = new List<Transfer>();

                var impSysAccRecs = importedPaymentDomain.GetAll()
                    .Where(x => x.PaymentState == ImportedPaymentState.Rns)
                    .Join(
                        suspenseAccountDomain.GetAll(),
                        x => x.PaymentId,
                        y => y.Id,
                        (impPay, susAcc) => new
                        {
                            susAcc,
                            impPay,
                            susAcc.Operations
                        })
                    .Where(x => x.susAcc.DistributeState != DistributionState.NotDistributed)
                    .ToArray();

                foreach (var x in impSysAccRecs)
                {
                    if (x.susAcc.DistributeState == DistributionState.PartiallyDistributed)
                    {
                        x.impPay.Sum = x.susAcc.RemainSum;
                        importedPaymentToSave.Add(x.impPay);
                        continue;
                    }

                    if (x.susAcc.DistributeState == DistributionState.Canceled ||
                        x.susAcc.DistributeState == DistributionState.Deleted)
                    {
                        x.impPay.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.Deleted;
                        importedPaymentToSave.Add(x.impPay);
                        continue;
                    }

                    var tempDetails = details.Get(x.susAcc.TransferGuid);

                    if (tempDetails == null)
                    {
                        continue;
                    }

                    if (tempDetails.Count() == 1)
                    {
                        var detail = tempDetails.First();

                        x.impPay.PersonalAccount = detail.Acc;
                        x.impPay.PersonalAccountDeterminationState = ImportedPaymentPersAccDeterminateState.Defined;
                        x.impPay.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.Distributed;
                        x.impPay.AddressByImport = detail.Acc.Room.RealityObject.Address + ", кв. " +
                            detail.Acc.Room.RoomNum;
                        x.impPay.OwnerByImport = detail.Acc != null && detail.Acc.AccountOwner != null
                            ? (detail.Acc.AccountOwner as IndividualAccountOwner).Return(y => y.Name) ??
                                (detail.Acc.AccountOwner as LegalAccountOwner).Return(y => y.Contragent.Name)
                            : string.Empty;

                        importedPaymentToSave.Add(x.impPay);

                        foreach (var transfer in detail.Transfers)
                        {
                            if (transfer.SourceGuid == x.susAcc.TransferGuid)
                            {
                                transfer.SetSourceGuid(x.impPay.BankDocumentImport.TransferGuid);
                                transferToUpd.Add(transfer);
                            }

                            transfer.Operation.SetOriginatorGuid(x.impPay.BankDocumentImport.TransferGuid);

                            if (transfer.Operation.OriginatorGuid == x.susAcc.TransferGuid)
                            {
                                transfer.Operation.SetOriginatorGuid(x.impPay.BankDocumentImport.TransferGuid);
                                operationToUpd.Add(transfer.Operation);
                            }
                        }
                    }
                    else
                    {
                        foreach (var detail in tempDetails)
                        {
                            var newImportedPayment = new ImportedPayment
                            {
                                BankDocumentImport = x.impPay.BankDocumentImport,
                                Account = x.impPay.Account,
                                ExternalAccountNumber = x.impPay.ExternalAccountNumber,
                                PersonalAccount = detail.Acc,
                                ReceiverNumber = x.impPay.ReceiverNumber,
                                AddressByImport = detail.Acc.Room.RealityObject.Address + ", кв. " +
                                    detail.Acc.Room.RoomNum,
                                OwnerByImport = detail.Acc != null && detail.Acc.AccountOwner != null
                                    ? (detail.Acc.AccountOwner as IndividualAccountOwner).Return(y => y.Name) ??
                                        (detail.Acc.AccountOwner as LegalAccountOwner).Return(y => y.Contragent.Name)
                                    : string.Empty,
                                FactReceiverNumber = x.impPay.FactReceiverNumber,
                                PaymentType = x.impPay.PaymentType,
                                PaymentId = x.impPay.PaymentId,
                                Sum = x.impPay.Sum,
                                PaymentDate = x.impPay.PaymentDate,
                                PaymentState = x.impPay.PaymentState,
                                PaymentNumberUs = x.impPay.PaymentNumberUs,
                                ExternalTransaction = x.impPay.ExternalTransaction,
                                Accepted = true,
                                PersonalAccountDeterminationState = ImportedPaymentPersAccDeterminateState.Defined,
                                PaymentConfirmationState = ImportedPaymentPaymentConfirmState.Distributed,
                                IsDeterminateManually = false
                            };

                            importedPaymentToSave.Add(newImportedPayment);

                            foreach (var transfer in detail.Transfers)
                            {
                                if (transfer.SourceGuid == x.susAcc.TransferGuid)
                                {
                                    transfer.SetSourceGuid(x.impPay.BankDocumentImport.TransferGuid);
                                    transferToUpd.Add(transfer);
                                }

                                transfer.Operation.SetOriginatorGuid(x.impPay.BankDocumentImport.TransferGuid);

                                if (transfer.Operation.OriginatorGuid == x.susAcc.TransferGuid)
                                {
                                    transfer.Operation.SetOriginatorGuid(x.impPay.BankDocumentImport.TransferGuid);
                                    operationToUpd.Add(transfer.Operation);
                                }
                            }
                        }
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, importedPaymentToSave, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, operationToUpd, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, transferToUpd, 10000, true, true);

                var bankDocImportToUpd = new List<BankDocumentImport>();

                bankDocumentImportDomain.GetAll()
                    .Select(
                        x => new
                        {
                            BankDocumentImport = x,
                            ImportedPaymentCount = importedPaymentDomain.GetAll().Count(y => y.BankDocumentImport.Id == x.Id),
                            PersAccDetermineCount = importedPaymentDomain.GetAll().Count(y => y.BankDocumentImport.Id == x.Id && y.PersonalAccount != null),
                            AcceptedCount =
                                importedPaymentDomain.GetAll()
                                    .Count(
                                        y =>
                                            y.BankDocumentImport.Id == x.Id && y.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed)
                        })
                    .AsEnumerable()
                    .ForEach(
                        x =>
                        {
                            x.BankDocumentImport.PersonalAccountDeterminationState = x.PersAccDetermineCount > 0
                                ? x.PersAccDetermineCount == x.ImportedPaymentCount
                                    ? PersonalAccountDeterminationState.Defined
                                    : PersonalAccountDeterminationState.PartiallyDefined
                                : PersonalAccountDeterminationState.NotDefined;

                            x.BankDocumentImport.PaymentConfirmationState = x.AcceptedCount > 0
                                ? x.AcceptedCount == x.ImportedPaymentCount
                                    ? PaymentConfirmationState.Distributed
                                    : PaymentConfirmationState.PartiallyDistributed
                                : PaymentConfirmationState.NotDistributed;

                            bankDocImportToUpd.Add(x.BankDocumentImport);
                        });

                TransactionHelper.InsertInManyTransactions(this.Container, bankDocImportToUpd, 10000, true, true);

                var impList = importedPaymentDomain.GetAll()
                    .Where(
                        x => x.PaymentType == ImportedPaymentType.Basic
                            || x.PaymentType == ImportedPaymentType.Payment
                            || x.PaymentType == ImportedPaymentType.Sum).ToList();

                impList.ForEach(x => { x.PaymentType = ImportedPaymentType.ChargePayment; });

                TransactionHelper.InsertInManyTransactions(this.Container, impList, 10000, true, true);
            }
            finally
            {
                this.Container.Release(unacceptedPaymentDomain);
                this.Container.Release(bankDocumentImportDomain);
                this.Container.Release(importedPaymentDomain);
                this.Container.Release(suspenseAccountDomain);
                this.Container.Release(transferDomain);
                this.Container.Release(personalAccountDomain);
                this.Container.Release(operationDomain);
                this.Container.Release(unacceptedPaymentPacketDomain);
            }

            return new BaseDataResult();
        }

        private void LinkUnacceptedPayments(
            IDomainService<UnacceptedPayment> unacceptedPaymentDomain,
            IDomainService<ImportedPayment> importedPaymentDomain,
            IDomainService<BasePersonalAccount> personalAccountDomain,
            List<long> packetIds)
        {
            var importedPaymentToSave = new List<ImportedPayment>();
            var bankDocumentToSave = new HashSet<BankDocumentImport>();

            var importedPyaments = importedPaymentDomain.GetAll()
                .Where(
                    y => unacceptedPaymentDomain.GetAll()
                        .Any(
                            z =>
                                packetIds.Contains(z.Packet.Id) && z.Id == y.PaymentId &&
                                    y.PaymentState == ImportedPaymentState.Rno));

            var persAccs = personalAccountDomain.GetAll()
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .Fetch(x => x.AccountOwner)
                .Where(x => importedPyaments.Any(z => z.Account == x.PersonalAccountNum))
                .Select(
                    x => new
                    {
                        PersAccId = x.Id,
                        PersonalAccountNum = x.PersonalAccountNum ?? "",
                        AddressByImport = x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum,
                        OwnerByImport = (x.AccountOwner as IndividualAccountOwner).Name ??
                            (x.AccountOwner as LegalAccountOwner).Contragent.Name,
                    })
                .ToDictionary(x => x.PersonalAccountNum);

            var persAccs1 = personalAccountDomain.GetAll()
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .Fetch(x => x.AccountOwner)
                .Where(
                    x => unacceptedPaymentDomain.GetAll()
                        .Any(
                            z =>
                                packetIds.Contains(z.Packet.Id) && z.PersonalAccount.Id == x.Id))
                .Select(
                    x => new
                    {
                        PersAccId = x.Id,
                        PersonalAccountNum = x.PersonalAccountNum ?? "",
                        AddressByImport = x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum,
                        OwnerByImport = (x.AccountOwner as IndividualAccountOwner).Name ??
                            (x.AccountOwner as LegalAccountOwner).Contragent.Name,
                    })
                .ToDictionary(x => x.PersonalAccountNum);

            var unacceptedPays = importedPaymentDomain.GetAll()
                .Where(x => x.PaymentState == ImportedPaymentState.Rno)
                .Join(
                    unacceptedPaymentDomain.GetAll(),
                    x => x.PaymentId,
                    y => y.Id,
                    (impPay, unPay) => new
                    {
                        PersAccId = unPay.PersonalAccount.Id,
                        unPay.PersonalAccount.PersonalAccountNum,
                        AddressByImport = unPay.PersonalAccount.Room.RealityObject.Address + ", кв. " +
                            unPay.PersonalAccount.Room.RoomNum,
                        OwnerByImport = (unPay.PersonalAccount.AccountOwner as IndividualAccountOwner).Name ??
                            (unPay.PersonalAccount.AccountOwner as LegalAccountOwner).Contragent.Name,
                        unPay.Accepted,
                        unPay.Packet.TransferGuid,
                        impPay,
                        PacketId = unPay.Packet.Id
                    })
                .Where(x => packetIds.Contains(x.PacketId))
                .ToArray();

            foreach (var x in unacceptedPays)
            {
                if (x.impPay.Account.IsEmpty() || !persAccs.ContainsKey(x.impPay.Account))
                {
                    var persAccInfo = persAccs.Get(x.PersonalAccountNum) ?? persAccs1.Get(x.PersonalAccountNum);

                    if (persAccInfo != null)
                    {
                        x.impPay.Account = x.PersonalAccountNum;
                        x.impPay.PersonalAccount = new BasePersonalAccount {Id = persAccInfo.PersAccId};
                        x.impPay.AddressByImport = persAccInfo.AddressByImport;
                        x.impPay.OwnerByImport = persAccInfo.OwnerByImport;
                        x.impPay.PersonalAccountDeterminationState = ImportedPaymentPersAccDeterminateState.Defined;
                    }
                }
                else
                {
                    var persAccInfo = persAccs.Get(x.impPay.Account) ?? persAccs1.Get(x.impPay.Account);

                    if (persAccInfo != null)
                    {
                        x.impPay.PersonalAccount = new BasePersonalAccount {Id = persAccInfo.PersAccId};
                        x.impPay.AddressByImport = persAccInfo.AddressByImport;
                        x.impPay.OwnerByImport = persAccInfo.OwnerByImport;
                        x.impPay.PersonalAccountDeterminationState = ImportedPaymentPersAccDeterminateState.Defined;
                    }
                }

                x.impPay.PaymentConfirmationState = x.Accepted
                    ? ImportedPaymentPaymentConfirmState.Distributed
                    : ImportedPaymentPaymentConfirmState.NotDistributed;

                if (x.impPay.PaymentType == ImportedPaymentType.Basic
                    || x.impPay.PaymentType == ImportedPaymentType.Payment
                    || x.impPay.PaymentType == ImportedPaymentType.Sum)
                {
                    x.impPay.PaymentType = ImportedPaymentType.ChargePayment;
                }

                importedPaymentToSave.Add(x.impPay);

                x.impPay.BankDocumentImport.SetGuid(x.TransferGuid);

                bankDocumentToSave.Add(x.impPay.BankDocumentImport);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, importedPaymentToSave, 10000, true, true);
            TransactionHelper.InsertInManyTransactions(this.Container, bankDocumentToSave, 10000, true, true);
        }
    }
}