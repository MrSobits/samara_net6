namespace Bars.Gkh.RegOperator.Imports.DebtorClaimWork
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Import;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;

    partial class DebtorClaimWorkImport
    {
        private const string HeaderLs = "LS";

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

                            if (this.records.Any() && record.AccountNumber.IsNotEmpty() &&
                                this.records.Select(x => x.AccountNumber).Contains(record.AccountNumber))
                            {
                                this.AddLog(record.RowNumber, record.AccountNumber, $"Дубль лс в файле импорта", false);
                                continue;
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

        private DebtorClaimWorkRecord ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new DebtorClaimWorkRecord(rowNumber);

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
                ["LS"] = -1,
                ["AMOUNT_DUE"] = -1,
                ["AMOUNT_DUE_PENI"] = -1,
                ["COUNT_DATE"] = -1,
                ["TYPE_OF_APPLICATION"] = -1,
                ["APPLICATION_DATE"] = -1,
                ["AMOUNT_OF_DEBT"] = -1,
                ["AMOUNT_OF_PENI"] = -1
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

        private bool TryExtractDebtor(DebtorClaimWorkRecord record, GkhExcelCell[] data)
        {
            var errorList = new List<string>();

            var accountNumber = this.GetValue(data, "LS");
            if (accountNumber.IsEmpty())
            {
                errorList.Add("LS");
            }
            record.AccountNumber = accountNumber;

            var amount = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "AMOUNT_DUE"));
            if (amount == null)
            {
                errorList.Add("AMOUNT_DUE");
            }
            else
            {
                record.DebtSum = (decimal)amount;
            }

            amount = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "AMOUNT_DUE_PENI"));
            if (amount == null)
            {
                errorList.Add("AMOUNT_DUE_PENI");
            }
            else
            {
                record.PenaltyDebt = (decimal)amount;
            }

            var date = this.ToDateTimeNullable(this.GetValue(data, "COUNT_DATE"));
            if (date == null)
            {
                errorList.Add("COUNT_DATE");
            }
            else
            {
                record.StartDate = (DateTime)date;
            }

            ApplicationType applicationType;


            if (!Enum.TryParse(this.GetValue(data, "TYPE_OF_APPLICATION"), out applicationType) 
                || !Enum.IsDefined(typeof(ApplicationType), applicationType))
            {
                errorList.Add("TYPE_OF_APPLICATION");
            }
            else
            {
                record.ApplicationType = applicationType;

                if (applicationType != ApplicationType.NoDocuments)
                {
                    date = this.ToDateTimeNullable(this.GetValue(data, "APPLICATION_DATE"));
                    if (date == null)
                    {
                        errorList.Add("APPLICATION_DATE");
                    }
                    else
                    {
                        record.ApplicationDate = date;
                    }

                    amount = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "AMOUNT_OF_DEBT"));
                    if (amount == null)
                    {
                        errorList.Add("AMOUNT_OF_DEBT");
                    }
                    else
                    {
                        record.LawsuitDebtSum = amount;
                    }

                    amount = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "AMOUNT_OF_PENI"));
                    if (amount == null)
                    {
                        errorList.Add("AMOUNT_OF_PENI");
                    }
                    else
                    {
                        record.LawsuitPenaltyDebt = amount;
                    }
                }
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
                return rows.First(row => row.FirstOrDefault(cell => cell.Value == DebtorClaimWorkImport.HeaderLs) != null);
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

            DateTime date;
            if (!string.IsNullOrEmpty(obj) && DateTime.TryParseExact(obj, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }

            return defaultValue;
        }
    }
}