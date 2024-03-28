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
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class InformationRepublicProgramCr : BasePrintForm
    {
        private DateTime startDate = DateTime.MinValue;
        private DateTime endDate = DateTime.MaxValue;
        private long[] municipalityIds;

        public InformationRepublicProgramCr() : base(new ReportTemplateBinary(Properties.Resources.InformationRepublicProgramCr))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.RF.InformationRepublicProgramCr"; }
        }

        public override string Desciption
        {
            get { return "Информация об участии в Республиканской адресной программе по проведению капитального ремонта МКД"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.InformationRepublicProgramCr"; }
        }

        public override string Name
        {
            get { return "Информация об участии в Республиканской адресной программе по проведению капитального ремонта МКД"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.startDate = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            this.endDate = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var m = baseParams.Params["municipalityIds"].ToString();
            this.municipalityIds = string.IsNullOrEmpty(m) ? new long[0] : m.Split(',').Select(x => x.ToLong()).ToArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var periodStartDate = this.startDate;
            var periodEndDate = this.endDate;
            this.startDate = this.startDate.Day != 1 ? new DateTime(this.startDate.Year, this.startDate.Month + 1, 1) : this.startDate;
            this.endDate = DateTime.DaysInMonth(this.endDate.Year, this.endDate.Month) != this.endDate.Day && this.endDate != DateTime.MinValue ? this.endDate.AddDays(-this.endDate.Day) : this.endDate;
           
            if (this.startDate >= this.endDate)
            {
                return;
            }

            reportParams.SimpleReportParams["StartDate"] = periodStartDate.ToShortDateString();
            reportParams.SimpleReportParams["EndDate"] = this.endDate.ToShortDateString();
            reportParams.SimpleReportParams["PeriodEndDate"] = periodEndDate.ToShortDateString();

            var munDictionary = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Name })
                    .OrderBy(x => x.Name)
                    .ToDictionary(x => x.Id, x => x.Name);

            var realtyObjMoMuDict = this.GetManOrgRealtyObjectsDict(periodEndDate);
            var contractRfDict = this.GetContractRfDict(periodStartDate, periodEndDate);
            var payments = this.GetPayments();

            var paymentsCr = payments
                .Where(x => x.TypePayment == TypePayment.Cr || x.TypePayment == TypePayment.Cr185)
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    x => new PaymentProxy
                    {
                        ChargePopulation = x.Sum(y => y.ChargePopulation),
                        PaidPopulation = x.Sum(y => y.PaidPopulation),
                    });

            var paymentsHiring = payments
                .Where(x => x.TypePayment == TypePayment.HireRegFund)
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    x => new PaymentProxy
                    {
                        ChargePopulation = x.Sum(y => y.ChargePopulation),
                        PaidPopulation = x.Sum(y => y.PaidPopulation),
                    });
            
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("Municipality");
            var sectionMo = sectionMu.ДобавитьСекцию("ManOrg");
            var sectionRo = sectionMo.ДобавитьСекцию("RealtyObject");

            foreach (var municipality in munDictionary)
            {
                if (!realtyObjMoMuDict.ContainsKey(municipality.Key))
                {
                    continue;
                }

                sectionMu.ДобавитьСтроку();

                // итоги по МунОбразованию
                var municipalTotals = this.GetTotalsDictionary();

                var i = 0;

                var dataByMu = realtyObjMoMuDict[municipality.Key].OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                
                // для вывода "домов без УО" в самом конце (по каждому Мун.Образованию) переносим их в конец словаря
                if (dataByMu.First().Key == string.Empty)
                {
                    var reltyObjWithoutMo = dataByMu.First();
                    dataByMu = dataByMu.Skip(1).ToDictionary(x => x.Key, x => x.Value);
                    dataByMu[string.Empty] = reltyObjWithoutMo.Value;
                }

                foreach (var dataByMo in dataByMu)
                {
                    sectionMo.ДобавитьСтроку();

                    var j = 0;

                    // итоги по УК
                    var manOrgTotals = this.GetTotalsDictionary();
                    foreach (var dataByRo in dataByMo.Value.OrderBy(x => x.Address))
                    {
                        ++i;
                        ++j;
                        sectionRo.ДобавитьСтроку();

                        sectionRo["NumberMu"] = i;
                        sectionRo["NumberMo"] = j;
                        sectionRo["Municipality"] = municipality.Value;
                        sectionRo["Address"] = dataByRo.Address;
                        sectionRo["RoType"] = dataByRo.TypeHouse.GetEnumMeta().Display;
                        sectionRo["RoCondition"] = dataByRo.ConditionHouse.GetEnumMeta().Display;
                        sectionRo["SocialCredit"] = dataByRo.IsBuildSocialMortgage.GetEnumMeta().Display;
                        sectionRo["StartExplotationDate"] = dataByRo.DateCommissioning.HasValue ? dataByRo.DateCommissioning.Value.Year.ToStr() : string.Empty;
                        sectionRo["LiveArea"] = dataByRo.AreaLiving;
                        sectionRo["TypeContract"] = dataByRo.TypeContractManOrgRealObj != 0 ? dataByRo.TypeContractManOrgRealObj.GetEnumMeta().Display : string.Empty;
                        sectionRo["MoName"] = dataByRo.ManOrgName;
                        sectionRo["MoType"] = dataByRo.TypeManagementManOrg != 0 ? dataByRo.TypeManagementManOrg.GetEnumMeta().Display : string.Empty;

                        var paymentCr = paymentsCr.ContainsKey(dataByRo.RoId) ? paymentsCr[dataByRo.RoId] : null;
                        var paymentHiring = paymentsHiring.ContainsKey(dataByRo.RoId) ? paymentsHiring[dataByRo.RoId] : null;
                        var contractRf = contractRfDict.ContainsKey(dataByRo.RoId) ? contractRfDict[dataByRo.RoId] : null;

                        this.FillPaymentsAndContractData(sectionRo, manOrgTotals, paymentCr, paymentHiring, contractRf);

                        manOrgTotals["LiveArea"] += dataByRo.AreaLiving.ToDecimal();
                    }

                    // заполнение итогов по УК
                    sectionMo["MoNmae"] = dataByMo.Value.Select(x => x.ManOrgName).FirstOrDefault();
                    this.FillTotals(j, municipality.Value, sectionMo, manOrgTotals);

                    this.AttachDictionaries(municipalTotals, manOrgTotals);
                }

                // заполнение итогов по МунОбразованию
                this.FillTotals(i, municipality.Value, sectionMu, municipalTotals);
            }
        }

        private Dictionary<long, Dictionary<string, List<RealtyObjManOrgMunicipality>>> GetManOrgRealtyObjectsDict(DateTime periodEndDate)
        {
            var serviceRealityObject = this.Container.Resolve<IDomainService<RealityObject>>();
            var serviceManOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var serviceManOrgContractRelation = this.Container.Resolve<IDomainService<ManOrgContractRelation>>();

            var managingOrgRealityObjectQuery = serviceManOrgContractRealityObject.GetAll()
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= periodEndDate)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= periodEndDate)
                    .Where(x => x.ManOrgContract.ManagingOrganization != null);
            
            // запрос идентификаторов договоров с УО
            var contractWithManOrgIdsQuery = managingOrgRealityObjectQuery.Select(x => x.ManOrgContract.Id);

            // запрос идентификаторов договоров, передавших управление
            var parentContractIdsQuery = serviceManOrgContractRelation.GetAll()
                .Where(x => contractWithManOrgIdsQuery.Contains(x.Parent.Id))
                .Where(x => contractWithManOrgIdsQuery.Contains(x.Children.Id))
                .Where(x => x.TypeRelation == TypeContractRelation.TransferTsjUk)
                .Select(x => x.Parent.Id);

            // дома УО (исключая ТСЖ, передавшие управление)
            var managingOrgRealityObject = managingOrgRealityObjectQuery
                .Where(x => !parentContractIdsQuery.Contains(x.ManOrgContract.Id))
                .Select(x => new RealtyObjManOrgMunicipality
                {
                    RoId = x.RealityObject.Id,
                    MuId = x.RealityObject.Municipality.Id,
                    MoId = x.ManOrgContract.ManagingOrganization.Id,
                    Address = x.RealityObject.Address,
                    AreaLiving = x.RealityObject.AreaLiving,
                    ConditionHouse = x.RealityObject.ConditionHouse,
                    DateCommissioning = x.RealityObject.DateCommissioning,
                    IsBuildSocialMortgage = x.RealityObject.IsBuildSocialMortgage,
                    TypeHouse = x.RealityObject.TypeHouse,
                    TypeContractManOrgRealObj = x.ManOrgContract.TypeContractManOrgRealObj,
                    ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    TypeManagementManOrg = x.ManOrgContract.ManagingOrganization.TypeManagement
                })
                .ToList();

            // запрос идентификаторов домов, имеющих УК
            var realtyObjWithManOrgIdsQuery = managingOrgRealityObjectQuery
                .Where(x => !parentContractIdsQuery.Contains(x.ManOrgContract.Id))
                .Select(x => x.RealityObject.Id);

            // Дома без УК
            var realityObjWithoutMo = serviceRealityObject.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Municipality.Id))
                .Where(x => !realtyObjWithManOrgIdsQuery.Contains(x.Id))
                .Select(x => new RealtyObjManOrgMunicipality
                {
                    RoId = x.Id,
                    MuId = x.Municipality.Id,
                    Address = x.Address,
                    AreaLiving = x.AreaLiving,
                    ConditionHouse = x.ConditionHouse,
                    DateCommissioning = x.DateCommissioning,
                    IsBuildSocialMortgage = x.IsBuildSocialMortgage,
                    TypeHouse = x.TypeHouse,
                    MoId = -1,
                    ManOrgName = string.Empty,
                })
                .ToList();

            var realtyObjManOrgList = managingOrgRealityObject.Union(realityObjWithoutMo).Distinct().ToList();

            return realtyObjManOrgList
                .GroupBy(x => x.MuId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.ManOrgName).ToDictionary(y => y.Key, y => y.ToList()));
        }

        private Dictionary<long, ContractRfObjectProxy> GetContractRfDict(DateTime periodStartDate, DateTime periodEndDate)
        {
            return this.Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => periodEndDate >= x.ContractRf.DateBegin || x.ContractRf.DateBegin == null)
                .Where(x => periodStartDate <= x.ContractRf.DateEnd || x.ContractRf.DateEnd == null)
                .Select(x => new { roId = x.RealityObject.Id, x.ContractRf.DocumentNum, x.TypeCondition, x.IncludeDate, x.ExcludeDate, x.ContractRf.DocumentDate })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    y => y.OrderBy(x => x.DocumentDate.HasValue ? (periodEndDate - x.DocumentDate.Value).TotalDays : int.MaxValue)
                            .ThenBy(x => x.ExcludeDate)
                            .Select(x => new ContractRfObjectProxy
                            {
                                DocumentNum = x.DocumentNum,
                                TypeCondition = x.TypeCondition,
                                IncludeDate = x.IncludeDate
                            })
                            .FirstOrDefault());
        }

        private List<ReltyObjectPaymentProxy> GetPayments()
        {
            return this.Container.Resolve<IDomainService<PaymentItem>>().GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Payment.RealityObject.Municipality.Id))
                .Where(x => x.TypePayment == TypePayment.Cr || x.TypePayment == TypePayment.Cr185 || x.TypePayment == TypePayment.HireRegFund)
                .Where(x => x.ChargeDate.HasValue && x.ChargeDate >= this.startDate && x.ChargeDate <= this.endDate)
                .Where(x => x.ManagingOrganization != null)
                .Select(x => new ReltyObjectPaymentProxy
                {
                    RoId = x.Payment.RealityObject.Id,
                    ChargePopulation = x.ChargePopulation,
                    PaidPopulation = x.PaidPopulation,
                    TypePayment = x.TypePayment
                })
                .ToList();
        }

        private Dictionary<string, decimal> GetTotalsDictionary()
        {
            var resultDictionary = new Dictionary<string, decimal>();
            resultDictionary["LiveArea"] = 0M;
            resultDictionary["PapulationCalculation"] = 0M;
            resultDictionary["PapulationPayment"] = 0M;
            resultDictionary["HiringPapulationCalculation"] = 0M;
            resultDictionary["HiringPapulationPayment"] = 0M;
            return resultDictionary;
        }

        private void AttachDictionaries(Dictionary<string, decimal> totals, Dictionary<string, decimal> addable)
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

        private void FillPaymentsAndContractData(Section section, Dictionary<string, decimal> totals, PaymentProxy paymentsCr, PaymentProxy paymentsHiring, ContractRfObjectProxy contractRfObject)
        {
            if (contractRfObject != null)
            {
                switch (contractRfObject.TypeCondition)
                {
                    case TypeCondition.Include:
                        section["GisuContract"] = "включен в договор";
                        section["ContractDate"] = contractRfObject.IncludeDate != null ? contractRfObject.IncludeDate.ToDateTime().ToShortDateString() : string.Empty;
                        if (contractRfObject.IncludeDate != null)
                        {
                            var includeDate = contractRfObject.IncludeDate.Value;
                            includeDate = includeDate.Day != 1 ? includeDate.AddDays((DateTime.DaysInMonth(includeDate.Year, includeDate.Month) - includeDate.Day) + 1) : includeDate;

                            var monthsAfterContractDate = 12 * (this.endDate.Year - includeDate.Year) + (this.endDate.Month - includeDate.Month);
                            section["MonthsAfterContractDate"] = monthsAfterContractDate <= 0 ? string.Empty : monthsAfterContractDate.ToStr();
                        }

                        break;
                    case TypeCondition.Exclude:
                        section["GisuContract"] = "исключен из договора";
                        break;
                }

                section["ContractNumber"] = contractRfObject.DocumentNum;
            }
            else
            {
                section["GisuContract"] = "отсутствует";
            }


            if (paymentsCr != null)
            {
                section["PapulationCalculation"] = paymentsCr.ChargePopulation;
                totals["PapulationCalculation"] += paymentsCr.ChargePopulation.ToDecimal();

                section["PapulationPayment"] = paymentsCr.PaidPopulation;
                totals["PapulationPayment"] += paymentsCr.PaidPopulation.ToDecimal();
            }

            if (paymentsHiring != null)
            {
                section["HiringPapulationCalculation"] = paymentsHiring.ChargePopulation;
                totals["HiringPapulationCalculation"] += paymentsHiring.ChargePopulation.ToDecimal();

                section["HiringPapulationPayment"] = paymentsHiring.PaidPopulation;
                totals["HiringPapulationPayment"] += paymentsHiring.PaidPopulation.ToDecimal();
            }
        }

        private void FillTotals(int count, string muName, Section section, Dictionary<string, decimal> totals)
        {
            section["Count"] = count;
            section["Municipality"] = muName;
            section["LiveArea"] = totals["LiveArea"];
            section["PapulationCalculation"] = totals["PapulationCalculation"];
            section["PapulationPayment"] = totals["PapulationPayment"];
            section["HiringPapulationCalculation"] = totals["HiringPapulationCalculation"];
            section["HiringPapulationPayment"] = totals["HiringPapulationPayment"];
        }

        private sealed class RealtyObjManOrgMunicipality : IEquatable<RealtyObjManOrgMunicipality>
        {
            public long RoId { get; set; }

            public long MoId { get; set; }

            public long MuId { get; set; }

            public TypeContractManOrg TypeContractManOrgRealObj { get; set; }

            public string ManOrgName { get; set; }

            public TypeManagementManOrg TypeManagementManOrg { get; set; }

            public DateTime? DateCommissioning { get; set; }

            public decimal? AreaLiving { get; set; }

            public TypeHouse TypeHouse { get; set; }

            public YesNo IsBuildSocialMortgage { get; set; }

            public ConditionHouse ConditionHouse { get; set; }

            public string Address { get; set; }

            public bool Equals(RealtyObjManOrgMunicipality other)
            {
                if (Object.ReferenceEquals(other, null)) { return false; }
                if (Object.ReferenceEquals(this, other)) { return true; }

                var res = this.RoId == other.RoId
                       && this.MoId == other.MoId
                       && this.MuId == other.MuId
                       && this.TypeContractManOrgRealObj == other.TypeContractManOrgRealObj
                       && this.ManOrgName == other.ManOrgName
                       && this.TypeManagementManOrg == other.TypeManagementManOrg
                       && this.DateCommissioning == other.DateCommissioning
                       && this.AreaLiving == other.AreaLiving
                       && this.TypeHouse == other.TypeHouse
                       && this.IsBuildSocialMortgage == other.IsBuildSocialMortgage
                       && this.ConditionHouse == other.ConditionHouse
                       && this.Address == other.Address;

                return res;
            }

            public override int GetHashCode()
            {
                int hashRoId = this.RoId.GetHashCode();
                int hashMoId = this.MoId.GetHashCode();
                int hashMuId = this.MuId.GetHashCode();
                int hashTypeContractManOrgRealObj = this.TypeContractManOrgRealObj.GetHashCode();
                int hashTypeManagementManOrg = this.TypeManagementManOrg.GetHashCode();
                int hashDateCommissioning = this.DateCommissioning == null ? 0 : this.DateCommissioning.GetHashCode();
                int hashAreaLiving = this.AreaLiving == null ? 0 : this.AreaLiving.GetHashCode();
                int hashTypeHouse = this.TypeHouse.GetHashCode();
                int hashIsBuildSocialMortgage = this.IsBuildSocialMortgage.GetHashCode();
                int hashConditionHouse = this.ConditionHouse.GetHashCode();
                int hashAddress = this.Address == null ? 0 : this.Address.GetHashCode();

                return hashRoId ^ hashMoId ^ hashMuId ^ hashTypeContractManOrgRealObj ^ hashTypeManagementManOrg
                       ^ hashDateCommissioning ^ hashAreaLiving ^ hashTypeHouse ^ hashIsBuildSocialMortgage
                       ^ hashConditionHouse ^ hashAddress;
            }
        }

        private sealed class PaymentProxy
        {
            public decimal? ChargePopulation { get; set; }

            public decimal? PaidPopulation { get; set; }
        }

        private sealed class ReltyObjectPaymentProxy
        {
            public long RoId { get; set; }

            public decimal? ChargePopulation { get; set; }

            public decimal? PaidPopulation { get; set; }

            public TypePayment TypePayment { get; set; }
        }

        private sealed class ContractRfObjectProxy
        {
            public string DocumentNum { get; set; }

            public TypeCondition TypeCondition { get; set; }

            public DateTime? IncludeDate { get; set; }
        }
    }
}