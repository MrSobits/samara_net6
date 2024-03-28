namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using System;
    using System.Globalization;
    using B4.Utils;
    using Enums;
    using Mapping;
    using Wcf.Contracts.PersonalAccount;

	/// <summary>
	/// Провайдер для Dbf (Питер)
	/// </summary>
	public class PersonalAccountPaymentInfoInDbfSpbImportMap : AbstractImportMap<PersonalAccountPaymentInfoIn>
    {
		/// <summary>
		/// Ограничение
		/// </summary>
        public override string PermissionKey
        {
            get { return "GkhRegOp.Settings.BankDocumentImport.Formats.DbfSpb"; }
        }

		/// <summary>
		/// Код провайдера
		/// </summary>
        public override string ProviderCode
        {
            get { return "dbf-spb"; }
        }

		/// <summary>
		/// Наименование провайдера
		/// </summary>
        public override string ProviderName
        {
            get { return "Dbf (Питер)"; }
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
        public PersonalAccountPaymentInfoInDbfSpbImportMap()
        {
            Func<object, object> dateParser = x =>
            {
                DateTime value;

                if (DateTime.TryParseExact(x.ToStr(), "ddmmyyyy", new CultureInfo("ru-RU"), DateTimeStyles.None, out value))
                {
                    return value;
                }

                return DateTime.MinValue;
            };

            Map(x => x.ExternalAccountNumber, x => x.SetLookuper("KKC"));
            Map(x => x.TargetPaid, x => x.SetLookuper("PLAT_KR"));
            Map(x => x.PaymentDate, x => x.SetLookuper("DATE_P"), dateParser);
            Map(x => x.PenaltyPaid, x => x.SetLookuper("PLAT_PENY"));
        }
    }
}