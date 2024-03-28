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

    public class CalculationsBetweenGisuByManOrg : BasePrintForm
    {
        #region Свойства

        private long[] municipalityIds;

        private DateTime reportDate;

        public IWindsorContainer Container { get; set; }

        public CalculationsBetweenGisuByManOrg()
            : base(new ReportTemplateBinary(Properties.Resources.CalculationsBetweenGisuByManOrg))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.CalculationsBetweenGisuByManOrg";
            }
        }

        public override string Name
        {
            get
            {
                return "Сверка расчетов между ГИСУ и УО";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сверка расчетов между ГИСУ и УО";
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
                return "B4.controller.report.CalculationsBetweenGisuByManOrg";
            }
        }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            reportDate = baseParams.Params["reportDate"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var servicePaymentItem = Container.Resolve<IDomainService<PaymentItem>>();
            var serviceTransferRfRecObj = Container.Resolve<IDomainService<TransferRfRecObj>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();

            var dateStart = new DateTime(reportDate.Year, 1, 1);
            var dateEnd = reportDate.Month == 12
                ? new DateTime(reportDate.Year, reportDate.Month, 31)
                : new DateTime(reportDate.Year, reportDate.Month + 1, 1);

            var municipalityDict = serviceMunicipality.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => new { muId = x.Id, muName = x.Name })
                .ToDictionary(x => x.muId, x => x.muName);

            var sumsByRealObjQuery = servicePaymentItem.GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Payment.RealityObject.Municipality.Id))
                         .Where(x => x.ChargeDate.HasValue && dateStart <= x.ChargeDate && x.ChargeDate < dateEnd)
                         .Where(x => x.TypePayment == TypePayment.Cr
                             || x.TypePayment == TypePayment.Cr185
                             || x.TypePayment == TypePayment.HireRegFund)
                         .Select(x =>
                             new
                             {
                                 realObjId = x.Payment.RealityObject.Id,
                                 x.Payment.RealityObject.Address,
                                 muId = x.Payment.RealityObject.Municipality.Id,
                                 muName = x.Payment.RealityObject.Municipality.Name,
                                 x.TypePayment,
                                 recalc = x.Recalculation ?? 0M,
                                 charge = x.ChargePopulation ?? 0M,
                                 paid = x.PaidPopulation ?? 0M
                             });

            // столбцы 2, 3, 4, 5, 7, 8, 9
            var sumsByRealObjDict = sumsByRealObjQuery.AsEnumerable()
                         .OrderBy(x => x.muName)
                         .ThenBy(x => x.Address)
                         .GroupBy(x => x.muId)
                         .ToDictionary(
                             x => x.Key,
                                x => x.GroupBy(y => y.realObjId)
                                        .ToDictionary(
                                             y => y.Key,
                                                 y => new
                                                 {
                                                     address = y.Select(z => z.Address).FirstOrDefault(),
                                                     muName = y.Select(z => z.muName).FirstOrDefault(),
                                                     chargeSumCr = y.Where(z => z.TypePayment == TypePayment.Cr).Sum(z => z.recalc + z.charge),
                                                     chargeSumCr185 = y.Where(z => z.TypePayment == TypePayment.Cr185).Sum(z => z.recalc + z.charge),
                                                     chargeSumCrHire = y.Where(z => z.TypePayment == TypePayment.HireRegFund).Sum(z => z.recalc + z.charge),
                                                     paidSumCr = y.Where(z => z.TypePayment == TypePayment.Cr).Sum(z => z.paid),
                                                     paidSumCr185 = y.Where(z => z.TypePayment == TypePayment.Cr185).Sum(z => z.paid),
                                                     paidSumCrHire = y.Where(z => z.TypePayment == TypePayment.HireRegFund).Sum(z => z.paid)
                                                 }));

            var realObjIdsQuery = sumsByRealObjQuery.Select(x => x.realObjId).Distinct();

            // столбец 11
            var sumToGisuDict = serviceTransferRfRecObj.GetAll()
                .Where(x => realObjIdsQuery.Contains(x.RealityObject.Id))
                .Where(x => x.TransferRfRecord.DateFrom.HasValue && dateStart <= x.TransferRfRecord.DateFrom && x.TransferRfRecord.DateFrom < dateEnd)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    sumToGisu = x.Sum ?? 0M
                })
                                 .AsEnumerable()
                                 .GroupBy(x => x.Id)
                                 .ToDictionary(x => x.Key, x => x.Sum(y => y.sumToGisu));

            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("MuSection");
            var section = sectionMo.ДобавитьСекцию("section");
            var num = 0;
            var totalResult = new decimal[14];
            foreach (var mun in sumsByRealObjDict)
            { 
                sectionMo.ДобавитьСтроку();
                var resultsByMunicip = new decimal[14];
                foreach (var realObj in mun.Value)
                {
                    section.ДобавитьСтроку();
                    ++num;
                    section["номер"] = num;
                    section["адрес"] = string.Format("{0}, {1}", realObj.Value.muName, realObj.Value.address);
                    section["начислКР"] = realObj.Value.chargeSumCr;
                    resultsByMunicip[0] += realObj.Value.chargeSumCr;

                    section["начислКР185"] = realObj.Value.chargeSumCr185;
                    resultsByMunicip[1] += realObj.Value.chargeSumCr185;

                    section["начислНайм"] = realObj.Value.chargeSumCrHire;
                    resultsByMunicip[2] += realObj.Value.chargeSumCrHire;

                    decimal chargeAll = realObj.Value.chargeSumCrHire + realObj.Value.chargeSumCr185 + realObj.Value.chargeSumCr;
                    section["начислВсе"] = chargeAll;
                    resultsByMunicip[3] += chargeAll;

                    section["оплачКР"] = realObj.Value.paidSumCr;
                    resultsByMunicip[4] += realObj.Value.paidSumCr;

                    section["оплачКР185"] = realObj.Value.paidSumCr185;
                    resultsByMunicip[5] += realObj.Value.paidSumCr185;

                    section["оплачНайм"] = realObj.Value.paidSumCrHire;
                    resultsByMunicip[6] += realObj.Value.paidSumCrHire;

                    decimal paidAll = realObj.Value.paidSumCrHire + realObj.Value.paidSumCr185 + realObj.Value.paidSumCr;
                    section["оплачВсе"] = paidAll;
                    resultsByMunicip[7] += paidAll;

                    var errCr = realObj.Value.chargeSumCr - realObj.Value.paidSumCr;
                    section["отклКР"] = errCr;
                    resultsByMunicip[9] += errCr;

                    var errCr185 = realObj.Value.chargeSumCr185 - realObj.Value.paidSumCr185;
                    section["отклКР185"] = errCr185;
                    resultsByMunicip[10] += errCr185;

                    var errCrHire = realObj.Value.chargeSumCrHire - realObj.Value.paidSumCrHire;
                    section["отклНайм"] = errCrHire;
                    resultsByMunicip[11] += errCrHire;

                    decimal errCharge;
                    decimal errPaid;
                    if (sumToGisuDict.ContainsKey(realObj.Key))
                    {
                        decimal sumManOrgToGisu = sumToGisuDict[realObj.Key];
                        section["уоВгису"] = sumManOrgToGisu;
                        resultsByMunicip[8] += sumManOrgToGisu;

                        errCharge = chargeAll - sumManOrgToGisu;
                        section["отклНачисл"] = errCharge;
                        resultsByMunicip[12] += errCharge;

                        errPaid = paidAll - sumManOrgToGisu;
                        section["отклОплач"] = errPaid;
                        resultsByMunicip[13] += errPaid;
                    }
                    else
                    {
                        errCharge = chargeAll;
                        section["отклНачисл"] = errCharge;
                        resultsByMunicip[12] += errCharge;

                        errPaid = paidAll;
                        section["отклОплач"] = errPaid;
                        resultsByMunicip[13] += errPaid;
                    }
                }

                sectionMo["МунОбр"] = municipalityDict[mun.Key];
                for (var i = 0; i < 14; ++i)
                {
                    sectionMo["сумПоМо" + (i + 3)] = resultsByMunicip[i];
                    totalResult[i] += resultsByMunicip[i];
                }
            }

            for (var i = 0; i < 14; ++i)
            {
                reportParams.SimpleReportParams["итого" + (i + 3)] = totalResult[i];
            }
        }
    }
}