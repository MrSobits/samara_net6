namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Enums;
    using Mapping;
    using Wcf.Contracts.PersonalAccount;

	/// <summary>
	/// Провайдер для Xml
	/// </summary>
	public class PersonalAccountPaymentInfoInDefaultXmlImportMap : AbstractImportMap<PersonalAccountPaymentInfoIn>
    {
		/// <summary>
		/// Ограничение
		/// </summary>
        public override string PermissionKey
        {
            get { return "GkhRegOp.Settings.BankDocumentImport.Formats.Xml"; }
        }

		/// <summary>
		/// Код провайдера
		/// </summary>
        public override string ProviderCode
        {
            get { return "Default"; }
        }

		/// <summary>
		/// Наименование провайдера
		/// </summary>
        public override string ProviderName
        {
            get { return "Xml"; }
        }

		/// <summary>
		/// Формат
		/// </summary>
        public override string Format
        {
            get { return "Xml"; }
        }

		/// <summary>
		/// Направление импорта
		/// </summary>
        public override ImportExportType Direction
        {
            get { return ImportExportType.Export | ImportExportType.Import; }
        }

		/// <summary>
		/// Конструктор
		/// </summary>
        public PersonalAccountPaymentInfoInDefaultXmlImportMap()
        {
            AutoMapProperties();
        }
    }
}