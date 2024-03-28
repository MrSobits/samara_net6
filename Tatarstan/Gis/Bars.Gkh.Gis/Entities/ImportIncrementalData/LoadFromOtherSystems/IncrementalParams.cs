namespace Bars.Gkh.Gis.Entities.ImportIncrementalData.LoadFromOtherSystems
{
    using System.Collections.Generic;
    using System.Text;
    using DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems;

    public class IncrementalParams
    {
        public long MgfFileId { get; set; }
        public List<Supplier> Suppliers { get; set; }
    }

    /// <summary>
    /// Вспомогательная сущность "Хранилище банка данных"
    /// </summary>
    public class DataBankStogare
    {

        /// <summary>
        /// Код банка данных
        /// </summary>
        public long DataBankId { get; set; }

        /// <summary>
        /// Строка соединения к БД
        /// </summary>
        public string ConnectionString { get; set; }


        /// <summary>
        /// Наименование буфферной схемы
        /// </summary>
        public string SchemaName { get; set; }

    }

    public class Supplier
    {
        public long DataSupplierId { get; set; }
        public string DataSupplier { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string Ogrn { get; set; }
        public List<DataBank> DataBankList { get; set; }
    }

    public class DataBank
    {
        public long ContragentId { get; set; }
        public string DataBankKey { get; set; }
        public string DataBankName { get; set; }
        public string RajonList { get; set; }
        public int DataStorageId { get; set; }
    }

    public class LoadLog
    {
        /// <summary>
        /// Лог с ошибками
        /// </summary>
        public StringBuilder ErrorMessages { get; set; }
        /// <summary>
        /// Статус загрузки
        /// </summary>
        public UploadStatuses Status { get; set; }
        /// <summary>
        /// Информационный лог
        /// </summary>
        public StringBuilder InfoMessages { get; set; }
        /// <summary>
        /// Лог с предупреждениями
        /// </summary>
        public StringBuilder WarningMessages { get; set; }
    }
    public class Section
    {
        public string SectionName { get; set; }
        public int RowCount { get; set; }
        public int Ordering { get; set; }
    }

    public class FormatTemplate
    {
        public int UploadId { get; set; }
        public string FileName { get; set; }
        public SupplierAndBank SupplierAndBank { get; set; }
        public List<Section> SectionList { get; set; }
        public List<TemplateHeader> HeaderList { get; set; }
        public Dictionary<string, List<Template>> Format { get; set; }
    }

    public class SupplierAndBank
    {
        public long MgfFileId { get; set; }
        public int DataBankId { get; set; }
        public int DataSupplierId { get; set; }
        public string DataBankKey { get; set; }
        public int DataStorageId { get; set; }
        public int PersonalAccountsCount { get; set; }
        public string DataSupplier { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string Ogrn { get; set; }
    }

    public class FileHeader
    {
        public string FormatVersion { get; set; }
        //ИНН
        public string SenderInn { get; set; }
        //КПП
        public string SenderKpp { get; set; }
        //ОГРН (ОГРНИП)
        public string SenderOgrn { get; set; }
        //Наименование организации (ФИО ИП)
        public string SenderOrgName { get; set; }
        //Ключ банка данных (Наименование банка данных)
        public string SenderDataBankKey { get; set; }
        //Год, за который предоставлены сведения
        public string SenderCalculationYear { get; set; }
        //Месяц, за который предоставлены сведения
        public string SenderCalculationMonth { get; set; }
        //Дата и время формирования файла
        public string SenderFileDateTime { get; set; }
        //ФИО отправителя
        public string SenderFullName { get; set; }
        public string SenderPhone { get; set; }
    }

    public class TemplateHeader
    {
        public int SectionNumber { get; set; }
        public string SectionName { get; set; }
        public string SectionTable { get; set; }
        public string SectionFile { get; set; }
        public int Ordering { get; set; }
        public bool IsUpload { get; set; }
    }

    public class DataStorage
    {
        public int Id { get; set; }
        public string DataStorageName { get; set; }
        public string Pref { get; set; }
        public int PersonalAccountCount { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseConnectionString { get; set; }
        public int MaxVolume { get; set; }
        public int DefaultVolume { get; set; }
    }

    public class DataSchema
    {
        public int Id { get; set; }
        public DataStorage DataStorage { get; set; }
        public string SchemaName { get; set; }
        public int Year { get; set; }
    }

    public class ChdTable
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string SequenceName { get; set; }
        public string SequenceColumnName { get; set; }
    }

    public class ConnectionParameters
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DbName { get; set; }
    }
}
