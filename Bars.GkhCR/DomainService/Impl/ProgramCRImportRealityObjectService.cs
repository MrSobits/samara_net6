namespace Bars.GkhCr.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.DomainService;
    using Bars.Gkh.Import;
    using Bars.GkhExcel;

    public class ProgramCRImportRealityObjectService : IProgramCRImportRealityObject
    {
        private Dictionary<string, int> headersDict;

        /// <inheritdoc />
        public long GetRealityObjectAddress(List<RealtyObjectAddressProxy> address, RecordProxy record, ILogImport logImport)
        {
            var result = string.IsNullOrEmpty(record.Housing)
                ? address.Where(x => string.IsNullOrEmpty(x.Housing)).ToList()
                : address.Where(x => x.Housing == record.Housing).ToList();

            result = string.IsNullOrEmpty(record.Building)
                ? address.Where(x => string.IsNullOrEmpty(x.Building)).ToList()
                : address.Where(x => x.Building == record.Building).ToList();

            result = string.IsNullOrEmpty(record.Liter)
                ? address.Where(x => string.IsNullOrEmpty(x.Liter)).ToList()
                : address.Where(x => x.Liter == record.Liter).ToList();

            if (result.Count == 0)
            {
                logImport.Error(
                    string.Empty,
                    string.Format("В системе не найден дом, соответствующий записи. Строка: {0}", record.RowNumber));
                return 0;
            }

            if (result.Count > 1)
            {
                logImport.Error(
                    string.Empty,
                    string.Format("В системе найдено несколько домов, соответствующих записи. Строка: {0}", record.RowNumber));
                return 0;
            }

            return result.First().RealityObjectId;
        }

        public Dictionary<string, int> InitHeader(bool hasRoIdColumn)
        {
            this.headersDict = new Dictionary<string, int>();

            var cnt = 0;
            this.headersDict["№"] = cnt++;

            // если столбец присутствует, сдвигаем все колонки на одну вправо
            if (hasRoIdColumn)
            {
                this.headersDict["RO_ID"] = cnt++;
            }
            this.headersDict["MUNICIPALITY"] = cnt++;
            this.headersDict["PLACENAME"] = cnt++;
            this.headersDict["STREET"] = cnt++;
            this.headersDict["HOUSENUM"] = cnt++;
            this.headersDict["LITER"] = cnt++;
            this.headersDict["HOUSING"] = cnt++;
            this.headersDict["BUILDING"] = cnt;

            return this.headersDict;
        }

        /// <inheritdoc />
        public Dictionary<int, string> InitNewHeader(Dictionary<int, string> dictionary, bool hasRoIdColumn)
        {
            var cnt = 0;
            dictionary[cnt++] = "№ П/П";

            // если столбец присутствует, сдвигаем все колонки на одну вправо
            if (hasRoIdColumn)
            {
                cnt++;
            }

            dictionary[cnt++] = "НАИМЕНОВАНИЕ МУНИЦИПАЛЬНОГО ОБРАЗОВАНИЯ";
            dictionary[cnt++] = "НАИМЕНОВАНИЕ НАСЕЛЕННОГО ПУНКТА";
            dictionary[cnt++] = "УЛИЦА";
            dictionary[cnt++] = "НОМЕР ДОМА";
            dictionary[cnt++] = "ЛИТЕР";
            dictionary[cnt++] = "КОРПУС";
            dictionary[cnt] = "СЕКЦИЯ";

            return dictionary;
        }

        /// <inheritdoc />
        public RecordProxy InitRecord(GkhExcelCell[] row, int rowNumber, bool hasRoIdColumn)
        {
            var record = new RecordProxy { RowNumber = rowNumber };

            record.LocalityName = this.Simplified(this.GetValue(row, "PLACENAME", hasRoIdColumn));
            record.StreetName = this.Simplified(this.GetValue(row, "STREET", hasRoIdColumn));
            record.Liter = this.Simplified(this.GetValue(row, "LITER", hasRoIdColumn));
            record.Housing = this.GetValue(row, "HOUSING", hasRoIdColumn);
            record.Building = this.Simplified(this.GetValue(row, "BUILDING", hasRoIdColumn));
            record.House = this.Simplified(this.GetValue(row, "HOUSENUM", hasRoIdColumn));
            return record;
        }

        public string Simplified(string initialString)
        {
            if (string.IsNullOrEmpty(initialString))
            {
                return initialString;
            }

            var trimmed = initialString.Trim();

            if (!trimmed.Contains(" "))
            {
                return trimmed;
            }

            var result = string.Join(" ", trimmed.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)));

            return result;
        }

        public string GetValue(GkhExcelCell[] data, string field, bool hasRoIdColumn)
        {
            var result = string.Empty;

            if (this.InitHeader(hasRoIdColumn).ContainsKey(field))
            {
                var index = this.InitHeader(hasRoIdColumn)[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result.Trim();
        }

    }
}