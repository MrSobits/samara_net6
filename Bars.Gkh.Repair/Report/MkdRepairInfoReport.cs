using System.Data;
using Bars.B4;
using Bars.B4.Modules.Pivot.Enum;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Repair.Entities;

namespace Bars.GkhCr.Regions.Tatarstan.Report
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using B4.Utils;
    using B4.Modules.Pivot;
    using B4.Modules.Reports;
    using Castle.Windsor;
    using Gkh.Utils;

    internal class MkdRepairInfoReport : BasePrintForm, IPivotModel
    {
        public override string Name
        {
            get { return "Информация по текущему ремонту МКД"; }
        }

        public override string ReportGenerator { get; set; }

        public override string Desciption
        {
            get { return "Информация по текущему ремонту МКД"; }
        }

        public override string GroupName
        {
            get { return "Текущий ремонт"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.MkdRepairInfoReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRepair.MkdRepairInfoReport"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {

        }

        public MkdRepairInfoReport() : base(null)
        {

        }

        public IWindsorContainer Container { get; set; }

        public string Params { get; set; }

        public object Data { get; set; }

        public IDomainService<ManOrgContractRealityObject> ManOrgRoContractService { get; set; }
        public IDomainService<ManOrgContractRelation> ManOrgContractRelationService { get; set; }
        public IDomainService<RepairObject> RepairObjectDomain { get; set; }
        public IDomainService<RepairWork> RepairWorkDomain { get; set; }
        public IDomainService<RepairWorkArchive> RepairWorkArchiveDomain { get; set; }

        public void LoadData()
        {
            if (string.IsNullOrEmpty(Params))
            {
                return;
            }

            var dynDict = DynamicDictionary.FromString(Params);

            var programId = dynDict.ContainsKey("programId") ? dynDict["programId"].ToInt() : 0;
            var municipalities = dynDict.ContainsKey("municipalityIds") ? dynDict["municipalityIds"].ToStr().Split("%2c+").Select(x => x.ToLong()).ToList() : new List<long>();
            var reportDate = dynDict.ContainsKey("reportDate") ? Uri.UnescapeDataString(dynDict["reportDate"].ToString()).ToDateTime() : (DateTime?)null;

            var table = new DataTable();

            table.Columns.Add(new DataColumn("Municipality"));
            table.Columns.Add(new DataColumn("Address"));
            table.Columns.Add(new DataColumn("Customer"));
            table.Columns.Add(new DataColumn("Builder"));
            table.Columns.Add(new DataColumn("WorkTypesAmount"));

            table.Columns.Add(new DataColumn("WorkName"));
            table.Columns.Add(new DataColumn("PlanSum", typeof(decimal)));
            table.Columns.Add(new DataColumn("CompleteSum", typeof(decimal)));
            table.Columns.Add(new DataColumn("PlanVolume", typeof(decimal)));
            table.Columns.Add(new DataColumn("CompleteVolume", typeof(decimal)));
            table.Columns.Add(new DataColumn("CompletePercent", typeof(decimal)));
            table.Columns.Add(new DataColumn("CompleteGraphicPercent", typeof(decimal)));
            table.Columns.Add(new DataColumn("GapPercent", typeof(decimal)));
            table.Columns.Add(new DataColumn("DateStart", typeof(DateTime)));
            table.Columns.Add(new DataColumn("DateEnd", typeof(DateTime)));
            table.Columns.Add(new DataColumn("WorkStartFact", typeof(int)));
            table.Columns.Add(new DataColumn("WorkEndFact", typeof(int)));
            table.Columns.Add(new DataColumn("WorkToBeStarted", typeof(int)));
            table.Columns.Add(new DataColumn("WorkToBeEnded", typeof(int)));
            table.Columns.Add(new DataColumn("ObjStartFact", typeof(int)));
            table.Columns.Add(new DataColumn("ObjEndFact", typeof(int)));
            table.Columns.Add(new DataColumn("ObjToBeStarted", typeof(int)));
            table.Columns.Add(new DataColumn("ObjToBeEnded", typeof(int)));


            var repairObjQuery = RepairObjectDomain.GetAll()
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id));

            var realityObjects = repairObjQuery
                .Select(x => new {
                    x.RealityObject.Id,
                    x.RealityObject.Address,
                    MunicipalityName = x.RealityObject.Municipality.Name,
                })
                .ToList();

            var dictRepairWorks = RepairWorkDomain.GetAll()
                .Where(x => x.RepairObject.RepairProgram.Id == programId)
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RepairObject.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    x.RepairObject.RealityObject.Id,
                    ObjectId = x.RepairObject.Id,
                    WorkName = x.Work.Name,
                    x.Builder,
                    PlanSum = x.Sum,
                    CompleteSum = x.CostSum,
                    PlanVolume = x.Volume,
                    CompleteVolume = x.VolumeOfCompletion,
                    CompletePercent = x.PercentOfCompletion,
                    x.DateStart,
                    x.DateEnd
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(z => z));

            var dictArchiveSmr = RepairWorkArchiveDomain.GetAll()
                .Where(x => x.RepairWork.Work.TypeWork == TypeWork.Work)
                .Where(x => x.RepairWork.RepairObject.RepairProgram.Id == programId)
                .Where(x => reportDate.HasValue && x.DateChangeRec <= reportDate.Value.Date)
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RepairWork.RepairObject.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    x.RepairWork.Id,
                    x.PercentOfCompletion,
                    x.VolumeOfCompletion,
                    x.CostSum,
                    x.ObjectCreateDate
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    y => y.OrderByDescending(x => x.ObjectCreateDate)
                        .Select(x => new
                        {
                            CompletePercent = x.PercentOfCompletion, 
                            CompleteVolume = x.VolumeOfCompletion,
                            CompleteSum = x.CostSum, 
                        })
                        .FirstOrDefault());

            //информация об УО
            var moContractData = ManOrgRoContractService.GetAll()
                .Where(x => repairObjQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                .WhereIf(reportDate.HasValue, x => (x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate <= reportDate)
                                               && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= reportDate))
                .Select(x => new
                {
                    x.RealityObject.Id, 
                    ContractId = x.ManOrgContract.Id,
                    x.ManOrgContract.ManagingOrganization.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(z => new{
                    z.ContractId,
                    z.Name
                }).FirstOrDefault());

            //информация о передаче управления УО
            var moRelData = ManOrgContractRelationService.GetAll()
                        .Where(x => x.TypeRelation == TypeContractRelation.TransferTsjUk)
                        .Select(x => new{
                            x.Parent.Id,
                            x.Children.ManagingOrganization.Contragent.Name,
                            x.Children.StartDate
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y);

            foreach (var ro in realityObjects)
            {

                var customer = "";
                if (moContractData.ContainsKey(ro.Id))
                {
                    var manOrgInfo = moContractData[ro.Id];
                    if (moRelData.Any() && moRelData.ContainsKey(manOrgInfo.ContractId))
                    {
                        customer = moRelData[manOrgInfo.ContractId]
                            .OrderByDescending(x => x.StartDate)
                            .Select(x => x.Name)
                            .FirstOrDefault();
                    }
                    else
                    {
                        customer = manOrgInfo.Name;
                    }
                }

                if (dictRepairWorks.ContainsKey(ro.Id))
                {
                    var objWorks = dictRepairWorks[ro.Id].ToArray();
                    var buiders = objWorks
                            .Select(x => x.Builder)
                            .Distinct()
                            .AggregateWithSeparator(", ");
                    var typeWorkCount = objWorks.Count();

                    foreach (var work in objWorks)
                    {
                        var row = table.NewRow();

                        row["Municipality"] = ro.MunicipalityName;
                        row["Address"] = ro.Address;
                        row["Customer"] = customer;

                        row["Builder"] = buiders;

                        row["WorkTypesAmount"] = typeWorkCount;

                        row["WorkName"] = work.WorkName;

                        row["PlanSum"] = work.PlanSum.ToDecimal();
                        row["PlanVolume"] = work.PlanVolume.ToDecimal();

                        var completePercent = 0m;

                        if (reportDate.HasValue && reportDate.Value.Date < DateTime.Now.Date && dictArchiveSmr.ContainsKey(work.Id))
                        {
                            var arch = dictArchiveSmr[work.Id];
                            row["CompleteVolume"] = arch.CompleteVolume.ToDecimal();
                            row["CompleteSum"] = arch.CompleteSum.ToDecimal();

                            completePercent = arch.CompletePercent.ToDecimal();
                            row["CompletePercent"] = completePercent.RoundDecimal(2);
                        }
                        else
                        {
                            row["CompleteVolume"] = work.CompleteVolume.ToDecimal();
                            row["CompleteSum"] = work.CompleteSum.ToDecimal();

                            completePercent = work.CompletePercent.ToDecimal();
                            row["CompletePercent"] = completePercent.RoundDecimal(2);
                        }

                        if (work.DateStart.HasValue)
                        {
                            row["DateStart"] = work.DateStart.Value;
                        }

                        if (work.DateEnd.HasValue)
                        {
                            row["DateEnd"] = work.DateEnd.Value;
                        }

                        var completeGraphicPercent = 0;
                        if (reportDate.HasValue && work.DateStart.HasValue && work.DateEnd.HasValue)
                        {
                            if (work.DateEnd.Value != work.DateStart.Value)
                            {
                                completeGraphicPercent = 100 * (reportDate.Value - work.DateStart.Value).Days / (work.DateEnd.Value - work.DateStart.Value).Days;
                                completeGraphicPercent = completeGraphicPercent > 100 ? 100 : completeGraphicPercent;
                                completeGraphicPercent = completeGraphicPercent < 0 ? 0 : completeGraphicPercent;
                                row["CompleteGraphicPercent"] = completeGraphicPercent;
                            }
                        }

                        var gapPercent = (completePercent - completeGraphicPercent).RoundDecimal(2);
                        gapPercent = gapPercent > 100 ? 100 : gapPercent;
                        row["GapPercent"] = Math.Abs(gapPercent);

                        row["WorkStartFact"] = completePercent > 0  ? 1 : 0;
                        row["WorkEndFact"] = (completePercent == 100 && work.DateEnd.HasValue && work.DateEnd.Value < reportDate) ? 1 : 0;
                        row["WorkToBeStarted"] = (completePercent == 0 && work.DateStart.HasValue && work.DateStart.Value < reportDate) ? 1 : 0;
                        row["WorkToBeEnded"] = (completePercent < 100 && work.DateEnd.HasValue && work.DateEnd.Value < reportDate) ? 1 : 0;

                        var data = dictRepairWorks[ro.Id].ToArray();

                        row["ObjStartFact"] = data.Any(x => x.CompletePercent > 0) ? 1 : 0;

                        row["ObjEndFact"] = data.All(x => x.CompletePercent == 100
                                                          && x.DateEnd.HasValue && reportDate.HasValue
                                                          && x.DateEnd < reportDate) ? 1 : 0;

                        row["ObjToBeStarted"] = data.All(x => !x.CompletePercent.HasValue || x.CompletePercent == 0)
                            && data.Any(x => x.DateStart.HasValue && reportDate.HasValue && x.DateStart < reportDate) ? 1 : 0;

                        row["ObjToBeEnded"] = data.Any(x => x.CompletePercent < 100)
                            && data.All(x => x.DateEnd.HasValue && reportDate.HasValue && x.DateEnd < reportDate) ? 1 : 0;
                        
                        table.Rows.Add(row);
                    }

                }
                else
                {
                    var row = table.NewRow();

                    row["Municipality"] = ro.MunicipalityName;
                    row["Address"] = ro.Address;
                    row["Customer"] = customer;
                    row["WorkName"] = "Нет работ";
                    row["WorkTypesAmount"] = 0;
                    table.Rows.Add(row);
                }

            }

            Data = table;
        }

        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatString = "n", FormatType = FormatType.Numeric };
            var datetimeCellFormat = new CellFormat { FormatString = "d", FormatType = FormatType.DateTime };
            var datetimeValueFormat = new ValueFormat { FormatString = "d", FormatType = FormatType.DateTime };

            return new PivotConfiguration
            {
                ModelName = "GkhRepair Report.MkdRepairInfoReport",
                Name = "MkdRepairInfoReport",
                Fields = new List<Field>
                {
                    new Field {
                       AreaIndex = 0,
                       Area = Area.RowArea,
                       DisplayName = "Муниципальное образование",
                       Name = "Municipality"
                    },
                    new Field {
                       AreaIndex = 1,
                       Area = Area.RowArea,
                       DisplayName = "Адрес",
                       Name = "Address"
                    },
                    new Field {
                       AreaIndex = 2,
                       Area = Area.RowArea,
                       DisplayName = "Заказчик",
                       Name = "Customer"
                    },
                    new Field {
                       AreaIndex = 3,
                       Area = Area.RowArea,
                       DisplayName = "Подрядчик",
                       Name = "Builder"
                    },
                    new Field {
                       AreaIndex = 4,
                       Area = Area.RowArea,
                       DisplayName = "Кол-во видов работ",
                       Name = "WorkTypesAmount"
                    },
                    new Field {
                        AreaIndex = 5,
                        Area = Area.FilterArea,
                        DisplayName = "Дом начат факт",
                        Name = "ObjStartFact",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 6,
                        Area = Area.FilterArea,
                        DisplayName = "Дом завершен факт",
                        Name = "ObjEndFact",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 7,
                        Area = Area.FilterArea,
                        DisplayName = "Дом д/б. начат",
                        Name = "ObjToBeStarted",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 8,
                        Area = Area.FilterArea,
                        DisplayName = "Дом д/б завершен",
                        Name = "ObjToBeEnded",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                       Area = Area.ColumnArea,
                       DisplayName = "Вид работы",
                       Name = "WorkName"
                    },
                    new Field {
                        AreaIndex = 0,
                       Area = Area.FilterArea,
                       DisplayName = "Смета (руб.)",
                       Name = "PlanSum",
                       SummaryType = SummaryType.Sum,
                       CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 1,
                       Area = Area.FilterArea,
                       DisplayName = "Факт (руб.)",
                       Name = "CompleteSum",
                       SummaryType = SummaryType.Sum,
                       CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 2,
                        Area = Area.FilterArea,
                        DisplayName = "Смета (кв.м.)",
                        Name = "PlanVolume",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 3,
                        Area = Area.FilterArea,
                        DisplayName = "Факт (кв.м.)",
                        Name = "CompleteVolume",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 4,
                        Area = Area.DataArea,
                        DisplayName = "Факт (%)",
                        Name = "CompletePercent",
                        SummaryType = SummaryType.Average,
                        CellFormat = numericCellFormat
                    },
                     new Field {
                         AreaIndex = 5,
                        Area = Area.FilterArea,
                        DisplayName = "% выполнения по графику",
                        Name = "CompleteGraphicPercent",
                        SummaryType = SummaryType.Average,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 6,
                        Area = Area.FilterArea,
                        DisplayName = "% отставания по графику",
                        Name = "GapPercent",
                        SummaryType = SummaryType.Average,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 7,
                        Area = Area.FilterArea,
                        DisplayName = "Начало работ",
                        Name = "DateStart",
                        SummaryType = SummaryType.Min,
                        CellFormat = datetimeCellFormat,
                        ValueFormat = datetimeValueFormat
                    },
                    new Field {
                        AreaIndex = 8,
                        Area = Area.FilterArea,
                        DisplayName = "Окончание работ",
                        Name = "DateEnd",
                        SummaryType = SummaryType.Max,
                        CellFormat = datetimeCellFormat,
                        ValueFormat = datetimeValueFormat
                    },
                    new Field {
                        AreaIndex = 9,
                        Area = Area.FilterArea,
                        DisplayName = "Работа начата факт",
                        Name = "WorkStartFact",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 10,
                        Area = Area.FilterArea,
                        DisplayName = "Работа завершена факт",
                        Name = "WorkEndFact",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 11,
                        Area = Area.FilterArea,
                        DisplayName = "Работа д/б. начата",
                        Name = "WorkToBeStarted",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    },
                    new Field {
                        AreaIndex = 12,
                        Area = Area.FilterArea,
                        DisplayName = "Работа д/б завершена",
                        Name = "WorkToBeEnded",
                        SummaryType = SummaryType.Sum,
                        CellFormat = numericCellFormat
                    }
                }
            };
        }
    }
}
