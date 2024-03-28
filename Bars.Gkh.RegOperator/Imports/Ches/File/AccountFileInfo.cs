namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.Modules.Caching;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Файл импорта для лицевых счетов
    /// </summary>
    public class AccountFileInfo : PeriodImportFileInfo<AccountFileInfo.Row>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="indicate"></param>
        public AccountFileInfo(FileType fileType, FileData fileData, ILogImport logImport, ChargePeriod period, Action<int, string> indicate)
            : base(fileType, fileData, logImport, period, indicate)
        {
            this.Provider = new DataProvider(
                new Dictionary<string, Tuple<int, bool>>
                {
                    {"HouseId", Tuple.Create(0, false)},
                    {"FiasHouseId", Tuple.Create(1, false)},
                    {"AddHouse", Tuple.Create(2, true)},
                    {"RoomAddress", Tuple.Create(3, false)},
                    {"RoomNumDate", Tuple.Create(4, false)},
                    {"RoomNumSet", Tuple.Create(5, false)},
                    {"TotalArea", Tuple.Create(6, false)},
                    {"TotalAreaDate", Tuple.Create(7, false)},
                    {"TotalAreaSet", Tuple.Create(8, false)},
                    {"LiveArea", Tuple.Create(9, false)},
                    {"FlatPlaceType", Tuple.Create(10, false)},
                    {"PropertyType", Tuple.Create(11, false)},
                    {"PropertyTypeDate", Tuple.Create(12, false)},
                    {"PropertyTypeSet", Tuple.Create(13, false)},
                    {"LsNum", Tuple.Create(14, true)},
                    {"ExtLsNum", Tuple.Create(15, true)},
                    {"BillType", Tuple.Create(16, true)},
                    {"Surname", Tuple.Create(17, false)},
                    {"Name", Tuple.Create(18, false)},
                    {"Lastname", Tuple.Create(19, false)},
                    {"BirthDate", Tuple.Create(20, false)},
                    {"Inn", Tuple.Create(21, false)},
                    {"Kpp", Tuple.Create(22, false)},
                    {"RenterName", Tuple.Create(23, false)},
                    {"BillTypeDate", Tuple.Create(24, false)},
                    {"BillTypeSet", Tuple.Create(25, false)},
                    {"Share", Tuple.Create(26, false)},
                    {"ShareDate", Tuple.Create(27, false)},
                    {"ShareSet", Tuple.Create(28, false)},
                    {"LsOpenDate", Tuple.Create(29, false)},
                    {"LsOpenStart", Tuple.Create(30, false)},
                    {"LsOpenSet", Tuple.Create(31, false)},
                    {"LsCloseDate", Tuple.Create(32, false)},
                    {"LsCloseStart", Tuple.Create(33, false)},
                    {"LsCloseSet", Tuple.Create(34, false)},
                    {"IsValid", Tuple.Create(35, false)},
                    {"Id", Tuple.Create(36, false)}
                },
                new Dictionary<string, int>
                {
                    { "LsNum", 20 },
                    { "RenterName", 300 }
                })
                .AddIndex("LsNum")
                .AddIndex("AddHouse");
        }

        /// <summary>
        /// Добавить строку в <see cref="Rows"/>
        /// </summary>
        /// <param name="data">Данные строки</param>
        /// <param name="rowNumber">Номер строки</param>
        /// <returns>Результат выполнения</returns>
        protected override bool AddRow(string[] data, int rowNumber)
        {
            var row = this.Provider.GetObject(data, rowNumber, this.AddRowExtractError);

            var successRead = false;
            if (row != null && row.IsValid)
            {
                successRead = true;
                row.RowNumber = rowNumber;

                if (row.LsCloseDate.HasValue && row.LsCloseDate.Value.IsValid() && !row.LsCloseStart.HasValue)
                {
                    this.AddFieldRequiredError("ls_close_start", row);
                    successRead = false;
                }

                if (successRead)
                {
                    this.Rows.Add(row);
                }
            }

            return successRead;
        }

        /// <inheritdoc />
        public override SummaryColumn[] GetSummaryColumns()
        {
            return new SummaryColumn[0];
        }

        /// <summary>
        /// Строка из файла импорта
        /// </summary>
        public class Row : IRow
        {
            private static readonly Regex roomRegex = new Regex(@"(?<RoomNum>[^,]+)(,к?\.?\s?(?<ChamberNum>\d+\s?\w*))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            private decimal? share;

            public int RowNumber { get; set; }
            public bool HasChangeAccount { get; set; }
            public bool HasChangeOwner { get; set; }
            public bool HasChangeRoom { get; set; }

            public string HouseId { get; set; }
            public string FiasHouseId { get; set; }
            public string AddHouse { get; set; }
            public string RoomAddress { get; set; }

            public string RoomNum => Row.roomRegex.Match(this.RoomAddress).Groups["RoomNum"]?.Value.Trim();
            public string ChamberNum => Row.roomRegex.Match(this.RoomAddress).Groups["ChamberNum"]?.Value.Trim();

            public DateTime RoomNumDate { get; set; }
            public DateTime RoomNumSet { get; set; }
            public decimal TotalArea { get; set; }
            public DateTime TotalAreaDate { get; set; }
            public DateTime TotalAreaSet { get; set; }
            public string LiveArea { get; set; }
            public RoomType FlatPlaceType { get; set; }
            public RoomOwnershipType PropertyType { get; set; }
            public DateTime PropertyTypeDate { get; set; }
            public DateTime PropertyTypeSet { get; set; }
            public string LsNum { get; set; }
            public string ExtLsNum { get; set; }
            public PersonalAccountOwnerType BillType { get; set; }
            public string Surname { get; set; }
            public string Name { get; set; }
            public string Lastname { get; set; }
            public DateTime? BirthDate { get; set; }
            public string Inn { get; set; }
            public string Kpp { get; set; }
            public string RenterName { get; set; }
            public DateTime? BillTypeDate { get; set; }
            public DateTime? BillTypeSet { get; set; }

            public decimal? Share
            {
                get
                {
                    // Если указана дата закрытия, то ЛС закрыт и доля собственности равна 0
                    if ((this.LsCloseDate ?? DateTime.MinValue).IsValid())
                    {
                        return 0;
                    }

                    return this.share;
                }
                set
                {
                    this.share = value;
                }
            }

            public DateTime? ShareDate { get; set; }
            public DateTime ShareSet { get; set; }
            public DateTime? LsOpenDate { get; set; }
            public DateTime? LsOpenStart { get; set; }
            public DateTime LsOpenSet { get; set; }
            public DateTime? LsCloseDate { get; set; }
            public DateTime? LsCloseStart { get; set; }
            public DateTime? LsCloseSet { get; set; }
            [Ignore]
            public bool IsValid { get; set; }
            [Ignore]
            [PrimaryKey]
            public long Id { get; set; }
        }
    }
}