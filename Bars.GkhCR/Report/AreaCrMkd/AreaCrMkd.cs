namespace Bars.GkhCr.Report.AreaCrMkd
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
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Olap-отчет Площадь охваченных капремонтом МКД (Журнал ч.5)
    /// </summary>
    public class AreaCrMkd : IPrintForm, IPivotModel
    {
        #region Свойства
        public IWindsorContainer Container { get; set; }
        public string Name
        {
            get
            {
                return "Площадь охваченных капремонтом МКД (Журнал ч.5)";
            }
        }

        public IList<string> ReportFormats { get; private set; }

        public string Desciption
        {
            get
            {
                return "Площадь охваченных капремонтом МКД (Журнал ч.5)";
            }
        }

        public string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public string ParamsController
        {
            get
            {
                return "B4.controller.report.AreaCrMkd";
            }
        }

        public string RequiredPermission
        {
            get
            {
                return "Reports.CR.AreaCrMkd";
            }
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

        public string Params { get; set; }

        public object Data { get; set; }
        #endregion

        public void LoadData()
        {
            if (string.IsNullOrEmpty(Params))
            {
                return;
            }

            var dynDict = DynamicDictionary.FromString(Params);

            var programId = dynDict.ContainsKey("programCrId") ? dynDict["programCrId"].ToLong() : 0;
            var finSourcesIdList = dynDict.ContainsKey("finSourceIds") ? dynDict["finSourceIds"].ToStr().Split("%2c+").Select(x => x.ToLong()).ToList() : new List<long>();
            var reportDate = dynDict.ContainsKey("reportDate") ? Uri.UnescapeDataString(dynDict["reportDate"].ToString()).ToDateTime() : DateTime.Now.Date;
            var graph = dynDict.ContainsKey("graph") ? dynDict["graph"].ToInt() : 0;  // график выполнения: учитывать = 1; не учитывать = 0 (по умолчанию)

            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Get(programId);
            var programDate = reportDate.Date.Year;
            if (programCr != null)
            {
                programDate = programCr.Period.DateEnd.HasValue
                                    ? programCr.Period.DateEnd.Value.Year
                                    : reportDate.Date.Year;
            }

            var table = new DataTable();

            table.Columns.Add(new DataColumn("Municipality"));
            table.Columns.Add(new DataColumn("Builder"));
            table.Columns.Add(new DataColumn("FinanceSource"));
            table.Columns.Add(new DataColumn("Address"));

            table.Columns.Add(new DataColumn("CountHouses", typeof(decimal)));
            table.Columns.Add(new DataColumn("TotalAreaMkd", typeof(decimal)));
            table.Columns.Add(new DataColumn("AreaMkdStartWork", typeof(decimal)));
            table.Columns.Add(new DataColumn("AreaMkdEndWork", typeof(decimal)));
            table.Columns.Add(new DataColumn("AreaMkdCurrentWork", typeof(decimal)));
            table.Columns.Add(new DataColumn("LimitSum", typeof(decimal)));

            #region Запросы
            var dictTypeWork = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                .WhereIf(finSourcesIdList.Count > 0, x => finSourcesIdList.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    ObjectCrId = x.ObjectCr.Id,
                    muName = x.ObjectCr.RealityObject.Municipality.Name,
                    addressObj = x.ObjectCr.RealityObject.Address,
                    totalAreaMkd = x.ObjectCr.RealityObject.AreaMkd ?? 0M,
                    FinSourceId = x.FinanceSource.Id,
                    FinSourceName = x.FinanceSource.Name
                })
                .AsEnumerable()
                .OrderBy(x => x.muName)
                .ThenBy(x => x.addressObj)
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, y => y
                    .GroupBy(z => z.FinSourceId)
                    .ToDictionary(z => z.Key, z => z
                    .Select(x => new
                    {
                        x.ObjectCrId,
                        x.muName,
                        x.addressObj,
                        x.totalAreaMkd,
                        x.FinSourceName
                    }).FirstOrDefault()));

            var dictBuildContract = Container.Resolve<IDomainService<BuildContract>>().GetAll()
            .Where(x => x.ObjectCr.ProgramCr.Id == programId
                && x.TypeContractBuild != TypeContractBuild.NotDefined)
                .Select(x => new
                {
                     ObjectCrId = x.ObjectCr.Id,
                     x.TypeContractBuild,
                     BuilderName = x.Builder.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, y => y.OrderBy(x => x.TypeContractBuild).Select(x => x.BuilderName).FirstOrDefault());

            // берем посл.запись из архива смр
            var dictArchieveSmr = Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programId
                     && x.TypeWorkCr.Work.TypeWork == TypeWork.Work
                     && x.DateChangeRec <= reportDate)
                .WhereIf(finSourcesIdList.Count > 0, x => finSourcesIdList.Contains(x.TypeWorkCr.FinanceSource.Id))
                .Select(x => new
                {
                    TypeWorkCrId = x.TypeWorkCr.Id,
                    objCrId = x.TypeWorkCr.ObjectCr.Id,
                    x.PercentOfCompletion,
                    AreaMkd = x.TypeWorkCr.ObjectCr.RealityObject.AreaMkd ?? 0M,
                    finsourcId = x.TypeWorkCr.FinanceSource.Id,
                    x.TypeWorkCr.DateStartWork,
                    x.TypeWorkCr.DateEndWork,
                    x.ObjectCreateDate
                })
                .AsEnumerable()
                .GroupBy(x => x.TypeWorkCrId)
                .ToDictionary(x => x.Key,
                    y => y.OrderByDescending(x => x.ObjectCreateDate)
                        .Select(x => new
                                         {
                                             x.TypeWorkCrId, 
                                             x.objCrId, 
                                             x.PercentOfCompletion, 
                                             x.AreaMkd, 
                                             x.finsourcId, 
                                             x.DateStartWork, 
                                             x.DateEndWork
                                         })
                        .FirstOrDefault());


            // Словарь по Архиву Смр
            var dictArch = dictArchieveSmr.GroupBy(x => x.Value.objCrId)
                .ToDictionary(x => x.Key, x => x
                    .GroupBy(z => z.Value.finsourcId)
                    .ToDictionary(z => z.Key, z => z
                    .Select(y => new
                                {
                                    y.Value.TypeWorkCrId,
                                    y.Value.PercentOfCompletion,
                                    y.Value.AreaMkd,
                                    y.Value.DateEndWork
                                }).ToList()));


            // Площадь МКД, по которым начаты работы
            var dictWorksStart = programDate >= 2013 ? dictArch
                .ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y.Key, y =>
                    {
                        var nullPercent = y.Value.Any(objCr => objCr.PercentOfCompletion == 0);
                        return nullPercent ? 0 : y.Value.Select(z => z.AreaMkd).FirstOrDefault();
                    }))
                : Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId
                    && x.Work.TypeWork == TypeWork.Work
                    && x.ObjectCreateDate <= reportDate)
                .WhereIf(finSourcesIdList.Count > 0, x => finSourcesIdList.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    typeWorkId = x.Id,
                    ObjectCrId = x.ObjectCr.Id,
                    areaMkd = x.ObjectCr.RealityObject.AreaMkd ?? 0M,
                    x.PercentOfCompletion,
                    finSourceId = x.FinanceSource.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, x => x
                    .GroupBy(y => y.finSourceId)
                    .ToDictionary(y => y.Key, y =>
                    {
                        var nullPercent = x.Any(objCr => objCr.PercentOfCompletion == 0);
                        return nullPercent ? 0 : x.Select(z => z.areaMkd).FirstOrDefault();
                    }));


            // Площадь МКД, по которым завершены работы
            var dictWorksEnd = new Dictionary<long, Dictionary<long, decimal>>();
            if (programDate >= 2013)
            {
                dictWorksEnd = graph == 0 ? dictArch
               .ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y.Key, y =>
                   {
                       var nullPercent = y.Value.All(objCr => objCr.PercentOfCompletion == 100);
                       return nullPercent ? y.Value.Select(z => z.AreaMkd).FirstOrDefault() : 0;
                   }))
                   : dictArch
                   .ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y.Key, y =>
                    {
                        var nullPercent = y.Value.All(objCr => objCr.PercentOfCompletion == 100 && (!objCr.DateEndWork.HasValue || objCr.DateEndWork <= reportDate));
                        return nullPercent ? y.Value.Select(z => z.AreaMkd).FirstOrDefault() : 0;
                    }));
            }
            else
            {
                    dictWorksEnd = graph == 0 ? Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programId
                        && x.Work.TypeWork == TypeWork.Work
                        && x.ObjectCreateDate <= reportDate)
                    .WhereIf(finSourcesIdList.Count > 0, x => finSourcesIdList.Contains(x.FinanceSource.Id))
                    .Select(x => new
                    {
                        typeWorkId = x.Id,
                        ObjectCrId = x.ObjectCr.Id,
                        areaMkd = x.ObjectCr.RealityObject.AreaMkd ?? 0M,
                        x.PercentOfCompletion,
                        finSourceId = x.FinanceSource.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCrId)
                    .ToDictionary(x => x.Key, x => x
                        .GroupBy(y => y.finSourceId)
                        .ToDictionary(y => y.Key, y =>
                        {
                            var nullPercent = x.All(objCr => objCr.PercentOfCompletion == 100);
                            return nullPercent ? x.Select(z => z.areaMkd).FirstOrDefault() : 0;
                        }))
                    : Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programId
                        && x.Work.TypeWork == TypeWork.Work
                        && x.ObjectCreateDate <= reportDate
                        && (!x.DateEndWork.HasValue || x.DateEndWork <= reportDate))
                    .WhereIf(finSourcesIdList.Count > 0, x => finSourcesIdList.Contains(x.FinanceSource.Id))
                    .Select(x => new
                    {
                        typeWorkId = x.Id,
                        ObjectCrId = x.ObjectCr.Id,
                        areaMkd = x.ObjectCr.RealityObject.AreaMkd ?? 0M,
                        x.PercentOfCompletion,
                        finSourceId = x.FinanceSource.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCrId)
                    .ToDictionary(x => x.Key, x => x
                        .GroupBy(y => y.finSourceId)
                        .ToDictionary(y => y.Key, y =>
                        {
                            var nullPercent = x.All(objCr => objCr.PercentOfCompletion == 100);
                            return nullPercent ? x.Select(z => z.areaMkd).FirstOrDefault() : 0;
                        }));
            }


            // Площадь МКД, по которым работы не завершены
            var dictWorksCurrent = new Dictionary<long, Dictionary<long, decimal>>();
            if (programDate >= 2013)
            {
                dictWorksCurrent = graph == 0 ? dictArch
               .ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y.Key, y =>
               {
                   var nullPercent = y.Value.All(objCr => objCr.PercentOfCompletion != 100);
                   return nullPercent ? y.Value.Select(z => z.AreaMkd).FirstOrDefault() : 0;
               }))
                   : dictArch
                   .ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y.Key, y =>
                   {
                       var nullPercent = y.Value.All(objCr => objCr.PercentOfCompletion != 100 && (!objCr.DateEndWork.HasValue || objCr.DateEndWork <= reportDate));
                       return nullPercent ? y.Value.Select(z => z.AreaMkd).FirstOrDefault() : 0;
                   }));
            }
            else
            {
                dictWorksCurrent = graph == 0 ? Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId      
                    && x.Work.TypeWork == TypeWork.Work
                    && x.ObjectCreateDate <= reportDate)
                .WhereIf(finSourcesIdList.Count > 0, x => finSourcesIdList.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    typeWorkId = x.Id,
                    ObjectCrId = x.ObjectCr.Id,
                    areaMkd = x.ObjectCr.RealityObject.AreaMkd ?? 0M,
                    x.PercentOfCompletion,
                    finSourceId = x.FinanceSource.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, x => x
                    .GroupBy(y => y.finSourceId)
                    .ToDictionary(y => y.Key, y =>
                    {
                        var nullPercent = x.All(objCr => objCr.PercentOfCompletion != 100);
                        return nullPercent ? y.Select(z => z.areaMkd).FirstOrDefault() : 0;
                    }))
                : Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId
                    && x.Work.TypeWork == TypeWork.Work
                    && x.ObjectCreateDate <= reportDate
                    && (!x.DateEndWork.HasValue || x.DateEndWork <= reportDate))
                .WhereIf(finSourcesIdList.Count > 0, x => finSourcesIdList.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    typeWorkId = x.Id,
                    ObjectCrId = x.ObjectCr.Id,
                    areaMkd = x.ObjectCr.RealityObject.AreaMkd ?? 0M,
                    x.PercentOfCompletion,
                    finSourceId = x.FinanceSource.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, x => x
                    .GroupBy(y => y.finSourceId)
                    .ToDictionary(y => y.Key, y =>
                    {
                        var nullPercent = x.All(objCr => objCr.PercentOfCompletion != 100);
                        return nullPercent ? y.Select(z => z.areaMkd).FirstOrDefault() : 0;
                    }));
            }

            // Сумма по лимиту
            var limitSum = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId
                     && x.ObjectCreateDate <= reportDate)
                .WhereIf(finSourcesIdList.Count > 0, x => finSourcesIdList.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    TypeWorkCrId = x.Id,
                    limitSum = x.Sum ?? 0M,
                    finsourceId = x.FinanceSource.Id,
                    objCrId = x.ObjectCr.Id,
                })
                .AsEnumerable()
                .GroupBy(x => x.objCrId)
                .ToDictionary(x => x.Key, x => x
                    .GroupBy(y => y.finsourceId)
                    .ToDictionary(y => y.Key, y => new 
                     {
                        sum = y.Sum(z => z.limitSum)
                     }));
            #endregion

            foreach (var finance in dictTypeWork)
            {
                foreach (var item in finance.Value)
                {
                    var row = table.NewRow();

                    row["Municipality"] = item.Value.muName;
                    row["FinanceSource"] = item.Value.FinSourceName;
                    row["Address"] = item.Value.addressObj;
                    row["CountHouses"] = 1;

                    if (dictBuildContract.ContainsKey(finance.Key))
                    {
                        row["Builder"] = dictBuildContract[finance.Key];
                    }

                    row["TotalAreaMkd"] = item.Value.totalAreaMkd;

                    if (dictWorksStart.ContainsKey(finance.Key) && dictWorksStart[finance.Key].ContainsKey(item.Key))
                    {
                        row["AreaMkdStartWork"] = dictWorksStart[finance.Key][item.Key];
                    }

                    if (dictWorksEnd.ContainsKey(finance.Key) && dictWorksEnd[finance.Key].ContainsKey(item.Key))
                    {
                        row["AreaMkdEndWork"] = dictWorksEnd[finance.Key][item.Key];
                    }

                    if (dictWorksCurrent.ContainsKey(finance.Key) && dictWorksCurrent[finance.Key].ContainsKey(item.Key))
                    {
                        row["AreaMkdCurrentWork"] = dictWorksCurrent[finance.Key][item.Key];
                    }

                    if (limitSum.ContainsKey(finance.Key) && limitSum[finance.Key].ContainsKey(item.Key))
                    {
                        row["LimitSum"] = limitSum[finance.Key][item.Key].sum;
                    }

                    table.Rows.Add(row);
                }
            }

            Data = table;
        }

        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatString = "n", FormatType = FormatType.Numeric };

            var config = new PivotConfiguration
            {
                Name = "AreaCrMkd",
                ModelName = "CR Report.AreaCrMkd",
                CustomSummary = (sender, e) => CustomSummary.GetValue(e)
            };

            var Fields = new List<Field>
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
                                    Name = "Builder",
                                    DisplayName = "Подрядчик"
                                },
                            new Field
                                {
                                    Area = Area.RowArea,
                                    AreaIndex = 2,
                                    Name = "FinanceSource",
                                    DisplayName = "Разрез финансирования"
                                },
                            new Field
                                {
                                    Area = Area.RowArea,
                                    AreaIndex = 3,
                                    Name = "Address",
                                    DisplayName = "Адрес"
                                },

                            // Значения
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 1,
                                    Name = "CountHouses",
                                    DisplayName = "Кол-во домов по программе",
                                    SummaryType = SummaryType.Custom,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 2,
                                    Name = "TotalAreaMkd",
                                    DisplayName = "Общая площадь МКД, кв.метров",
                                    SummaryType = SummaryType.Custom,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 3,
                                    Name = "AreaMkdStartWork",
                                    DisplayName = "Площадь МКД, по которым начаты работы",
                                    SummaryType = SummaryType.Custom,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 4,
                                    Name = "AreaMkdEndWork",
                                    DisplayName = "Площадь МКД, по которым завершены работы",
                                    SummaryType = SummaryType.Custom,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 5,
                                    Name = "AreaMkdCurrentWork",
                                    DisplayName = "Площадь МКД, по которым работы не завершены",
                                    SummaryType = SummaryType.Custom,
                                    CellFormat = numericCellFormat
                                },
                            new Field
                                {
                                    Area = Area.DataArea,
                                    AreaIndex = 6,
                                    Name = "LimitSum",
                                    DisplayName = "Сумма по лимиту",
                                    SummaryType = SummaryType.Sum,
                                    CellFormat = numericCellFormat
                                }
                        };
            config.Fields = Fields;
            return config;           
        }
    }
}