namespace Bars.Gkh.RegOperator.Imports
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;
    using Bars.B4;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Services.DataContracts;
    using Bars.Gkh.RegOperator.Services.ServiceContracts;

    public class OfflinePaymentAndChargeImport : GkhImportBase
    {
        private readonly IService _service;
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public OfflinePaymentAndChargeImport(IService service)
        {
            _service = service;
        }

        #region Overrides of GkhImportBase

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get { return Id; }
        }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport
        {
            get { return "PersonalAccountImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт начислений и оплат (оффлайн версия)"; }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "xml"; }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.PersonalAccountChargePaymentImportXml"; }
        }

        protected override ImportResult ImportUsingGkhApi(BaseParams baseParams)
        {
            var file = baseParams.Files.First().Value;

            var parameters = GetParameters(file);
            if (parameters == null)
            {
                return new ImportResult(StatusImport.CompletedWithError)
                {
                    Message = "Неверное содержимое файла"
                };
            }

            var result = _service.SyncChargePaymentRkc(parameters);

            return new ImportResult(StatusImport.CompletedWithoutError) {Data = result};
        }

        #endregion

        private SyncChargePaymentRkcRecord GetParameters(FileData file)
        {
            var serializer = new XmlSerializer(typeof (SyncChargePaymentRkcRecord), new XmlRootAttribute("record"));
            using (var reader = XmlReader.Create(new StreamReader(new MemoryStream(file.Data))))
            {
                return serializer.Deserialize(reader) as SyncChargePaymentRkcRecord;
            }
        }
    }
}