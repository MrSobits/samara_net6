namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Pivot;
    using Bars.B4.Modules.Pivot.Enum;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Olap-отчет Полная информация по охваченному капремонту МКД (Журнал ч.6) 
    /// </summary>
    public class JournalCr6 : BasePrintForm, IPivotModel
    {
        public JournalCr6()
            : base(null)
        {
        }

        public override string Name
        {
            get { return "Полная информация по охваченному капремонту МКД (Журнал ч.6) "; }
        }

        public override string ReportGenerator { get; set; }

        public override string Desciption
        {
            get { return "Полная информация по охваченному капремонту МКД (Журнал ч.6) "; }
        }

        public override string GroupName
        {
            get { return "Отчеты Кап.ремонт"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.JournalCr6"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.JournalCr6"; }
        }

        public IWindsorContainer Container { get; set; }

        public string Params { get; set; }

        public object Data { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
        }

        public void LoadData()
        {
            if (string.IsNullOrEmpty(this.Params))
            {
                return;
            }

            var dynDict = DynamicDictionary.FromString(this.Params);

            var programId = dynDict.ContainsKey("programCrId") ? dynDict["programCrId"].ToInt() : 0;
            var municipalities = dynDict.ContainsKey("municipalityIds")
                                     ? dynDict["municipalityIds"].ToStr().Split("%2c+").Select(x => x.ToLong()).ToList()
                                     : new List<long>();
            var reportDate = dynDict.ContainsKey("reportDate") && !string.IsNullOrEmpty(dynDict["reportDate"].ToString())
                                 ? Uri.UnescapeDataString(dynDict["reportDate"].ToString()).ToDateTime()
                                 : DateTime.Today;

            var table = new DataTable();

            table.Columns.Add(new DataColumn("Municipality"));
            table.Columns.Add(new DataColumn("Builder"));
            table.Columns.Add(new DataColumn("Address"));
            table.Columns.Add(new DataColumn("NumberGji"));

            table.Columns.Add(new DataColumn("FinanceSource"));
            table.Columns.Add(new DataColumn("WorkName"));
            table.Columns.Add(new DataColumn("WorkCode"));

            table.Columns.Add(new DataColumn("PlanVolume", typeof(decimal)));
            table.Columns.Add(new DataColumn("PlanSum", typeof(decimal)));

            table.Columns.Add(new DataColumn("DateStart", typeof(DateTime)));
            table.Columns.Add(new DataColumn("DateEnd", typeof(DateTime)));

            table.Columns.Add(new DataColumn("CompleteVolume", typeof(decimal)));
            table.Columns.Add(new DataColumn("CompleteSum", typeof(decimal)));
            table.Columns.Add(new DataColumn("CompletePercent", typeof(decimal)));

            table.Columns.Add(new DataColumn("CompleteGraphicPercent", typeof(decimal)));
            table.Columns.Add(new DataColumn("GapPercent", typeof(decimal)));

            table.Columns.Add(new DataColumn("ActCount", typeof(int)));
            table.Columns.Add(new DataColumn("ActSum", typeof(decimal)));

            table.Columns.Add(new DataColumn("StartFact", typeof(int)));
            table.Columns.Add(new DataColumn("EndFact", typeof(int)));
            table.Columns.Add(new DataColumn("DateAcceptGji", typeof(DateTime)));
            table.Columns.Add(new DataColumn("DateEndBuilder", typeof(DateTime)));
            table.Columns.Add(new DataColumn("DateWorkStoped", typeof(DateTime)));
            table.Columns.Add(new DataColumn("ManagingOrganization"));
            
            var dictObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == programId)
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                    {
                        x.Id,
                        x.RealityObject.Address,
                        MuName = x.RealityObject.Municipality.Name,
                        x.DateAcceptCrGji,
                        x.DateEndBuilder,
                        x.DateEndWork,
                        x.GjiNum
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var dictTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => new
                    {
                        x.Id,
                        ObjectCrId = x.ObjectCr.Id,
                        RealityObjectId = x.ObjectCr.RealityObject.Id,
                        FinSourceId = x.FinanceSource.Id,
                        FinSourceName = x.FinanceSource.Name,
                        WorkId = x.Work.Id,
                        WorkName = x.Work.Name,
                        WorkCode = x.Work.Code,
                        x.PercentOfCompletion,
                        x.Sum,
                        x.Volume,
                        x.CostSum,
                        x.VolumeOfCompletion,
                        x.DateStartWork,
                        x.DateEndWork
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key, 
                    y => y.Select(x => new 
                                    {
                                        x.ObjectCrId,
                                        x.RealityObjectId,
                                        x.FinSourceId, 
                                        x.FinSourceName, 
                                        x.WorkId, 
                                        x.WorkName, 
                                        x.WorkCode,
                                        x.PercentOfCompletion, 
                                        x.Sum, 
                                        x.Volume, 
                                        x.CostSum, 
                                        x.VolumeOfCompletion,
                                        x.DateStartWork,
                                        x.DateEndWork
                                    })
                .FirstOrDefault());

            var dictPerfWorkAct = this.Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll()
                .Where(x => x.State.Name == "Принят ТОДК" || x.State.Name == "Принято ГЖИ")
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programId)
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .GroupBy(x => x.TypeWorkCr.Id)
                .Select(x => new
                {
                    x.Key,
                    Count = x.Count(),
                    Sum = x.Sum(y => y.Sum)
                })
                .ToDictionary(x => x.Key, y => new { y.Count, y.Sum });
            
            var dictBuildContract = this.Container.Resolve<IDomainService<BuildContract>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => new
                    {
                        ObjectCrId = x.ObjectCr.Id,
                        x.TypeContractBuild,
                        BuilderName = x.Builder.Contragent.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.BuilderName).Distinct().Aggregate((s, s1) => string.Format("{0}, {1}", s, s1)));
            
            var dictArchieveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programId)
                .Where(x => x.DateChangeRec <= reportDate.Date) 
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => new
                    {
                        TypeWorkCrId = x.TypeWorkCr.Id,
                        x.PercentOfCompletion,
                        x.VolumeOfCompletion,
                        x.CostSum,
                        x.TypeWorkCr.DateStartWork,
                        x.TypeWorkCr.DateEndWork,
                        x.ObjectCreateDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.TypeWorkCrId)
                .ToDictionary(
                    x => x.Key, 
                    y => y.OrderByDescending(x => x.ObjectCreateDate)
                        .Select(x => new
                                    {
                                        x.PercentOfCompletion, 
                                        x.VolumeOfCompletion, 
                                        x.CostSum, 
                                        x.DateStartWork, 
                                        x.DateEndWork
                                    })
                        .FirstOrDefault());

            var dictWorksEnd = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.ObjectCreateDate <= reportDate 
                    && x.ObjectCr.ProgramCr.Id == programId
                    && x.Work.TypeWork != TypeWork.Service)
                .Select(x => new
                    {
                        ObjectCrId = x.ObjectCr.Id,
                        x.PercentOfCompletion
                    })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, y => y.Count() - y.Count(x => x.PercentOfCompletion == 100));

            var dateContractFilter = reportDate == DateTime.MinValue ? DateTime.MinValue : reportDate;
            var dictManOrgContractRo = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ManOrgContract.StartDate <= dateContractFilter)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= dateContractFilter)
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key);

            var moContractDirectManagServ = this.Container.Resolve<IDomainService<RealityObjectDirectManagContract>>();

            foreach (var item in dictTypeWork)
            {
                var row = table.NewRow();

                row["FinanceSource"] = item.Value.FinSourceName;
                row["WorkName"] = item.Value.WorkName;
                row["WorkCode"] = item.Value.WorkCode;

                if (dictObjectCr.ContainsKey(item.Value.ObjectCrId))
                {
                    row["Municipality"] = dictObjectCr[item.Value.ObjectCrId].MuName;
                    row["Address"] = dictObjectCr[item.Value.ObjectCrId].Address;
                    row["NumberGji"] = dictObjectCr[item.Value.ObjectCrId].GjiNum;

                    var dateAccept = dictObjectCr[item.Value.ObjectCrId].DateAcceptCrGji.ToDateTime();
                    if (dateAccept > DateTime.MinValue)
                    {
                        row["DateAcceptGji"] = dateAccept;
                    }

                    var dateEndBuilder = dictObjectCr[item.Value.ObjectCrId].DateEndBuilder.ToDateTime();
                    if (dateEndBuilder > DateTime.MinValue)
                    {
                        row["DateEndBuilder"] = dateEndBuilder;
                    }

                    var dateWorkStoped = dictObjectCr[item.Value.ObjectCrId].DateEndWork.ToDateTime();
                    if (dateWorkStoped > DateTime.MinValue)
                    {
                        row["DateWorkStoped"] = dateWorkStoped;
                    }
                }

                if (dictPerfWorkAct.ContainsKey(item.Key))
                {
                    row["ActCount"] = dictPerfWorkAct[item.Key].Count;
                    row["ActSum"] = dictPerfWorkAct[item.Key].Sum.ToDecimal();
                }

                if (dictBuildContract.ContainsKey(item.Value.ObjectCrId))
                {
                    row["Builder"] = dictBuildContract[item.Value.ObjectCrId];
                }

                if (dictManOrgContractRo.ContainsKey(item.Value.RealityObjectId))
                {
                    var manOrg = dictManOrgContractRo[item.Value.RealityObjectId];
                    
                    // исключая записи по передаче управления
                    var manOrgNames = manOrg
                        .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.ManagingOrgJskTsj)
                        .Select(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag
                            ? moContractDirectManagServ.GetAll().Any(y => y.Id == x.ManOrgContract.Id && y.IsServiceContract) 
                                ? "Непосредственное управление с договором на оказание услуг" 
                                : "Непосредственное управление"
                            : x.ManOrgContract.ManagingOrganization.Contragent.Name);

                    row["ManagingOrganization"] = manOrgNames.FirstOrDefault() ?? string.Empty;
                }

                row["PlanSum"] = item.Value.Sum.ToDecimal();
                row["PlanVolume"] = item.Value.Volume.ToDecimal();
                
                if (reportDate.Date < DateTime.Now.Date)
                {
                    if (dictArchieveSmr.ContainsKey(item.Key))
                    {
                        var dateStart = dictArchieveSmr[item.Key].DateStartWork.ToDateTime();
                        var dateEnd = dictArchieveSmr[item.Key].DateEndWork.ToDateTime();

                        var percentGraphic = dateStart != DateTime.MinValue
                                            && dateEnd != DateTime.MinValue
                                            && dateStart != dateEnd
                                            && dateStart.Date < reportDate.Date
                                                 ? ((reportDate.Date - dateStart.Date).TotalDays / ((dateEnd.Date - dateStart.Date).TotalDays + 1)).ToDecimal()
                                                 : 0m;

                        percentGraphic = percentGraphic > 1 ? 1 : percentGraphic;

                        if (dateStart > DateTime.MinValue)
                        {
                            row["DateStart"] = dateStart;
                        }

                        if (dateEnd > DateTime.MinValue)
                        {
                            row["DateEnd"] = dateEnd;
                        }

                        row["CompleteGraphicPercent"] = (percentGraphic * 100).RoundDecimal(2);
                        row["CompleteVolume"] = dictArchieveSmr[item.Key].VolumeOfCompletion.ToDecimal();
                        row["CompleteSum"] = dictArchieveSmr[item.Key].CostSum.ToDecimal();
                        row["CompletePercent"] = dictArchieveSmr[item.Key].PercentOfCompletion.ToDecimal().RoundDecimal(2);

                        var percentFact = dictArchieveSmr[item.Key].PercentOfCompletion.HasValue
                            ? dictArchieveSmr[item.Key].PercentOfCompletion.Value / 100
                            : 0;

                        percentFact = percentFact > 1 ? 1 : percentFact;

                        row["GapPercent"] = percentFact == 1 || percentFact > percentGraphic
                            ? 0
                            : ((percentGraphic - percentFact) * 100).RoundDecimal(2);

                        row["StartFact"] = percentFact > 0 ? 1 : 0;

                        if (dictWorksEnd.ContainsKey(item.Value.ObjectCrId))
                        {
                            row["EndFact"] = dictWorksEnd[item.Value.ObjectCrId] == 0 ? 1 : 0;
                        }
                        else
                        {
                            row["EndFact"] = 0;
                        }
                    }
                }
                else
                {
                    var dateStart = item.Value.DateStartWork.ToDateTime();
                    var dateEnd = item.Value.DateEndWork.ToDateTime();

                    var percentGraphic = dateStart != DateTime.MinValue
                                            && dateEnd != DateTime.MinValue
                                            && dateStart != dateEnd
                                            && dateStart.Date < reportDate.Date
                                                 ? ((reportDate.Date - dateStart.Date).TotalDays / ((dateEnd.Date - dateStart.Date).TotalDays + 1)).ToDecimal()
                                                 : 0m;

                    percentGraphic = percentGraphic > 1 ? 1 : percentGraphic;

                    if (dateStart > DateTime.MinValue)
                    {
                        row["DateStart"] = dateStart;
                    }

                    if (dateEnd > DateTime.MinValue)
                    {
                        row["DateEnd"] = dateEnd;
                    }

                    row["CompleteGraphicPercent"] = (percentGraphic * 100).RoundDecimal(2);
                    row["CompleteVolume"] = item.Value.VolumeOfCompletion.ToDecimal();
                    row["CompleteSum"] = item.Value.CostSum.ToDecimal();
                    row["CompletePercent"] = item.Value.PercentOfCompletion.ToDecimal().RoundDecimal(2);

                    var percentFact = item.Value.PercentOfCompletion.HasValue
                        ? item.Value.PercentOfCompletion.Value / 100
                        : 0;

                    percentFact = percentFact > 1 ? 1 : percentFact;

                    row["GapPercent"] = percentFact == 1 || percentFact > percentGraphic
                        ? 0
                        : ((percentGraphic - percentFact) * 100).RoundDecimal(2);

                    row["StartFact"] = percentFact > 0 ? 1 : 0;

                    if (dictWorksEnd.ContainsKey(item.Value.ObjectCrId))
                    {
                        row["EndFact"] = dictWorksEnd[item.Value.ObjectCrId] == 0 ? 1 : 0;
                    }
                    else
                    {
                        row["EndFact"] = 0;
                    }
                }

                table.Rows.Add(row);
            }

            this.Data = table;
        }

        /// <summary>
        /// The get configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="PivotConfiguration"/>.
        /// </returns>
        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatString = "n", FormatType = FormatType.Numeric };
            var datetimeCellFormat = new CellFormat { FormatString = "d", FormatType = FormatType.DateTime };

            var datetimeValueFormat = new ValueFormat { FormatString = "d", FormatType = FormatType.DateTime };

            return new PivotConfiguration
                {
                    ModelName = "CR Report.JournalCr6",
                    Name = "JournalCr6",
                    Fields = new List<Field>
                        {
                            new Field
                                {
                                    Area = Area.RowArea,
                                    DisplayName = "Адрес",
                                    Name = "Address"
                                },
                            new Field
                                {
                                    Area = Area.RowArea,
                                    DisplayName = "Подрядчик",
                                    Name = "Builder"
                                },
                            new Field
                                {
                                    Area = Area.RowArea,
                                    DisplayName = "Муниципальное образование",
                                    Name = "Municipality"
                                },
                            new Field
                                {
                                    Area = Area.ColumnArea,
                                    DisplayName = "Вид работы",
                                    Name = "WorkName"
                                },
                            new Field
                                {
                                    Area = Area.ColumnArea,
                                    DisplayName = "Код работы",
                                    Name = "WorkCode"
                                },
                            new Field
                                {
                                    Area = Area.ColumnArea,
                                    DisplayName = "Разрез финансирования",
                                    Name = "FinanceSource"
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    DisplayName = "Факт (%)",
                                    Name = "CompletePercent",
                                    SummaryType = SummaryType.Average,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    DisplayName = "Факт (руб.)",
                                    Name = "CompleteSum",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    DisplayName = "Факт (кв.м.)",
                                    Name = "CompleteVolume",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    DisplayName = "% отставания по графику",
                                    Name = "GapPercent",
                                    SummaryType = SummaryType.Average,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Смета (кв.м.)",
                                    Name = "PlanVolume",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Смета (руб.)",
                                    Name = "PlanSum",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "% выполнения по графику",
                                    Name = "CompleteGraphicPercent",
                                    SummaryType = SummaryType.Average,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Количество актов",
                                    Name = "ActCount",
                                    SummaryType = SummaryType.Sum
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Сумма по актам",
                                    Name = "ActSum",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Дом начат факт",
                                    Name = "StartFact",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Дом завершен факт",
                                    Name = "EndFact",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Начало работ",
                                    Name = "DateStart",
                                    SummaryType = SummaryType.Min,
                                    CellFormat = datetimeCellFormat,
                                    ValueFormat = datetimeValueFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Окончание работ",
                                    Name = "DateEnd",
                                    SummaryType = SummaryType.Max,
                                    CellFormat = datetimeCellFormat,
                                    ValueFormat = datetimeValueFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Дом принят ГЖИ",
                                    Name = "DateAcceptGji",
                                    SummaryType = SummaryType.Max,
                                    CellFormat = datetimeCellFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Дом завершен подрядчиком",
                                    Name = "DateEndBuilder",
                                    SummaryType = SummaryType.Max,
                                    CellFormat = datetimeCellFormat,
                                    ValueFormat = datetimeValueFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Работы остановлены по требованию ГЖИ РТ",
                                    Name = "DateWorkStoped",
                                    SummaryType = SummaryType.Max,
                                    CellFormat = datetimeCellFormat,
                                    ValueFormat = datetimeValueFormat
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Реестровый номер ГЖИ",
                                    Name = "NumberGji"
                                },
                            new Field
                                {
                                    Area = Area.FilterArea,
                                    DisplayName = "Управляющая организация",
                                    Name = "ManagingOrganization"
                                }
                        }
                };
        }
    }
}