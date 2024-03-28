namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Enums;
    using Castle.Windsor;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    public class DetectRepeatingProgramDistribServicesReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        #region Входящие параметры
        private List<long> municipalities;
        private long programCr;
        private List<long> programCrAdditional;
        #endregion        

        private readonly List<string> workCodes = new List<string> { "20", "1", "4", "5", "8", "9", "10", "11", "18", "19", "2", "3", "6", "13", "15", 
            "12", "21", "16", "14", "7", "17" };

        private readonly Dictionary<string, string> aggregatedWorkCodesDict;
        private readonly List<string> aggregatedWorkCodes;

        public DetectRepeatingProgramDistribServicesReport() : base(new ReportTemplateBinary(Properties.Resources.DetectRepeatingProgramDistribServices))
        {
            aggregatedWorkCodesDict = workCodes.ToDictionary(x => x, x => x);
            aggregatedWorkCodes = aggregatedWorkCodesDict.Values.Distinct().ToList();
        }

        public override string Name
        {
            get
            {
                return "Выявление повторных объектов по программам капремонта (распределение услуг)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Выявление повторных объектов по программам капремонта (распределение услуг)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Ход капремонта";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.DetectRepeatingProgramDistribServices";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.DetectRepeatingProgramDistribServices";
            }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var municipalityParam = baseParams.Params["municipalityIds"].ToString();
            municipalities = !string.IsNullOrEmpty(municipalityParam) ?  municipalityParam.Split(',').Select(x => x.ToLong()).ToList() : new List<long>();
            programCr = baseParams.Params["programCrId"].ToInt();
            var programAddParam = baseParams.Params["dopProgram"].ToString();
            programCrAdditional = !string.IsNullOrEmpty(programAddParam) ? programAddParam.Split(',').Select(x => x.ToLong()).ToList() : new List<long>();
        }

        private int rowNumber = 0;

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
            var serviceProgramCr = Container.Resolve<IDomainService<ProgramCr>>();
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();

            var allPrograms = new List<long> { this.programCr };
            allPrograms.AddRange(programCrAdditional);

            var programCrDict = serviceProgramCr.GetAll()
                                .Where(x => allPrograms.Contains(x.Id))
                                .Select(x => new { x.Id, x.Name, t = x.Id == programCr ? 0 : 1 })
                                .AsEnumerable()
                                .OrderBy(x => x.t)
                                .ToDictionary(x => x.Id, x => x.Name);

            var muList = serviceMunicipality.GetAll()
                .WhereIf(this.municipalities.Count > 0, x => this.municipalities.Contains(x.Id))
                .Select(x => new { x.Id, x.Name, x.Group })
                .OrderBy(x => x.Group ?? x.Name)
                .ThenBy(x => x.Name)
                .ToList();

            var alphabeticalGroups = new List<List<long>>();

            var lastGroup = "extraordinaryString";

            // Формирование списка МО по группам, где МО без групп
            // относятся к одной без указаниям наименования
            foreach (var municipality in muList)
            {
                if (municipality.Group != lastGroup)
                {
                    lastGroup = municipality.Group;
                    alphabeticalGroups.Add(new List<long>());
                }

                if (alphabeticalGroups.Any())
                {
                    alphabeticalGroups.Last().Add(municipality.Id);
                }
            }

            var muNameAndGroupByMuIdDict = muList.ToDictionary(y => y.Id, y => new { y.Name, y.Group });

            var realtyObjectSubquery = serviceTypeWorkCr.GetAll()
                               .Where(x => x.ObjectCr.ProgramCr.Id == programCr)
                               .WhereIf(this.municipalities.Count > 0, x => municipalities.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                               .Where(x => serviceTypeWorkCr.GetAll()
                                                  .Any(y => programCrAdditional.Contains(y.ObjectCr.ProgramCr.Id)
                                                      && y.ObjectCr.RealityObject.Id == x.ObjectCr.RealityObject.Id))
                               .Select(x => x.ObjectCr.RealityObject.Id);

            var workDataByCodeByProgramByRoByMuDict =
                serviceTypeWorkCr.GetAll()
                                 .Where(x => realtyObjectSubquery.Contains(x.ObjectCr.RealityObject.Id))
                                 .Where(x => allPrograms.Contains(x.ObjectCr.ProgramCr.Id))
                                 .Select(
                                     x => new
                                         {
                                             muId = x.ObjectCr.RealityObject.Municipality.Id,
                                             x.ObjectCr.RealityObject.Address,
                                             ProgramCrId = x.ObjectCr.ProgramCr.Id,
                                             WorkCode = x.Work.Code,
                                             TypeWork = x.Work.TypeWork,
                                             x.Sum,
                                             x.Volume
                                         })
                                 .AsEnumerable()
                                 .GroupBy(x => x.muId)
                                 .ToDictionary(
                                     x => x.Key,
                                     x => x.GroupBy(y => y.Address)
                                           .ToDictionary(
                                               y => y.Key,
                                               y => y.GroupBy(z => z.ProgramCrId)
                                                     .Select(u => new
                                                     {
                                                        groupedItem = u,
                                                        sumWork = u.Where(z => z.TypeWork == TypeWork.Work).Sum(z => z.Sum).ToDecimal(),
                                                        sumService = u.Where(z => z.TypeWork == TypeWork.Service).Sum(z => z.Sum).ToDecimal()
                                                     })
                                                     .Where(u => u.sumService <= 0 || u.sumService > 0 && u.sumWork != 0)
                                                     .ToDictionary(
                                                        w => w.groupedItem.Key,
                                                        w => 
                                                        {
                                                            var tmpDict = w.groupedItem.GroupBy(u => u.WorkCode)
                                                                    .ToDictionary(
                                                                        u => u.Key,
                                                                        u => new TypeWorkCrProxy
                                                                              {
                                                                                  Sum = u.Sum(z => z.Sum),
                                                                                  Volume = u.Sum(z => z.Volume)
                                                                              });

                                                            var dictSumWork = tmpDict.ToDictionary(z => z.Key, v => v.Value.Sum);
                                                            var dictVolumeWork = tmpDict.ToDictionary(z => z.Key, v => v.Value.Volume);
                                                            
                                                            if (w.sumService > 0)
                                                            {
                                                                var dopSum = dictSumWork.ToDictionary(k => k.Key, v => v.Value * (w.sumService / w.sumWork));
                                                                var tmpSum = dopSum.Sum(v => v.Value).ToDecimal();
                                                                var err = Math.Round(w.sumService - tmpSum, 2);
                                                                
                                                                var firstWork = dopSum.FirstOrDefault(z => z.Value > 0 && (z.Value + err) > 0).Key;
                                                                
                                                                if (firstWork != null)
                                                                {
                                                                    dopSum[firstWork] += err;
                                                                }
                                                                
                                                                tmpDict = dictSumWork.Where(z => workCodes.Contains(z.Key))
                                                                                     .ToDictionary(
                                                                                        z => z.Key,
                                                                                        v => new TypeWorkCrProxy()
                                                                                            {
                                                                                                Volume = dictVolumeWork[v.Key],
                                                                                                Sum = v.Value + dopSum[v.Key]
                                                                                            });
                                                            }
                                                            
                                                            return tmpDict;
                                                        })
                                                    )
                                                );

            // Секция названий программ КР
            var sectionPrograms = reportParams.ComplexReportParams.ДобавитьСекцию("sectionProgram");

            foreach (var programId in allPrograms)
            {
                sectionPrograms.ДобавитьСтроку();
                sectionPrograms["program"] = programCrDict[programId];
            }

            var sectionMain = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMain");
            var sectionGroup = sectionMain.ДобавитьСекцию("sectionGroup");
            var sectionGroupMunicipality = sectionMain.ДобавитьСекцию("sectionGroupMunicipality");
            var sectionGroupMunicipalityObjectCrGroup = sectionGroupMunicipality.ДобавитьСекцию("sectionGroupMunicipalityObjectCrGroup");
            var sectionGroupMunicipalityObjectCr = sectionGroupMunicipalityObjectCrGroup.ДобавитьСекцию("sectionGroupMunicipalityObjectCr");
            var sectionGroupMunicipalitySummary = sectionGroupMunicipality.ДобавитьСекцию("sectionGroupMunicipalitySummary");
            var sectionGroupSummary = sectionMain.ДобавитьСекцию("sectionGroupSummary");
            var sectionSummary = reportParams.ComplexReportParams.ДобавитьСекцию("sectionSummary");

            rowNumber = 0;

            var summary = new Dictionary<long, Dictionary<string, TypeWorkCrProxy>>();

            foreach (var group in alphabeticalGroups)
            {
                if (!group.Any())
                {
                    continue;
                }

                // Формируем словарь наименований МО только для тех,
                // по которым нашлись объекты капремонта с сформированными по ним суммами работ
                var existMunicipalities = group.Where(x => workDataByCodeByProgramByRoByMuDict.ContainsKey(x))
                    .ToDictionary(x => x, x => muNameAndGroupByMuIdDict[x].Name);

                // Добавление строк только в случае наличия данных,
                // чтобы пустой шаблон формировался корректно
                if (existMunicipalities.Keys.Any())
                {
                    sectionMain.ДобавитьСтроку();

                    var groupName = muNameAndGroupByMuIdDict[group.First()].Group;

                    // Добавление наименования группы МО (при наличии)
                    if (!string.IsNullOrEmpty(groupName))
                    {
                        sectionGroup.ДобавитьСтроку();
                        sectionGroup["GroupName"] = groupName;
                    }

                    // Заполнение секции с МО + итоги по каждому району
                    var groupSummary = FillMunicipalityGroupData(
                        sectionGroupMunicipality,
                        sectionGroupMunicipalityObjectCrGroup,
                        sectionGroupMunicipalityObjectCr,
                        sectionGroupMunicipalitySummary,
                        existMunicipalities,
                        programCrDict,
                        workDataByCodeByProgramByRoByMuDict);

                    // Итоги группы, только для группы с указанным нименованием
                    if (!string.IsNullOrEmpty(groupName))
                    {
                        FillSummary(sectionGroupSummary, groupSummary, programCrDict);
                    }

                    //AddToSummary(summary, groupSummary);
                }
            }

            // Общие итоги
            FillSummary(sectionSummary, summary, programCrDict);
        }

        private void FillSummary(Section section, Dictionary<long, Dictionary<string, TypeWorkCrProxy>> workDataByCodeByProgramDict, Dictionary<long, string> programCrDict)
        {
            foreach (var prog in programCrDict)
            {
                section.ДобавитьСтроку();
                section["program"] = prog.Value;

                if(!workDataByCodeByProgramDict.ContainsKey(prog.Key))
                {
                    continue;
                }

                var data = workDataByCodeByProgramDict[prog.Key];

                foreach (var workCode in aggregatedWorkCodes)
                {
                    if (data.ContainsKey(workCode))
                    {
                        var typeWorkCrProxy = data[workCode];
                        section["sum" + workCode] = typeWorkCrProxy.Sum;

                        section["volume" + workCode] = typeWorkCrProxy.Volume;
                    }
                    else
                    {
                        section["sum" + workCode] = 0;
                        section["volume" + workCode] = 0;
                    }
                }
            }
        }

        private Dictionary<long, Dictionary<string, TypeWorkCrProxy>> FillMunicipalityGroupData(
            Section municipalitySection,
            Section objectGroupSection,
            Section objectSection,
            Section municipalitySummarySection,
            Dictionary<long, string> municipalityDict,
            Dictionary<long, string> programCrDict,
            IDictionary<long, Dictionary<string, Dictionary<long, Dictionary<string, TypeWorkCrProxy>>>> workDataByCodeByProgramByRoByMuDict)
        {
            var municipalityGroupSummary = new Dictionary<long, Dictionary<string, TypeWorkCrProxy>>();

            foreach (var municipality in municipalityDict.OrderBy(x => x.Value))
            {
                municipalitySection.ДобавитьСтроку();
                municipalitySection["MunicipalityName"] = municipality.Value;

                var municipalityData = workDataByCodeByProgramByRoByMuDict[municipality.Key];

                var municipalitySummary = new Dictionary<long, Dictionary<string, TypeWorkCrProxy>>();                

                foreach (var realtyObject in municipalityData.OrderBy(x => x.Key))
                {
                    objectGroupSection.ДобавитьСтроку();
                    rowNumber++;

                    var address = realtyObject.Key;
                    var workDataByCodeByProgramDict = realtyObject.Value;

                    // Заполняем основную программу
                    var mainProgramData = workDataByCodeByProgramDict[programCr];
                    var realtyObjectSummaryDict = mainProgramData
                        .Where(x => x.Value.Sum > 0 || x.Value.Volume > 0)
                        .ToDictionary(x => x.Key, x => new TypeWorkCrProxy { Sum = x.Value.Sum, Volume = x.Value.Volume });

                    var modifiedCodes = new List<string>();                    

                    objectSection.ДобавитьСтроку();
                    FillObjectCrData(objectSection, mainProgramData, programCrDict[programCr], address, rowNumber.ToString());

                    // Заполняем доп программы
                    foreach (var programData in workDataByCodeByProgramDict.Where(x => x.Key != programCr))
                    {
                        objectSection.ДобавитьСтроку();
                        FillObjectCrData(objectSection, programData.Value, programCrDict[programData.Key], address, rowNumber.ToString());

                        // Подсчет суммы по дому
                        foreach (var data in programData.Value.Where(data => realtyObjectSummaryDict.ContainsKey(data.Key)))
                        {                            
                            if (!(data.Value.Sum > 0) && !(data.Value.Volume > 0))
                            {
                                continue;
                            }

                            modifiedCodes.Add(data.Key);
                            realtyObjectSummaryDict[data.Key].Sum += data.Value.Sum;
                            realtyObjectSummaryDict[data.Key].Volume += data.Value.Volume;
                        }
                    }

                    // Итоги по дому 
                    var realtyObjectSummary = realtyObjectSummaryDict.Where(x => modifiedCodes.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

                    FillObjectCrData(objectGroupSection, realtyObjectSummary, string.Empty, address, string.Empty);

                    var dataForAddToSummary = new Dictionary<long, Dictionary<string, TypeWorkCrProxy>>();

                    var listWorkDistinctRo = workDataByCodeByProgramDict.Values.SelectMany(x => x.Keys).Distinct().ToList();

                    foreach (var work in listWorkDistinctRo)
                    {
                        var cr = workDataByCodeByProgramDict.SelectMany(x => x.Value.Keys).Count(x => x == work);
                        if (cr > 1)
                        {
                            foreach (var progCr in workDataByCodeByProgramDict.Where(x => x.Value.ContainsKey(work)))
                            {
                                if (dataForAddToSummary.ContainsKey(progCr.Key))
                                {
                                    if (dataForAddToSummary[progCr.Key].ContainsKey(work))
                                    {
                                        dataForAddToSummary[progCr.Key][work].Sum += progCr.Value[work].Sum;
                                        dataForAddToSummary[progCr.Key][work].Volume += progCr.Value[work].Volume;
                                    }
                                    else
                                    {
                                        dataForAddToSummary[progCr.Key].Add(work, new TypeWorkCrProxy { Sum = progCr.Value[work].Sum, Volume = progCr.Value[work].Volume });                                        
                                    }
                                }
                                else
                                {
                                    dataForAddToSummary.Add(progCr.Key, new Dictionary<string, TypeWorkCrProxy>());
                                    dataForAddToSummary[progCr.Key].Add(work, new TypeWorkCrProxy { Sum = progCr.Value[work].Sum, Volume = progCr.Value[work].Volume });                                    
                                }
                            }                            
                        }
                    }

                    AddToSummary(municipalitySummary, dataForAddToSummary);
                }

                // Итоги по муниципальному образованию
                FillSummary(municipalitySummarySection, municipalitySummary, programCrDict);
                //AddToSummary(municipalityGroupSummary, municipalitySummary);
            }

            return municipalityGroupSummary;
        }

        private void AddToSummary(Dictionary<long, Dictionary<string, TypeWorkCrProxy>> summary, Dictionary<long, Dictionary<string, TypeWorkCrProxy>> toBeAddedData)
        {
            foreach (var programData in toBeAddedData)
            {
                if (!summary.ContainsKey(programData.Key))
                {
                    summary[programData.Key] = programData.Value;
                }
                else
                {
                    foreach (var work in programData.Value)
                    {
                        if (!summary[programData.Key].ContainsKey(work.Key))
                        {
                            summary[programData.Key][work.Key] = work.Value;
                        }
                        else
                        {
                            summary[programData.Key][work.Key].Sum += work.Value.Sum;
                            summary[programData.Key][work.Key].Volume += work.Value.Volume;
                        }
                    }
                }
            }
        }

        private void FillObjectCrData(Section section, IDictionary<string, TypeWorkCrProxy> data, string programName, string address, string num)
        {
            section["num"] = num;
            section["address"] = address;
            section["program"] = programName;

            foreach (var workCode in aggregatedWorkCodes)
            {
                if (data.ContainsKey(workCode))
                {
                    var typeWorkCrProxy = data[workCode];
                    section["sum" + workCode] = typeWorkCrProxy.Sum;

                    section["volume" + workCode] = typeWorkCrProxy.Volume;                    
                }
                else
                {
                    section["sum" + workCode] = string.Empty;
                    section["volume" + workCode] = string.Empty;
                }
            }
        }

        class TypeWorkCrProxy
        {
            public decimal? Volume;
            public decimal? Sum;
        }

        public override string ReportGenerator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}