//ToDo Короче этот файл просто костуль. Насамом деле это был онужно прост одля тго чтобы на клиенте в выпадающем списке был выбор в ыормате Json
//ToDo На клиенте делается перехват обработчика данного сыбытия и отправляется на другой контроллер 
namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Enums;
    using Mapping;
    using Wcf.Contracts.PersonalAccount;

	/// <summary>
	/// Провайдер для Json
	/// </summary>
	public class PersonalAccountPaymentInfoInDefaultJsonImportMap : AbstractImportMap<PersonalAccountPaymentInfoIn>
    {
		/// <summary>
		/// Ограничение
		/// </summary>
        public override string PermissionKey
        {
            get { return "GkhRegOp.Settings.BankDocumentImport.Formats.Json"; }
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
            get { return "Json"; }
        }

		/// <summary>
		/// Формат
		/// </summary>
        public override string Format
        {
            get { return "Json"; }
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
        public PersonalAccountPaymentInfoInDefaultJsonImportMap()
        {
            AutoMapProperties();
        }
    }
}