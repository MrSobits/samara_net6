// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkScheldudeInfoReport.cs" company="Bars-Group">
//   Bars-Group
// </copyright>
// <summary>
//   Olap-отчет Информация по графикам производства работ (Журнал ч.2)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Pivot;
    using Bars.B4.Modules.Pivot.Enum;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Olap-отчет Информация по графикам производства работ (Журнал ч.2)
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class WorkScheldudeInfoReport : BasePrintForm, IPivotModel
    {
        /// <summary>
        /// The program id.
        /// </summary>
        private long programId;

        /// <summary>
        /// The report date.
        /// </summary>
        private DateTime reportDate = DateTime.Now.Date;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkScheldudeInfoReport"/> class.
        /// </summary>
        public WorkScheldudeInfoReport() : base(null)
        {
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Информация по графикам производства работ (Журнал ч.2)";
            }
        }

        public override string ReportGenerator { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "Информация по графикам производства работ (Журнал ч.2)";
            }
        }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        /// <summary>
        /// Gets the parameters controller.
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.WorkScheldudeInfoReport";
            }
        }

        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.WorkScheldudeInfo";
            }
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// The prepare report.
        /// </summary>
        /// <param name="reportParams">
        /// The report parameters.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
        }
        
        /// <summary>
        /// The load data.
        /// </summary>
        public void LoadData()
        {
            if (string.IsNullOrEmpty(this.Params))
            {
                return;
            }

            var dynDict = DynamicDictionary.FromString(this.Params);

            this.programId = dynDict.GetAs<long>("programCrId", 0);
            if (this.programId == 0)
            {
                return;
            }
            
            var programCrYear = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programId).Period.DateStart.Year;
            var programAugust15 = new DateTime(programCrYear, 8, 15);

            var dictReportDate = dynDict.ContainsKey("reportDate")
                                  ? Uri.UnescapeDataString(dynDict["reportDate"].ToString()).ToDateTime()
                                  : DateTime.Now.Date;

            this.reportDate = dictReportDate == DateTime.Today.Date ? dictReportDate.AddDays(1) : dictReportDate; // TODO пункт3
            
            var table = new DataTable();

            table.Columns.Add(new DataColumn("Municipality"));
            table.Columns.Add(new DataColumn("Builder"));
            table.Columns.Add(new DataColumn("NumberGji"));
            table.Columns.Add(new DataColumn("Address"));
            
            table.Columns.Add(new DataColumn("WorkName"));
            table.Columns.Add(new DataColumn("WorkCode"));

            table.Columns.Add(new DataColumn("WorkCount", typeof(int)));
            table.Columns.Add(new DataColumn("MissCount", typeof(int)));
            table.Columns.Add(new DataColumn("WorkStartDate", typeof(DateTime)));
            table.Columns.Add(new DataColumn("WorkEndDate", typeof(DateTime)));
            table.Columns.Add(new DataColumn("Later15August", typeof(int)));
            table.Columns.Add(new DataColumn("ScheduleLagg", typeof(decimal)));
            
            var dictObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                                             .Where(x => x.ProgramCr.Id == this.programId)
                                             .Select(x => new
                                                     {
                                                         x.Id,
                                                         x.RealityObject.Address,
                                                         MuName = x.RealityObject.Municipality.Name,
                                                         x.GjiNum
                                                     })
                                             .AsEnumerable()
                                             .GroupBy(x => x.Id)
                                             .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var archiveRecords = this.Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                    .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == this.programId)
                    .Where(x => x.ObjectCreateDate <= this.reportDate)
                    .Select(x => new
                            {
                                x.Id,
                                x.DateChangeRec,
                                typeWorkId = x.TypeWorkCr.Id,
                                percentOfCompletion = x.PercentOfCompletion
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.typeWorkId)
                    .ToDictionary(
                        x => x.Key,
                        x => 
                        { 
                            var archiveRec = x.OrderByDescending(p => p.DateChangeRec).ThenByDescending(p => p.Id).FirstOrDefault();
                            return (archiveRec != null) ? archiveRec.percentOfCompletion : 0;
                        });

            var dictTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>()
                                    .GetAll()
                                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programId)
                                    .Select(
                                        x => new
                                                 {
                                                     typeWorkId = x.Id,
                                                     ObjectCrId = x.ObjectCr.Id,
                                                     WorkId = x.Work.Id,
                                                     WorkName = x.Work.Name,
                                                     WorkCode = x.Work.Code,
                                                     x.PercentOfCompletion,
                                                     x.DateStartWork,
                                                     x.DateEndWork
                                                 })
                                    .AsEnumerable()
                                    .GroupBy(x => x.ObjectCrId)
                                    .ToDictionary(
                                        x => x.Key,
                                        x => x.Select(y =>
                                                {
                                                    var missed = !y.DateStartWork.HasValue
                                                                    || y.DateStartWork.Value == DateTime.MinValue
                                                                    || !y.DateEndWork.HasValue
                                                                    || y.DateEndWork.Value == DateTime.MinValue;

                                                    var later15August = y.DateEndWork > programAugust15;

                                                    DateTime? startDate = null;
                                                    DateTime? endDate = null;
                                                    decimal? laggPercent = null;

                                                    if (!missed)
                                                    {
                                                        startDate = y.DateStartWork;
                                                        endDate = y.DateEndWork;

                                                        var start = startDate.ToDateTime();
                                                        var end = endDate.ToDateTime();

                                                        var validGraph = start != DateTime.MinValue && end != DateTime.MinValue 
                                                                         && start != end && start.Date < reportDate.Date;

                                                        if (validGraph)
                                                        {
                                                            var dateStart = y.DateStartWork.ToDateTime();
                                                            var dateEnd = y.DateEndWork.ToDateTime();

                                                            var percentGraphic = 
                                                                ((reportDate.Date - dateStart.Date).TotalDays / ((dateEnd.Date - dateStart.Date).TotalDays + 1)).ToDecimal();

                                                            percentGraphic = percentGraphic > 1 ? 1 : (percentGraphic < 0 ? 0 : percentGraphic);
                                                            
                                                            var archRec = programCrYear >= 2013
                                                                              ? (archiveRecords.ContainsKey(y.typeWorkId)
                                                                                     ? archiveRecords[y.typeWorkId]
                                                                                     : null)
                                                                              : y.PercentOfCompletion;

                                                            var percentFact = archRec.HasValue ? archRec.Value / 100 : 0;
                                                            percentFact = percentFact > 1 ? 1 : percentFact;
                                                            laggPercent = percentFact == 1 || percentFact > percentGraphic
                                                                              ? 0
                                                                              : (percentGraphic - percentFact).RoundDecimal(4);
                                                        }
                                                    }

                                                    return new
                                                            {
                                                                y.WorkName,
                                                                y.WorkCode,
                                                                y.ObjectCrId,
                                                                missed,
                                                                later15August,
                                                                startDate,
                                                                endDate,
                                                                laggPercent
                                                            };
                                                }).ToList());
           
            var dictBuildContract = this.Container.Resolve<IDomainService<BuildContract>>().GetAll()
                                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programId && x.TypeContractBuild != TypeContractBuild.NotDefined)
                                    .Select(x => new
                                    {
                                        ObjectCrId = x.ObjectCr.Id,
                                        x.TypeContractBuild,
                                        BuilderName = x.Builder.Contragent.Name
                                    })
                                    .AsEnumerable()
                                    .GroupBy(x => x.ObjectCrId)
                                    .ToDictionary(x => x.Key, y => y.OrderBy(x => x.TypeContractBuild).Select(x => x.BuilderName).FirstOrDefault());

            foreach (var objectCr in dictTypeWork)
            {
                var municipality = string.Empty;
                var address = string.Empty;
                var numberGji = string.Empty;
                if (dictObjectCr.ContainsKey(objectCr.Key))
                {
                    municipality = dictObjectCr[objectCr.Key].MuName;
                    address = dictObjectCr[objectCr.Key].Address;
                    numberGji = dictObjectCr[objectCr.Key].GjiNum;
                }

                var builder = string.Empty;
                if (dictBuildContract.ContainsKey(objectCr.Key))
                {
                    builder = dictBuildContract[objectCr.Key];
                }

                foreach (var item in objectCr.Value)
                {
                    var row = table.NewRow();

                    row["WorkName"] = item.WorkName;
                    row["WorkCode"] = item.WorkCode;

                    row["Municipality"] = municipality;
                    row["Address"] = address;
                    row["NumberGji"] = numberGji;
                    row["Builder"] = builder;

                    row["WorkCount"] = 1;

                    // TODO пункт1
                    if (item.laggPercent.HasValue)
                    {
                        row["ScheduleLagg"] = item.laggPercent.Value;
                    }
                    else
                    {
                        row["ScheduleLagg"] = 0;
                    }

                    row["MissCount"] = item.missed;
                    if (item.startDate.HasValue)
                    {
                        row["WorkStartDate"] = item.startDate.Value;
                    }

                    if (item.endDate.HasValue)
                    {
                        row["WorkEndDate"] = item.endDate.Value;
                    }

                    row["Later15August"] = item.later15August;

                    if (item.laggPercent.HasValue)
                    {
                        row["ScheduleLagg"] = item.laggPercent.Value;
                    }

                    table.Rows.Add(row);
                }
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
            var percentCellFormat = new CellFormat { FormatString = "p", FormatType = FormatType.Numeric };
            var datetimeCellFormat = new CellFormat { FormatString = "d", FormatType = FormatType.DateTime };
            var datetimeValueFormat = new ValueFormat { FormatString = "d", FormatType = FormatType.DateTime };

            return new PivotConfiguration
                       {
                           ModelName = "CR Report.WorkScheldudeInfoReport",
                           Name = "WorkScheldudeInfoReport",
                           Fields =
                               new List<Field>
                                   {
                                       // RowArea
                                       new Field
                                           {
                                               Area = Area.RowArea,
                                               DisplayName = "Муниципальное образование",
                                               Name = "Municipality",
                                               AreaIndex = 0
                                           },
                                       new Field
                                           {
                                               Area = Area.RowArea,
                                               DisplayName = "Подрядчик",
                                               Name = "Builder",
                                               AreaIndex = 1,
                                           },
                                       new Field
                                           {
                                               Area = Area.RowArea,
                                               DisplayName = "Реестровый номер ГЖИ",
                                               Name = "NumberGji",
                                               AreaIndex = 2,
                                           },
                                       new Field
                                           {
                                               Area = Area.RowArea,
                                               DisplayName = "Адрес",
                                               Name = "Address",
                                               AreaIndex = 3,
                                           },
                                           
                                       // ColumnArea
                                       new Field
                                           {
                                               Area = Area.ColumnArea,
                                               DisplayName = "Код работы",
                                               Name = "WorkCode",
                                               AreaIndex = 0
                                           },
                                       new Field
                                           {
                                               Area = Area.ColumnArea,
                                               DisplayName = "Виды работ",
                                               Name = "WorkName",
                                               AreaIndex = 1
                                           },
                                       
                                       // DataArea
                                       new Field
                                           {
                                               Area = Area.DataArea,
                                               DisplayName = "Работы",
                                               Name = "WorkCount",
                                               AreaIndex = 1,
                                               SummaryType = SummaryType.Sum,
                                               CellFormat = numericCellFormat
                                           },
                                      new Field
                                           {
                                               Area = Area.DataArea,
                                               DisplayName = "Отсутствуют графики",
                                               Name = "MissCount",
                                               AreaIndex = 2,
                                               SummaryType = SummaryType.Sum,
                                               CellFormat = numericCellFormat
                                           },
                                      new Field
                                           {
                                               Name = "WorkStartDate",
                                               Area = Area.DataArea,
                                               DisplayName = "Начало работ",
                                               CellFormat = datetimeCellFormat,
                                               ValueFormat = datetimeValueFormat,
                                               SummaryType = SummaryType.Min,
                                               AreaIndex = 3
                                           },
                                      new Field
                                           {
                                               Name = "WorkEndDate",
                                               Area = Area.DataArea,
                                               DisplayName = "Окончание работ",
                                               CellFormat = datetimeCellFormat,
                                               ValueFormat = datetimeValueFormat,
                                               SummaryType = SummaryType.Max,
                                               AreaIndex = 4
                                           },
                                      new Field
                                           {
                                               Area = Area.DataArea,
                                               DisplayName = "Окончание позже 15 августа",
                                               Name = "Later15August",
                                               SummaryType = SummaryType.Sum,
                                               CellFormat = numericCellFormat,
                                               AreaIndex = 5
                                           },
                                      new Field
                                           {
                                               Area = Area.DataArea,
                                               DisplayName = "% отставания от графика",
                                               Name = "ScheduleLagg",
                                               SummaryType = SummaryType.Average,
                                               CellFormat = percentCellFormat,
                                               AreaIndex = 6
                                           }
                                   }
                       };
        }
    }
}
