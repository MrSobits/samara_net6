namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Enums;
    using Gkh.Domain;

	/// <summary>
	/// Вью модель для Документы, загруженные из банка
	/// </summary>
	public class BankDocumentImportViewModel : BaseViewModel<BankDocumentImport>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
		/// <returns>Результат получения списка</returns>
		public override IDataResult List(IDomainService<BankDocumentImport> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var statementId = baseParams.Params.GetAsId("statementId");
            var showConfirmed = loadParams.Filter.GetAs<bool>("showConfirmed");
            var showDeleted = loadParams.Filter.GetAs<bool>("showDeleted");
            var showRegisters = loadParams.Filter.GetAs<bool>("showRegisters");
            var personalAccountNumber = loadParams.Filter.GetAs<string>("personalAccountNumber");
            var paymentDate = loadParams.Filter.GetAs<DateTime>("paymentDate");

            var linkDomain = this.Container.ResolveDomain<BankAccountStatementLink>();
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();
            try
            {
                var data = domainService.GetAll()
                    .WhereIf(statementId > 0, z => !linkDomain.GetAll().Any(x => x.Document.Id == z.Id))
                    .WhereIf(!showConfirmed, x =>
                            x.PaymentConfirmationState != PaymentConfirmationState.Distributed &&
                            x.PaymentConfirmationState != PaymentConfirmationState.PartiallyDistributed)
                    .WhereIf(!showDeleted, x => x.PaymentConfirmationState != PaymentConfirmationState.Deleted)
                    .WhereIf(showRegisters, x => x.PersonalAccountDeterminationState == PersonalAccountDeterminationState.NotDefined
                        || importedPaymentDomain.GetAll().Any(y => y.BankDocumentImport.Id == x.Id && (y.FactReceiverNumber == null || y.FactReceiverNumber == "") && x.PaymentConfirmationState != PaymentConfirmationState.Deleted))
                    .WhereIf(!string.IsNullOrEmpty(personalAccountNumber), x => importedPaymentDomain.GetAll().Any(y => y.BankDocumentImport.Id == x.Id && y.PersonalAccount.PersonalAccountNum == personalAccountNumber))
                    .WhereIf(paymentDate != DateTime.MinValue, x => importedPaymentDomain.GetAll().Any(y => y.BankDocumentImport.Id == x.Id && y.PaymentDate == paymentDate))
                    .Select(x => new
                    {
                        x.Id,
                        ImportDate = x.ImportDate.Date,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.ImportedSum,
                        x.PaymentAgentCode,
                        x.PaymentAgentName,
                        PACount = importedPaymentDomain.GetAll().Count(y => y.BankDocumentImport.Id == x.Id),
                        x.PersonalAccountDeterminationState,
                        x.BankStatement,
                        x.PaymentConfirmationState,
                        FileName = (x.LogImport.File.Name == null || x.LogImport.File.Name == "")
                                   ? ""
                                   : x.LogImport.File.Name + "." + x.LogImport.File.Extention,
                        x.LogImport.File,
                        x.CheckState,
                        x.AcceptDate,
                        x.ImportType
                    })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(linkDomain);
                this.Container.Release(importedPaymentDomain);
            }
        }

		/// <summary>
		/// Получить объект
		/// </summary>
		/// <param name="domain">Домен</param><param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult Get(IDomainService<BankDocumentImport> domain, BaseParams baseParams)
	    {
			var id = baseParams.Params.GetAs<long>("id");
		    var obj = domain.GetAll()
			    .Select(x => new
			    {
				    x.Id,
				    x.BankStatement,
				    x.DistributePenalty,
				    x.DocumentDate,
				    x.DocumentNumber,
				    x.DocumentType,
				    x.ImportDate,
				    x.ImportedSum,
				    x.LogImport,
				    FileName = x.LogImport.File.Name.IsNotEmpty()
					    ? x.LogImport.File.Name + "." + x.LogImport.File.Extention
					    : "",
				    x.PaymentAgentCode,
				    x.PaymentAgentName,
				    x.PaymentConfirmationState,
				    x.PersonalAccountDeterminationState,
				    x.State,
				    x.Status,
				    x.TransferGuid,
                    x.CheckState,
                    x.DistributedSum,
                    x.AcceptedSum
			    })
			    .FirstOrDefault(x => x.Id == id);

		    return new BaseDataResult(obj);
	    }
    }
}