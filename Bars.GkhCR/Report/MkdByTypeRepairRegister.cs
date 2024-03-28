namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Реестр по видам работ
    /// </summary>
    internal class MkdByTypeRepairRegister : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programCrId;

        private long[] municipalityIds;

        public MkdByTypeRepairRegister()
            : base(new ReportTemplateBinary(Properties.Resources.MkdByTypeRepairRegister))
        {
        }

        public override string Name
        {
            get
            {
                return "Реестр по видам работ";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Реестр по видам работ";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Формы программы";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.MkdByTypeRepairRegister";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.MkdByTypeRepairRegister";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            var municipalityStr = baseParams.Params["municipalityIds"].ToStr();
            this.municipalityIds = string.IsNullOrEmpty(municipalityStr)
                                       ? new long[0]
                                       : municipalityStr.Split(',').Select(x => x.ToLong()).ToArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceFinanceSource = this.Container.Resolve<IDomainService<FinanceSource>>();

            // Финансирование по 185-ФЗ
            var financeSourceId185Fz =
                serviceFinanceSource.GetAll().Where(x => x.Code == "1").Select(x => x.Id).FirstOrDefault();

            // Финансирование по 185-ФЗ (по доп. программам)
            var financeSourceId185FzAdditional =
                serviceFinanceSource.GetAll().Where(x => x.Code == "3").Select(x => x.Id).FirstOrDefault();

            var workGroups = Enumerable.Range(1, 6).Select(x => x.ToStr()).ToDictionary(x => x, x => "1");
            Enumerable.Range(12, 3).Select(x => x.ToStr()).ForEach(x => workGroups[x] = x);
            Enumerable.Range(18, 6).Select(x => x.ToStr()).ForEach(x => workGroups[x] = x);
            workGroups["16"] = "16";
            workGroups["17"] = "16";
            workGroups["88"] = "88";//Установка ИТП
            Enumerable.Range(141, 3).Select(x => x.ToStr()).ForEach(x => workGroups[x] = x);//лифты

            var serviceTypeWork =
                this.Container.Resolve<IDomainService<TypeWorkCr>>()
                    .GetAll()
                    .Where(x => x.FinanceSource.Id == financeSourceId185Fz || x.FinanceSource.Id == financeSourceId185FzAdditional)
                    .Where(x => workGroups.Keys.Contains(x.Work.Code))
                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId);

            var municipalityList = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Name })
                    .OrderBy(x => x.Name)
                    .ToList();

            var crObjects = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => serviceTypeWork.Select(y => y.ObjectCr.Id).Contains(x.Id))
                .Select(x => new
                        {
                            objectCrId = x.Id,
                            x.RealityObject.Address,
                            muId = x.RealityObject.Municipality.Id
                        })
                .OrderBy(x => x.Address)
                .AsEnumerable()
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => x.ToList());

            var data =
                serviceTypeWork.Select(
                    x =>
                    new
                    {
                        typeWorkId = x.Id,
                        objectCrId = x.ObjectCr.Id,
                        workCode = x.Work.Code,
                        typeWork = x.Work.TypeWork,
                        volume = x.Volume,
                        percent = x.PercentOfCompletion,
                        sum = x.Sum,
                        finSource = x.FinanceSource.Code
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.objectCrId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        x.GroupBy(p => p.finSource)
                        .ToDictionary(
                            p => p.Key,
                            p =>
                            p.GroupBy(y => workGroups.ContainsKey(y.workCode) ? workGroups[y.workCode] : "-1")
                                .ToDictionary(
                                    y => y.Key,
                                    y => new TypeWorkProxy
                                            {
                                                volume = y.Sum(t => t.volume),
                                                costSum = y.Sum(t => t.sum) ?? 0M,
                                                percent = y.Sum(t => t.percent)
                                    })));
            
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var sectionTotal = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotal");
            sectionTotal.ДобавитьСтроку();
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var totalsDict = new Dictionary<string, Dictionary<string, TypeWorkProxy>>();
            var counter = 0;
            foreach (var municipality in municipalityList)
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["MO"] = municipality.Name;
                if (!crObjects.ContainsKey(municipality.Id))
                {
                    continue;
                }

                // словарь итогов по муню образованию
                var muTotalsDict = new Dictionary<string, Dictionary<string, TypeWorkProxy>>();

                var crObjectsData = crObjects[municipality.Id];

                foreach (var objectCr in crObjectsData)
                {
                    if (!data.ContainsKey(objectCr.objectCrId))
                    {
                        continue;
                    }

                    sectionRo.ДобавитьСтроку();
                    sectionRo["num"] = ++counter;
                    sectionRo["address"] = objectCr.Address;

                    var dataByObjectCr = data[objectCr.objectCrId];
                    this.FillSection(sectionRo, dataByObjectCr, string.Empty);
                    this.FillTotalDictionary(dataByObjectCr, muTotalsDict);
                }

                this.FillSection(sectionMu, muTotalsDict, "Mu");
                this.FillTotalDictionary(muTotalsDict, totalsDict);
            }

            this.FillSection(sectionTotal, totalsDict, "Total");
        }

        private void FillSection(Section section, Dictionary<string, Dictionary<string, TypeWorkProxy>> dataDictionary, string postfix)
        {
            // столбец 48
            section[string.Format("sum{0}", postfix)] = dataDictionary.Values.Sum(x => x.Values.Sum(y => y.costSum));
            
            foreach (var dataByFinSource in dataDictionary)
            {
                // заполнение столбцов 4-12 и 16-47
                foreach (var dataByWorkCode in dataByFinSource.Value)
                {
                    section[string.Format("sum{0}_{1}{2}", dataByWorkCode.Key, dataByFinSource.Key, postfix)] =
                        dataByWorkCode.Value.costSum;

                    section[string.Format("volume{0}_{1}{2}", dataByWorkCode.Key, dataByFinSource.Key, postfix)] =
                        dataByWorkCode.Value.volume;

                    //для столбцов 36-47
                    section[string.Format("percent{0}_{1}{2}", dataByWorkCode.Key, dataByFinSource.Key, postfix)] =
                        dataByWorkCode.Value.percent;
                }

                // заполнение столбцов 3 и 14
                section[string.Format("sum{0}{1}", dataByFinSource.Key, postfix)] =
                    dataByFinSource.Value.Values.Sum(x => x.costSum);
            }
        }

        private void FillTotalDictionary(Dictionary<string, Dictionary<string, TypeWorkProxy>> dataDictionary, Dictionary<string, Dictionary<string, TypeWorkProxy>> totalDataDictionary)
        {
            foreach (var dataByFinSource in dataDictionary)
            {
                if (!totalDataDictionary.ContainsKey(dataByFinSource.Key))
                {
                    totalDataDictionary[dataByFinSource.Key] = new Dictionary<string, TypeWorkProxy>();
                }

                foreach (var dataByWorkCode in dataByFinSource.Value)
                {
                    if (!totalDataDictionary[dataByFinSource.Key].ContainsKey(dataByWorkCode.Key))
                    {
                        totalDataDictionary[dataByFinSource.Key][dataByWorkCode.Key] = new TypeWorkProxy { costSum = 0, volume = 0, percent = 0};
                    }

                    totalDataDictionary[dataByFinSource.Key][dataByWorkCode.Key].costSum += dataByWorkCode.Value.costSum;
                    totalDataDictionary[dataByFinSource.Key][dataByWorkCode.Key].volume += dataByWorkCode.Value.volume;
                    totalDataDictionary[dataByFinSource.Key][dataByWorkCode.Key].percent += dataByWorkCode.Value.percent;
                }
            }
        }
    }

    internal sealed class TypeWorkProxy
    {
        public decimal? volume;

        public decimal costSum;

        public decimal? percent;
    }
}