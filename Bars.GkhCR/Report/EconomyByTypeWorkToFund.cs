namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Report.ComparePrograms;
    using Castle.Windsor;

    class EconomyByTypeWorkToFund : BasePrintForm
    {
        #region Свойства
        private long programCrOneId;
        private long programCrTwoId;
        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public EconomyByTypeWorkToFund()
            : base(new ReportTemplateBinary(Properties.Resources.EconomyByTypeWorkToFund))
        {
        }

        public override string Name
        {
            get { return "Экономия по видам работ (внутри дома) для Фонда"; }
        }

        public override string Desciption
        {
            get { return "Экономия по видам работ (внутри дома) для Фонда"; }
        }

        public override string GroupName
        {
            get { return "Формы программы"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.EconomyByTypeWorkToFund"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.EconomyByTypeWorkToFund"; }
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrOneId = baseParams.Params["programCrId1"].ToInt();
            programCrTwoId = baseParams.Params["programCrId2"].ToInt();

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceFinanceSourceResource = Container.Resolve<IDomainService<FinanceSourceResource>>();
            var serviceFinanceSource = Container.Resolve<IDomainService<FinanceSource>>();
            var serviceBasePaymentOrder = Container.Resolve<IDomainService<BasePaymentOrder>>();

            var municipalityDict = serviceMunicipality.GetAll()
                       .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                       .OrderBy(x => x.Name)
                       .Select(x => new
                       {
                           muId = x.Id,
                           muName = x.Name
                       })
                       .ToDictionary(x => x.muId, x => x.muName);

            var municipalityGroupDict = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name, x.Group })
                .OrderBy(x => x.Name)
                .AsEnumerable()
                .GroupBy(x => x.Group ?? string.Empty)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Id, y => y.Name));

            var periodPr2 = Container.Resolve<IDomainService<ProgramCr>>().Load(programCrTwoId).Period.Id;

            // Финансирование по 185-ФЗ
            var financeSourceId = serviceFinanceSource.GetAll().Where(x => x.Code == "1").Select(x => x.Id).FirstOrDefault();

            var realObjIdByPrQuery = serviceTypeWorkCr.GetAll()
                   .Where(x => x.ObjectCr.ProgramCr.Id == programCrOneId)
                   .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                   .Where(x => x.FinanceSource.Id == financeSourceId)
                   .Where(x => serviceTypeWorkCr.GetAll()
                                      .Any(y => y.ObjectCr.ProgramCr.Id == programCrTwoId
                                          && y.ObjectCr.RealityObject.Id == x.ObjectCr.RealityObject.Id
                                         && y.FinanceSource.Id == financeSourceId))
                   .Select(x => x.ObjectCr.RealityObject.Id);

            var infoProgramCr = serviceTypeWorkCr.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => realObjIdByPrQuery.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrOneId || x.ObjectCr.ProgramCr.Id == programCrTwoId)
                .Where(x => x.FinanceSource.Id == financeSourceId)
                .OrderBy(x => x.ObjectCr.RealityObject.Municipality.Name)
                .ThenBy(x => x.ObjectCr.RealityObject.Address)
                .Select(x => new
                {
                    munId = x.ObjectCr.RealityObject.Municipality.Id,
                    muName = x.ObjectCr.RealityObject.Municipality.Name,
                    prCrId = x.ObjectCr.ProgramCr.Id,
                    realObjId = x.ObjectCr.RealityObject.Id,
                    x.ObjectCr.RealityObject.Address,
                    x.Work.TypeWork,
                    x.Work.Code,
                    sumWork = x.Sum ?? 0M
                })
                                 .AsEnumerable()
                                 .GroupBy(x => x.munId)
                                 .ToDictionary(x => x.Key, x => x
                                     .GroupBy(y => y.realObjId)
                                     .ToDictionary(y => y.Key, y =>
                                     {
                                         var sumOnlyWorkByPr1 = y.Where(t => t.prCrId == programCrOneId && t.TypeWork == TypeWork.Work)
                                             .GroupBy(t => t.Code)
                                             .ToDictionary(t => t.Key, 
                                                            t => t.Sum(z => z.sumWork));

                                         var sumOnlyWorkByPr2 = y.Where(t => t.prCrId == programCrTwoId && t.TypeWork == TypeWork.Work)
                                             .GroupBy(t => t.Code)
                                             .ToDictionary(t => t.Key, 
                                                            t => t.Sum(z => z.sumWork));

                                         var sumOnlyWorksByPr1 = sumOnlyWorkByPr1.Sum(t => t.Value);
                                         var sumOnlyWorksByPr2 = sumOnlyWorkByPr2.Sum(t => t.Value);


                                         var sumOnlyServByPr1 = y.Where(t => t.prCrId == programCrOneId && t.TypeWork == TypeWork.Service).Sum(t => t.sumWork);
                                         var sumOnlyServByPr2 = y.Where(t => t.prCrId == programCrTwoId && t.TypeWork == TypeWork.Service).Sum(t => t.sumWork);

                                         // распределение суммы услуг по работам
                                         GetDictByDistributedSums(sumOnlyWorkByPr1, sumOnlyServByPr1, sumOnlyWorksByPr1);
                                         GetDictByDistributedSums(sumOnlyWorkByPr2, sumOnlyServByPr2, sumOnlyWorksByPr2);

                                         var address = y.Select(z => z.Address).FirstOrDefault();

                                         return GetDataForRealObjBySumEconomy(sumOnlyWorkByPr1, sumOnlyWorkByPr2, sumOnlyWorksByPr1, sumOnlyWorksByPr2, address);
                                     }));

            var resDict = infoProgramCr
                .ToDictionary(
                    x => x.Key,
                    x => x.Value
                            .Where(y => y.Value.ExistRealObjInBothProgram)
                            .ToDictionary(y => y.Key, y => new
                            {
                                y.Value.Address,
                                y.Value.MessageOfResultCompare,
                                y.Value.SumDiffByWorks,
                                y.Value.SumOnlyWorksByPr1
                            }));

            var realObjIdList = resDict.SelectMany(x => x.Value.Select(z => z.Key)).Distinct().ToList();

            // столбец 3, 5 
            var start = 1000;
            var tmpRealObjId = realObjIdList.Count > start ? realObjIdList.Take(1000).ToArray() : realObjIdList.ToArray();

            var sumToAndSumEconomyList = serviceFinanceSourceResource.GetAll()
                .Where(x => tmpRealObjId.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.FinanceSource.Id == financeSourceId)
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrOneId)
                .Select(x => new
                {
                    realObjId = x.ObjectCr.RealityObject.Id,
                    sumTo = (x.BudgetMu + x.BudgetSubject + x.FundResource) ?? 0M,
                    sumOwners = x.OwnerResource ?? 0M
                })
                                 .ToList();

            while (start < realObjIdList.Count)
            {
                var tmpRealObj = realObjIdList.Skip(start).Take(1000).ToArray();

                sumToAndSumEconomyList.AddRange(serviceFinanceSourceResource.GetAll()
                    .Where(x => tmpRealObj.Contains(x.ObjectCr.RealityObject.Id))
                    .Where(x => x.FinanceSource.Id == financeSourceId)
                    .Where(x => x.ObjectCr.ProgramCr.Id == programCrOneId)
                    .Select(x => new
                    {
                        realObjId = x.ObjectCr.RealityObject.Id,
                        sumTo = (x.BudgetMu + x.BudgetSubject + x.FundResource) ?? 0M,
                        sumOwners = x.OwnerResource ?? 0M
                    })
                    .ToList());

                start += 1000;
            }

            var sumToAndSumEconomy = sumToAndSumEconomyList
                                    .GroupBy(x => x.realObjId)
                                        .ToDictionary(x => x.Key, x => new
                                        {
                                            sumTo = x.Select(y => y.sumTo).FirstOrDefault(),
                                            sumOwners = x.Select(y => y.sumOwners).FirstOrDefault()
                                        });

            // столбец 4 (Дата и номер платежного документа)
            start = 1000;
            var tmpListRealObjId = realObjIdList.Count > start ? realObjIdList.Take(1000).ToArray() : realObjIdList.ToArray();

            var payOrdByRealObjList = serviceBasePaymentOrder.GetAll()
                .Where(x => tmpListRealObjId.Contains(x.BankStatement.ObjectCr.RealityObject.Id))
                .Where(x => x.BankStatement.Period.Id == periodPr2)
                .Where(x => x.TypePaymentOrder == TypePaymentOrder.In)
                .Where(x => x.TypeFinanceSource != TypeFinanceSource.OccupantFunds)
                .Select(x => new
                {
                    RealObjId = x.BankStatement.ObjectCr.RealityObject.Id,
                    data = string.Format("{1} {0}", x.DocumentNum, x.DocumentDate.HasValue ? x.DocumentDate.ToDateTime().ToShortDateString() : string.Empty),
                })
                 .ToList();

            while (start < realObjIdList.Count)
            {
                var tmpListRealObj = realObjIdList.Skip(start).Take(1000).ToArray();

                payOrdByRealObjList.AddRange(serviceBasePaymentOrder.GetAll()
                    .Where(x => tmpListRealObj.Contains(x.BankStatement.ObjectCr.RealityObject.Id))
                    .Select(x => new
                    {
                        RealObjId = x.BankStatement.ObjectCr.RealityObject.Id,
                        data = string.Format("{0} от {1}", x.DocumentNum, x.DocumentDate.HasValue ? x.DocumentDate.ToDateTime().ToShortDateString() : string.Empty)
                    })
                    .ToList());

                start += 1000;
            }

            var payOrdByRealObjDict = payOrdByRealObjList
                                    .GroupBy(x => x.RealObjId)
                                        .ToDictionary(x => x.Key, x =>
                                        {
                                            var dateAndNum = x.Select(y => y.data).ToList();
                                            var res = dateAndNum.Aggregate((a, b) => a + ", " + b);
                                            return res;
                                        });

            var sectionGroup = reportParams.ComplexReportParams.ДобавитьСекцию("sectionGroup");
            var sectionGroupMunicipality = sectionGroup.ДобавитьСекцию("sectionGroupMunicipality");
            var sectionNoGroupMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionNoGroupMunicipality");

            var allSumTo = 0M;
            var allSumEcon = 0M;
            foreach (var group in municipalityGroupDict.OrderBy(x => x.Key))
            {
                var munSection = sectionNoGroupMunicipality;
                var section = munSection.ДобавитьСекцию("section");
                var groupBool = false;
                var totSumToGr = 0M;
                var totSumEconGr = 0M;
                if (!string.IsNullOrEmpty(group.Key))
                {
                    sectionGroup.ДобавитьСтроку();
                    sectionGroup["GrName"] = group.Key;
                    munSection = sectionGroupMunicipality;
                    section = munSection.ДобавитьСекцию("section");
                    groupBool = true;
                }

                foreach (var mun in group.Value)
                {
                    munSection.ДобавитьСтроку();
                    var totSumTo = 0M;
                    var totSumEcon = 0M;
                    if (resDict.ContainsKey(mun.Key))
                    {
                        foreach (var realObj in resDict[mun.Key])
                        {
                            section.ДобавитьСтроку();
                            section["municipality"] = municipalityDict[mun.Key];
                            section["address"] = realObj.Value.Address;
                            decimal sumEcon;
                            if (sumToAndSumEconomy.ContainsKey(realObj.Key))
                            {
                                section["sumTo"] = sumToAndSumEconomy[realObj.Key].sumTo;
                                totSumTo += sumToAndSumEconomy[realObj.Key].sumTo;

                                var sumPerc = realObj.Value.SumOnlyWorksByPr1 != 0
                                                  ? (1 - sumToAndSumEconomy[realObj.Key].sumOwners / realObj.Value.SumOnlyWorksByPr1)
                                                  : 1;
                                sumEcon = realObj.Value.SumDiffByWorks * sumPerc;
                                section["sumEcon"] = sumEcon;
                                totSumEcon += sumEcon;
                            }
                            else
                            {
                                sumEcon = realObj.Value.SumDiffByWorks;
                                section["sumEcon"] = sumEcon;
                                totSumEcon += sumEcon;
                            }

                            if (payOrdByRealObjDict.ContainsKey(realObj.Key))
                            {
                                section["dateAndNum"] = payOrdByRealObjDict[realObj.Key];
                            }

                            section["returnBud"] = 0;
                            section["dateAndNum2"] = 0;
                            section["limitCompare"] = realObj.Value.MessageOfResultCompare;
                        }
                    }

                    munSection["munName"] = mun.Value;
                    munSection["totSumTo"] = totSumTo;
                    allSumTo += totSumTo;

                    munSection["totSumEcon"] = totSumEcon;
                    allSumEcon += totSumEcon;
                    
                    if (groupBool)
                    {
                        totSumToGr += totSumTo;
                        totSumEconGr += totSumEcon;
                    }
                }

                if (groupBool)
                {
                    sectionGroup["totSumToGr"] = totSumToGr;
                    sectionGroup["totSumEconGr"] = totSumEconGr;
                }
            }

            reportParams.SimpleReportParams["allSumTo"] = allSumTo;
            reportParams.SimpleReportParams["allSumEcon"] = allSumEcon;
        }

        private SumEconomyProxy GetDataForRealObjBySumEconomy(Dictionary<string, decimal> distrSumsByPr1, Dictionary<string, decimal> distrSumsByPr2,
                                                                decimal sumOnlyWorksByPr1, decimal sumOnlyWorksByPr2, string realObjAddress)
        {
            var resCompare = string.Empty;
            var sumDiffByWorks = 0M;
            var res = false;

            // если дом находится в обоих программах
            if (distrSumsByPr1.Count > 0 && distrSumsByPr2.Count > 0)
            {
                // если лимиты программ равны
                if (sumOnlyWorksByPr1 == sumOnlyWorksByPr2)
                {
                    resCompare = "Лимиты равны";

                    // если есть работа в программе 1, которая дороже аналогичной работы в программе 2 (р1 > p2) или есть работа в программе 1, но ее нет в программе 2, и ее стоимотсть не равна нулю
                    var Lim1EqLim2 = distrSumsByPr1
                        .Any(t => distrSumsByPr2.ContainsKey(t.Key)
                            && t.Value > distrSumsByPr2[t.Key]
                            || !distrSumsByPr2.ContainsKey(t.Key) 
                            && t.Value != 0);

                    if (Lim1EqLim2)
                    {
                        sumDiffByWorks = distrSumsByPr1
                            .Where(t => distrSumsByPr2.ContainsKey(t.Key)
                            && t.Value < distrSumsByPr2[t.Key])
                            .Sum(t => distrSumsByPr2[t.Key] - t.Value);

                        var sumByNewWorks = distrSumsByPr2
                            .Where(t => !distrSumsByPr1.ContainsKey(t.Key))
                            .Sum(t => t.Value);

                        sumDiffByWorks += sumByNewWorks;
                    }

                    res |= Lim1EqLim2;
                }
                else if (sumOnlyWorksByPr1 > sumOnlyWorksByPr2)
                {
                    // иначе если лимит программы 1 > лимита программы 2
                    resCompare = "Лимиты меньше";

                    // если есть работа в программе 1 , которая дешевле аналогичной работы в программе 2 (р1 < p2) // или есть новый вид работы в программе 2 
                    var Lim1MoreLim2 = distrSumsByPr1
                        .Any(t => distrSumsByPr2.ContainsKey(t.Key)
                            && t.Value < distrSumsByPr2[t.Key]
                            || distrSumsByPr2.Any(z => !distrSumsByPr1.ContainsKey(z.Key)));

                    if (Lim1MoreLim2)
                    {
                        sumDiffByWorks = distrSumsByPr1
                            .Where(t => distrSumsByPr2.ContainsKey(t.Key)
                                && t.Value < distrSumsByPr2[t.Key])
                                .Sum(t => distrSumsByPr2[t.Key] - t.Value);

                        var sumByNewWorks = distrSumsByPr2
                            .Where(t => !distrSumsByPr1.ContainsKey(t.Key))
                            .Sum(t => t.Value);

                        sumDiffByWorks += sumByNewWorks;
                    }

                    res |= Lim1MoreLim2;
                }
                else
                {
                    // иначе если лимит программы 1 < лимита программы 2
                    resCompare = "Лимиты больше";

                    // если есть работа в программе 1, которая дороже аналогичной работы в программе 2 (р1 > р2) или есть работа в программе 1, но ее нет в программе 2, и ее стоимость не равна нулю
                    var Lim1LessLim2 = distrSumsByPr1
                        .Any(t => distrSumsByPr2.ContainsKey(t.Key)
                            && t.Value > distrSumsByPr2[t.Key]
                            || !distrSumsByPr2.ContainsKey(t.Key) 
                            && t.Value != 0);

                    if (Lim1LessLim2)
                    {
                        sumDiffByWorks = distrSumsByPr1
                            .Where(t => distrSumsByPr2.ContainsKey(t.Key)
                                && t.Value > distrSumsByPr2[t.Key])
                                .Sum(t => Math.Abs(distrSumsByPr2[t.Key] - t.Value));

                        var sumByLostWorks = distrSumsByPr1
                            .Where(t => !distrSumsByPr2.ContainsKey(t.Key))
                            .Sum(t => t.Value);

                        sumDiffByWorks += sumByLostWorks;
                    }

                    res |= Lim1LessLim2;
                }
            }

            return new SumEconomyProxy
                       {
                           ExistRealObjInBothProgram = res,
                           Address = realObjAddress,
                           MessageOfResultCompare = resCompare,
                           SumDiffByWorks = sumDiffByWorks,
                           SumOnlyWorksByPr1 = sumOnlyWorksByPr1
                       };
        }

        private void GetDictByDistributedSums(Dictionary<string, decimal> sumByOnlyWorkDict, decimal sumOnlyService, decimal sumByOnlyWork)
        {
            if (sumOnlyService > 0)
            {
                if (sumByOnlyWork <= 0)
                {
                    var firstWorkCode = sumByOnlyWorkDict.Select(t => t.Key).FirstOrDefault();
                    if (firstWorkCode != null && sumByOnlyWorkDict.ContainsKey(firstWorkCode))
                    {
                        sumByOnlyWorkDict[firstWorkCode] += sumOnlyService;
                    }
                }
                else
                {
                    var dopSums = sumByOnlyWorkDict.ToDictionary(t => t.Key, t => (t.Value * sumOnlyService / sumByOnlyWork));
                    var tmpSum = dopSums.Sum(t => t.Value);
                    var err = Math.Round(sumOnlyService - tmpSum, 2);

                    var codeWork = sumByOnlyWorkDict.FirstOrDefault(t => t.Value > 0 && t.Value + err > 0).Key;
                    if (codeWork != null)
                    {
                        sumByOnlyWorkDict[codeWork] += err;
                    }

                    foreach (var sum in dopSums)
                    {
                        sumByOnlyWorkDict[sum.Key] += sum.Value;
                    }
                }
            }
        }

        private class SumEconomyProxy
        {
            public bool ExistRealObjInBothProgram;

            public string Address;

            public string MessageOfResultCompare;

            public decimal SumDiffByWorks;

            public decimal SumOnlyWorksByPr1;

        }
    }
}
