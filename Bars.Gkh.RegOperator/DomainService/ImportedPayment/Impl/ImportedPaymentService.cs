namespace Bars.Gkh.RegOperator.DomainService.Impl
{
	using System;
	using System.Linq;
	using B4;
	using B4.Modules.NH.Extentions;
	using B4.Utils;
	using Castle.Windsor;
	using Entities;
	using Enums;
	using Gkh.Entities;

	/// <summary>
	/// Сервис для работы с "Импортируемая оплата"
	/// </summary>
	public class ImportedPaymentService : IImportedPaymentService
	{
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис для "Импортируемая оплата"
        /// </summary>
        public IDomainService<ImportedPayment> ImportedPaymentDomain { get; set; }

        /// <summary>
        /// Домен-сервис для "Лицевой счет"
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }
        /// <summary>
        /// Домен-сервис для "Документы, загруженные из банка"
        /// </summary>
        public IDomainService<BankDocumentImport> BankDocumentImport { get; set; }

        /// <summary>
        /// Домен-сервис для "Жилой дом расчетного счета"
        /// </summary>
        public IDomainService<CalcAccountRealityObject> CalcAccountRealityObjectDomain { get; set; }

		/// <summary>
		/// Сопоставить ЛС
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат</returns>
		public IDataResult ComparePersonalAccount(BaseParams baseParams)
		{
			var paymentId = baseParams.Params.GetAs<long>("paymentId");
			var paId = baseParams.Params.GetAs<long>("paId");
            var result = this.ComparePersonalAccount(paymentId, paId);
		    
            return result;
		}

        /// <summary>
        /// Сопоставить ЛС
        /// </summary>
        /// <param name="paymentId">Идентификатор оплаты</param>
        /// <param name="paId">Идентификатор лицевого счета</param>
        /// <returns>Результат</returns>
        public IDataResult ComparePersonalAccount(long paymentId, long paId)
		{
            var payment = this.ImportedPaymentDomain.Get(paymentId);
            if (payment == null)
			{
				return new BaseDataResult(false, "Не найдена импортируемая оплата");
			}

			var pAccount = PersonalAccountDomain.Get(paId);
			if (pAccount == null)
			{
				return new BaseDataResult(false, "Не найден лицевой счет для сопоставления");
			}

			payment.PersonalAccount = pAccount;
			payment.PersonalAccountDeterminationState = ImportedPaymentPersAccDeterminateState.Defined;
			payment.FactReceiverNumber = this.GetBankAccountNumber(pAccount.Room.RealityObject, payment.PaymentDate);
		    payment.IsDeterminateManually = true;
            payment.PersonalAccountNotDeterminationStateReason = null;

            Container.InTransaction(() =>
            {
                this.ImportedPaymentDomain.Update(payment);
                this.ChangeStatusBankDocument(payment.BankDocumentImport);
            });

            return new BaseDataResult(true, "Сопоставление ЛС прошло успешно");
		}

		private string GetBankAccountNumber(RealityObject ro, DateTime paymentDate)
		{
			var specAccount = this.GetSpecialAccount(ro, paymentDate);

			string accnum;
			if (specAccount != null)
			{
				accnum = specAccount.Return(x => x.AccountNumber);
			}
			else
			{
				accnum = this.GetRegopAccount(ro, paymentDate)
					.Return(x => x.ContragentCreditOrg)
					.Return(x => x.SettlementAccount);
			}

			return accnum;
		}

		private SpecialCalcAccount GetSpecialAccount(RealityObject ro, DateTime paymentDate)
		{
			var account = CalcAccountRealityObjectDomain.GetAll()
				.Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
				.Where(x => x.RealityObject.Id == ro.Id)
				.Select(x => x.Account)
				.Where(x => x.DateOpen <= paymentDate)
				.Where(x => !x.DateClose.HasValue || x.DateClose.Value >= paymentDate)
				.OrderByDescending(x => x.DateOpen)
				.FirstOrDefault() as SpecialCalcAccount;

			return account;
		}

		private RegopCalcAccount GetRegopAccount(RealityObject ro, DateTime paymentDate)
		{
			var account = CalcAccountRealityObjectDomain.GetAll()
				.Where(x => x.RealityObject.Id == ro.Id)
				.Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator)
				.Where(x => x.Account.DateOpen <= paymentDate)
				.Where(x => !x.Account.DateClose.HasValue || x.Account.DateClose >= paymentDate)
				.OrderByDescending(x => x.Account.DateOpen)
				.Select(x => x.Account)
				.FirstOrDefault() as RegopCalcAccount;

			return account;
		}

	    private void ChangeStatusBankDocument(BankDocumentImport bankDocumentImport)
	    {
	        var query = ImportedPaymentDomain.GetAll()
	            .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
	            .Where(x => x.PersonalAccountDeterminationState != ImportedPaymentPersAccDeterminateState.Defined);

	        bankDocumentImport.PersonalAccountDeterminationState = !query.Any()
	            ? PersonalAccountDeterminationState.Defined
	            : PersonalAccountDeterminationState.PartiallyDefined;

	        this.BankDocumentImport.Update(bankDocumentImport);
	    }
	}
}