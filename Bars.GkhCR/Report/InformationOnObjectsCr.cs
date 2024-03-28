namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class InformationOnObjectsCr : BasePrintForm
    {
        #region Входные параметры
        private DateTime dateReport = DateTime.Now;
        private long programCr;
        private List<long> financeSource = new List<long>();        
        #endregion

        #region Вспомогательные параметры
        private List<string> workCodes;
        #endregion

        public IWindsorContainer Container { get; set; }

        public InformationOnObjectsCr()
            : base(new ReportTemplateBinary(Properties.Resources.InformationOnObjectsCr))
        {
        }

        public override string Name
        {
            get { return "Отчет Информация по объектам о ходе КР по работам (ГЖИ)"; }
        }

        public override string Desciption
        {
            get { return "Отчет Информация по объектам о ходе КР по работам (ГЖИ)"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.InformationOnObjectsCr"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.InformationOnObjectsCr";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateReport = baseParams.Params["dateReport"].ToDateTime();

            var finSource = baseParams.Params["FinanceSource"].ToString();
            financeSource = string.IsNullOrEmpty(finSource) ? new List<long>() : finSource.Split(',').Select(x => x.ToLong()).ToList();
            programCr = baseParams.Params["ProgramCr"].ToInt();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var dictCodeWork = new Dictionary<string, string>();
            dictCodeWork.Add("1", "Ремонт внутридомовой инженерной системы теплоснабжения");
            dictCodeWork.Add("2", "Ремонт внутридомовой инженерной системы ГВС");
            dictCodeWork.Add("3", "Ремонт внутридомовой инженерной системы ХВС");
            dictCodeWork.Add("4", "Ремонт внутридомовой инженерной системы канализации");
            dictCodeWork.Add("5", "Ремонт внутридомовой инженерной системы газоснабжения");
            dictCodeWork.Add("6", "Ремонт внутридомовой инженерной системы электроснабжения");
            dictCodeWork.Add("7", "Установка приборов учета, узлов управления тепла");
            dictCodeWork.Add("8", "Установка приборов учета, узлов управления ГВС");
            dictCodeWork.Add("9", "Установка приборов учета, узлов управления ХВС");
            dictCodeWork.Add("10", "Установка приборов учета, узлов управления электроэнергии");
            dictCodeWork.Add("11", "Установка приборов учета, узлов управления газа");
            dictCodeWork.Add("12", "Ремонт подвального помещения");
            dictCodeWork.Add("13", "Ремонт крыши");
            dictCodeWork.Add("14", "Ремонт/замена лифтового оборудования");
            dictCodeWork.Add("15", "Ремонт лифтовой шахты");
            dictCodeWork.Add("16", "Ремонт фасада");
            dictCodeWork.Add("17", "Утепление фасада");
            dictCodeWork.Add("999", "Прочие ремонтно-строительные работы");
            dictCodeWork.Add("19", "Устройство систем противопожарной автоматики и дымоудаления");
            dictCodeWork.Add("20", "Благоустройство дворовых территорий");
            dictCodeWork.Add("21", "Ремонт подъездов");
            dictCodeWork.Add("22", "Ремонт конструкций методом инъектирования");
            dictCodeWork.Add("23", "Монтаж и демонтаж балконов");

            var vertColumn = reportParams.ComplexReportParams.ДобавитьСекцию("SectionVer");

            var columnNum = 9;            
            foreach (var work in dictCodeWork)
            {
                vertColumn.ДобавитьСтроку();

                vertColumn["Колонка1"] = columnNum++;
                vertColumn["Колонка2"] = columnNum++;
                vertColumn["Колонка3"] = columnNum++;
                vertColumn["Колонка4"] = columnNum++;
                vertColumn["Колонка5"] = columnNum++;
                vertColumn["Колонка6"] = columnNum++;
                vertColumn["Колонка7"] = columnNum++;
                vertColumn["Колонка8"] = columnNum++;
                vertColumn["Колонка9"] = columnNum++;
                
                vertColumn["ТипРабот"] = work.Value;
                vertColumn["СметаОбъем"] = string.Format("$СметаОбъем{0}$", work.Key);
                vertColumn["СметаСумма"] = string.Format("$СметаСумма{0}$", work.Key);
                vertColumn["ГрафНачальнаяДата"] = string.Format("$ГрафНачальнаяДата{0}$", work.Key);
                vertColumn["ГрафКонечнаяДата"] = string.Format("$ГрафКонечнаяДата{0}$", work.Key);
                vertColumn["ФактОбъем"] = string.Format("$ФактОбъем{0}$", work.Key);
                vertColumn["ФактПроцент"] = string.Format("$ФактПроцент{0}$", work.Key);
                vertColumn["ФактСумма"] = string.Format("$ФактСумма{0}$", work.Key);
                vertColumn["АктКоличество"] = string.Format("$АктКоличество{0}$", work.Key);
                vertColumn["АктСумма"] = string.Format("$АктСумма{0}$", work.Key);
            }

            this.workCodes = dictCodeWork.Keys.ToList();
            
            var objectCrQuery = Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == programCr);

            var objectCrIdsQuery = objectCrQuery.Select(x => x.Id);

            var objectCrInfo = objectCrQuery
                .Select(x => new 
                {
                    x.Id,
                    x.RealityObject.Address,
                    x.DateAcceptCrGji,
                    x.DateEndWork,
                    x.DateStopWorkGji,
                    x.GjiNum,
                    municipalityName = x.RealityObject.Municipality.Name
                })
                .ToList();

            var serviceBuildContract = Container.Resolve<IDomainService<BuildContract>>();
            var distObjectCrNameBuild = serviceBuildContract.GetAll()
                .Where(x => objectCrIdsQuery.Contains(x.ObjectCr.Id))
                .Select(x => new { ObjectCrId = x.ObjectCr.Id, NameBuild = x.Builder.Contragent.Name })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, v => v.First().NameBuild);
            
            var objectCrWorksDict = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .WhereIf(financeSource.Count > 0, x => financeSource.Contains(x.FinanceSource.Id))    
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCr)
                .Where(x => this.workCodes.Contains(x.Work.Code))
                .Select(x => new
                {
                    TypeWorkCrId = x.Id,
                    CodeWork = x.Work.Code,
                    SmetaVolume = x.Volume.ToDecimal(),
                    SmetaSum = x.Sum.ToDecimal(),
                    GrafDateStart = x.DateStartWork.ToDateTime(),
                    GrafDateEnd = x.DateEndWork.ToDateTime(),
                    FactVolume = x.VolumeOfCompletion.ToDecimal(),
                    FactSum = x.CostSum.ToDecimal(),
                    FactPercentage = x.PercentOfCompletion.ToDecimal(),
                    FinSourceId = x.FinanceSource.Id,
                    FinSourceName = x.FinanceSource.Name,
                    ObjectCrId = x.ObjectCr.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(
                    x => x.Key,
                    f => f.GroupBy(x => new { x.FinSourceId, x.FinSourceName })
                        .ToDictionary(
                            x => x.Key,
                            v => v.GroupBy(x => x.CodeWork)
                                .ToDictionary(x => x.Key, x => x.First())));

            var progCr = Container.Resolve<IDomainService<ProgramCr>>().Get(programCr);

            var year = progCr.Period.DateEnd.HasValue ? progCr.Period.DateEnd.Value.Year : dateReport.Year;
            
            var archiveRecs = (year < 2013) ? null
                : Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                .WhereIf(financeSource.Count > 0, x => financeSource.Contains(x.TypeWorkCr.FinanceSource.Id))
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == this.programCr)
                .Where(x => this.workCodes.Contains(x.TypeWorkCr.Work.Code))
                .Where(x => x.DateChangeRec <= this.dateReport)
                .Select(x => new
                {
                    x.Id,
                    TypeWorkCrId = x.TypeWorkCr.Id,
                    FactVolume = x.VolumeOfCompletion.ToDecimal(),
                    FactSum = x.CostSum.ToDecimal(),
                    FactPercentage = x.PercentOfCompletion.ToDecimal(),
                    x.DateChangeRec
                })
                .AsEnumerable()
                .GroupBy(x => x.TypeWorkCrId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                        {
                            var archiveRec = x.OrderByDescending(y => y.DateChangeRec).ThenByDescending(y => y.Id).First();
                            return new { archiveRec.FactVolume, archiveRec.FactSum, archiveRec.FactPercentage};
                        });

            var performedWorkActDict = Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll()
                .Where(x => objectCrIdsQuery.Contains(x.TypeWorkCr.ObjectCr.Id))
                .Select(x => new 
                {
                    TypeWorkCrId = x.TypeWorkCr.Id,
                    ActSum = x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.TypeWorkCrId)
                .ToDictionary(x => x.Key, x => new { ActSum = x.Sum(z => z.ActSum ?? 0), ActCount = x.Count() });

            // Вывод данных
            var sectionData = reportParams.ComplexReportParams.ДобавитьСекцию("SectionData");

            foreach (var objectCr in objectCrInfo.OrderBy(x => x.municipalityName).ThenBy(x => x.Address))
            {
                var objectCrId = objectCr.Id;

                if (objectCrWorksDict.ContainsKey(objectCrId))
                {
                    foreach (var finSource in objectCrWorksDict[objectCrId].OrderBy(x => x.Key.FinSourceName))
                    {
                        sectionData.ДобавитьСтроку();
                        sectionData["МО"] = objectCr.municipalityName;
                        sectionData["Подрядчик"] = distObjectCrNameBuild.ContainsKey(objectCrId)
                                                       ? distObjectCrNameBuild[objectCrId]
                                                       : string.Empty;

                        sectionData["НомерГЖИ"] = objectCr.GjiNum;
                        sectionData["ОбъектКр"] = objectCr.Address;
                        if (objectCr.DateAcceptCrGji != DateTime.MinValue)
                        {
                            sectionData["ДатаПринятия"] = objectCr.DateAcceptCrGji;
                        }

                        if (objectCr.DateEndWork != DateTime.MinValue)
                        {
                            sectionData["ДатаЗавершенияРабот"] = objectCr.DateEndWork;
                        }

                        if (objectCr.DateStopWorkGji != DateTime.MinValue)
                        {
                            sectionData["ДатаОстановки"] = objectCr.DateStopWorkGji;
                        }

                        sectionData["ИсточникФинансирования"] = finSource.Key.FinSourceName;

                        var totalSmetaVolume = 0M;
                        var totalSmetaSum = 0M;
                        var totalGrafDateStart = new List<DateTime>();
                        var totalGrafDateEnd = new List<DateTime>();
                        var totalFactVolume = 0M;
                        var totalFactPercentage = new List<decimal>();
                        var totalFactSum = 0M;
                        var totalActCount = 0;
                        var totalActSum = 0M;

                        foreach (var work in finSource.Value)
                        {
                            var itemData = work.Value;
                            var code = work.Key;

                            sectionData["СметаОбъем" + code] = itemData.SmetaVolume;
                            totalSmetaVolume += itemData.SmetaVolume;

                            sectionData["СметаСумма" + code] = itemData.SmetaSum;
                            totalSmetaSum += itemData.SmetaSum;

                            if (itemData.GrafDateStart != DateTime.MinValue)
                            {
                                sectionData["ГрафНачальнаяДата" + code] = itemData.GrafDateStart;
                                totalGrafDateStart.Add(itemData.GrafDateStart.Date);
                            }

                            if (itemData.GrafDateEnd != DateTime.MinValue)
                            {
                                sectionData["ГрафКонечнаяДата" + code] = itemData.GrafDateEnd;
                                totalGrafDateEnd.Add(itemData.GrafDateEnd);
                            }

                            var factVolume = itemData.FactVolume;
                            var factPercentage = itemData.FactPercentage;
                            var factSum = itemData.FactSum;

                            if (archiveRecs != null && archiveRecs.ContainsKey(itemData.TypeWorkCrId))
                            {
                                var archrec = archiveRecs[itemData.TypeWorkCrId];

                                factVolume = archrec.FactVolume;
                                factPercentage = archrec.FactPercentage;
                                factSum = archrec.FactSum;
                            }

                            sectionData["ФактОбъем" + code] = factVolume;
                            totalFactVolume += factVolume;

                            sectionData["ФактПроцент" + code] = factPercentage;
                            totalFactPercentage.Add(factPercentage);

                            sectionData["ФактСумма" + code] = factSum;
                            totalFactSum += factSum;

                            if (performedWorkActDict.ContainsKey(itemData.TypeWorkCrId))
                            {
                                var actData = performedWorkActDict[itemData.TypeWorkCrId];
                                
                                sectionData["АктКоличество" + code] = actData.ActCount;
                                totalActCount += actData.ActCount;
                                
                                sectionData["АктСумма" + code] = actData.ActSum;
                                totalActSum += actData.ActSum;
                            }
                        }

                        sectionData["ИтогоСметаОбъем"] = totalSmetaVolume;
                        sectionData["ИтогоСметаСумма"] = totalSmetaSum;
                        if (totalGrafDateStart.Any())
                        {
                            sectionData["ИтогоГрафНачальнаяДата"] = totalGrafDateStart.Min();
                        }

                        if (totalGrafDateEnd.Any())
                        {
                            sectionData["ИтогоГрафКонечнаяДата"] = totalGrafDateEnd.Max();
                        }

                        sectionData["ИтогоФактОбъем"] = totalFactVolume;
                        sectionData["ИтогоФактСумма"] = totalFactSum;
                        sectionData["ИтогоФактПроцент"] = totalFactPercentage.Average();
                        sectionData["ИтогоАктКоличество"] = totalActCount;
                        sectionData["ИтогоАктСумма"] = totalActSum;
                    }
                }
            }
        }
    }
}