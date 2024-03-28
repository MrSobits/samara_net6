namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;

    partial class AgentPIRDocumentImport
    {
        private const string HeaderLs = "acc_number";

        private void ProcessData(byte[] fileData, string fileExtention)
        {
            this.records.Clear();

            try
            {
                using (var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
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

                            if (this.records.Any() && record.Number.IsNotEmpty() &&
                                this.records.Select(x => x.Number).Contains(record.Number))
                            {
                                this.AddLog(record.RowNumber, record.Number, $"Дубль номера документа в файле импорта", false);
                            }

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

        private AgentPIRDocumentRecord ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new AgentPIRDocumentRecord(rowNumber);

            if (data.Length <= 1)
            {
                return record;
            }

            record.IsValidRecord = this.TryExtractDebtor(record, data);
            return record;
        }

        private void InitHeader(GkhExcelCell[] data)
        {
            this.headersDict = new Dictionary<string, int>
            {
                ["acc_number"] = -1,
                ["street"] = -1,
                ["house"] = -1,
                ["corp"] = -1,
                ["flat"] = -1,
                ["sum_debt"] = -1,
                ["sum_peni"] = -1,
                ["sum_duty"] = -1,
                ["fio"] = -1,
                ["number"] = -1,
                ["date"] = -1,
                ["type"] = -1,
                ["enum_type"] = -1
            };

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToLower();
                if (this.headersDict.ContainsKey(header))
                {
                    this.headersDict[header] = index;
                }
            }
        }

        private bool TryExtractDebtor(AgentPIRDocumentRecord record, GkhExcelCell[] data)
        {
            var errorList = new List<string>();

            var accountNumber = this.GetValue(data, "acc_number");
            if (accountNumber.IsEmpty())
            {
                errorList.Add("acc_number");
            }
            record.AccountNumber = accountNumber;

            var text = this.GetValue(data, "street");
            if (text == null)
            {
                errorList.Add("street");
            }
            else
            {
                record.Street = text;
            }

            text = this.GetValue(data, "house");
            if (text == null)
            {
                errorList.Add("house");
            }
            else
            {
                record.House = text;
            }

            text = this.GetValue(data, "corp");
            if (text == null)
            {
                errorList.Add("corp");
            }
            else
            {
                record.Corp = text;
            }

            text = this.GetValue(data, "flat");
            if (text == null)
            {
                errorList.Add("flat");
            }
            else
            {
                record.Flat = text;
            }

            var amount = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "sum_debt"));
            if (amount == null)
            {
                errorList.Add("sum_debt");
            }
            else
            {
                record.DebtSum = (decimal)amount;
            }

            amount = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "sum_peni"));
            if (amount == null)
            {
                errorList.Add("sum_peni");
            }
            else
            {
                record.PeniSum = (decimal)amount;
            }

            amount = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "sum_duty"));
            if (amount == null)
            {
                errorList.Add("sum_duty");
            }
            else
            {
                record.Duty = (decimal)amount;
            }

            text = this.GetValue(data, "fio");
            if (text == null)
            {
                errorList.Add("fio");
            }
            else
            {
                record.FIO = text;
            }

            text = this.GetValue(data, "number");
            if (text == null)
            {
                errorList.Add("number");
            }
            else
            {
                record.Number = text;
            }

            var date = this.ToDateTimeNullable(this.GetValue(data, "date"));
            if (date == null)
            {
                errorList.Add("date");
            }
            else
            {
                record.DocumentDate = (DateTime)date;
            }


            if (!Enum.TryParse(this.GetValue(data, "enum_type"), out AgentPIRDocumentType documentType) || !Enum.IsDefined(typeof(AgentPIRDocumentType), documentType))
            {
                errorList.Add("enum_type");
            }
            else
            {
                record.DocumentType = documentType;
            }

            if (errorList.Count > 0)
            {
                this.AddLog(record.RowNumber, accountNumber, $"Не заполнены обязательные поля: {errorList.AggregateWithSeparator(",")}", false);
                return false;
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

        private GkhExcelCell[] GetHeaderIdsRow(IEnumerable<GkhExcelCell[]> rows)
        {
            try
            {
                return rows.First(row => row.First(cell => cell.Value == AgentPIRDocumentImport.HeaderLs) != null);
            }
            catch
            {
                throw new Exception("Импортируемый файл не соответствует формату");
            }
        }

        private DateTime? ToDateTimeNullable(string obj, DateTime? defaultValue = null, string format = "dd.MM.yyyy")
        {
            if (obj.Length > 10)
            {
                obj = obj.Substring(0, 10);
            }

            if (!string.IsNullOrEmpty(obj) && DateTime.TryParseExact(obj, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }

            return defaultValue;
        }
    }
}