namespace Bars.GkhRf.Report
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

    internal class CorrectionOfLimits : BasePrintForm
    {
        #region Свойства

        private long programCrOneId;

        private long programCrTwoId;

        private long[] municipalityIds;

        private DateTime reportDate;

        private Dictionary<int, decimal> totals = new Dictionary<int, decimal>() 
        { { 4, 0M }, { 5, 0M }, { 6, 0M }, { 9, 0M }, { 10, 0M }, { 11, 0M }, { 13, 0M }, { 14, 0M }, { 15, 0M }, { 16, 0M } };

        public IWindsorContainer Container { get; set; }

        public CorrectionOfLimits()
            : base(new ReportTemplateBinary(Properties.Resources.CorrectionOfLimits))
        {
        }

        public override string Name
        {
            get
            {
                return "Корректировка лимитов";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Корректировка лимитов";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CorrectionOfLimits";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.CorrectionOfLimits";
            }
        }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrOneId = baseParams.Params["programCrId1"].ToLong();
            programCrTwoId = baseParams.Params["programCrId2"].ToLong();

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            reportDate = baseParams.Params["reportDate"].ToDateTime();

            var dateStartPrOne = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrOneId).Period.DateStart;
            var dateEndPrOne = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrOneId).Period.DateEnd;

            var dateStartPrTwo = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrTwoId).Period.DateStart;
            var dateEndPrTwo = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrTwoId).Period.DateEnd;

            if (dateStartPrOne != dateStartPrTwo || dateEndPrOne != dateEndPrTwo)
            {
                throw new Exception("Ошибка формирования отчета. Программы должны быть с одинаковым периодом! ");
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceFinanceSourceResource = Container.Resolve<IDomainService<FinanceSourceResource>>();
            var serviceBasePaymentOrder = Container.Resolve<IDomainService<BasePaymentOrder>>();
            var serviceManOrgContractRealityObject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
            var serviceObjectCr = Container.Resolve<IDomainService<ObjectCr>>();

            var municipalityDict = serviceMunicipality.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => new { muId = x.Id, muName = x.Name })
                .ToDictionary(x => x.muId, x => x.muName);

            var realObjByManOrgQuery = serviceManOrgContractRealityObject.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate <= reportDate)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= reportDate)
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj 
                    || x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                .Where(x => x.ManOrgContract.ManagingOrganization != null);

            var realtyObjectIdsQuery = realObjByManOrgQuery.Select(x => x.RealityObject.Id);

            var typeWorkCrQuery = serviceTypeWorkCr.GetAll()
                .Where(x => realtyObjectIdsQuery.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrOneId || x.ObjectCr.ProgramCr.Id == programCrTwoId);

            var objCrQuery = typeWorkCrQuery
                .OrderBy(x => x.ObjectCr.RealityObject.Municipality.Name)
                .ThenBy(x => x.ObjectCr.RealityObject.Address)
                .Select(x => new
                {
                    muId = x.ObjectCr.RealityObject.Municipality.Id,
                    roId = x.ObjectCr.RealityObject.Id,
                    x.ObjectCr.RealityObject.Address,
                    progId = x.ObjectCr.ProgramCr.Id,
                    x.Work.TypeWork,
                    Sum = x.Sum ?? 0M
                });

            var realObjQuery = typeWorkCrQuery.Select(x => x.ObjectCr.RealityObject.Id);

            // столбцы 2, 6, 11
            var objCrDict = objCrQuery
                .AsEnumerable()
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.roId)
                        .ToDictionary(
                            y => y.Key, 
                            y => new
                            {
                                address = y.Select(z => z.Address).FirstOrDefault(),
                                sumWorksPr1 = y.Where(z => z.TypeWork == TypeWork.Work && z.progId == programCrOneId).Sum(z => z.Sum),
                                sumWorksPr2 = y.Where(z => z.TypeWork == TypeWork.Work && z.progId == programCrTwoId).Sum(z => z.Sum)
                            }));

            // столбец 3
            var manOrgByRealObj = realObjByManOrgQuery
                .Where(x => serviceObjectCr.GetAll()
                        .Where(y => y.ProgramCr.Id == programCrOneId || y.ProgramCr.Id == programCrTwoId)
                        .Any(y => y.RealityObject.Id == x.RealityObject.Id))
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    startDate = x.ManOrgContract.StartDate ?? DateTime.MinValue
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.OrderByDescending(y => y.startDate)
                          .Select(y => y.Name)
                          .FirstOrDefault());

            // столбцы 4, 5, 9, 10
            var sumsFromFinSourceRes = serviceFinanceSourceResource.GetAll()
                .Where(x => realObjQuery.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrOneId || x.ObjectCr.ProgramCr.Id == programCrTwoId)
                .Select(x => new
                {
                    muId = x.ObjectCr.RealityObject.Municipality.Id,
                    roId = x.ObjectCr.RealityObject.Id,
                    progId = x.ObjectCr.ProgramCr.Id,
                    BudgetSubject = x.BudgetSubject ?? 0M,
                    BudgetMu = x.BudgetMu ?? 0M,
                    FundResource = x.FundResource ?? 0M,
                    OwnerResource = x.OwnerResource ?? 0M
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key, 
                    x => new 
                    {
                        totSumPr1 = x.Where(z => z.progId == programCrOneId).Sum(z => z.BudgetMu + z.BudgetSubject + z.FundResource + z.OwnerResource),
                        sumOwnPr1 = x.Where(z => z.progId == programCrOneId).Sum(z => z.OwnerResource),

                        totSumPr2 = x.Where(z => z.progId == programCrTwoId).Sum(z => z.BudgetMu + z.BudgetSubject + z.FundResource + z.OwnerResource),
                        sumOwnPr2 = x.Where(z => z.progId == programCrTwoId).Sum(z => z.OwnerResource)
                    });

            var periodPrOne = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrOneId).Period.Id;
            var startYear = new DateTime(reportDate.Year, 1, 1);

            // столбец 12
            var paymentsGisuToManOrg = serviceBasePaymentOrder.GetAll()
                .Where(x => realObjQuery.Contains(x.BankStatement.ObjectCr.RealityObject.Id))
                .Where(x => x.BankStatement.Period.Id == periodPrOne)
                .Where(x => x.BidDate.HasValue && x.BidDate >= startYear && x.BidDate <= reportDate)
                .Where(x => x.TypePaymentOrder == TypePaymentOrder.In)
                .GroupBy(x => x.BankStatement.ObjectCr.RealityObject.Id)
                .Select(x => new
                {
                    roId = x.Key,
                    sum = x.Sum(y => y.Sum)
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x.Sum(z => z.sum ?? 0M));

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var section = sectionMu.ДобавитьСекцию("section");
            var sectionTot = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTot");

            var nameProgOne = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrOneId).Name;
            var nameProgTwo = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrTwoId).Name;
            reportParams.SimpleReportParams["program1"] = nameProgOne;
            reportParams.SimpleReportParams["program2"] = nameProgTwo;
            reportParams.SimpleReportParams["reportDate"] = reportDate.ToShortDateString();
            var counter = 0;

            foreach (var mun in objCrDict)
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["MuName"] = municipalityDict[mun.Key];
                var totalsMu = new List<int> { 4, 5, 6, 9, 10, 11, 13, 14, 15, 16 }.ToDictionary(x => x, x => 0m);

                foreach (var realObj in mun.Value)
                {
                    section.ДобавитьСтроку();
                    section["column1"] = ++counter;
                    section["column2"] = realObj.Value.address;

                    section["column6"] = realObj.Value.sumWorksPr1;
                    totalsMu[6] += realObj.Value.sumWorksPr1;

                    var column11 = realObj.Value.sumWorksPr2;
                    section["column11"] = column11;
                    totalsMu[11] += column11;

                    if (manOrgByRealObj.ContainsKey(realObj.Key))
                    {
                        section["column3"] = manOrgByRealObj[realObj.Key];
                    }

                    var column4 = 0M;
                    var column5 = 0M;
                    var column9 = 0M;
                    var column10 = 0M;
                    if (sumsFromFinSourceRes.ContainsKey(realObj.Key))
                    {
                        var columns = sumsFromFinSourceRes[realObj.Key];
                        column4 = columns.totSumPr1;
                        section["column4"] = column4;
                        totalsMu[4] += column4;

                        column5 = columns.sumOwnPr1;
                        section["column5"] = column5;
                        totalsMu[5] += column5;

                        column9 = columns.totSumPr2;
                        section["column9"] = column9;
                        totalsMu[9] += column9;

                        column10 = columns.sumOwnPr2;
                        section["column10"] = column10;
                        totalsMu[10] += column10;
                    }

                    var column13 = column4 - column9;
                    section["column13"] = column13;
                    totalsMu[13] += column13;

                    var column14 = column5 - column10;
                    section["column14"] = column14;
                    totalsMu[14] += column14;


                    var column12 = 0M;
                    if (paymentsGisuToManOrg.ContainsKey(realObj.Key))
                    {
                        column12 = paymentsGisuToManOrg[realObj.Key];
                        section["column12"] = column12;
                    }

                    var column15 = realObj.Value.sumWorksPr1 - column11;
                    section["column15"] = column15;
                    totalsMu[15] += column15;

                    var column16 = column10 - column12;
                    section["column16"] = column16;
                    totalsMu[16] += column16;
                }

                FillSection(sectionMu, totalsMu, "columnMu");
                AttachDictionaries(totals, totalsMu);
            }

            sectionTot.ДобавитьСтроку();
            FillSection(sectionTot, totals, "columnTot");

        }

        private void FillSection(Section section, Dictionary<int, decimal> data, string postfix)
        {
            foreach (var item in data)
            {
                if (item.Value != 0M)
                {
                    section[string.Format("{0}{1}", postfix, item.Key.ToStr())] = item.Value;
                }
            }
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

    }
}