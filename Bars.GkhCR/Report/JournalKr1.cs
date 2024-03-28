namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Pivot;
    using Bars.B4.Modules.Pivot.Enum;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Процент отставания и выполнения работ, информация по приборам учета (Журнал ч.1)
    /// </summary>
    public class JournalKr1 : IPrintForm, IPivotModel
    {
        public IWindsorContainer Container { get; set; }

        private int programCrId;
        private DateTime reportDate = DateTime.MinValue;

        public string Name 
        { 
            get
            {
                return "Процент отставания и выполнения работ, информация по приборам учета (Журнал ч.1)";
            }
        }

        public IList<string> ReportFormats { get; private set; }

        public string Desciption
        {
            get
            {
                return "Процент отставания и выполнения работ, информация по приборам учета (Журнал ч.1)";
            }
        }

        public string GroupName
        {
            get
            {
                return "ГЖИ";
            }
        }

        public string ParamsController 
        { 
            get
            {
                return "B4.controller.report.JournalKr1";
            } 
        }

        public string RequiredPermission 
        {
            get
            {
                return "Reports.CR.JournalKr1";
            }
        }

        public string Params { get; set; }

        public object Data { get; set; }

        public void LoadData()
        {
            var dict = DynamicDictionary.FromString(Params);

            var dateReport = Uri.UnescapeDataString(dict["reportDate"].ToString()).ToDateTime();

            programCrId = dict["programCrId"].ToInt();
            reportDate = dateReport != DateTime.MinValue ? dateReport : DateTime.Now;

            //Если собирают отчет с дато1 >= 2013 года то тянем через Архив СМР
            //Иначе берем просто последние записи
            var data = reportDate.Date.Year >= 2013
                ? Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                    .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId)
                    .Where(x => x.ObjectCreateDate <= reportDate)
                    .Select(x => new
                    {
                        typeWorkId = x.TypeWorkCr.Id,
                        x.TypeWorkCr.DateStartWork,
                        x.TypeWorkCr.DateEndWork,
                        x.TypeWorkCr.ObjectCr.GjiNum,
                        x.TypeWorkCr.ObjectCr.RealityObject.Address,
                        x.TypeWorkCr.Sum,
                        x.TypeWorkCr.Volume,
                        Municipality = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Name,
                        RealObjId = x.TypeWorkCr.ObjectCr.RealityObject.Id,
                        FinanceSourceId = x.TypeWorkCr.FinanceSource.Id,
                        FinanceSourceName = x.TypeWorkCr.FinanceSource.Name,
                        WorkId = x.TypeWorkCr.Work.Id,
                        WorkName = x.TypeWorkCr.Work.Name,
                        WorkCode = x.TypeWorkCr.Work.Code,
                        x.Id,
                        x.CostSum,
                        x.VolumeOfCompletion,
                        x.PercentOfCompletion,
                        x.ObjectCreateDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.typeWorkId)
                    .Select(x => x.OrderByDescending(y => y.ObjectCreateDate).First())
                    .ToList()
                : Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                    .Select(x => new
                    {
                        typeWorkId = x.Id,
                        x.DateStartWork,
                        x.DateEndWork,
                        x.ObjectCr.GjiNum,
                        x.ObjectCr.RealityObject.Address,
                        x.Sum,
                        x.Volume,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        RealObjId = x.ObjectCr.RealityObject.Id,
                        FinanceSourceId = x.FinanceSource.Id,
                        FinanceSourceName = x.FinanceSource.Name,
                        WorkId = x.Work.Id,
                        WorkName = x.Work.Name,
                        WorkCode = x.Work.Code,
                        x.Id,
                        x.CostSum,
                        x.VolumeOfCompletion,
                        x.PercentOfCompletion,
                        x.ObjectCreateDate
                    })
                    .ToList();
            
            var recs = data
                .Select(x => 
                {
                    var dateStart = x.DateStartWork.ToDateTime();
                    var dateEnd = x.DateEndWork.ToDateTime();

                    var percentGraphic = dateStart != DateTime.MinValue 
                                            && dateEnd != DateTime.MinValue
                                            && dateStart != dateEnd
                                            && dateStart.Date < reportDate.Date
                                                 ? ((reportDate.Date - dateStart.Date).TotalDays / ((dateEnd.Date - dateStart.Date).TotalDays + 1)).ToDecimal()
                                                 : 0m;

                    percentGraphic = percentGraphic > 1 ? 1 : percentGraphic;

                    var percentFact = (x.PercentOfCompletion ?? 0) / 100;

                    var percentReal = percentFact > 1 ? 1 : percentFact;

                    var percentLag = percentReal == 1 || percentReal > percentGraphic
                                         ? 0
                                         : (percentGraphic - percentReal);

                    return new
                    {
                        x.Municipality,
                        x.GjiNum,
                        x.Address,
                        x.FinanceSourceId,
                        x.FinanceSourceName,
                        x.WorkId,
                        x.WorkName,
                        x.WorkCode,
                        x.Sum,
                        x.Volume,
                        DateStart = x.DateStartWork.ToDateTime(),
                        DateEnd = x.DateEndWork.ToDateTime(),
                        x.Id,
                        x.CostSum,
                        x.VolumeOfCompletion,
                        PercentOfCompletion = percentFact,
                        x.RealObjId,
                        x.ObjectCreateDate,
                        WorkCount = 1,
                        WorkComplete = x.PercentOfCompletion == 100 ? 1 : 0,
                        VolumeOfComplete100 = x.PercentOfCompletion >= 100 ? x.VolumeOfCompletion : 0,
                        PercentScheduler = percentGraphic,
                        PercentLag = percentLag
                    };
                })
                .ToList();

            var table = new DataTable();

            table.Columns.Add(new DataColumn("Municipality"));
            table.Columns.Add(new DataColumn("GjiNum"));
            table.Columns.Add(new DataColumn("Address"));
            table.Columns.Add(new DataColumn("FinanceSourceId"));
            table.Columns.Add(new DataColumn("FinanceSourceName"));
            table.Columns.Add(new DataColumn("WorkId"));
            table.Columns.Add(new DataColumn("WorkName"));
            table.Columns.Add(new DataColumn("WorkCode"));
            table.Columns.Add(new DataColumn("Sum", typeof(decimal)));
            table.Columns.Add(new DataColumn("Volume", typeof(decimal)));
            table.Columns.Add(new DataColumn("Id"));
            table.Columns.Add(new DataColumn("DateStart", typeof(DateTime)));
            table.Columns.Add(new DataColumn("DateEnd", typeof(DateTime)));
            table.Columns.Add(new DataColumn("CostSum", typeof(decimal)));
            table.Columns.Add(new DataColumn("VolumeOfCompletion", typeof(decimal)));
            table.Columns.Add(new DataColumn("PercentOfCompletion", typeof(decimal)));
            table.Columns.Add(new DataColumn("RealObjId"));
            table.Columns.Add(new DataColumn("ObjectCreateDate"));
            table.Columns.Add(new DataColumn("WorkCount", typeof(int)));
            table.Columns.Add(new DataColumn("WorkComplete", typeof(int)));
            table.Columns.Add(new DataColumn("VolumeOfComplete100", typeof(decimal)));
            table.Columns.Add(new DataColumn("PercentScheduler", typeof(decimal)));
            table.Columns.Add(new DataColumn("PercentLag", typeof(decimal)));

            foreach (var rec in recs)
            {
                var record = table.NewRow();

                record["Municipality"] = rec.Municipality;
                record["GjiNum"] = rec.GjiNum;
                record["Address"] = rec.Address;
                record["FinanceSourceId"] = rec.FinanceSourceId;
                record["FinanceSourceName"] = rec.FinanceSourceName;
                record["WorkId"] = rec.WorkId;
                record["WorkName"] = rec.WorkName;
                record["WorkCode"] = rec.WorkCode;
                record["Sum"] = rec.Sum.GetValueOrDefault();
                record["Volume"] = rec.Volume.GetValueOrDefault();
                record["Id"] = rec.Id;
                record["DateStart"] = rec.DateStart;
                record["DateEnd"] = rec.DateStart;
                record["CostSum"] = rec.CostSum.GetValueOrDefault();
                record["VolumeOfCompletion"] = rec.VolumeOfCompletion.GetValueOrDefault();
                record["PercentOfCompletion"] = rec.PercentOfCompletion;
                record["RealObjId"] = rec.RealObjId;
                record["ObjectCreateDate"] = rec.ObjectCreateDate;
                record["WorkCount"] = rec.WorkCount;
                record["WorkComplete"] = rec.WorkComplete;
                record["VolumeOfComplete100"] = rec.VolumeOfComplete100.GetValueOrDefault();
                record["PercentScheduler"] = rec.PercentScheduler;
                record["PercentLag"] = rec.PercentLag;

                table.Rows.Add(record);
            }

            Data = recs;
        }

        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatString = "n", FormatType = FormatType.Numeric };
            var datetimeCellFormat = new CellFormat { FormatString = "d", FormatType = FormatType.DateTime };
            var percentCellFormat = new CellFormat { FormatString = "p", FormatType = FormatType.Numeric };

            var datetimeValueFormat = new ValueFormat { FormatString = "d", FormatType = FormatType.DateTime };

            return new PivotConfiguration
                {
                    Name = "JournalKr1",
                    ModelName = "CR Report.JournalKr1",
                    Fields = new List<Field>
                        {
                            // Строки
                            new Field
                                {
                                    Area = Area.RowArea,
                                    AreaIndex = 0,
                                    Name = "Municipality",
                                    DisplayName = "Муниципальное образование"
                                },
                            new Field
                                {
                                    Area = Area.RowArea,
                                    AreaIndex = 1,
                                    Name = "Address",
                                    DisplayName = "Адрес"
                                },
                            new Field
                                {
                                    Area = Area.RowArea,
                                    AreaIndex = 2,
                                    Name = "GjiNum",
                                    DisplayName = "Реестровый номер ГЖИ"
                                },

                            // Столбцы
                            new Field
                                {
                                    Area = Area.ColumnArea,
                                    AreaIndex = 0,
                                    Name = "FinanceSourceName",
                                    DisplayName = "Разрез финансирования"
                                },
                            new Field
                                {
                                    Area = Area.ColumnArea,
                                    AreaIndex = 1,
                                    Name = "WorkCode",
                                    DisplayName = "Код работы"
                                },
                            new Field
                                {
                                    Area = Area.ColumnArea,
                                    AreaIndex = 2,
                                    Name = "WorkName",
                                    DisplayName = "Виды работ"
                                },

                            // Значения
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 1,
                                    Name = "VolumeOfCompletion",
                                    DisplayName = "Факт (кв.м)",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 2,
                                    Name = "CostSum",
                                    DisplayName = "Факт (руб.)",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 3,
                                    Name = "PercentOfCompletion",
                                    DisplayName = "Факт (%)",
                                    SummaryType = SummaryType.Average,
                                    CellFormat = percentCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 4,
                                    Name = "WorkCount",
                                    DisplayName = "Количество работ",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 5,
                                    Name = "WorkComplete",
                                    DisplayName = "Завершено работ",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 6,
                                    Name = "VolumeOfCompletion",
                                    DisplayName = "Завершено объем",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 7,
                                    Name = "VolumeOfComplete100",
                                    DisplayName = "Завершено объем 100%",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    AreaIndex = 8,
                                    Name = "Volume",
                                    DisplayName = "Смета(кв.м.)",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    AreaIndex = 9,
                                    Name = "Sum",
                                    DisplayName = "Смета(руб.)",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    AreaIndex = 10,
                                    Name = "PercentScheduler",
                                    DisplayName = "% выполнения по графику",
                                    SummaryType = SummaryType.Average,
                                    CellFormat = percentCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    AreaIndex = 11,
                                    Name = "PercentLag",
                                    DisplayName = "% отставания от графика",
                                    SummaryType = SummaryType.Average,
                                    CellFormat = percentCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    AreaIndex = 12,
                                    Name = "DateStart",
                                    DisplayName = "Начало работ",
                                    SummaryType = SummaryType.Min,
                                    CellFormat = datetimeCellFormat,
                                    ValueFormat = datetimeValueFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    AreaIndex = 13,
                                    Name = "DateEnd",
                                    DisplayName = "Окончание работ",
                                    SummaryType = SummaryType.Max,
                                    CellFormat = datetimeCellFormat,
                                    ValueFormat = datetimeValueFormat
                                }
                        }
                };
        }

        public Stream GetTemplate()
        {
            throw new System.NotImplementedException();
        }

        public void PrepareReport(ReportParams reportParams)
        {
            throw new System.NotImplementedException();
        }

        public void SetUserParams(BaseParams baseParams)
        {
        }

        public string ReportGenerator { get; set; }
    }
}