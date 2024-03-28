namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Import;
    using Wcf.Contracts.PersonalAccount;

	/// <summary>
	/// Провайдер импорта документов из банка
	/// </summary>
	public interface IBankDocumentImportProvider
	{
		/// <summary>
		/// Создать Импорт документа из банка
		/// </summary>
		/// <param name="paymentInfo">Строка импорта платежки</param>
		/// <param name="importType">Тип импорта</param>
		/// <param name="logImport">Лог импорта</param>
		/// <param name="providerCode">Код провайдера</param>
		/// <param name="fileName">Имя файла</param>
		/// <returns></returns>
		Entities.BankDocumentImport CreateBankDocumentImport(
			IEnumerable<PersonalAccountPaymentInfoIn> paymentInfo,
			BankDocumentImportType importType,
			ILogImport logImport,
			string providerCode = null,
			string fileName = null);

	    /// <summary>
	    /// Индикатор прогресса выполнения
	    /// </summary>
	    Action<int, string> IndicateAction { get; set; }
	}

	/// <summary>
	/// Тип импорта документа из банка
	/// </summary>
	public enum BankDocumentImportType
	{
		/// <summary>
		/// Импорт банковского документа
		/// </summary>
		BankDocument = 10,

		/// <summary>
		/// Импорта социальной поддержки
		/// </summary>
		SocialSupport = 20
	}
}