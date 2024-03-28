namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Import;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;

    partial class AgentPIRDebtorCreditImport
    {
        private const string HeaderLs = "acc_number";

        private string _fileName;
        private byte[] _fileData;
        private string _fileExtention;

        private void ProcessData(string fileName, byte[] fileData, string fileExtention)
        {
            this.records.Clear();

            _fileName = fileName;
            _fileData = fileData;
            _fileExtention = fileExtention;

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

        private AgentPIRDebtorCreditRecord ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new AgentPIRDebtorCreditRecord(rowNumber);

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
                ["credit"] = -1,
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

        private bool TryExtractDebtor(AgentPIRDebtorCreditRecord record, GkhExcelCell[] data)
        {
            var errorList = new List<string>();

            var fileManager = this.Container.Resolve<IFileManager>();

            var accountNumber = this.GetValue(data, "acc_number");
            if (accountNumber.IsEmpty())
            {
                errorList.Add("acc_number");
            }
            else
            {
                record.AccountNumber = accountNumber;
            }

            var amount = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "credit"));
            if (amount == null)
            {
                errorList.Add("cerdit");
            }
            else
            {
                record.Credit = (decimal)amount;
            }

            record.Date = DateTime.Now;
            record.User = UserManager.GetActiveUser().Name;
            record.File = fileManager.SaveFile(_fileName, _fileExtention, _fileData);

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
                return rows.First(row => row.First(cell => cell.Value == AgentPIRDebtorCreditImport.HeaderLs) != null);
            }
            catch
            {
                throw new Exception("Импортируемый файл не соответствует формату");
            }
        }
    }
}