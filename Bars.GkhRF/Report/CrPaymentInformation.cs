namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using Castle.Windsor;

    using Bars.GkhRf.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Enums;

    using B4;

    using Gkh.Entities;

    public class CrPaymentInformation : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private List<long> municipalityIdsList = new List<long>();

        public CrPaymentInformation() : base(new ReportTemplateBinary(Properties.Resources.CrPaymentInformation))
        {

        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.CrPaymentInformation";
            }
        }

        public override string Name
        {
            get { return "01_Сведения об оплате КР"; }
        }

        public override string Desciption
        {
            get { return "01_Сведения об оплате КР"; }
        }

        public override string GroupName
        {
            get { return "Отчеты Рег.Фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CrPaymentInformation"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();

            var strMunicipalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityIdsList = !string.IsNullOrEmpty(strMunicipalIds) ? strMunicipalIds.Split(',').Select(x => x.ToLong()).ToList() : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            //Формируем словарь с ключами - Id всех муниципальных образований
            var municipaltys = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(this.municipalityIdsList.Count > 0, x => municipalityIdsList.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToDictionary(x => x.Id, x => x.Name);

            //Количество домов по МО в мониторинге, исключая снесенные
            var roCount = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => x.ConditionHouse == ConditionHouse.Razed)
                .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.Municipality.Id))
                .GroupBy(x => x.Municipality.Id)
                .Select(x => new
                    {
                        x.Key,
                        count = x.Select(y => y.Id).Count()
                    })
                .ToDictionary(x => x.Key, x => x.count);
            
            //Количество домов по МО, включенных в договоры с УК
            var roInContractsCount = Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
                .Where(x => x.TypeCondition == TypeCondition.Include)
                .Where(x => x.IncludeDate <= dateEnd)
                .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.RealityObject.Municipality.Id))
                .GroupBy(x => x.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    count = x.Select(y => y.RealityObject.Id).Distinct().Count()
                })
                .ToDictionary(x => x.Key, x => x.count);

            //Количество домов по МО, у которых  за выбранный период существует хотя бы 1 запись в реестре оплат КР
            var roWithPaymentCount = Container.Resolve<IDomainService<PaymentItem>>().GetAll()
                .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.Payment.RealityObject.Municipality.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.ChargeDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.ChargeDate <= dateEnd)
                .GroupBy(x => x.Payment.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    count = x.Select(y => y.Payment.RealityObject.Id).Distinct().Count()
                })
                .ToDictionary(x => x.Key, x => x.count);

            //Словарь по МО и типу оплаты 
            var chargesDict = Container.Resolve<IDomainService<PaymentItem>>().GetAll()
                .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.Payment.RealityObject.Municipality.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.ChargeDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.ChargeDate <= dateEnd)
                .GroupBy(x => new { muId = x.Payment.RealityObject.Municipality.Id, x.TypePayment })
                .Select(x => new
                {
                    x.Key,
                    chargePopulation = x.Sum(y => y.ChargePopulation),
                    recalculation = x.Sum(y => y.Recalculation),
                    paidPopulationSum = x.Sum(y => y.PaidPopulation)
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Key,
                    computedSum = (x.chargePopulation ?? 0) + (x.recalculation ?? 0),
                    paidPopulationSum = x.paidPopulationSum ?? 0
                })
                .ToList();

            // Словарь суммы "начисления населению" и "Перерасчет прошлого периода"
            var chargePaymentItemDict = chargesDict.GroupBy(x => x.Key.TypePayment)
                .ToDictionary(
                    x => x.Key,
                    x => x.ToDictionary(y => y.Key.muId, y => y.computedSum ));

            // Словарь суммы "оплачено населению"
            var paidPaymentItemDict = chargesDict.GroupBy(x => x.Key.TypePayment)
                .ToDictionary(
                    x => x.Key,
                    x => x.ToDictionary(y => y.Key.muId, y => y.paidPopulationSum));

            Func<TypePayment, Dictionary<long, decimal>> getChargePayment = (type) => chargePaymentItemDict.ContainsKey(type) ? chargePaymentItemDict[type] : new Dictionary<long, decimal>();
            Func<TypePayment, Dictionary<long, decimal>> getPaidPayment = (type) => paidPaymentItemDict.ContainsKey(type) ? paidPaymentItemDict[type] : new Dictionary<long, decimal>();
            
            //капитальный ремонт
            var сomputedCr = getChargePayment(TypePayment.Cr);
            var paidCr = getPaidPayment(TypePayment.Cr);

            //найм
            var computedRent = getChargePayment(TypePayment.HireRegFund);
            var paidRent = getPaidPayment(TypePayment.HireRegFund);
            
            //капитальный ремонт по 185-ФЗ
            var computed185Fz = getChargePayment(TypePayment.Cr185);
            var paid185Fz = getPaidPayment(TypePayment.Cr185);
            
            //Текущий ремонт здания
            var computedCurrentRepair = getChargePayment(TypePayment.BuildingCurrentRepair);
            var paidCurrentRepair = getPaidPayment(TypePayment.BuildingCurrentRepair);
            var codesCurrentRepair = new List<string> { "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            var regFundBuildRepair = GetPaidRegFund(TypePayment.BuildingCurrentRepair, codesCurrentRepair);

            //Текущий ремонт сан. тех. сетей
            var computedSanTechRepair = getChargePayment(TypePayment.SanitaryEngineeringRepair);
            var paidSanTechRepair = getPaidPayment(TypePayment.SanitaryEngineeringRepair);
            var codesSanTechRepair = new List<string> { "2", "3", "4" };
            var regFundSanTechRepair = GetPaidRegFund(TypePayment.SanitaryEngineeringRepair, codesSanTechRepair);

            //Текущий ремонт сетей ЦО
            var computedHeatingRepair = getChargePayment(TypePayment.HeatingRepair);
            var paidHeatingRepair = getPaidPayment(TypePayment.HeatingRepair);
            var codesHeatingRepair = new List<string> { "1", "5" };
            var regFundHeatingRepair = GetPaidRegFund(TypePayment.HeatingRepair, codesHeatingRepair);

            //Текущий ремонт сетей электроснабжения
            var computedElectricRepair = getChargePayment(TypePayment.ElectricalRepair);
            var paidElectricRepair = getPaidPayment(TypePayment.ElectricalRepair);
            var codesElectricRepair = new List<string> { "6" };
            var regFundElectricRepair = GetPaidRegFund(TypePayment.ElectricalRepair, codesElectricRepair);

            //Текущий ремонт здания и внутридомовых сетей
            var сomputedBuildSystems = getChargePayment(TypePayment.BuildingRepair);
            var paidBuildSystems = getPaidPayment(TypePayment.BuildingRepair);
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            reportParams.SimpleReportParams["CurrentDate"] = DateTime.Now.ToShortDateString();
            reportParams.SimpleReportParams["DateStart"] = dateStart != DateTime.MinValue ? dateStart.ToShortDateString() : string.Empty;
            reportParams.SimpleReportParams["DateEnd"] = dateEnd != DateTime.MinValue ? dateEnd.ToShortDateString() : string.Empty;

            int i = 0;

            var totals = new decimal[26];

            foreach (var key in municipaltys.Keys)
            {
  
                decimal sumComputed = 0;
                decimal sumPaid = 0;

                section.ДобавитьСтроку();
                
                section["Number"] = ++i;
                section["MunicipalityName"] = municipaltys[key];
               
                if (roCount.ContainsKey(key))
                {
                    section["ROCountMonitoring"] = roCount[key] != 0 ? roCount[key].ToString() : string.Empty;
                    totals[0] += roCount[key];
                }
                else section["ROCountMonitoring"] = string.Empty;

                if (roInContractsCount.ContainsKey(key))
                {
                    section["ROCountManOrgContract"] = roInContractsCount[key] != 0 ? roInContractsCount[key].ToString() : string.Empty;
                    totals[1] += roInContractsCount[key];
                }
                else section["ROCountManOrgContract"] = string.Empty;

                if (roWithPaymentCount.ContainsKey(key))
                {
                    section["ROCountUploaded"] = roWithPaymentCount[key] != 0 ? roWithPaymentCount[key].ToString() : string.Empty;
                    totals[2] += roWithPaymentCount[key];
                }
                else section["ROCountUploaded"] = string.Empty;

                if (сomputedCr.ContainsKey(key))
                {
                    section["ComputedCR"] = сomputedCr[key] != 0 ? сomputedCr[key].ToString() : string.Empty;
                    sumComputed += сomputedCr[key];
                    totals[3] += сomputedCr[key];
                }
                else section["ComputedCR"] = string.Empty;

                if (paidCr.ContainsKey(key))
                {
                    section["PaidCR"] = paidCr[key] != 0 ? paidCr[key].ToString() : string.Empty;
                    sumPaid += paidCr[key];
                    totals[4] += paidCr[key];
                }
                else section["PaidCR"] = string.Empty;

                if (computedRent.ContainsKey(key))
                {
                    section["ComputedRent"] = computedRent[key] != 0 ? computedRent[key].ToString() : string.Empty;
                    sumComputed += computedRent[key];
                    totals[5] += computedRent[key];
                }
                else section["ComputedRent"] = string.Empty;

                if (paidRent.ContainsKey(key))
                {
                    section["PaidRent"] = paidRent[key] != 0 ? paidRent[key].ToString() : string.Empty;
                    sumPaid += paidRent[key];
                    totals[6] += paidRent[key];
                }
                else section["PaidRent"] = string.Empty;

                if (computed185Fz.ContainsKey(key))
                {
                    section["Computed185FZ"] = computed185Fz[key] != 0 ? computed185Fz[key].ToString() : string.Empty;
                    sumComputed += computed185Fz[key];
                    totals[7] += computed185Fz[key];
                }
                else section["Computed185FZ"] = string.Empty;

                if (paid185Fz.ContainsKey(key))
                {
                    section["Paid185FZ"] = paid185Fz[key] != 0 ? paid185Fz[key].ToString() : string.Empty;
                    sumPaid += paid185Fz[key];
                    totals[8] += paid185Fz[key];
                }
                else section["Paid185FZ"] = string.Empty;

                if (computedCurrentRepair.ContainsKey(key))
                {
                        section["ComputedBuildRepair"] = computedCurrentRepair[key] != 0 ? computedCurrentRepair[key].ToString() : string.Empty;
                        sumComputed += computedCurrentRepair[key];
                        totals[9] += computedCurrentRepair[key];
                    
                }
                else section["ComputedBuildRepair"] = string.Empty;

                if (paidCurrentRepair.ContainsKey(key))
                {
                    section["PaidBuildRepair"] = paidCurrentRepair[key] != 0 ? paidCurrentRepair[key].ToString() : string.Empty;
                    sumPaid += paidCurrentRepair[key];
                    totals[10] += paidCurrentRepair[key];
                }
                else section["PaidBuildRepair"] = string.Empty;

                if (regFundBuildRepair.ContainsKey(key))
                {
                    section["RegFundBuildRepair"] = regFundBuildRepair[key] != 0 ? regFundBuildRepair[key].ToString() : string.Empty;
                    totals[11] += regFundBuildRepair[key];
                }
                else section["RegFundBuildRepair"] = string.Empty;

                if (computedSanTechRepair.ContainsKey(key))
                {
                    section["ComputedSanTechRepair"] = computedSanTechRepair[key] != 0 ? computedSanTechRepair[key].ToString() : string.Empty;
                    sumComputed += computedSanTechRepair[key];
                    totals[12] += computedSanTechRepair[key];
                }
                else section["ComputedSanTechRepair"] = string.Empty ;

                if (paidSanTechRepair.ContainsKey(key))
                {
                    section["PaidSanTechRepair"] = paidSanTechRepair[key] != 0 ? paidSanTechRepair[key].ToString() : string.Empty;
                    sumPaid += paidSanTechRepair[key];
                    totals[13] += paidSanTechRepair[key];
                }
                else section["PaidSanTechRepair"] = string.Empty;

                if (regFundSanTechRepair.ContainsKey(key))
                {
                    section["RegFundSanTechRepair"] = regFundSanTechRepair[key] != 0 ? regFundSanTechRepair[key].ToString() : string.Empty;
                    totals[14] = regFundSanTechRepair[key];
                }
                else section["RegFundSanTechRepair"] = string.Empty;

                if (computedHeatingRepair.ContainsKey(key))
                {
                    section["ComputedHeatingRepair"] = computedHeatingRepair[key] != 0 ? computedHeatingRepair[key].ToString() : string.Empty;
                    sumComputed += computedHeatingRepair[key];
                    totals[15] += computedHeatingRepair[key];
                }
                else section["ComputedHeatingRepair"] = string.Empty;

                if (paidHeatingRepair.ContainsKey(key))
                {
                    section["PaidHeatingRepair"] = paidHeatingRepair[key] != 0 ? paidHeatingRepair[key].ToString() : string.Empty;
                    sumPaid += paidHeatingRepair[key];
                    totals[16] += paidHeatingRepair[key];
                }
                else section["PaidHeatingRepair"] = string.Empty;

                if (regFundHeatingRepair.ContainsKey(key))
                {
                    section["RegFundHeatingRepair"] = regFundHeatingRepair[key] != 0 ? regFundHeatingRepair[key].ToString() : string.Empty;
                    totals[17] = regFundHeatingRepair[key];
                }
                else section["RegFundHeatingRepair"] = string.Empty;

                if (computedElectricRepair.ContainsKey(key))
                {
                    section["ComputedElectricRepair"] = computedElectricRepair[key] != 0 ? computedElectricRepair[key].ToString() : string.Empty;
                    sumComputed += computedElectricRepair[key];
                    totals[18] += computedElectricRepair[key];
                }
                else section["ComputedElectricRepair"] = string.Empty;

                if (paidElectricRepair.ContainsKey(key))
                {
                    section["PaidElectricRepair"] = paidElectricRepair[key] != 0 ? paidElectricRepair[key].ToString() : string.Empty;
                    sumPaid += paidElectricRepair[key];
                    totals[19] += paidElectricRepair[key];
                }
                else section["PaidElectricRepair"] = string.Empty;

                if (regFundElectricRepair.ContainsKey(key))
                {
                    section["RegFundElectricRepair"] = regFundElectricRepair[key] != 0 ? regFundElectricRepair[key].ToString() : string.Empty;
                    totals[20] = regFundElectricRepair[key];
                }
                else section["RegFundElectricRepair"] = string.Empty;

                if (сomputedBuildSystems.ContainsKey(key))
                {
                    section["ComputedBuildSystemsRepair"] = сomputedBuildSystems[key] != 0 ? сomputedBuildSystems[key].ToString() : string.Empty;
                    sumComputed += сomputedBuildSystems[key];
                    totals[21] += сomputedBuildSystems[key];
                }
                else section["ComputedBuildSystemsRepair"] = string.Empty;

                if (paidBuildSystems.ContainsKey(key))
                {
                    section["PaidBuildSystemRepair"] = paidBuildSystems[key] != 0 ? paidBuildSystems[key].ToString() : string.Empty;
                    sumPaid += paidBuildSystems[key];
                    totals[22] += paidBuildSystems[key];
                }
                else section["PaidBuildSystemRepair"] = string.Empty;


                section["SumComputed"] = sumComputed != 0 ? sumComputed.ToString() : string.Empty; 
                totals[23] += sumComputed;

                section["SumPaid"] = sumPaid != 0 ? sumPaid.ToString() : string.Empty; 
                totals[24] += sumPaid;


                reportParams.SimpleReportParams["TotalROCountMonitoring"] = totals[0] != 0 ? totals[0].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalROCountManOrgContract"] = totals[1] != 0 ? totals[1].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalROCountUploaded"] = totals[2] != 0 ? totals[2].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalComputedCR"] = totals[3] != 0 ? totals[3].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalPaidCR"] = totals[4] != 0 ? totals[4].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalComputedRent"] = totals[5] != 0 ? totals[5].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalPaidRent"] = totals[6] != 0 ? totals[6].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalComputed185FZ"] = totals[7] != 0 ? totals[7].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalPaid185FZ"] = totals[8] != 0 ? totals[8].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalComputedBuildRepair"] = totals[9] != 0 ? totals[9].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalPaidBuildRepair"] = totals[10] != 0 ? totals[10].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalRegFundBuildRepair"] = totals[11] != 0 ? totals[11].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalComputedSanTechRepair"] = totals[12] != 0 ? totals[12].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalPaidSanTechRepair"] = totals[13] != 0 ? totals[13].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalRegFundSanTechRepair"] = totals[14] != 0 ? totals[14].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalComputedHeatingRepair"] = totals[15] != 0 ? totals[15].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalPaidHeatingRepair"] = totals[16] != 0 ? totals[16].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalRegFundHeatingRepair"] = totals[17] != 0 ? totals[17].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalComputedElectricRepair"] = totals[18] != 0 ? totals[18].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalPaidElectricRepair"] = totals[19] != 0 ? totals[19].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalRegFundElectricRepair"] = totals[20] != 0 ? totals[20].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalComputedBuildSystemsRepair"] = totals[21] != 0 ? totals[21].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalPaidBuildSystemRepair"] = totals[22] != 0 ? totals[22].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalSumComputed"] = totals[23] != 0 ? totals[23].ToString() : string.Empty;
                reportParams.SimpleReportParams["TotalSumPaid"] = totals[24] != 0 ? totals[24].ToString() : string.Empty;
            }
        }

        private Dictionary<long, decimal> GetPaidRegFund(TypePayment typePayment, List<string> codes)
        {
            var roIdsQuery = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
               .Where(x => x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Complete
                    && municipalityIdsList.Contains(x.ObjectCr.RealityObject.Municipality.Id)
                    && codes.Contains(x.Work.Code))
               .Select(x => x.ObjectCr.RealityObject.Id)
               .Distinct();

            var PlaymItem = Container.Resolve<IDomainService<PaymentItem>>().GetAll()
                .Where(x => x.TypePayment == typePayment && roIdsQuery.Contains(x.Payment.RealityObject.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.ChargeDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.ChargeDate <= dateEnd)
                .GroupBy(x => x.Payment.RealityObject.Municipality.Id)
                .Select(x => new
                    {
                        MunicipalityId = x.Key,
                        PaidPopulation = x.Sum(y => y.PaidPopulation)
                    })
                .AsEnumerable()
                .Select(x => new
                    {
                        x.MunicipalityId,
                        PaidCurrentRepairSum = x.PaidPopulation ?? 0
                    })
                .ToList()
                .ToDictionary(x => x.MunicipalityId, x => x.PaidCurrentRepairSum / 2);

            return PlaymItem;
        }
    }
}