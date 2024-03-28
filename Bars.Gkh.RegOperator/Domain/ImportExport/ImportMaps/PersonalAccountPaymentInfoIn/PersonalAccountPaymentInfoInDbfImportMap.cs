namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Enums;
    using Mapping;
    using Wcf.Contracts.PersonalAccount;

	/// <summary>
	/// Провайдер для Dbf
	/// </summary>
	public class PersonalAccountPaymentInfoInDbfImportMap : AbstractImportMap<PersonalAccountPaymentInfoIn>
    {
		/// <summary>
		/// Ограничение
		/// </summary>
        public override string PermissionKey
        {
            get { return "GkhRegOp.Settings.BankDocumentImport.Formats.Dbf"; }
        }

		/// <summary>
		/// Код провайдера
		/// </summary>
        public override string ProviderCode
        {
            get { return "default"; }
        }

		/// <summary>
		/// Наименование провайдера
		/// </summary>
        public override string ProviderName
        {
            get { return "Dbf"; }
        }

		/// <summary>
		/// Формат
		/// </summary>
        public override string Format
        {
            get { return "dbf"; }
        }

		/// <summary>
		/// Направление импорта
		/// </summary>
        public override ImportExportType Direction
        {
            get { return ImportExportType.Import; }
        }

		/// <summary>
		/// Конструктор
		/// </summary>
        public PersonalAccountPaymentInfoInDbfImportMap()
        {
            Map(x => x.AccountNumber, x => x.SetLookuper("PayUNID"));
            Map(x => x.ReceiptId, x => x.SetLookuper("PayBULL"));
            Map(x => x.PaymentDate, x => x.SetLookuper("PayDate"));
            Map(x => x.DocumentNumber, x => x.SetLookuper("PP_Numb"));
            Map(x => x.DocumentDate, x => x.SetLookuper("PP_Date"));
            Map(x => x.TargetPaid, x => x.SetLookuper("PaySUMM"));
            Map(x => x.Surname, x => x.SetLookuper("PayNAME1"));
            Map(x => x.Name, x => x.SetLookuper("PayNAME2"));
            Map(x => x.Patronymic, x => x.SetLookuper("PayNAME3"));
        }
    }
}