using Bars.Gkh.Overhaul.Report.PublishedProgramReport;

namespace Bars.Gkh.Overhaul.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Pivot;
    using Bars.B4.Modules.Pivot.Enum;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class PublishedProgramReport : IPrintForm, IPivotModel
    {
        // параметры
        private long[] municipalityIds;
        private int startYear;
        private int endYear;

        public IWindsorContainer Container { get; set; }

        public string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.PublishedProgramReport";
            }
        }

        public IList<string> ReportFormats
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return "Отчет по опубликованной программе2";
            }
        }

        public string Desciption
        {
            get
            {
                return "Отчет по опубликованной программе";
            }
        }

        public string GroupName
        {
            get
            {
                return "Региональная программа";
            }
        }

        public string ParamsController
        {
            get
            {
                return "B4.controller.report.PublishedProgramReport";
            }
        }

        public string Params { get; set; }

        public object Data { get; set; }

        public void LoadData()
        {
            var dict = DynamicDictionary.FromString(Params);

            var municipalityIdsList = dict.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split("%2c+").Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            startYear = dict["startYear"].ToInt();
            endYear = dict["endYear"].ToInt();

            var recs = Container.Resolve<IPublishedProgramReportDataProvider>().GetData(municipalityIds, startYear, endYear);

            var data =
                recs.GroupBy(x => new { x.MuName, x.Settlement, x.Address, x.Ooi, x.Year })
                    .Select(y => new
                            {
                                y.Key.MuName,
                                y.Key.Settlement,
                                y.Key.Address,
                                y.Key.Year,
                                y.Key.Ooi,
                                TotalAreaMkd = y.Select(z => z.TotalAreaMkd).First(),
                                TotalAreaLivNotLiv = y.Select(z => z.TotalAreaLivNotLiv).First(),
                                TotalAreaLiving = y.Select(z => z.TotalAreaLiving).First(),
                                //CountRo = y.Select(x => x.House).Distinct().Count(),
                                CountOOi = y.Sum(x => x.Ooi.Count(z => z == ',') + 1),
                                Cost = y.Sum(x => x.Cost)
                            });

            var table = new DataTable();
            table.Columns.Add(new DataColumn("Year"));
            table.Columns.Add(new DataColumn("MUName"));
            table.Columns.Add(new DataColumn("Settlement"));
            table.Columns.Add(new DataColumn("Address"));
            table.Columns.Add(new DataColumn("TotalAreaMKD", typeof(decimal)));
            table.Columns.Add(new DataColumn("TotalAreaLivNotLiv", typeof(decimal)));
            table.Columns.Add(new DataColumn("TotalAreaLiving", typeof(decimal)));
            table.Columns.Add(new DataColumn("OOI"));
            table.Columns.Add(new DataColumn("HouseCount", typeof(int)));
            table.Columns.Add(new DataColumn("OOICount", typeof(int)));
            table.Columns.Add(new DataColumn("Cost", typeof(decimal)));

            foreach (var item in data)
            {
                var row = table.NewRow();

                row["MUName"] = item.MuName;
                row["Settlement"] = item.Settlement;
                row["Address"] = item.Address;
                row["TotalAreaMKD"] = item.TotalAreaMkd;
                row["TotalAreaLivNotLiv"] = item.TotalAreaLivNotLiv;
                row["TotalAreaLiving"] = item.TotalAreaLiving;
                row["OOI"] = item.Ooi;
                row["Year"] = item.Year;
                row["HouseCount"] = 1;
                row["OOICount"] = item.CountOOi;
                row["Cost"] = item.Cost;

                table.Rows.Add(row);
            }

            this.Data = table;

            Container.Release(recs);
        }

        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatString = "n", FormatType = FormatType.Numeric };
            var numericValueFormat = new ValueFormat {FormatString = "n", FormatType = FormatType.Numeric};

            var config = new PivotConfiguration
            {
                Name = "PublishedProgram",
                ModelName = "Report.PublishedProgramReport",
                CustomSummary = (sender, e) => CustomSummary.GetValue(e)
            };

            var fields = new List<Field>
            {
                new Field
                {
                    Name = "MUName",
                    Area = Area.RowArea,
                    AreaIndex = 0,
                    DisplayName = "Муниципальный район"
                },
                new Field
                {
                    Name = "Settlement",
                    Area = Area.RowArea,
                    AreaIndex = 1,
                    DisplayName = "Муниципальное образование"
                },
                new Field
                {
                    Name = "Address",
                    Area = Area.RowArea,
                    DisplayName = "Адрес объекта",
                    AreaIndex = 2
                },
                new Field
                {
                    Name = "TotalAreaMKD",
                    Area = Area.RowArea,
                    DisplayName = "Общая площадь МКД (кв.м.)",
                    AreaIndex = 3,
                    SummaryType = SummaryType.Sum,
                    ValueFormat = numericValueFormat
                },
                new Field
                {
                    Name = "TotalAreaLivNotLiv",
                    Area = Area.RowArea,
                    DisplayName = "Общая площадь жилых и нежилых помещений (кв.м.)",
                    AreaIndex = 4,
                    SummaryType = SummaryType.Sum,
                    ValueFormat = numericValueFormat
                },
                new Field
                {
                    Name = "TotalAreaLiving",
                    Area = Area.RowArea,
                    DisplayName = "Площадь жилых помещений (кв.м.)",
                    AreaIndex = 5,
                    SummaryType = SummaryType.Sum,
                    ValueFormat = numericValueFormat
                },
                new Field
                {
                    Name = "OOI",
                    Area = Area.RowArea,
                    DisplayName = "ООИ",
                    AreaIndex = 6
                },
                new Field
                {
                    Name = "Year",
                    Area = Area.ColumnArea,
                    DisplayName = "Год",
                    AreaIndex = 7
                },
                new Field
                {
                    Name = "HouseCount",
                    Area = Area.DataArea,
                    DisplayName = "Количество домов",
                    AreaIndex = 8,
                    SummaryType = SummaryType.Custom
                },
                new Field
                {
                    Name = "OOICount",
                    Area = Area.DataArea,
                    DisplayName = "Количество ООИ",
                    AreaIndex = 9,
                    SummaryType = SummaryType.Sum
                },
                new Field
                {
                    Name = "Cost",
                    Area = Area.DataArea,
                    DisplayName = "Стоимость, руб.",
                    AreaIndex = 10,
                    SummaryType = SummaryType.Sum,
                    CellFormat = numericCellFormat,
                    ValueFormat = numericValueFormat
                }
            };

            config.Fields = fields;
            return config;
        }

        public Stream GetTemplate()
        {
            throw new NotImplementedException();
        }

        public void PrepareReport(ReportParams reportParams)
        {
            throw new NotImplementedException();
        }

        public void SetUserParams(BaseParams baseParams)
        {
        }

        public string ReportGenerator { get; set; }
    }
}