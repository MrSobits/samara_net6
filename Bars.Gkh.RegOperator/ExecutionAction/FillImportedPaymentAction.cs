namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using NHibernate.Linq;

    /// <summary>
    /// Заполнение оплат в реестре оплат платежного агента
    /// </summary>
    public class FillImportedPaymentAction : BaseExecutionAction
    {
        /// <summary>
        /// Container
        /// </summary>
        /// <summary>
        /// Код
        /// </summary>
        public override string Description => "Заполнение оплат в реестре оплат платежного агента";

        public override string Name => "Заполнение оплат в реестре оплат платежного агента";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var importedPaymentRepo = this.Container.ResolveRepository<ImportedPayment>();
            var persAccDomain = this.Container.ResolveRepository<BasePersonalAccount>();
            var calcAccRealObjDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var bankDocImportDomain = this.Container.ResolveDomain<BankDocumentImport>();

            try
            {
                var persAccInfo = persAccDomain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.PersonalAccountNum,
                            RoId = x.Room.RealityObject.Id,
                            Address = x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum,
                            Owner = (x.AccountOwner as IndividualAccountOwner).Name ??
                                (x.AccountOwner as LegalAccountOwner).Contragent.Name
                        })
                    .ToDictionary(x => x.PersonalAccountNum);

                var calcAccDict = calcAccRealObjDomain.GetAll()
                    .Fetch(x => x.Account)
                    .ToList()
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                var inProgress = true;

                while (inProgress)
                {
                    var importedPayments = importedPaymentRepo
                        .GetAll()
                        .Where(x => persAccDomain.GetAll().Any(y => y.PersonalAccountNum == x.Account && x.Account != null && x.Account != ""))
                        .Fetch(x => x.PersonalAccount)
                        .Where(x => x.FactReceiverNumber == null)
                        .Take(100000)
                        .ToList();

                    foreach (var importedPayment in importedPayments)
                    {
                        var persAcc =
                            persAccInfo.Get(
                                importedPayment.PersonalAccount != null
                                    ? importedPayment.PersonalAccount.PersonalAccountNum ?? string.Empty
                                    : importedPayment.Account ?? string.Empty);

                        if (persAcc != null)
                        {
                            var calcAcc = calcAccDict.ContainsKey(persAcc.RoId)
                                ? calcAccDict.Get(persAcc.RoId)
                                    .Where(x => x.Account.DateOpen <= importedPayment.PaymentDate)
                                    .Where(
                                        x =>
                                            !x.Account.DateClose.HasValue ||
                                                x.Account.DateClose >= importedPayment.PaymentDate)
                                    .Select(x => x.Account)
                                    .FirstOrDefault()
                                : null;

                            importedPayment.FactReceiverNumber = calcAcc != null
                                ? calcAcc.TypeAccount == TypeCalcAccount.Special
                                    ? (calcAcc as SpecialCalcAccount).Return(x => x.AccountNumber)
                                    : calcAcc.TypeAccount == TypeCalcAccount.Regoperator
                                        ? (calcAcc as RegopCalcAccount).Return(x => x.ContragentCreditOrg)
                                            .Return(x => x.SettlementAccount)
                                        : " "
                                : " ";
                            importedPayment.PersonalAccount = new BasePersonalAccount {Id = persAcc.Id};
                            importedPayment.PersonalAccountDeterminationState =
                                ImportedPaymentPersAccDeterminateState.Defined;
                            importedPayment.AddressByImport = persAcc.Address;
                            importedPayment.OwnerByImport = persAcc.Owner;
                        }
                        else
                        {
                            importedPayment.FactReceiverNumber = " ";
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(this.Container, importedPayments, 10000, true, true);

                    if (importedPayments.Count == 0)
                    {
                        inProgress = false;
                    }
                }

                var impPays = importedPaymentRepo.GetAll()
                    .Where(
                        x =>
                            x.PersonalAccount == null &&
                                x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed)
                    .AsEnumerable()
                    .Select(
                        x =>
                        {
                            x.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.Deleted;

                            return x;
                        })
                    .ToArray();

                TransactionHelper.InsertInManyTransactions(this.Container, impPays, 10000, true, true);

                var bankDocImportToUpd = new List<BankDocumentImport>();

                bankDocImportDomain.GetAll()
                    .Select(
                        x => new
                        {
                            BankDocumentImport = x,
                            ImportedPaymentCount = importedPaymentRepo.GetAll().Count(y => y.BankDocumentImport.Id == x.Id),
                            PersAccDetermineCount = importedPaymentRepo.GetAll().Count(y => y.BankDocumentImport.Id == x.Id && y.PersonalAccount != null),
                            AcceptedCount =
                                importedPaymentRepo.GetAll()
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
            }
            finally
            {
                this.Container.Release(importedPaymentRepo);
                this.Container.Release(persAccDomain);
                this.Container.Release(calcAccRealObjDomain);
                this.Container.Release(bankDocImportDomain);
            }

            return new BaseDataResult();
        }
    }
}