namespace Bars.Gkh.Regions.Tyumen.DomainServices.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Import;
    using Bars.GkhExcel;

    public class ProgramCRImportRealityObjectService : IProgramCRImportRealityObject
    {
        private Dictionary<string, int> headersDict;
        
        /// <inheritdoc />
        public long GetRealityObjectAddress(List<RealtyObjectAddressProxy> address, RecordProxy record, ILogImport logImport)
        {
            int count = 0;
            var notDefinedLiter = FiasStructureTypeEnum.NotDefined.ToInt().ToString();
            if (record.Liter.IsNotEmpty())
            {
                count++;
                record.Building = record.Liter;
                record.Liter = FiasStructureTypeEnum.Letter.ToInt().ToString();
            }

            if (record.Structure.IsNotEmpty())
            {
                count ++;
                record.Building = record.Structure;
                record.Liter = FiasStructureTypeEnum.Structure.ToInt().ToString();
            }
            
            if (record.Construction.IsNotEmpty())
            {
                count ++;
                record.Building = record.Construction;
                record.Liter = FiasStructureTypeEnum.Construction.ToInt().ToString();
            }

            if (count == 0)
            {
                record.Liter = notDefinedLiter;
            }

            if (count > 1)
            {
                logImport.Error(
                    string.Empty,
                    $"Необходимо заполнить только один тип строения. Строка: {record.RowNumber}");
                return 0;
            }

            var result = address
                .WhereIf(record.Liter != notDefinedLiter, x => x.Liter == record.Liter)
                .WhereIf(record.Liter == notDefinedLiter, x => x.Liter == null || x.Liter == notDefinedLiter)
                .Where(x => x.Building.ToStr() == record.Building.ToStr())
                .Where(x => x.Housing.ToStr() == record.Housing.ToStr())
                .ToList();

            if (result.Count == 0)
            {
                logImport.Error(
                    string.Empty,
                    string.Format($"В системе не найден дом, соответствующий адресу: {record}. Строка: {0}", record.RowNumber));
                return 0;
            }

            if (result.Count > 1)
            {
                logImport.Error(
                    string.Empty,
                    string.Format($"В системе найдено несколько домов, соответствующих адресу: {record}. Строка: {0}", record.RowNumber));
                return 0;
            }

            return result.First().RealityObjectId;

        }

        /// <inheritdoc />
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
            this.headersDict["HOUSING"] = cnt++;
            this.headersDict["STRUCTURE"] = cnt++;
            this.headersDict["CONSTRUCTION"] = cnt++;
            this.headersDict["LITER"] = cnt++;

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
            dictionary[cnt++] = "КОРПУС";
            dictionary[cnt++] = "СТРОЕНИЕ";
            dictionary[cnt++] = "СООРУЖЕНИЕ";
            dictionary[cnt++] = "ЛИТЕР";

            return dictionary;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
    
        /// <inheritdoc />
        public RecordProxy InitRecord(GkhExcelCell[] row, int rowNumber, bool hasRoIdColumn)
        {
            var record = new RecordProxy { RowNumber = rowNumber };

            record.LocalityName = this.Simplified(this.GetValue(row, "PLACENAME", hasRoIdColumn));
            record.StreetName = this.Simplified(this.GetValue(row, "STREET", hasRoIdColumn));
            record.House = this.Simplified(this.GetValue(row, "HOUSENUM", hasRoIdColumn));
            record.Housing = this.GetValue(row, "HOUSING", hasRoIdColumn);
            record.Structure = this.Simplified(this.GetValue(row, "STRUCTURE", hasRoIdColumn));
            record.Construction = this.Simplified(this.GetValue(row, "CONSTRUCTION", hasRoIdColumn));
            record.Liter = this.Simplified(this.GetValue(row, "LITER", hasRoIdColumn));
            return record;
        }
    }
}