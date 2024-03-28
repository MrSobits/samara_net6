namespace Bars.Gkh.RegOperator.Imports.VTB24
{
    using Domain.ImportExport.Enums;
    using Domain.ImportExport.Mapping;
    using Wcf.Contracts.PersonalAccount;

	/// <summary>
	/// Провайдер для Xml (ВТБ24 Камчатка)
	/// </summary>
	public class VtbImportMap : AbstractImportMap<PersonalAccountPaymentInfoIn>
    {
		/// <summary>
		/// Ограничение
		/// </summary>
		public override string PermissionKey
        {
            get { return "GkhRegOp.Settings.BankDocumentImport.Formats.Vtb24"; }
        }

		/// <summary>
		/// Код провайдера
		/// </summary>
		public override string ProviderCode
        {
            get { return "vtb24origin"; }
        }

		/// <summary>
		/// Наименование провайдера
		/// </summary>
		public override string ProviderName
        {
            get { return "Xml (ВТБ24 Камчатка)"; }
        }

		/// <summary>
		/// Формат
		/// </summary>
		public override string Format
        {
            get { return "XML"; }
        }

		/// <summary>
		/// Направление импорта
		/// </summary>
		public override ImportExportType Direction
        {
            get { return ImportExportType.Import; }
        }
    }
}
