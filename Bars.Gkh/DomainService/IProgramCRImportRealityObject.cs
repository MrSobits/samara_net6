namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;

    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.FiasHelper;
    using Bars.GkhExcel;

    public interface IProgramCRImportRealityObject
    {
        /// <summary>
        /// Получить id дома из строки импорта
        /// </summary>
        long GetRealityObjectAddress (List<RealtyObjectAddressProxy> result, RecordProxy record, ILogImport logImport);

        /// <summary>
        /// Словарь заголовка
        /// </summary>
        Dictionary<string, int> InitHeader(bool hasRoIdColumn);

        /// <summary>
        /// Словарь заголовка
        /// </summary>
        Dictionary<int, string> InitNewHeader(Dictionary<int, string> dictionary, bool hasRoIdColumn);

        /// <summary>
        /// Создать строку импорта
        /// </summary>
        RecordProxy InitRecord(GkhExcelCell[] row, int rowNumber, bool hasRoIdColumn);

        /// <summary>
        /// Получить значение поля
        /// </summary>
        string GetValue(GkhExcelCell[] data, string field, bool hasRoIdColumn);
    }

    public sealed class RealtyObjectAddressProxy
    {
        public string KladrCode { get; set; }

        public string AoGuid { get; set; }

        public long Id { get; set; }

        public string House { get; set; }

        public string Housing { get; set; }

        public string Building { get; set; }

        public long RealityObjectId { get; set; }

        public long MunicipalityId { get; set; }

        public string Structure { get; set; }

        public string Construction { get; set; }
        
        public string Liter { get; set; }

    }

    /// <summary>
    /// Класс адреса из одной распарсенной строки экселины
    /// </summary>
    public sealed class RecordProxy
    {
        public bool isValidRecord { get; set; }

        public string LocalityName { get; set; }

        public DynamicAddress Address { get; set; }

        public long MunicipalityId { get; set; }

        public string StreetName { get; set; }

        public string CodeKladrStreet { get; set; }

        public string House { get; set; }

        //литер
        public string Liter { get; set; }

        //сооружение
        public string Construction { get; set; }

        //строение
        public string Structure { get; set; }

        //корпус
        public string Housing { get; set; }

        //номер строения
        public string Building { get; set; }

        public FiasAddress FiasAddress { get; set; }

        public int RowNumber { get; set; }

        public override string ToString()
        {
            string LiterCorrected = (Liter == "0" ? "" : $"литер {Liter}, ");
            return $"{LocalityName},улица {StreetName}, {LiterCorrected}корпус {Housing}, строение {Building}, дом {House}";
        }
    }
}