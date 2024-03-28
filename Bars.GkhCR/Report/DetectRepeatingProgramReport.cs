namespace Bars.GkhCr.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    
    class DetectRepeatingProgramReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalities;

        private int programCr;

        private List<long> programCrAdditional;

        private readonly List<string> workCodes = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", 
            "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "29", "30", "88", "1018", "1019", "1020", "1021" };
        private readonly List<string> workCodesWithoutVolume = new List<string> { "1018", "1020" };

        private readonly Dictionary<string, string> aggregatedWorkCodesDict;
        private readonly List<string> aggregatedWorkCodes;

        public DetectRepeatingProgramReport()
            : base(new ReportTemplateBinary(Properties.Resources.DetectRepeatingProgram))
        {
            aggregatedWorkCodesDict = workCodes.ToDictionary(x => x, x => x);
            // Агрегируем значения по работам с кодами 1018 и 1019 в одно поле (с кодом 1018)
            aggregatedWorkCodesDict["1019"] = "1018";
            aggregatedWorkCodes = aggregatedWorkCodesDict.Values.Distinct().ToList();
        }

        public override string Name
        {
            get
            {
                return "Выявление повторных объектов по программам капремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Выявление повторных объектов по программам капремонта";
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
                return "B4.controller.report.DetectRepeatingProgram";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.DetectRepeatingProgram";
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

        public override string ReportGenerator { get; set; }

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
                                .Select(x => new { x.Id, x.Name })
                                .ToDictionary(x => x.Id, x => x.Name);

            var muList = serviceMunicipality.GetAll()
                .Select(x => new { x.Id, x.Name, x.Group })
                .OrderBy(x => x.Group ?? x.Name)
                .ThenBy(x => x.Name)
                .ToList();

            var alphabeticalGroups = new List<List<long>>();

            var lastGroup = "extraordinaryString";

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
                               .WhereIf(this.municipalities.Count> 0, x => this.municipalities.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                               .Where(x => workCodes.Contains(x.Work.Code))
                               .Where(x => x.FinanceSource.TypeFinanceGroup == TypeFinanceGroup.ProgramCr)
                               .Where(x => serviceTypeWorkCr.GetAll()
                                                  .Any(y => programCrAdditional.Contains(y.ObjectCr.ProgramCr.Id) 
                                                      && y.ObjectCr.RealityObject.Id == x.ObjectCr.RealityObject.Id
                                                      && y.FinanceSource.TypeFinanceGroup == TypeFinanceGroup.ProgramCr
                                                      && workCodes.Contains(y.Work.Code)))
                               .Select(x => x.ObjectCr.RealityObject.Id);

            var workDataByCodeByProgramByRoByMuDict = serviceTypeWorkCr.GetAll()
                         .Where(x => realtyObjectSubquery.Contains(x.ObjectCr.RealityObject.Id))
                         .Where(x => workCodes.Contains(x.Work.Code))
                         .Where(x => allPrograms.Contains(x.ObjectCr.ProgramCr.Id))
                         .Where(x => x.FinanceSource.TypeFinanceGroup == TypeFinanceGroup.ProgramCr)
                         .Select(x => new
                             {
                                 muId = x.ObjectCr.RealityObject.Municipality.Id,
                                 x.ObjectCr.RealityObject.Address,
                                 ProgramCrId = x.ObjectCr.ProgramCr.Id,
                                 x.Work.Code,
                                 x.Sum,
                                 x.Volume
                             })
                         .AsEnumerable()
                         .GroupBy(x => x.muId)
                         .ToDictionary(
                             x => x.Key,
                             x =>
                             x.GroupBy(y => y.Address)
                              .ToDictionary(
                                  y => y.Key,
                                  y =>
                                  y.GroupBy(z => z.ProgramCrId)
                                   .ToDictionary(
                                       w => w.Key,
                                       w => w.GroupBy(v => aggregatedWorkCodesDict[v.Code]).ToDictionary(
                                           v => v.Key,
                                           v => new TypeWorkCrProxy { Volume = v.Sum(u => u.Volume), Sum = v.Sum(u => u.Sum) } 
                                       )
                                   )
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

                sectionMain.ДобавитьСтроку();

                var groupName = muNameAndGroupByMuIdDict[group.First()].Group;

                if (!string.IsNullOrEmpty(groupName))
                {
                    sectionGroup.ДобавитьСтроку();
                    sectionGroup["GroupName"] = groupName;
                }
                
                var groupSummary = FillMunicipalityGroupData(
                    sectionGroupMunicipality, 
                    sectionGroupMunicipalityObjectCrGroup, 
                    sectionGroupMunicipalityObjectCr, 
                    sectionGroupMunicipalitySummary,
                    group.ToDictionary(x => x, x => muNameAndGroupByMuIdDict[x].Name),
                    programCrDict,
                    workDataByCodeByProgramByRoByMuDict); 
                
                // Итоги группы
                if (!string.IsNullOrEmpty(groupName))
                {
                    FillSummary(sectionGroupSummary, groupSummary, programCrDict);
                }

                AddToSummary(summary, groupSummary);
            }

            // Общие итоги
            FillSummary(sectionSummary, summary, programCrDict);
        }

        private void FillSummary(Section section, Dictionary<long, Dictionary<string, TypeWorkCrProxy>> workDataByCodeByProgramDict, Dictionary<long, string> programCrDict)
        {
            foreach (var workDataByCodeByProgram in workDataByCodeByProgramDict)
            {
                section.ДобавитьСтроку();
                section["program"] = programCrDict[workDataByCodeByProgram.Key];
                
                var data = workDataByCodeByProgram.Value;

                foreach (var workCode in aggregatedWorkCodes)
                {
                    if (data.ContainsKey(workCode))
                    {
                        var typeWorkCrProxy = data[workCode];
                        section["sum" + workCode] = typeWorkCrProxy.Sum;

                        if (!workCodesWithoutVolume.Contains(workCode))
                        {
                            section["volume" + workCode] = typeWorkCrProxy.Volume;
                        }
                    }
                    else
                    {
                        section["sum" + workCode] = string.Empty;
                        section["volume" + workCode] = string.Empty;
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
                if (!workDataByCodeByProgramByRoByMuDict.ContainsKey(municipality.Key))
                {
                    continue;
                }

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
                            if (workCodesWithoutVolume.Contains(data.Key))
                            {
                                if (data.Value.Sum > 0)
                                {
                                    modifiedCodes.Add(data.Key);
                                    realtyObjectSummaryDict[data.Key].Sum += data.Value.Sum;
                                }
                            }
                            else if (data.Value.Sum > 0 || data.Value.Volume > 0)
                            {
                                modifiedCodes.Add(data.Key);
                                realtyObjectSummaryDict[data.Key].Sum += data.Value.Sum;
                                realtyObjectSummaryDict[data.Key].Volume += data.Value.Volume;
                            }
                        }
                    }

                    // Итоги по дому 
                    var realtyObjectSummary = realtyObjectSummaryDict.Where(x => modifiedCodes.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

                    FillObjectCrData(objectGroupSection, realtyObjectSummary, string.Empty, address, string.Empty);

                    AddToSummary(municipalitySummary, workDataByCodeByProgramDict);
                }

                // Итоги по муниципальному образованию
                FillSummary(municipalitySummarySection, municipalitySummary, programCrDict);
                AddToSummary(municipalityGroupSummary, municipalitySummary);
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

                    if (!workCodesWithoutVolume.Contains(workCode))
                    {
                        section["volume" + workCode] = typeWorkCrProxy.Volume;
                    }
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
    }
}
