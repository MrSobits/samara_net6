namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class ListHousesByProgramCr : BasePrintForm
    {
        #region Свойства
        private long[] programCrIds;
        private long[] municipalityIds;
        private DateTime reportDate;
        private int assemblyTo = 10;
        public IWindsorContainer Container { get; set; }

        public ListHousesByProgramCr()
            : base(new ReportTemplateBinary(Properties.Resources.ListHousesByProgramCr))
        {
        }

        public override string RequiredPermission
        {
            get { return "Reports.RF.ListHousesByProgramCr"; }
        }

        public override string Name
        {
            get { return "Перечень домов по программе"; }
        }

        public override string Desciption
        {
            get { return "Перечень домов по программе"; }
        }

        public override string GroupName
        {
            get { return "Формы программы"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ListHousesByProgramCr"; }
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            var programCrIdList = baseParams.Params.GetAs("programCrIds", string.Empty);
            programCrIds = !string.IsNullOrEmpty(programCrIdList) ? programCrIdList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            reportDate = baseParams.Params["reportDate"].ToDateTime();

            // 10 - по всем домам (по умолчанию); 20 - по наличию договора с ГИСУ
            assemblyTo = baseParams.Params["assemblyTo"].ToInt();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceContractRfObject = Container.Resolve<IDomainService<ContractRfObject>>();
            var serviceObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
            var serviceRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            var servicePaymentItem = Container.Resolve<IDomainService<PaymentItem>>();
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();

            var workCodesList = new List<string>() { "1018", "1020", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", 
                "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "999", "29" };

            #region Запросы
            var municipalityDict = serviceMunicipality.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => new { muId = x.Id, muName = x.Name })
                .ToDictionary(x => x.muId, x => x.muName);

            var allRealObjByGisu = serviceContractRfObject.GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .Select(x => x.RealityObject.Id)
                         .ToList();

            // столбцы: 2, 3, 4
            var realtyObjectCrQuery = programCrIds[0] != 0 ? serviceObjectCr.GetAll()
                .WhereIf(assemblyTo == 20, x => allRealObjByGisu.Contains(x.RealityObject.Id))
                .Where(x => programCrIds.Contains(x.ProgramCr.Id))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    realityId = x.RealityObject.Id,
                    moId = x.RealityObject.Municipality.Id,
                    moName = x.RealityObject.Municipality.Name,
                    address = x.RealityObject.Address,
                    dateCommissioning = x.RealityObject.DateCommissioning.HasValue ? x.RealityObject.DateCommissioning.Value.Year : DateTime.MaxValue.Year,
                })
            : serviceRealityObject.GetAll()
                     .WhereIf(assemblyTo == 20, x => allRealObjByGisu.Contains(x.Id))
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                     .Select(
                         x => new
                                  {
                                      realityId = x.Id,
                                      moId = x.Municipality.Id,
                                      moName = x.Municipality.Name,
                                      address = x.Address,
                                      dateCommissioning = x.DateCommissioning.HasValue ? x.DateCommissioning.Value.Year : DateTime.MaxValue.Year,
                                  });

            var realityIdQuery = realtyObjectCrQuery.Select(x => x.realityId).Distinct();

            var realtyObjectCrDict = realtyObjectCrQuery.AsEnumerable()
                                  .OrderBy(x => x.moName)
                                  .ThenBy(x => x.address)
                                  .GroupBy(x => x.moId)
                                  .ToDictionary(x => x.Key,
                                      x => x.GroupBy(y => y.realityId).ToDictionary(y => y.Key, y => y.FirstOrDefault()));


            // столбец 7; RealityObjId - NameProgramCrList
            var nameProgramCrDict = serviceObjectCr.GetAll()
                         .Where(x => realityIdQuery.Contains(x.RealityObject.Id)
                         && x.ProgramCr.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden
                         && x.ProgramCr.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Print)
                         .Select(x => new { realityId = x.RealityObject.Id, nameProgCr = x.ProgramCr.Name })
                         .AsEnumerable()
                         .GroupBy(x => x.realityId)
                         .ToDictionary(x => x.Key, x => x.Select(y => y.nameProgCr).ToList());

            // столбец 5, 6 (дома с договорами ГИСУ)
            var infoRealtyObjByGisuList = serviceContractRfObject.GetAll()
                    .Where(x => realityIdQuery.Contains(x.RealityObject.Id))
                    .Select(x => new
                    {
                        realityId = x.RealityObject.Id,
                        numDoc = x.ContractRf.DocumentNum,
                        dateDoc = x.ContractRf.DocumentDate ?? DateTime.MinValue,
                        dateExl = x.ExcludeDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.realityId)
                    .ToDictionary(x => x.Key, 
                        y => y.OrderBy(x => (reportDate - x.dateDoc).TotalDays)
                            .ThenBy(x => x.dateExl)
                            .FirstOrDefault());


            // столбец 8, 9, 10, 11; значения из Реестра Оплат КР
            var dateStart = new DateTime(reportDate.Year, 1, 1);
            var countMonth = (reportDate.Month - dateStart.Month) + 1;

            var paymentsByprogramCr = servicePaymentItem.GetAll()
                    .Where(x => realityIdQuery.Contains(x.Payment.RealityObject.Id))
                    .Where(x => x.ChargeDate.HasValue && x.ChargeDate >= dateStart && x.ChargeDate < reportDate)
                    .Where(x => x.TypePayment == TypePayment.HireRegFund || x.TypePayment == TypePayment.Cr || x.TypePayment == TypePayment.Cr185)
                    .Where(x => x.ManagingOrganization != null)
                    .GroupBy(x => x.Payment.RealityObject.Id)
                    .Select(x => new
                    {
                        x.Key,
                        chPop = x.Sum(y => y.ChargePopulation),
                        paidPop = x.Sum(y => y.PaidPopulation)
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.Key, x =>
                    {
                        var ChargePopulation = x.chPop ?? 0M;
                        var PaidPopulation = x.paidPop ?? 0M;

                        var paymentCr = ChargePopulation != 0M ? (PaidPopulation / ChargePopulation) * 100 : 0;
                        var sumTot = countMonth != 0 ? (PaidPopulation / countMonth) * 12 : 0;
                        return new { ChargePopulation, PaidPopulation, paymentCr, sumTot };
                    });

            // Сумма, Объем по коду работ
            var workCodesDict = serviceTypeWorkCr.GetAll()
                        .Where(x => programCrIds.Contains(x.ObjectCr.ProgramCr.Id))
                        .Where(x => realityIdQuery.Contains(x.ObjectCr.RealityObject.Id))
                        .Where(x => workCodesList.Contains(x.Work.Code))
                        .GroupBy(x => new { x.ObjectCr.RealityObject.Id, x.Work.Code })
                        .Select(x => new
                        {
                            realObjId = x.Key.Id,
                            x.Key.Code,
                            sum = x.Sum(y => y.Sum),
                            vol = x.Sum(y => y.Volume)
                        })
                       .AsEnumerable()
                       .GroupBy(x => x.realObjId)
                       .ToDictionary(
                           x => x.Key,
                           x => x.GroupBy(y => y.Code)
                                 .ToDictionary(
                                    y => y.Key, 
                                    y => new
                                        {
                                            sum = y.Sum(z => z.sum ?? 0M),
                                            vol = y.Sum(z => z.vol ?? 0M)
                                        })); 
            #endregion

            #region Вывод данных
            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("MuSection");
            var section = sectionMo.ДобавитьСекцию("section");
            var num = 0;
            foreach (var municip in realtyObjectCrDict)
            {
                sectionMo.ДобавитьСтроку();
                var chargePopulationByMo = 0M;
                var paidPopulationByMo = 0M;
                var paymentCrByMo = 0M;
                var sumTotByMo = 0M;
                var sumCostWorksByMo = 0M;
                var countPercByMo = 0;
                var totalsByMo = workCodesList.GroupBy(x => x).ToDictionary(x => x.Key, x => new SumAndVolProxy { sum = 0M, vol = 0M });

                foreach (var realObj in municip.Value)
                {
                    section.ДобавитьСтроку();
                    ++num;
                    section["номер"] = num;
                    section["район"] = realObj.Value.moName;
                    section["адрес"] = realObj.Value.address;
                    section["годВвода"] = realObj.Value.dateCommissioning;

                    if (infoRealtyObjByGisuList.ContainsKey(realObj.Key))
                    {
                        section["номерДогГису"] = infoRealtyObjByGisuList[realObj.Key].numDoc;
                        section["датаДогГису"] = infoRealtyObjByGisuList[realObj.Key].dateDoc;
                    }

                    var programCrNameList = string.Empty;
                    if (nameProgramCrDict.ContainsKey(realObj.Key))
                    {
                        var nameProgramList = nameProgramCrDict[realObj.Key];
                        programCrNameList = nameProgramList.Aggregate((a, b) => a + ", " + b);
                    }

                    section["ПрогрКР"] = programCrNameList;

                    if (paymentsByprogramCr.ContainsKey(realObj.Key))
                    {
                        section["начислКР"] = paymentsByprogramCr[realObj.Key].ChargePopulation;
                        chargePopulationByMo += paymentsByprogramCr[realObj.Key].ChargePopulation;

                        section["оплачКР"] = paymentsByprogramCr[realObj.Key].PaidPopulation;
                        paidPopulationByMo += paymentsByprogramCr[realObj.Key].PaidPopulation;

                        section["оплКР"] = paymentsByprogramCr[realObj.Key].paymentCr;
                        paymentCrByMo += paymentsByprogramCr[realObj.Key].paymentCr;
                        ++countPercByMo;

                        section["суммСобрСр"] = paymentsByprogramCr[realObj.Key].sumTot;
                        sumTotByMo += paymentsByprogramCr[realObj.Key].sumTot;
                    }

                    var sumCostByWorks = 0M;
                    if (workCodesDict.ContainsKey(realObj.Key))
                    {
                        foreach (var work in workCodesDict[realObj.Key])
                        {
                            section["сумма" + work.Key] = work.Value.sum;
                            sumCostByWorks += work.Value.sum;

                            section["объем" + work.Key] = work.Value.vol;
                            totalsByMo[work.Key].sum += work.Value.sum;
                            totalsByMo[work.Key].vol += work.Value.vol;
                        }
                    }

                    section["суммСтмКР"] = sumCostByWorks;
                    sumCostWorksByMo += sumCostByWorks;
                }

                sectionMo["МунОбр"] = municipalityDict[municip.Key];
                sectionMo["МОначислКР"] = chargePopulationByMo;
                sectionMo["МОоплачКР"] = paidPopulationByMo;
                sectionMo["МОоплКР"] = countPercByMo != 0 ? paymentCrByMo / countPercByMo : 0;
                sectionMo["МОсуммСобрСр"] = sumTotByMo;
                sectionMo["МОсуммСтмКР"] = sumCostWorksByMo;

                foreach (var totByMo in totalsByMo)
                {
                    sectionMo["МОсумма" + totByMo.Key] = totByMo.Value.sum;
                    sectionMo["МОобъем" + totByMo.Key] = totByMo.Value.vol;
                }
            }
            #endregion
        }

        private class SumAndVolProxy
        {
            public decimal sum;

            public decimal vol;
        }
    }
}