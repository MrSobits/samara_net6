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
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Реестр договоров объекта КР ГЖИ
    /// </summary>
    public class ContractObjectCrRegister : BasePrintForm
    {
        private long programCrId;

        public ContractObjectCrRegister()
            : base(new ReportTemplateBinary(Properties.Resources.ContractObjectCrRegister))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ContractObjectCrRegister";
            }
        }

        public override string Name
        {
            get { return "Реестр договоров объекта КР ГЖИ"; }
        }

        public override string Desciption
        {
            get { return "Реестр договоров объекта КР ГЖИ"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ContractObjectCrRegister"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceManOrgContractRealityObject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var serviceContragentContact = Container.Resolve<IDomainService<ContragentContact>>();
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceContractCr = Container.Resolve<IDomainService<ContractCr>>();

            // столбцы 1, 2, 3, 4, 5, 6, 17, 22
            var infoObjectCrByMoQuery = serviceTypeWorkCr.GetAll()
                         .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                         .Where(x => !x.DateStartWork.HasValue || x.DateStartWork <= DateTime.Now.Date);

            var infoObjectCrByMoOrderQuery = infoObjectCrByMoQuery
                         .OrderBy(x => x.ObjectCr.RealityObject.Municipality.Name)
                         .ThenBy(x => x.ObjectCr.RealityObject.Address)
                         .Select(x => new
                         {
                             objectCrId = x.ObjectCr.Id,
                             numGji = x.ObjectCr.GjiNum ?? string.Empty,
                             dateRegGji = x.ObjectCr.DateGjiReg ?? DateTime.MinValue,
                             municipality = x.ObjectCr.RealityObject.Municipality.Name,
                             realityId = x.ObjectCr.RealityObject.Id,
                             addressReality = x.ObjectCr.RealityObject.Address,
                             sum = x.Sum ?? 0M,
                             typeWork = x.Work.TypeWork,
                             codeWork = x.Work.Code
                         });

            var infoObjectCrByMo = infoObjectCrByMoOrderQuery
                                    .AsEnumerable()
                                    .GroupBy(x => x.numGji)
                                    .ToDictionary(x => x.Key, 
                                                    x => x.Select(
                                                        y => new ContractInfoProxy
                                                        {
                                                            objectCrId = y.objectCrId,
                                                            numGji = y.numGji,
                                                            dateRegGji = y.dateRegGji,
                                                            municipality = y.municipality,
                                                            realityId = y.realityId,
                                                            addressReality = y.addressReality,
                                                            limitFinance = x.Sum(z => z.sum),
                                                            sumSmr = x.Where(z => z.typeWork == TypeWork.Work).Sum(z => z.sum),
                                                            sumDevExpPsd = x.Where(z => (z.codeWork == "1018" || z.codeWork == "1019")).Sum(z => z.sum),
                                                            sumTechPsd = x.Where(z => z.codeWork == "1020").Sum(z => z.sum)
                                                        })
                                                        .Distinct()
                                                        .ToList());

            var realityIdQuery = infoObjectCrByMoQuery.Select(x => x.ObjectCr.RealityObject.Id);
            var objectCrIdQuery = infoObjectCrByMoQuery.Select(x => x.ObjectCr.Id);

            // столбцы 7, 8
            var infoManagOrg = serviceManOrgContractRealityObject.GetAll()
                               .Where(x => realityIdQuery.Contains(x.RealityObject.Id)
                                   && (!x.ManOrgContract.StartDate.HasValue
                                       || x.ManOrgContract.StartDate <= DateTime.Now.Date)
                                   && (!x.ManOrgContract.EndDate.HasValue
                                       || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                                       && x.ManOrgContract.ManagingOrganization.Contragent != null)
                               .Select(x => new ManOrgProxy
                               {
                                   realityId = x.RealityObject.Id,
                                   managOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                                   managOrgType = x.ManOrgContract.ManagingOrganization.TypeManagement,
                                   managOrgAddress = x.ManOrgContract.ManagingOrganization.Contragent.JuridicalAddress,
                                   managOrgTel = x.ManOrgContract.ManagingOrganization.Contragent.Phone
                               })
                               .AsEnumerable()
                               .GroupBy(x => x.realityId)
                               .ToDictionary(x => x.Key, x => x.OrderBy(y => y.managOrgType).ToList());

            // столбцы 9, 10, 11, 12
            var infoContractCrPsdQuery = serviceContractCr.GetAll()
                         .Where(x => objectCrIdQuery.Contains(x.ObjectCr.Id)
                             && (!x.DateFrom.HasValue || x.DateFrom <= DateTime.Now.Date)
                             && x.TypeContractObject.Key == "Psd"
                             && x.Contragent != null)
                         .Select(x => new
                         {
                             numGji = x.ObjectCr.GjiNum,
                             objectCrId = x.ObjectCr.Id,
                             contrgentId = x.Contragent.Id,
                             realityId = x.ObjectCr.RealityObject.Id,
                             numDoc = x.DocumentNum,
                             dateDoc = x.DateFrom ?? DateTime.MinValue,
                             nameContragent = x.Contragent.Name,
                             address = x.Contragent.JuridicalAddress,
                             inn = x.Contragent.Inn,
                             sumCon = x.SumContract
                         });

            var infoContractCrPsdDict = infoContractCrPsdQuery
                                        .AsEnumerable()
                                        .GroupBy(x => x.numGji)
                                        .ToDictionary(x => x.Key, x => x.ToList());

            // столбец 11
            var contragentPsdIdQuery = infoContractCrPsdQuery.Select(x => x.contrgentId);

            var infoContractPsdByFio = serviceContragentContact.GetAll()
                         .Where(x => contragentPsdIdQuery.Contains(x.Contragent.Id)
                             && (!x.DateStartWork.HasValue || x.DateStartWork <= DateTime.Now.Date)
                             && (!x.DateEndWork.HasValue || x.DateEndWork >= DateTime.Now.Date)
                             && (x.Position.Code == "1" || x.Position.Code == "4")
                             && x.Contragent != null)
                         .Select(x => new { contragentId = x.Contragent.Id, fio = x.FullName, tel = x.Phone })
                         .AsEnumerable()
                         .GroupBy(x => x.contragentId)
                         .ToDictionary(x => x.Key);

            // столбцы 13, 14, 15, 16
            var infoContractCrExpDict = serviceContractCr.GetAll()
                         .Where(x => objectCrIdQuery.Contains(x.ObjectCr.Id)
                             && (!x.DateFrom.HasValue || x.DateFrom <= DateTime.Now.Date)
                             && x.TypeContractObject.Key == "Expertise"
                             && x.Contragent != null)
                         .Select(x => new
                         {
                             numGji = x.ObjectCr.GjiNum,
                             objectCrId = x.ObjectCr.Id,
                             contrgentId = x.Contragent.Id,
                             realityId = x.ObjectCr.RealityObject.Id,
                             numDoc = x.DocumentNum,
                             dateDoc = x.DateFrom ?? DateTime.MinValue,
                             sumCon = x.SumContract,
                             sumSmeta = x.ObjectCr.SumSmrApproved ?? 0M
                         })
                        .AsEnumerable()
                        .GroupBy(x => x.numGji)
                        .ToDictionary(x => x.Key, x => x.ToList());

            // столбцы 18, 19, 20, 21
            var infoContractCrTechQuery = serviceContractCr.GetAll()
                          .Where(x => objectCrIdQuery.Contains(x.ObjectCr.Id)
                             && (!x.DateFrom.HasValue || x.DateFrom <= DateTime.Now.Date)
                             && x.TypeContractObject.Key == "TechSepervision"
                             && x.Contragent != null)
                         .Select(x => new
                         {
                             numGji = x.ObjectCr.GjiNum,
                             objectCrId = x.ObjectCr.Id,
                             contrgentId = x.Contragent.Id,
                             realityId = x.ObjectCr.RealityObject.Id,
                             numDoc = x.DocumentNum,
                             dateDoc = x.DateFrom ?? DateTime.MinValue,
                             nameContragent = x.Contragent.Name,
                             address = x.Contragent.JuridicalAddress,
                             inn = x.Contragent.Inn,
                             sumCon = x.SumContract
                         });

            var infoContractCrTechDict = infoContractCrTechQuery
                                        .AsEnumerable()
                                        .GroupBy(x => x.numGji)
                                        .ToDictionary(x => x.Key, x => x.ToList());

            // столбец 20
            var contragentTechIdQuery = infoContractCrTechQuery.Select(x => x.contrgentId);

            var infoContractTechByFio = serviceContragentContact.GetAll()
                         .Where(x => contragentTechIdQuery.Contains(x.Contragent.Id)
                             && (!x.DateEndWork.HasValue || x.DateStartWork <= DateTime.Now.Date)
                             && (!x.DateEndWork.HasValue || x.DateEndWork >= DateTime.Now.Date)
                             && (x.Position.Code == "1" || x.Position.Code == "4")
                             && x.Contragent != null)
                         .Select(x => new { contragentId = x.Contragent.Id, fio = x.FullName, tel = x.Phone })
                         .AsEnumerable()
                         .GroupBy(x => x.contragentId)
                         .ToDictionary(x => x.Key);


            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var programCr = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrId);
            reportParams.SimpleReportParams["reportDate"] = programCr.Period.DateEnd.HasValue ? programCr.Period.DateEnd.Value.Year : DateTime.MinValue.Year;
            foreach (var contract in infoObjectCrByMo)
            {
                foreach (var item in contract.Value)
                {
                    section.ДобавитьСтроку();

                    section["NN"] = item.numGji;
                    if (item.dateRegGji != DateTime.MinValue)
                    {
                        section["date"] = item.dateRegGji;
                    }

                    section["Municipality"] = item.municipality;
                    section["Address"] = item.addressReality;
                    section["Limit"] = item.limitFinance;
                    section["SumSMR"] = item.sumSmr;
                    section["sumContractSmeta"] = item.limitFinance;
                    section["name"] = infoManagOrg.ContainsKey(item.realityId) ? infoManagOrg[item.realityId][0].managOrgName : string.Empty;
                    section["addTel"] = infoManagOrg.ContainsKey(item.realityId) ? (infoManagOrg[item.realityId][0].managOrgAddress + ", тел." + infoManagOrg[item.realityId][0].managOrgTel) : string.Empty;
                    section["sumContractTot"] = item.sumDevExpPsd;
                    section["sumContractConTot"] = item.sumTechPsd;


                    var list = new List<int>();

                    var ContractCrPsdDict = infoContractCrPsdDict.ContainsKey(item.numGji)
                                 ? infoContractCrPsdDict[item.numGji]
                                 : null;

                    var ContractCrExpDict = infoContractCrExpDict.ContainsKey(item.numGji)
                                 ? infoContractCrExpDict[item.numGji]
                                 : null;

                    var ContractCrTechDict = infoContractCrTechDict.ContainsKey(item.numGji)
                                 ? infoContractCrTechDict[item.numGji]
                                 : null;


                    if (infoContractCrPsdDict.ContainsKey(item.numGji))
                    {
                        list.Add(ContractCrPsdDict.Count);
                    }

                    if (infoContractCrExpDict.ContainsKey(item.numGji))
                    {
                        list.Add(ContractCrExpDict.Count);
                    }
                    if (infoContractCrTechDict.ContainsKey(item.numGji))
                    {
                        list.Add(ContractCrTechDict.Count);
                    }

                    var g = list.Count > 0 ? list.Max() : 0;

                    for (var i = 0; i < g; i++)
                    {
                        if (i > 0)
                        {
                            section.ДобавитьСтроку();
                            if (infoManagOrg.ContainsKey(item.realityId))
                                NewStr(section, item, infoManagOrg[item.realityId][0]);
                        }

                        if (ContractCrPsdDict != null && ContractCrPsdDict.Count > i)
                        {

                            var infoContractPsdFioPhone = string.Empty;

                            var contrId = ContractCrPsdDict[i].contrgentId;
                            if (infoContractPsdByFio.ContainsKey(contrId))
                            {
                                var tmpItem = infoContractPsdByFio[contrId].FirstOrDefault();

                                infoContractPsdFioPhone = tmpItem.fio + ", " + tmpItem.tel;
                            }

                            section["numContractDev"] = ContractCrPsdDict[i].numDoc;
                            if (ContractCrPsdDict[i].dateDoc != DateTime.MinValue)
                            {
                                section["dateContractDev"] = ContractCrPsdDict[i].dateDoc;
                            }

                            var tmpAlldata = new List<string>()
                                                                {
                                                                    ContractCrPsdDict[i].nameContragent, 
                                                                    ContractCrPsdDict[i].address, 
                                                                    string.Format("тел. {0}", infoContractPsdFioPhone), 
                                                                    string.Format("ИНН: {0}", ContractCrPsdDict[i].inn)
                                                                };
                            var alldata = tmpAlldata.Where(x => !x.IsEmpty()).Aggregate((a, b) => a + ", " + b);

                            section["alldata"] = alldata;

                            section["sumContractDev"] = ContractCrPsdDict[i].sumCon;
                        }
                        else
                        {
                            section["numContractDev"] = string.Empty;
                            section["dateContractDev"] = string.Empty;
                            section["alldata"] = string.Empty;
                            section["sumContractDev"] = string.Empty;
                        }

                        if (ContractCrExpDict != null && ContractCrExpDict.Count > i)
                        {
                            section["numContractExp"] = ContractCrExpDict[i].numDoc;
                            section["dateContractExp"] = ContractCrExpDict[i].dateDoc;
                            section["sumContractExp"] = ContractCrExpDict[i].sumCon;

                        }
                        else
                        {
                            section["numContractExp"] = string.Empty;
                            section["dateContractExp"] = string.Empty;
                            section["sumContractExp"] = string.Empty;
                        }

                        if (ContractCrTechDict != null && ContractCrTechDict.Count > i)
                        {
                            var infoContractTechFioPhone = string.Empty;

                            var contrId = ContractCrTechDict[i].contrgentId;
                            if (infoContractTechByFio.ContainsKey(contrId))
                            {
                                var tmpItem = infoContractTechByFio[contrId].FirstOrDefault();

                                infoContractTechFioPhone = tmpItem.fio + ", " + tmpItem.tel;
                            }

                            section["numContractControl"] = ContractCrTechDict[i].numDoc;
                            if (ContractCrTechDict[i].dateDoc != DateTime.MinValue)
                            {
                                section["dateContractControl"] = ContractCrTechDict[i].dateDoc;
                            }

                            var tmpAlldataControl = new List<string>()
                                                        {
                                                            ContractCrTechDict[i].nameContragent,
                                                            ContractCrTechDict[i].address,
                                                            string.Format("тел. {0}", infoContractTechFioPhone), 
                                                            string.Format("ИНН: {0}", ContractCrTechDict[i].inn)
                                                        };
                            var alldataControl = tmpAlldataControl.Where(x => !x.IsEmpty()).Aggregate((a, b) => a + ", " + b);

                            section["alldataControl"] = alldataControl;

                            section["sumContractControl"] = ContractCrTechDict[i].sumCon;
                        }
                        else
                        {
                            section["numContractControl"] = string.Empty;
                            section["dateContractControl"] = string.Empty;
                            section["alldataControl"] = string.Empty;
                            section["sumContractControl"] = string.Empty;
                        }
                    }
                }
            }
        }

        private void NewStr(Section section, ContractInfoProxy contract, ManOrgProxy manOrg)
        {
            section["NN"] = contract.numGji;
            if (contract.dateRegGji != DateTime.MinValue)
            {
                section["date"] = contract.dateRegGji;
            }

            section["Municipality"] = contract.municipality;
            section["Address"] = contract.addressReality;
            section["Limit"] = contract.limitFinance;
            section["SumSMR"] = contract.sumSmr;
            section["name"] = manOrg.managOrgName;
            section["addTel"] = manOrg.managOrgAddress + ", " + manOrg.managOrgTel;
            section["sumContractTot"] = contract.sumDevExpPsd;
            section["sumContractConTot"] = contract.sumTechPsd;
        }

        private class ManOrgProxy
        {
            public long realityId;

            public string managOrgName;

            public TypeManagementManOrg managOrgType;

            public string managOrgAddress;

            public string managOrgTel;
        }

        private struct ContractInfoProxy
        {
            public long objectCrId;

            public string numGji;

            public DateTime dateRegGji;

            public string municipality;

            public long realityId;

            public string addressReality;

            public decimal limitFinance;

            public decimal sumSmr;

            public decimal sumDevExpPsd;

            public decimal sumTechPsd;
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
