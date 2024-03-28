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
    using Bars.GkhCr.Report.JournalKr34;

    using Castle.Windsor;

    /// <summary>
    /// Отчет Информация по начатым и завершенным капремонтом домам (Журнал ч.3,4)
    /// </summary>
    public class JournalKr34Report : IPrintForm, IPivotModel
    {
        private int programCrId;

        private DateTime reportDate = DateTime.Now;

        private int typeWork;

        public IWindsorContainer Container { get; set; }

        public string Name
        {
            get { return "Отчет Информация по начатым и завершенным капремонтом домам (Журнал ч.3,4)"; }
        }

        public IList<string> ReportFormats { get; private set; }

        public string Desciption
        {
            get { return "Отчет Информация по начатым и завершенным капремонтом домам (Журнал ч.3,4)"; }
        }

        public string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public string ParamsController
        {
            get { return "B4.controller.report.JournalKr34"; }
        }

        public string RequiredPermission
        {
            get
            {
                return "Reports.CR.JournalKr34";
            }
        }

        public string Params { get; set; }

        public object Data { get; set; }

        public void LoadData()
        {
            var table = new DataTable();

            table.Columns.Add(new DataColumn("Municipality"));
            table.Columns.Add(new DataColumn("GjiNum"));
            table.Columns.Add(new DataColumn("Address"));
            table.Columns.Add(new DataColumn("WorkName"));
            table.Columns.Add(new DataColumn("WorkCode"));

            table.Columns.Add(new DataColumn("DateStartWork", typeof(DateTime)));
            table.Columns.Add(new DataColumn("DateEndWork", typeof(DateTime)));

            table.Columns.Add(new DataColumn("InProg", typeof(int)));
            table.Columns.Add(new DataColumn("InProgHouse", typeof(int)));

            table.Columns.Add(new DataColumn("MustPlanWork", typeof(int)));
            table.Columns.Add(new DataColumn("MustFactWork", typeof(int)));
            table.Columns.Add(new DataColumn("NotMustFactWork", typeof(int)));
            table.Columns.Add(new DataColumn("MustNotFactWork", typeof(int)));
            table.Columns.Add(new DataColumn("NotMustNotFactWork", typeof(int)));
            table.Columns.Add(new DataColumn("FactWork", typeof(int)));

            table.Columns.Add(new DataColumn("MustPlanHouse", typeof(int)));
            table.Columns.Add(new DataColumn("MustFactHouse", typeof(int)));
            table.Columns.Add(new DataColumn("NotMustFactHouse", typeof(int)));
            table.Columns.Add(new DataColumn("MustNotFactHouse", typeof(int)));
            table.Columns.Add(new DataColumn("NotMustNotFactHouse", typeof(int)));
            table.Columns.Add(new DataColumn("FactHouse", typeof(int)));

            var dict = DynamicDictionary.FromString(Params);
            programCrId = dict["programCrId"].ToInt();
            typeWork = dict["typeWork"].ToInt();

            var date = dict.ContainsKey("reportDate") ? Uri.UnescapeDataString(dict["reportDate"].ToString()).ToDateTime() : DateTime.MinValue;
            reportDate = date != DateTime.MinValue ? date : DateTime.Now.Date;

            var recs = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .Select(x => new
                    {
                        TypeWorkId = x.Id,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        x.ObjectCr.GjiNum,
                        x.ObjectCr.RealityObject.Address,
                        RealObjId = x.ObjectCr.RealityObject.Id,
                        WorkId = x.Work.Id,
                        WorkName = x.Work.Name,
                        WorkCode = x.Work.Code,
                        x.DateStartWork,
                        x.DateEndWork,
                        x.PercentOfCompletion
                    })
                .ToList();
            
            // Если собирают отчет с дато1 >= 2013 года то тянем через Архив СМР
            // Иначе берем просто последние записи
            if (reportDate.Date.Year >= 2013)
            {
                var archiveDict = Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                    .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId)
                    .Where(x => x.DateChangeRec <= reportDate)
                    .Select(x => new
                        {
                            TypeWorkId = x.TypeWorkCr.Id,
                            x.PercentOfCompletion,
                            x.ObjectCreateDate
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.TypeWorkId)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ObjectCreateDate).First().PercentOfCompletion);

                recs = recs.Select(x => new
                    {
                        x.TypeWorkId,
                        x.Municipality,
                        x.GjiNum,
                        x.Address,
                        x.RealObjId,
                        x.WorkId,
                        x.WorkName,
                        x.WorkCode,
                        x.DateStartWork,
                        x.DateEndWork,
                        PercentOfCompletion = archiveDict.ContainsKey(x.TypeWorkId) ? archiveDict[x.TypeWorkId] : x.PercentOfCompletion
                    })
                    .ToList();
            }

            var dictTypeWork = new Dictionary<long, bool>();

            foreach (var rec in recs)
            {
                if (dictTypeWork.ContainsKey(rec.TypeWorkId))
                    continue;

                dictTypeWork.Add(rec.TypeWorkId, true);

                var record = table.NewRow();

                record["Municipality"] = rec.Municipality;
                record["GjiNum"] = rec.GjiNum;
                record["Address"] = rec.Address;
                record["WorkName"] = rec.WorkName;
                record["WorkCode"] = rec.WorkCode;
                record["InProg"] = 1;
                record["InProgHouse"] = 0;
                record["MustPlanHouse"] = 0;
                record["MustFactHouse"] = 0;
                record["NotMustFactHouse"] = 0;
                record["MustNotFactHouse"] = 0;
                record["NotMustNotFactHouse"] = 0;
                record["FactHouse"] = 0;

                var dateStartWork = rec.DateStartWork.GetValueOrDefault(DateTime.MinValue);
                var dateEndWork = rec.DateEndWork.GetValueOrDefault(DateTime.MinValue);
                var percentOfCompletion = rec.PercentOfCompletion.GetValueOrDefault(0m);

                if (dateStartWork > DateTime.MinValue)
                {
                    record["DateStartWork"] = dateStartWork;
                }

                if (dateEndWork > DateTime.MinValue)
                {
                    record["DateEndWork"] = dateEndWork;
                }

                if (typeWork == 0)
                {
                    // Расчитываем для ЖурналХодаКапремонта3
                    if (dateStartWork != DateTime.MinValue)
                    {
                        record["MustPlanWork"] = dateStartWork <= reportDate ? 1 : 0;
                        record["MustFactWork"] = percentOfCompletion > 0 && dateStartWork <= reportDate ? 1 : 0;
                        record["NotMustFactWork"] = percentOfCompletion > 0 && dateStartWork > reportDate ? 1 : 0;
                        record["MustNotFactWork"] = percentOfCompletion == 0 && dateStartWork <= reportDate ? 1 : 0;
                        record["NotMustNotFactWork"] = percentOfCompletion == 0 && dateStartWork > reportDate ? 1 : 0;

                        // значения аналогичны must_plan_work и must_notfact_work, для упрощения customSummary
                        record["MustPlanHouse"] = dateStartWork <= reportDate ? 1 : 0;
                        record["MustFactHouse"] = percentOfCompletion > 0 && dateStartWork <= reportDate ? 1 : 0;
                        record["NotMustFactHouse"] = percentOfCompletion > 0 && dateStartWork > reportDate ? 1 : 0;
                        record["MustNotFactHouse"] = percentOfCompletion == 0 && dateStartWork <= reportDate ? 1 : 0;
                        record["NotMustNotFactHouse"] = percentOfCompletion == 0 && dateStartWork > reportDate ? 1 : 0;
                    }

                    record["FactWork"] = percentOfCompletion > 0 ? 1 : 0;
                    record["FactHouse"] = percentOfCompletion > 0 ? 1 : 0;
                }
                else
                {
                    // Расчитываем для ЖурналХодаКапремонта4
                    if (dateEndWork != DateTime.MinValue)
                    {
                        record["MustPlanWork"] = dateEndWork <= reportDate ? 1 : 0;
                        record["MustFactWork"] = percentOfCompletion == 100 && dateEndWork <= reportDate ? 1 : 0;
                        record["NotMustFactWork"] = percentOfCompletion == 100 && dateEndWork > reportDate ? 1 : 0;
                        record["MustNotFactWork"] = percentOfCompletion < 100 && dateEndWork <= reportDate ? 1 : 0;
                        record["NotMustNotFactWork"] = percentOfCompletion < 100 && dateEndWork > reportDate ? 1 : 0;

                        // значения аналогичны must_plan_work и must_notfact_work, для упрощения customSummary
                        record["MustPlanHouse"] = dateEndWork <= reportDate ? 1 : 0;
                        record["MustFactHouse"] = percentOfCompletion == 100 && dateEndWork <= reportDate ? 1 : 0;
                        record["NotMustFactHouse"] = percentOfCompletion == 100 && dateEndWork > reportDate ? 1 : 0;
                        record["MustNotFactHouse"] = percentOfCompletion < 100 && dateEndWork <= reportDate ? 1 : 0;
                        record["NotMustNotFactHouse"] = percentOfCompletion < 100 && dateEndWork > reportDate ? 1 : 0;
                    }

                    record["FactWork"] = percentOfCompletion == 100 ? 1 : 0;
                    record["FactHouse"] = percentOfCompletion == 100 ? 1 : 0;
                }

                table.Rows.Add(record);
            }

            Data = table;
        }

        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatType = FormatType.Numeric };
 
            var datetimeCellFormat = new CellFormat { FormatString = "d", FormatType = FormatType.DateTime };
            var config = new PivotConfiguration
                             {
                                 Name = "JournalKr34",
                                 ModelName = "CR Report.JournalKr34",
                                 CustomSummary = (sender, e) => CustomSummary.GetValue(e, typeWork)
                             };

            var datetimeValueFormat = new ValueFormat { FormatString = "d", FormatType = FormatType.DateTime };

            var fields = new List<Field>
                {
                    new Field
                        {
                            Name = "GjiNum",
                            Area = Area.RowArea,
                            AreaIndex = 1,
                            DisplayName = "Реестровый номер ГЖИ"
                        },
                    new Field
                        {
                            Name = "Address",
                            Area = Area.RowArea,
                            DisplayName = "Адрес",
                            AreaIndex = 2
                        },
                    new Field
                        {
                            Name = "Municipality",
                            Area = Area.RowArea,
                            AreaIndex = 0,
                            DisplayName = "Муниципальное образование"
                        },
                    new Field
                        {
                            Name = "WorkCode",
                            AreaIndex = 0,
                            Area = Area.ColumnArea,
                            DisplayName = "Код работы"
                        },
                    new Field
                        {
                            Name = "WorkName",
                            Area = Area.ColumnArea,
                            AreaIndex = 1,
                            DisplayName = "Виды работ"
                        },
                    new Field
                        {
                            Name = "InProgHouse",
                            Area = Area.DataArea,
                            AreaIndex = 0,
                            DisplayName = "По программе домов",
                            SummaryType = SummaryType.Custom,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Name = "InProg",
                            Area = Area.FilterArea,
                            AreaIndex = 0,
                            DisplayName = "По программе",
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Name = "MustPlanWork",
                            Area = Area.FilterArea,
                            AreaIndex = 1,
                            DisplayName = "Д.б по плану работ",
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Name = "MustPlanHouse",
                            Area = Area.DataArea,
                            AreaIndex = 1,
                            DisplayName = "Д.б. по плану домов",
                            SummaryType = SummaryType.Custom,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Name = "MustFactWork",
                            AreaIndex = 2,
                            Area = Area.FilterArea,
                            DisplayName = "Д.б. и факт работ",
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Name = "MustFactHouse",
                            DisplayName = "Д.б. и факт домов",
                            Area = Area.DataArea,
                            AreaIndex = 2,
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Area = Area.FilterArea,
                            DisplayName = "Не д.б., но факт работ",
                            Name = "NotMustFactWork",
                            AreaIndex = 3,
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Area = Area.DataArea,
                            DisplayName = "Не д.б., но факт домов",
                            Name = "NotMustFactHouse",
                            AreaIndex = 3,
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Area = Area.FilterArea,
                            DisplayName = "Д.б., но не факт работ",
                            Name = "MustNotFactWork",
                            AreaIndex = 4,
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Area = Area.DataArea,
                            DisplayName = "Д.б., но не факт домов",
                            Name = "MustNotFactHouse",
                            AreaIndex = 4,
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Area = Area.FilterArea,
                            DisplayName = "Не д.б. и не факт работ",
                            Name = "NotMustNotFactWork",
                            AreaIndex = 5,
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Area = Area.DataArea,
                            DisplayName = "Не д.б. и не факт домов",
                            Name = "NotMustNotFactHouse",
                            AreaIndex = 5,
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Name = "FactWork",
                            DisplayName = "Факт работ",
                            Area = Area.FilterArea,
                            AreaIndex = 6,
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Area = Area.DataArea,
                            AreaIndex = 6,
                            DisplayName = "Факт домов",
                            Name = "FactHouse",
                            SummaryType = SummaryType.Custom,
                            CellFormat = numericCellFormat
                        },
                    new Field
                        {
                            Area = Area.DataArea,
                            AreaIndex = 7,
                            DisplayName = "Начало работ",
                            Name = "DateStartWork",
                            SummaryType = SummaryType.Min,
                            CellFormat = datetimeCellFormat,
                            ValueFormat = datetimeValueFormat
                        },
                    new Field
                        {
                            Area = Area.DataArea,
                            AreaIndex = 8,
                            DisplayName = "Окончание работ",
                            Name = "DateEndWork",
                            SummaryType = SummaryType.Max,
                            CellFormat = datetimeCellFormat,
                            ValueFormat = datetimeValueFormat
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
