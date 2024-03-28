namespace Bars.Gkh.RegOperator.Imports.OwnerRoom
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
    using Bars.Gkh.RegOperator.Enums;

    using GkhExcel;
 
    partial class OwnerRoomImport
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
                                this.AddLog(record.RowNumber, $"Дубль ЛС {record.AccountNumber}", false);
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

        private RoomRecord ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new RoomRecord(rowNumber);

            if (data.Length <= 1)
            {
                return record;
            }

            record.IsValidRecord = this.TryExtractPersonalAccount(record, data) 
                && this.TryExtractRoomInfo(record, data) 
                && this.TryExtractOwner(record, data);

            return record;
        }

        private GkhExcelCell[] GetHeaderIdsRow(IEnumerable<GkhExcelCell[]> rows)
        {
            try
            {
                return rows.First(row => row.FirstOrDefault(cell => cell.Value.ToUpper() == HeaderLs) != null);
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
                ["LS"] = -1,
                ["TOTAL_AREA"] = -1,
                ["TOTAL_AREA_DATE"] = -1,
                ["LIVE_AREA"] = -1,
                ["FLAT_PLACE_TYPE"] = -1,
                ["PROPERTY_TYPE"] = -1,
                ["PROPERTY_TYPE_DATE"] = -1,
                ["BILL_TYPE"] = -1,
                ["SURNAME"] = -1,
                ["NAME"] = -1,
                ["LASTNAME"] = -1,
                ["BIRTH_DATE"] = -1,
                ["INN"] = -1,
                ["KPP"] = -1,
                ["RENTER_NAME"] = -1,
                ["SHARE"] = -1,
                ["SHARE_DATE"] = -1,
                ["LS_DATE"] = -1,
                ["LS_DATE_DATE"] = -1,
                ["RKC_LS_NUM"] = -1,
                ["RKC_LS_NUM_DATE"] = -1
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

        private bool TryExtractPersonalAccount(RoomRecord record, GkhExcelCell[] data)
        {
            var accountNumber = this.GetValue(data, "LS");

            if (accountNumber.IsEmpty())
            {
                this.AddLog(record.RowNumber, "Не задан номер ЛС", false);
                return false;
            }

            record.AccountNumber = accountNumber;

            record.AreaShare = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "SHARE"));
            record.AreaShareDate = this.ToDateTimeNullable(this.GetValue(data, "SHARE_DATE"));
            record.PersAccNumExternalSystems = this.GetValue(data, "RKC_LS_NUM");
            record.PersAccNumExternalSystemsDate = this.ToDateTimeNullable(this.GetValue(data, "RKC_LS_NUM_DATE"));
            record.AccountCreateDate = this.ToDateTimeNullable(this.GetValue(data, "LS_DATE"));
            record.AccountCreateDateStart = this.ToDateTimeNullable(this.GetValue(data, "LS_DATE_DATE"));

            return true;
        }

        private bool TryExtractRoomInfo(RoomRecord record, GkhExcelCell[] data)
        {
            record.Area = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "TOTAL_AREA"));
            record.AreaDate = this.ToDateTimeNullable(this.GetValue(data, "TOTAL_AREA_DATE"));

            record.LivingArea = ConvertHelper.ConvertTo<decimal?>(this.GetValue(data, "LIVE_AREA"));

            var roomType = this.GetValue(data, "FLAT_PLACE_TYPE");
            switch (roomType.ToLower())
            {
                case "жилое":
                    record.RoomType = RoomType.Living;
                    break;

                case "нежилое":
                    record.RoomType = RoomType.NonLiving;
                    break;
            }

            var ownershipType = this.GetValue(data, "PROPERTY_TYPE");
            switch (ownershipType.ToLower())
            {
                case "частная":
                    record.OwnershipType = RoomOwnershipType.Private;
                    break;

                case "муниципальная":
                    record.OwnershipType = RoomOwnershipType.Municipal;
                    break;

                case "государственная":
                    record.OwnershipType = RoomOwnershipType.Goverenment;
                    break;

                case "коммерческая":
                    record.OwnershipType = RoomOwnershipType.Commerse;
                    break;
            }

            record.OwnershipTypeDate = this.ToDateTimeNullable(this.GetValue(data, "PROPERTY_TYPE_DATE"));

            return true;
        }

        private bool TryExtractOwner(RoomRecord record, GkhExcelCell[] data)
        {
            var ownerType = this.GetValue(data, "BILL_TYPE");

            if (string.IsNullOrEmpty(ownerType))
            {
                this.AddLog(record.RowNumber, "Не задан тип счета", false);
                return false;
            }

            switch (ownerType.ToLower())
            {
                case "счет физ.лица":
                    record.OwnerType = PersonalAccountOwnerType.Individual;
                    break;

                case "счет юр.лица":
                    record.OwnerType = PersonalAccountOwnerType.Legal;
                    break;

                default:
                    this.AddLog(record.RowNumber, "Неизвестный тип собственника: " + ownerType, false);
                    return false;
            }

            if (record.OwnerType == PersonalAccountOwnerType.Individual)
            {
                record.OwnerPhysSurname = this.GetValue(data, "SURNAME");
                record.OwnerPhysFirstName = this.GetValue(data, "NAME");
                record.OwnerPhysSecondName = this.GetValue(data, "LASTNAME");
                record.BirthDate = this.ToDateTimeNullable(this.GetValue(data, "BIRTH_DATE"));
            }
            else
            {
                record.OwnerJurInn = this.GetValue(data, "INN");
                record.OwnerJurKpp = this.GetValue(data, "KPP");
                record.OwnerJurName = this.GetValue(data, "RENTER_NAME");
            }

            return true;
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            field = field.ToUpper();
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