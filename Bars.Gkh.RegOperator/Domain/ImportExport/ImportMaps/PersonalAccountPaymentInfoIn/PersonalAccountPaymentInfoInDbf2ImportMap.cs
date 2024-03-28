namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Enums;
    using Mapping;
    using Wcf.Contracts.PersonalAccount;

	/// <summary>
	/// Провайдер для Dbf2
	/// </summary>
	public class PersonalAccountPaymentInfoInDbf2ImportMap : AbstractImportMap<PersonalAccountPaymentInfoIn>
    {
		/// <summary>
		/// Код провайдера
		/// </summary>
        public override string ProviderCode
        {
            get { return "dbf2"; }
        }

		/// <summary>
		/// Ограничение
		/// </summary>
        public override string PermissionKey
        {
            get { return "GkhRegOp.Settings.BankDocumentImport.Formats.Dbf2"; }
        }

		/// <summary>
		/// Наименование провайдера
		/// </summary>
        public override string ProviderName
        {
            get { return "Dbf2"; }
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
        public PersonalAccountPaymentInfoInDbf2ImportMap()
        {
            Map(x => x.DocumentDate, x => x.SetLookuper("DT_BANK"));
            Map(x => x.DocumentNumber, x => x.SetLookuper("PLP"));
            Map(x => x.PaymentDate, x => x.SetLookuper("DT_PAY"));
            Map(x => x.SumPaid, x => x.SetLookuper("SUM_PL"));
            Map(x => x.SumPenalty, x => x.SetLookuper("SUM_PN"));
            Map(x => x.AccountNumber, x => x.SetLookuper("LS"));
            Map(x => x.ExternalAccountNumber, x => x.SetLookuper("OUTERLS"));
            Map(x => x.Period, x => x.SetLookuper("PERIOD"));
            Map(x => x.Name, x => x.SetLookuper("FIO"));
            Map(x => x.PaymentCenterCode, x => x.SetLookuper("EPD"));
        }
    }
}