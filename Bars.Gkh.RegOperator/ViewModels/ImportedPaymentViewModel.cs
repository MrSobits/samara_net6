namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.RegOperator.Enums;

    using Entities;

    using NHibernate.Linq;

    /// <summary>
	/// Вью модель для "Импортируемая оплата"
	/// </summary>
	public class ImportedPaymentViewModel : BaseViewModel<ImportedPayment>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат получения списка</returns>
		public override IDataResult List(IDomainService<ImportedPayment> domainService, BaseParams baseParams)
        {
            var realObjPayAccDomain = this.Container.ResolveDomain<RealityObjectPaymentAccount>();

            try
            {
                var loadParams = this.GetLoadParam(baseParams);

                var bankDocumentImportId = loadParams.Filter.GetAs<long>("bankDocumentImportId");
				var showRsNotEqual = loadParams.Filter.GetAs<bool>("showRsNotEqual");
				var showNotDefined = loadParams.Filter.GetAs<bool>("showNotDefined");
				var showNotDistributed = loadParams.Filter.GetAs<bool>("showNotDistributed");
				var showNotDeleted = loadParams.Filter.GetAs<bool>("showNotDeleted");

				var data = domainService.GetAll()
                    .Fetch(x => x.PersonalAccount)
                    .ThenFetch(x => x.AccountOwner)
                    .WhereIf(bankDocumentImportId > 0, x => x.BankDocumentImport.Id == bankDocumentImportId)
                    .WhereIf(showRsNotEqual, x => x.ReceiverNumber != x.FactReceiverNumber || x.ReceiverNumber == null || x.ReceiverNumber == "")
                    .WhereIf(showNotDefined, x => x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.NotDefined)
                    .WhereIf(showNotDistributed, x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.NotDistributed)
                    .WhereIf(showNotDeleted, x => x.PaymentConfirmationState != ImportedPaymentPaymentConfirmState.Deleted)
					.Select(x => new
                    {
						x.Id,
                        x.Account,
                        x.PaymentType,
                        x.Sum,
                        x.PaymentDate,
                        x.PaymentState,
                        x.PaymentNumberUs,
                        x.ReceiverNumber,
                        PersonalAccount = x.PersonalAccount.PersonalAccountNum,
                        x.PersonalAccount.PersAccNumExternalSystems,
                        x.AddressByImport,
                        x.OwnerByImport,
                        Address = x.PersonalAccount.Room.RealityObject.Address + ", кв. " + x.PersonalAccount.Room.RoomNum,
                        Owner = x.PersonalAccount != null
                            ? (x.PersonalAccount.AccountOwner as IndividualAccountOwner).Name ??
                              (x.PersonalAccount.AccountOwner as LegalAccountOwner).Contragent.Name
                            : "",
                        ReceiverNumberFact = x.FactReceiverNumber,
                        x.PersonalAccountDeterminationState,
                        x.PaymentConfirmationState,
                        x.IsDeterminateManually,
						BankDocumentImportId = x.BankDocumentImport.Id,
						x.BankDocumentImport.DocumentNumber,
						x.BankDocumentImport.DocumentDate,
						x.BankDocumentImport.PaymentAgentName,
                        PersonalAccountState = x.PersonalAccount.State,
                        x.AcceptDate,
                        x.PersonalAccountNotDeterminationStateReason
					})
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();
                data = data.Order(loadParams).Paging(loadParams);

                return new ListDataResult(data.ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(realObjPayAccDomain);
            }
        }
    }
}