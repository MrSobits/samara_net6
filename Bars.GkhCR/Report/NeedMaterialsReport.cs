namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;

    public class NeedMaterialsReport : BasePrintForm
    {

        public IWindsorContainer Container { get; set; }

        #region Входные параметры
        private long programCrId;
        private DateTime dateReport;
        private List<long> municipalityIds;
        private List<long> finSourceIds;
        private List<long> worksIds;
        #endregion

        //Отчет Потребность в материалах
        public NeedMaterialsReport(): base(new ReportTemplateBinary(Properties.Resources.NeedMaterials))
        {
        }

        public override string Name
        {
            get { return "Потребность в материалах"; }
        }

        public override string Desciption
        {
            get { return "Потребность в материалах"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.NeedMaterials"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.NeedMaterialsReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToLong();

            this.dateReport = baseParams.Params["reportDate"].ToDateTime();            

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            this.municipalityIds = !string.IsNullOrEmpty(municipalityStr)
                                  ? municipalityStr.Split(',').Select(x => x.ToLong()).ToList()
                                  : new List<long>();            

            var finSourcesStr = baseParams.Params["finSources"].ToString();
            this.finSourceIds = !string.IsNullOrEmpty(finSourcesStr)
                               ? finSourcesStr.Split(',').Select(x => x.ToLong()).ToList()
                               : new List<long>();

            var worksStr = baseParams.Params["workCr"].ToString();
            this.worksIds = !string.IsNullOrEmpty(worksStr)
                               ? worksStr.Split(',').Select(x => x.ToLong()).ToList()
                               : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var servicePerformedWorkAct = this.Container.Resolve<IDomainService<PerformedWorkAct>>();
            var serviceTypeWorkCr = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceWork = this.Container.Resolve<IDomainService<Work>>();
            var serviceFinanceSource = this.Container.Resolve<IDomainService<FinanceSource>>();
            var serviceResourceStatement = this.Container.Resolve<IDomainService<ResourceStatement>>();

            var worksDict = serviceWork.GetAll()
                           .WhereIf(this.worksIds.Count > 0, x => this.worksIds.Contains(x.Id))
                           .Select(x => new { x.Id, x.Name })
                           .ToDictionary(x => x.Id, x => x.Name);

            var finSourceDict = serviceFinanceSource.GetAll()
                           .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.Id))
                           .Select(x => new { x.Id, x.Name })
                           .ToDictionary(x => x.Id, x => x.Name);

            var dictMuName = serviceMunicipality.GetAll()
                                   .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                                   .Select(x => new { x.Id, x.Name })
                                   .OrderBy(x => x.Name)
                                   .ToDictionary(x => x.Id, v => v.Name);

            var queryObjCr = serviceObjectCr.GetAll()
                               .Where(x => x.ProgramCr.Id == this.programCrId)
                               .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var queryObjCrId = queryObjCr.Select(x => x.Id).Distinct();

            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);

            var dictObjPerformedWorkAct =
                servicePerformedWorkAct.GetAll()
                                       .Where(x => x.DateFrom <= this.dateReport)
                                       .Where(x => queryObjCrId.Contains(x.ObjectCr.Id))
                                       .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                                       .WhereIf(this.worksIds.Count > 0, x => this.worksIds.Contains(x.TypeWorkCr.Work.Id))
                                       .Where(x => x.State.Name == "Принято ГЖИ" || x.State.Name == "Принят ТОДК")
                                       .Select(x => new 
                                       {
                                           objCrId = x.ObjectCr.Id,
                                           finSourceId = x.TypeWorkCr.FinanceSource.Id,
                                           workId = x.TypeWorkCr.Work.Id,
                                           x.Volume, 
                                           x.Sum
                                       })
                                       .AsEnumerable()
                                       .GroupBy(x => x.objCrId)
                                       .ToDictionary(
                                       x => x.Key, 
                                       x => x.GroupBy(y => y.finSourceId)
                                             .ToDictionary(
                                             y => y.Key, 
                                             y => y.GroupBy(z => z.workId)
                                                   .ToDictionary(
                                                   z => z.Key, 
                                                   z => new
                                                        {
                                                            count = z.Count(),
                                                            sum = z.Sum(p => p.Sum)
                                                        })));

            var dictObjResourceStatementSum = serviceResourceStatement.GetAll()
                .Where(x => queryObjCrId.Contains(x.EstimateCalculation.ObjectCr.Id))
                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.EstimateCalculation.TypeWorkCr.FinanceSource.Id))
                .WhereIf(this.worksIds.Count > 0, x => this.worksIds.Contains(x.EstimateCalculation.TypeWorkCr.Work.Id))
                .Select(x => new
                {
                    objCrId = x.EstimateCalculation.ObjectCr.Id,
                    finSourceId = x.EstimateCalculation.TypeWorkCr.FinanceSource.Id,
                    workId = x.EstimateCalculation.TypeWorkCr.Work.Id,
                    x.TotalCost
                })
                .AsEnumerable()
                .GroupBy(x => x.objCrId)
                .ToDictionary(
                x => x.Key,
                x => x.GroupBy(y => y.finSourceId)
                        .ToDictionary(
                        y => y.Key,
                        y => y.GroupBy(z => z.workId)
                            .ToDictionary(
                            z => z.Key,
                            z => z.Sum(p => p.TotalCost))));

            var dictObjTypeWorkCr = 
               serviceTypeWorkCr.GetAll()
                                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                .WhereIf(this.worksIds.Count > 0, x => this.worksIds.Contains(x.Work.Id))
                                .Where(x => queryObjCrId.Contains(x.ObjectCr.Id))
                                .Select(x => new 
                                {
                                    objCrId = x.ObjectCr.Id,
                                    x.ObjectCr.RealityObject.Address,
                                    finSourceId = x.FinanceSource.Id,
                                    workId = x.Work.Id,
                                    x.Volume
                                })
                                .AsEnumerable()
                                .GroupBy(x => x.objCrId)
                                .ToDictionary(
                                x => x.Key,
                                x => x.GroupBy(y => y.finSourceId)
                                        .ToDictionary(
                                        y => y.Key,
                                        y => y.GroupBy(z => z.workId)
                                            .ToDictionary(
                                            z => z.Key, 
                                            z => z.Sum(p => p.Volume))));

            var repManOrgContractRealityObject = 
                this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= this.dateReport)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= this.dateReport);

            var realObjIds = queryObjCr.Select(x => x.RealityObject.Id).Distinct();

            var manOrgContractIdsQuery = repManOrgContractRealityObject
                .Where(x => realObjIds.Contains(x.RealityObject.Id))
                .Select(x => x.ManOrgContract.Id);
            
            var contractRelationDict =
               this.Container.Resolve<IDomainService<ManOrgContractRelation>>()
                   .GetAll()
                   .Where(x => manOrgContractIdsQuery.Contains(x.Parent.Id))
                   .Where(x => x.Children.StartDate <= this.dateReport)
                   .Where(x => x.Children.EndDate == null || x.Children.EndDate >= this.dateReport)
                   .Select(x => new
                   {
                       x.Parent.Id,
                       ManagingOrganizationId = x.Children.ManagingOrganization.Id,
                       ManagingOrganizationName = x.Children.ManagingOrganization.Contragent.Name
                   })
                   .AsEnumerable()
                   .GroupBy(x => x.Id)
                   .ToDictionary(
                           x => x.Key,
                           x => new ManOrgContractProxy
                           {
                               ManageOrgId = x.First().ManagingOrganizationId,
                               Name = x.First().ManagingOrganizationName
                           });

            // УО дома
            var realityObjectManOrgDict = repManOrgContractRealityObject
                .Where(x => realObjIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    manOrgContractId = x.Id,
                    x.ManOrgContract.TypeContractManOrgRealObj,
                    moId = (long?)x.ManOrgContract.ManagingOrganization.Id,
                    moName = x.ManOrgContract.ManagingOrganization.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                        x => x.Key,
                        y =>
                        {
                            var moContractList = y.Select(x => new ManOrgContractProxy
                            {
                                ManageOrgId = x.moId,
                                TypeContractManOrgRealObj = x.TypeContractManOrgRealObj,
                                Name = x.moName
                            }).ToList();
                            return this.GetManOrg(moContractList, contractRelationDict);
                        });
            
            var dictMuObjCr = queryObjCr
                .Select(x => new { ObjCrId = x.Id, MuId = x.RealityObject.Municipality.Id, address = x.RealityObject.Address, roId = x.RealityObject.Id })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key, x => x.ToList());
            
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionTotal = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotal");
            var sectionHouse = sectionMu.ДобавитьСекцию("sectionHouse");
            var sectionData = sectionHouse.ДобавитьСекцию("sectionData");

            reportParams.SimpleReportParams["Program"] = programCr.Name;
            reportParams.SimpleReportParams["Date"] = this.dateReport.ToShortDateString();

            var num = 0;
            var totalDictionary = this.GetTotalDictionary();

            foreach (var municipality in dictMuName)
            {
                sectionMu.ДобавитьСтроку();

                sectionMu["MunicipalityName"] = municipality.Value;

                if (!dictMuObjCr.ContainsKey(municipality.Key))
                {
                    continue;
                }

                var municipalityTotalDictionary = this.GetTotalDictionary();

                foreach (var objCr in dictMuObjCr[municipality.Key].OrderBy(x => x.address))
                {
                    if (dictObjTypeWorkCr.ContainsKey(objCr.ObjCrId))
                    {
                        sectionHouse.ДобавитьСтроку();
                        ++num;
                        var realtyObjTotalDictionary = this.GetTotalDictionary();
                        
                        var dataByObjCr = dictObjTypeWorkCr[objCr.ObjCrId];
                        sectionHouse["Address"] = objCr.address;
                        sectionHouse["Customer"] = realityObjectManOrgDict.ContainsKey(objCr.roId) ? realityObjectManOrgDict[objCr.roId].Name : string.Empty;
                        foreach (var dataByFinSource in dataByObjCr)
                        {
                            foreach (var dataByWork in dataByFinSource.Value)
                            {
                                sectionData.ДобавитьСтроку();
                                sectionData["Number"] = num;
                                sectionData["MunicipalityName"] = municipality.Value;
                                sectionData["Address"] = objCr.address;
                                sectionData["Customer"] = realityObjectManOrgDict.ContainsKey(objCr.roId) ? realityObjectManOrgDict[objCr.roId].Name : string.Empty;
                                sectionData["TypeWork"] = worksDict[dataByWork.Key];
                                sectionData["FinSource"] = finSourceDict[dataByFinSource.Key];

                                if (dataByWork.Value.HasValue && dataByWork.Value != 0)
                                {
                                    sectionData["CountOnEstimate"] = dataByWork.Value;
                                }

                                if (dictObjResourceStatementSum.ContainsKey(objCr.ObjCrId)
                                    && dictObjResourceStatementSum[objCr.ObjCrId].ContainsKey(dataByFinSource.Key)
                                    && dictObjResourceStatementSum[objCr.ObjCrId][dataByFinSource.Key].ContainsKey(dataByWork.Key))
                                {
                                    sectionData["SumOnEstimate"] = dictObjResourceStatementSum[objCr.ObjCrId][dataByFinSource.Key][dataByWork.Key];
                                }
                                
                                if (dictObjPerformedWorkAct.ContainsKey(objCr.ObjCrId) 
                                    && dictObjPerformedWorkAct[objCr.ObjCrId].ContainsKey(dataByFinSource.Key) 
                                    && dictObjPerformedWorkAct[objCr.ObjCrId][dataByFinSource.Key].ContainsKey(dataByWork.Key))
                                {
                                    var dataByPerformedWorkAct = dictObjPerformedWorkAct[objCr.ObjCrId][dataByFinSource.Key][dataByWork.Key];
                                    sectionData["CountOnAct"] = dataByPerformedWorkAct.count;
                                    sectionData["SumOnAct"] = dataByPerformedWorkAct.sum;
                                }
                            }
                        }

                        realtyObjTotalDictionary[3] = dataByObjCr.Select(x => x.Value.Sum(y => y.Value)).Sum().ToDecimal();

                        if (dictObjResourceStatementSum.ContainsKey(objCr.ObjCrId))
                        {
                            realtyObjTotalDictionary[4] = dictObjResourceStatementSum[objCr.ObjCrId].SelectMany(x => x.Value).Sum(x => x.Value).ToDecimal();
                        }

                        if (dictObjPerformedWorkAct.ContainsKey(objCr.ObjCrId))
                        {
                            realtyObjTotalDictionary[2] = dictObjPerformedWorkAct[objCr.ObjCrId].Select(x => x.Value.Sum(y => y.Value.sum)).Sum().ToDecimal();
                            realtyObjTotalDictionary[1] = dictObjPerformedWorkAct[objCr.ObjCrId].Select(x => x.Value.Sum(y => y.Value.count)).Sum().ToDecimal();
                        }

                        // заполнение итогов по дому
                        this.FillTotalSection(sectionHouse, realtyObjTotalDictionary, "House");
                        this.AttachDictionaries(municipalityTotalDictionary, realtyObjTotalDictionary);
                    }
                }

                // заполнение итогов по мун.образованию
                this.FillTotalSection(sectionMu, municipalityTotalDictionary, "Mu");
                this.AttachDictionaries(totalDictionary, municipalityTotalDictionary);
            }

            // заполнение итогов
            sectionTotal.ДобавитьСтроку();
            this.FillTotalSection(sectionTotal, totalDictionary, string.Empty);
        }

        private void FillTotalSection(Section section, Dictionary<int, decimal> totalDict, string type)
        {
            section[string.Format("Total{0}1", type)] = totalDict[1];
            section[string.Format("Total{0}2", type)] = totalDict[2];
            section[string.Format("Total{0}3", type)] = totalDict[3];
            section[string.Format("Total{0}4", type)] = totalDict[4];
        }

        private Dictionary<int, decimal> GetTotalDictionary()
        {
            var dict = new Dictionary<int, decimal>();

            for (int i = 1; i <= 4; i++)
            {
                dict[i] = 0M;
            }

            return dict;
        }

        private void AttachDictionaries(Dictionary<int, decimal> totals, Dictionary<int, decimal> addable)
        {
            foreach (var key in addable.Keys)
            {
                if (totals.ContainsKey(key))
                {
                    totals[key] += addable[key];
                }
                else
                {
                    totals[key] = addable[key];
                }
            }
        }

        private ManOrgContractProxy GetManOrg(List<ManOrgContractProxy> manOrgContractList, Dictionary<long, ManOrgContractProxy> contractRelationDict)
        {
            if (manOrgContractList.Count == 1)
            {
                return manOrgContractList.First();
            }

            var managingOrgOwners = manOrgContractList.FirstOrDefault(x => x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners);

            if (managingOrgOwners != null)
            {
                return managingOrgOwners;
            }

            var jskTsj = manOrgContractList.FirstOrDefault(x => x.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj);

            if (jskTsj != null)
            {
                var contractRelationList = contractRelationDict.Keys.Intersect(
                        manOrgContractList.Select(x => x.ManageOrgId.HasValue ? x.ManageOrgId.Value : 0)).ToList();

                var res = jskTsj;

                if (contractRelationList.Any())
                {
                    res.Name = string.Format("{0} ({1})", res.Name, contractRelationDict[contractRelationList.First()].Name);
                }

                return res;
            }

            return manOrgContractList.First();
        }

        private sealed class ManOrgContractProxy
        {
            public long? ManageOrgId { get; set; }

            public TypeContractManOrg TypeContractManOrgRealObj { get; set; }

            public string Name { get; set; }
        }
    }


}