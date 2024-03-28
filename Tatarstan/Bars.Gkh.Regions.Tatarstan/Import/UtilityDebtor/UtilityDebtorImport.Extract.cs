namespace Bars.Gkh.Regions.Tatarstan.Import.UtilityDebtor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.GkhExcel;

    using Castle.Core.Internal;

    partial class UtilityDebtorImport
    {
        private const string HeaderDebtor = "DEBTOR";


        private void ProcessData(byte[] fileData, string fileExtention)
        {
            this.records.Clear();

            try
            {
                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new ImportException("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    if (fileExtention == "xlsx")
                    {
                        excel.UseVersionXlsx();
                    }

                    using (var memoryStreamFile = new MemoryStream(fileData))
                    {
                        memoryStreamFile.Seek(0, SeekOrigin.Begin);

                        excel.Open(memoryStreamFile);

                        var rows = excel.GetRows(0, 0);

                        // получаем строку с идентификаторами заголовков
                        var headerIdsRow = this.GetHeaderIdsRow(rows);

                        this.InitHeader(headerIdsRow);

                        // начинаем цикл с i = номер строки после строки с идентификаторами
                        var startIndex = rows.IndexOf(headerIdsRow) + 1;

                        for (var i = startIndex; i < rows.Count; ++i)
                        {
                            var record = this.ProcessLine(rows[i], i + 1);

                            if (record.IsValidRecord)
                            {
                                this.records.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Некорректный файл", ex);
            }
        }

        private GkhExcelCell[] GetHeaderIdsRow(IEnumerable<GkhExcelCell[]> rows)
        {
            try
            {
                return rows.First(row => row.FirstOrDefault(cell => cell.Value == HeaderDebtor) != null);
            }
            catch
            {
                throw new Exception("Импортируемый файл не соответствует формату");
            }
        }

        private void InitHeader(GkhExcelCell[] data)
        {
            this.headersDict = new Dictionary<string, int>
            {
                ["DEBTOR"] = -1,
                ["SUBSCRIBERTYPE"] = -1,
                ["BASEDEBT"] = -1,
                ["PENALTYDEBT"] = -1,
                ["DELAYDAYS"] = -1,
                ["ADDRESSFIAS"] = -1,
                ["OSP_SUBDIVISION"] = -1,
                ["REG_NUM"] = -1,
                ["EXCITATIONDATE"] = -1,
                ["COMPLETIONDATE"] = -1,
                ["CREDITOR"] = -1,
                ["CLAUSE"] = -1,
                ["PARAGRAPH"] = -1,
                ["SUBPARAGRAPH"] = -1,
                ["EXECUTIVE_DOC_NUM"] = -1,
                ["EXECUTIONSTATUS"] = -1,
                ["DEPARTURE_LIMIT_ACT"] = -1,
                ["DEPARTURE_LIMIT_ACT_DATE"] = -1,
                ["ARREST_PROPERTY_ACT"] = -1,
                ["ARREST_PROPERTY_ACT_DATE"] = -1
            };

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToUpper();
                if (this.headersDict.ContainsKey(header))
                {
                    this.headersDict[header] = index;
                }
            }
        }

        private UtilityDebtorRecord ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new UtilityDebtorRecord(rowNumber);

            if (data.Length <= 1)
            {
                return record;
            }

            record.IsValidRecord = this.TryExtractUtilityDebtor(record, data)
                && this.TryExtractExecutoryProcess(record, data)
                && this.TryExtractSeizureOfProperty(record, data)
                && this.TryExtractDepartureRestriction(record, data);

            return record;
        }

        private bool TryExtractUtilityDebtor(UtilityDebtorRecord record, GkhExcelCell[] data)
        {
            var houseGuid = ConvertHelper.ConvertTo<Guid?>(this.GetValue(data, "ADDRESSFIAS"));

            if (houseGuid == null || houseGuid.Equals(default(Guid)))
            {
                this.AddLog(record.RowNumber, "Не задан код ФИАС", false);
                return false;
            }

            record.HouseGuid = houseGuid;
            record.AccountOwner = this.GetValue(data, "DEBTOR");
            record.OwnerType = ConvertHelper.ConvertTo<OwnerType>(this.GetValue(data, "SUBSCRIBERTYPE"));

            record.ChargeDebt = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "BASEDEBT"));
            record.PenaltyDebt = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "PENALTYDEBT"));
            record.CountDaysDelay = ConvertHelper.ConvertTo<int?>(this.GetValue(data, "DELAYDAYS"));

            return true;
        }

        private bool TryExtractExecutoryProcess(UtilityDebtorRecord record, GkhExcelCell[] data)
        {
            var registrationNumber = this.GetValue(data, "REG_NUM");

            if (!registrationNumber.IsNullOrEmpty())
            {
                record.RegistrationNumber = registrationNumber;
                record.JurInstitutionCode = this.GetValue(data, "OSP_SUBDIVISION");
                record.DateStart = this.ToDateTimeNullable(this.GetValue(data, "EXCITATIONDATE"));
                record.DateEnd = this.ToDateTimeNullable(this.GetValue(data, "COMPLETIONDATE"));
                record.Creditor = this.GetValue(data, "CREDITOR");
                record.Clause = this.GetValue(data, "CLAUSE");
                record.Paragraph = this.GetValue(data, "PARAGRAPH");
                record.Subparagraph = this.GetValue(data, "SUBPARAGRAPH");
                record.DocumentNumber = this.GetValue(data, "EXECUTIVE_DOC_NUM");
                record.StateCode = this.GetValue(data, "EXECUTIONSTATUS");
            }

            return true;
        }

        private bool TryExtractSeizureOfProperty(UtilityDebtorRecord record, GkhExcelCell[] data)
        {
            var haveSeizureOfProperty = ConvertHelper.ConvertTo<bool>(this.GetValue(data, "ARREST_PROPERTY_ACT"));

            if (!record.RegistrationNumber.IsNullOrEmpty() && haveSeizureOfProperty)
            {
                record.HaveSeizureOfProperty = true;
                record.SeizureOfPropertyDocDate = this.ToDateTimeNullable(this.GetValue(data, "ARREST_PROPERTY_ACT_DATE"));
            }

            return true;
        }

        private bool TryExtractDepartureRestriction(UtilityDebtorRecord record, GkhExcelCell[] data)
        {
            var haveDepartureRestriction = ConvertHelper.ConvertTo<bool>(this.GetValue(data, "DEPARTURE_LIMIT_ACT"));

            if (!record.RegistrationNumber.IsNullOrEmpty() && haveDepartureRestriction)
            {
                record.HaveDepartureRestriction = true;
                record.SeizureOfPropertyDocDate = this.ToDateTimeNullable(this.GetValue(data, "DEPARTURE_LIMIT_ACT_DATE"));
            }

            return true;
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (this.headersDict.ContainsKey(field))
            {
                var index = this.headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result?.Trim() ?? string.Empty;
        }

        private DateTime? ToDateTimeNullable(string obj, DateTime? defaultValue = null, string format = "dd.MM.yyyy")
        {
            if (obj.Length > 10)
            {
                obj = obj.Substring(0, 10);
            }

            return string.IsNullOrEmpty(obj) ? defaultValue : DateTime.ParseExact(obj, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }
}
