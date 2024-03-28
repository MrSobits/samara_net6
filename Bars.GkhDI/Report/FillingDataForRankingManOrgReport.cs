namespace Bars.GkhDi.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    using NHibernate;

    /// <summary>
    /// Заполнение данных для рейтинга УК в разделе Деятельность УК
    /// </summary>
    public class FillingDataForRankingManOrgReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalityIdsList = new List<long>();

        private long periodDiId;

        public FillingDataForRankingManOrgReport()
            : base(new ReportTemplateBinary(Properties.Resources.FillingDataForRankingManOrg))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.Di.FillingDataForRankingManOrg";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Заполнение данных для рейтинга УК в разделе Деятельность УК";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Раскрытие информации";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.FillingDataForRankingManOrg";
            }
        }

        public override string Name
        {
            get
            {
                return "Заполнение данных для рейтинга УК в разделе Деятельность УК";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            periodDiId = baseParams.Params.GetAs<long>("periodDi");

            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIdsList = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        protected ISession OpenSession()
        {
            return Container.Resolve<ISessionProvider>().GetCurrentSession();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Where(x => this.municipalityIdsList.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Id, x => x.Name);

            var DisclosureInfoQuery = this.Container.Resolve<IDomainService<DisclosureInfo>>()
                .GetAll()
                .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.ManagingOrganization.Contragent.Municipality.Id))
                .Where(x => x.PeriodDi.Id == this.periodDiId)
                .Where(x => x.ManagingOrganization != null);

            var managingOrgWorkModeQuery = this.Container.Resolve<IDomainService<ManagingOrgWorkMode>>().GetAll().Where(x => x.StartDate != null);
            var fileProjectContractQuery = this.Container.Resolve<IDomainService<Documents>>().GetAll().Where(x => x.FileProjectContract != null);
            var finActivityManagRealityObjQuery = this.Container.Resolve<IDomainService<FinActivityManagRealityObj>>().GetAll();
            var finActivityRepairCategoryQuery = this.Container.Resolve<IDomainService<FinActivityRepairCategory>>().GetAll();
            var directorQuery = this.Container.Resolve<IDomainService<ContragentContact>>().GetAll().Where(x => x.Position != null && x.Position.Code == "1");
            var finActivityDocsQuery = this.Container.Resolve<IDomainService<FinActivityDocs>>().GetAll().Where(x => x.BookkepingBalance != null);
            var finActivityQuery = this.Container.Resolve<IDomainService<FinActivity>>().GetAll().Where(x => x.ClaimDamage != null || x.FailureService != null || x.NonDeliveryService != null);
            var finActivityManagCategoryQuery = this.Container.Resolve<IDomainService<FinActivityManagCategory>>().GetAll()
                .Where(x => x.DebtPopulationEnd != null
                    || x.DebtPopulationStart != null
                    || x.ExactPopulation != null
                    || x.ExpenseManaging != null
                    || x.IncomeManaging != null
                    || x.IncomeUsingGeneralProperty != null);
            var finActivityFinActivityCommunalServiceQuery = this.Container.Resolve<IDomainService<FinActivityCommunalService>>().GetAll()
                .Where(x => x.Exact != null
                    || x.IncomeFromProviding != null
                    || x.DebtPopulationStart != null
                    || x.DebtPopulationEnd != null
                    || x.DebtManOrgCommunalService != null
                    || x.PaidByMeteringDevice != null
                    || x.PaidByGeneralNeeds != null
                    || x.PaymentByClaim != null);
            var finActivityRepairSourceQuery = this.Container.Resolve<IDomainService<FinActivityRepairSource>>().GetAll().Where(x => x.Sum != null);
            var adminRespQuery = this.Container.Resolve<IDomainService<AdminResp>>().GetAll();
            var membershipUnionsQuery = this.Container.Resolve<IDomainService<ManagingOrgMembership>>().GetAll();

            var serviceManOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var period = this.Container.Resolve<IDomainService<PeriodDi>>().GetAll().FirstOrDefault(x => x.Id == this.periodDiId);

            var startDate = period.DateStart;
            var endDate = period.DateEnd;

            var terminateContractQuery = serviceManOrgContractRealityObject.GetAll()
                .Where(
                    x =>
                        x.ManOrgContract.EndDate >= startDate.Value.AddYears(-1) &&
                        x.ManOrgContract.EndDate < endDate.Value.AddYears(-1));

            var info = DisclosureInfoQuery
                .Select(x => new DiInfoProxy
                {
                    diInfoId = x.Id,
                    municipality = x.ManagingOrganization.Contragent != null && x.ManagingOrganization.Contragent.Municipality != null ? x.ManagingOrganization.Contragent.Municipality.Id : -1,
                    manOrgName = x.ManagingOrganization.Contragent != null ? x.ManagingOrganization.Contragent.Name : string.Empty,
                    manOrgId = x.ManagingOrganization.Id,
                    director = x.ManagingOrganization.Contragent != null && directorQuery.Any(y => y.Contragent.Id == x.ManagingOrganization.Contragent.Id),
                    jurAddress = x.ManagingOrganization.Contragent != null && x.ManagingOrganization.Contragent.FiasJuridicalAddress != null,
                    factAddress = x.ManagingOrganization.Contragent != null && x.ManagingOrganization.Contragent.FiasFactAddress != null,
                    workMode = managingOrgWorkModeQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id),
                    workersCount = x.ManagingOrganization.NumberEmployees > 0,
                    fileProjectContract = fileProjectContractQuery.Any(y => y.DisclosureInfo.Id == x.Id),
                    adminResponsibility = x.AdminResponsibility == YesNoNotSet.No || adminRespQuery.Any(y => y.DisclosureInfo.Id == x.Id),
                    membershipUnions = x.MembershipUnions == YesNoNotSet.No || membershipUnionsQuery
                                                                               .Where(y => (y.DateStart.Value >= x.PeriodDi.DateStart.Value && y.DateEnd.Value <= x.PeriodDi.DateEnd.Value)
                                                                                            || (x.PeriodDi.DateStart.Value >= y.DateStart.Value
                                                                                                && ((y.DateEnd.HasValue && y.DateEnd.Value >= x.PeriodDi.DateStart.Value) || !y.DateEnd.HasValue)))
                                                                               .Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id),
                    finActivityManagRealityObj = finActivityManagRealityObjQuery.Any(y => y.DisclosureInfo.Id == x.Id),
                    terminateContract = x.TerminateContract == YesNoNotSet.No || terminateContractQuery.Any(y => y.ManOrgContract.ManagingOrganization.Id == x.ManagingOrganization.Id),
                    finActivityRepairCategory = finActivityRepairCategoryQuery.Where(y => y.PeriodService != null).Any(y => y.DisclosureInfo.Id == x.Id),
                    finStatements = finActivityDocsQuery.Any(y => y.DisclosureInfo.Id == x.Id),
                    claimsPayment = finActivityQuery.Any(y => y.DisclosureInfo.Id == x.Id),
                    managementByCategory = finActivityManagCategoryQuery.Any(y => y.DisclosureInfo.Id == x.Id),
                    publicService = finActivityFinActivityCommunalServiceQuery.Any(y => y.DisclosureInfo.Id == x.Id),
                    houseRepair = finActivityRepairCategoryQuery.Where(y => y.WorkByRepair != null || y.WorkByBeautification != null).Any(y => y.DisclosureInfo.Id == x.Id),
                    houseRepairSum = finActivityRepairSourceQuery.Any(y => y.DisclosureInfo.Id == x.Id),
                })
                .AsEnumerable()
                .GroupBy(x => x.municipality)
                .ToDictionary(x => x.Key, x => x.ToList());



            // Список домов в управлении УК в заданном периоде
            var manOrgRobjectsDict = serviceManOrgContractRealityObject.GetAll()
                .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= endDate)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= startDate)
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Select(x => new
                                    {
                                        manOrgId = x.ManOrgContract.ManagingOrganization.Id,
                                        robjectInfo = new
                                                        {
                                                            robjectId = x.RealityObject.Id,
                                                            address = x.RealityObject.Address
                                                        }
                                    })
                .AsEnumerable()
                .GroupBy(x => x.manOrgId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.robjectInfo).Distinct().ToList());

            var manageMkdQuery = finActivityManagRealityObjQuery
                .Where(x => x.PresentedToRepay.HasValue
                    || x.ReceivedProvidedService.HasValue
                    || x.SumDebt.HasValue
                    || x.SumFactExpense.HasValue
                    || x.SumIncomeManage.HasValue);

            var finActivityRobjCommunalServiceQuery = this.Container.Resolve<IDomainService<FinActivityRealityObjCommunalService>>().GetAll()
                .Where(x => x.PaidOwner.HasValue
                    || x.DebtOwner.HasValue
                    || x.PaidByIndicator.HasValue
                    || x.PaidByAccount.HasValue);

            var serviceProviderQuery = this.Container.Resolve<IDomainService<BaseService>>().GetAll();
            var serviceProfitQuery = this.Container.Resolve<IDomainService<BaseService>>().GetAll();

            var roInfo = Container.Resolve<IDomainService<DisclosureInfoRelation>>().GetAll()
                 .Where(x => DisclosureInfoQuery.Select(y => y.Id).Contains(x.DisclosureInfo.Id))
                 .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == this.periodDiId)
                 .Select(x => new DiRealtyObjectProxy
                               {
                                   disclosureInfoId = x.DisclosureInfo.Id,
                                   realtyObjectId = x.DisclosureInfoRealityObj.RealityObject.Id,
                                   diRealObjId = x.DisclosureInfoRealityObj.Id,
                                   paymentOfClaims = x.DisclosureInfoRealityObj.ClaimCompensationDamage.HasValue
                                                   || x.DisclosureInfoRealityObj.ClaimReductionPaymentNonDelivery.HasValue
                                                   || x.DisclosureInfoRealityObj.ClaimReductionPaymentNonService.HasValue,
                                   mkdManagement = manageMkdQuery.Any(y => y.DisclosureInfo.Id == x.DisclosureInfo.Id && y.RealityObject.Id == x.DisclosureInfoRealityObj.RealityObject.Id),
                                   finActivityCommunalService = finActivityRobjCommunalServiceQuery.Any(y => y.DisclosureInfoRealityObj.Id == x.DisclosureInfoRealityObj.Id),
                                   finActivityRepair = x.DisclosureInfoRealityObj.WorkRepair.HasValue
                                                   || x.DisclosureInfoRealityObj.WorkLandscaping.HasValue
                                                   || x.DisclosureInfoRealityObj.Subsidies.HasValue
                                                   || x.DisclosureInfoRealityObj.Credit.HasValue
                                                   || x.DisclosureInfoRealityObj.FinanceLeasingContract.HasValue
                                                   || x.DisclosureInfoRealityObj.FinanceEnergServContract.HasValue
                                                   || x.DisclosureInfoRealityObj.OccupantContribution.HasValue
                                                   || x.DisclosureInfoRealityObj.OtherSource.HasValue,
                                   service17Provider = serviceProviderQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "17"),
                                   service17Profit = serviceProfitQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "17"),
                                   service18Provider = serviceProviderQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "18"),
                                   service18Profit = serviceProfitQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "18"),
                                   service19Provider = serviceProviderQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "19"),
                                   service19Profit = serviceProfitQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "19"),
                                   service20Provider = serviceProviderQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "20"),
                                   service20Profit = serviceProfitQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "20"),
                                   service21Provider = serviceProviderQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "21"),
                                   service21Profit = serviceProfitQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "21"),
                                   service22Provider = serviceProviderQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "22"),
                                   service22Profit = serviceProfitQuery.Any(y => y.DisclosureInfoRealityObj == x.DisclosureInfoRealityObj && y.TemplateService.Code == "22")
                               })
                 .AsEnumerable()
                 .GroupBy(x => x.disclosureInfoId)
                 .ToDictionary(
                       x => x.Key,
                       x => x.GroupBy(y => y.realtyObjectId)
                             .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionMo = sectionMu.ДобавитьСекцию("sectionMo");
            var sectionRo = sectionMo.ДобавитьСекцию("sectionRo");

            int counter = 0;

            foreach (var municipality in municipalityDict)
            {
                if (!info.ContainsKey(municipality.Key))
                {
                    continue;
                }

                sectionMu.ДобавитьСтроку();
                sectionMu["MuName"] = municipality.Value;

                var muData = info[municipality.Key];
                var percentRoByMu = 0M;
                var houseCountByMu = 0;
                var percentMoList = new List<decimal>();

                foreach (var diInfo in muData)
                {
                    sectionMo.ДобавитьСтроку();
                    sectionMo["MoName"] = diInfo.manOrgName;

                    var realtyObjects = manOrgRobjectsDict.ContainsKey(diInfo.manOrgId)
                                            ? manOrgRobjectsDict[diInfo.manOrgId]
                                            : null;

                    var diRoInfoDict = roInfo.ContainsKey(diInfo.diInfoId) ? roInfo[diInfo.diInfoId] : null;

                    int infoCountByMo;
                    var moDiPercent = this.GetPercentByMo(diInfo, out infoCountByMo);
                    percentMoList.Add(moDiPercent);

                    if (realtyObjects != null)
                    {
                        var percentByMo = 0M;
                        foreach (var realtyObject in realtyObjects)
                        {
                            var diRo = diRoInfoDict != null && diRoInfoDict.ContainsKey(realtyObject.robjectId)
                                           ? diRoInfoDict[realtyObject.robjectId]
                                           : null;
                            var roDiPercent = this.GetPercentByRo(diRo, infoCountByMo);

                            percentByMo += roDiPercent;

                            this.FillRow(sectionRo, diInfo, diRo, ++counter, moDiPercent, roDiPercent, municipality.Value, realtyObject.address);
                        }

                        percentRoByMu += percentByMo;
                        houseCountByMu += realtyObjects.Count;
                        sectionMo["percentRoTotalMo"] = realtyObjects.Count != 0 ? (percentByMo / realtyObjects.Count) / 100 : 0;
                    }
                    else
                    {
                        this.FillRow(sectionRo, diInfo, null, ++counter, moDiPercent, 0, municipality.Value, string.Empty);
                        sectionMo["percentRoTotalMo"] = 0;
                    }

                    sectionMo["percentMoTotalMo"] = moDiPercent / 100;
                }

                var percentMoTotal = percentMoList.Average();
                sectionMu["percentMoTotal"] = percentMoTotal / 100;
                sectionMu["percentRoTotal"] = houseCountByMu != 0 ? (percentRoByMu / houseCountByMu) / 100 : 0;
            }
        }

        private void FillRow(Section sectionRo, DiInfoProxy diInfoProxy, DiRealtyObjectProxy diRealtyObjectProxy, int num, decimal? percentMo, decimal percentRo, string muName, string address)
        {
            sectionRo.ДобавитьСтроку();

            sectionRo["num"] = num;
            sectionRo["MuName"] = muName;
            sectionRo["manOrgName"] = diInfoProxy.manOrgName;
            sectionRo["director"] = diInfoProxy.director ? 1 : 0;
            sectionRo["jurAddress"] = diInfoProxy.jurAddress ? 1 : 0;
            sectionRo["factAddress"] = diInfoProxy.factAddress ? 1 : 0;
            sectionRo["workMode"] = diInfoProxy.workMode ? 1 : 0;
            sectionRo["workersCount"] = diInfoProxy.workersCount ? 1 : 0;
            sectionRo["fileProjectContract"] = diInfoProxy.fileProjectContract ? 1 : 0;
            sectionRo["adminResponsibility"] = diInfoProxy.adminResponsibility ? 1 : 0;
            sectionRo["membershipUnions"] = diInfoProxy.membershipUnions ? 1 : 0;
            sectionRo["finActivityManagRealityObj"] = diInfoProxy.finActivityManagRealityObj ? 1 : 0;
            sectionRo["terminateContract"] = diInfoProxy.terminateContract ? 1 : 0;
            sectionRo["finActivityRepairCategory"] = diInfoProxy.finActivityRepairCategory ? 1 : 0;
            sectionRo["finStatements"] = diInfoProxy.finStatements ? 1 : 0;
            sectionRo["claimsPayment"] = diInfoProxy.claimsPayment ? 1 : 0;
            sectionRo["managementByCategory"] = diInfoProxy.managementByCategory ? 1 : 0;
            sectionRo["publicService"] = diInfoProxy.publicService ? 1 : 0;
            sectionRo["houseRepair"] = diInfoProxy.houseRepair ? 1 : 0;
            sectionRo["houseRepairSum"] = diInfoProxy.houseRepairSum ? 1 : 0;
            sectionRo["percentMo"] = (percentMo ?? 0) / 100;
            sectionRo["address"] = address;

            if (diRealtyObjectProxy != null)
            {
                sectionRo["paymentOfClaims"] = diRealtyObjectProxy.paymentOfClaims ? 1 : 0;
                sectionRo["mkdManagement"] = diRealtyObjectProxy.mkdManagement ? 1 : 0;
                sectionRo["finActivityCommunalService"] = diRealtyObjectProxy.finActivityCommunalService ? 1 : 0;
                sectionRo["finActivityRepair"] = diRealtyObjectProxy.finActivityRepair ? 1 : 0;
                sectionRo["service17Provider"] = diRealtyObjectProxy.service17Provider ? 1 : 0;
                sectionRo["service17Profit"] = diRealtyObjectProxy.service17Profit ? 1 : 0;
                sectionRo["service18Provider"] = diRealtyObjectProxy.service18Provider ? 1 : 0;
                sectionRo["service18Profit"] = diRealtyObjectProxy.service18Profit ? 1 : 0;
                sectionRo["service19Provider"] = diRealtyObjectProxy.service19Provider ? 1 : 0;
                sectionRo["service19Profit"] = diRealtyObjectProxy.service19Profit ? 1 : 0;
                sectionRo["service20Provider"] = diRealtyObjectProxy.service20Provider ? 1 : 0;
                sectionRo["service20Profit"] = diRealtyObjectProxy.service20Profit ? 1 : 0;
                sectionRo["service21Provider"] = diRealtyObjectProxy.service21Provider ? 1 : 0;
                sectionRo["service21Profit"] = diRealtyObjectProxy.service21Profit ? 1 : 0;
                sectionRo["service22Provider"] = diRealtyObjectProxy.service22Provider ? 1 : 0;
                sectionRo["service22Profit"] = diRealtyObjectProxy.service22Profit ? 1 : 0;
                sectionRo["percentRo"] = percentRo / 100;
            }
            else
            {
                sectionRo["paymentOfClaims"] = 0;
                sectionRo["mkdManagement"] = 0;
                sectionRo["finActivityCommunalService"] = 0;
                sectionRo["finActivityRepair"] = 0;
                sectionRo["service17Provider"] = 0;
                sectionRo["service17Profit"] = 0;
                sectionRo["service18Provider"] = 0;
                sectionRo["service18Profit"] = 0;
                sectionRo["service19Provider"] = 0;
                sectionRo["service19Profit"] = 0;
                sectionRo["service20Provider"] = 0;
                sectionRo["service20Profit"] = 0;
                sectionRo["service21Provider"] = 0;
                sectionRo["service21Profit"] = 0;
                sectionRo["service22Provider"] = 0;
                sectionRo["service22Profit"] = 0;
                sectionRo["percentRo"] = percentRo / 100;
            }
        }

        private decimal GetPercentByMo(DiInfoProxy diInfoProxy, out int count)
        {
            int trueCount = 0;
            if (diInfoProxy.director) ++trueCount;
            if (diInfoProxy.jurAddress) ++trueCount;
            if (diInfoProxy.factAddress) ++trueCount;
            if (diInfoProxy.workMode) ++trueCount;
            if (diInfoProxy.workersCount) ++trueCount;
            if (diInfoProxy.fileProjectContract) ++trueCount;
            if (diInfoProxy.adminResponsibility) ++trueCount;
            if (diInfoProxy.membershipUnions) ++trueCount;
            if (diInfoProxy.finActivityManagRealityObj) ++trueCount;
            if (diInfoProxy.terminateContract) ++trueCount;
            if (diInfoProxy.finActivityRepairCategory) ++trueCount;
            if (diInfoProxy.finStatements) ++trueCount;
            if (diInfoProxy.claimsPayment) ++trueCount;
            if (diInfoProxy.managementByCategory) ++trueCount;
            if (diInfoProxy.publicService) ++trueCount;
            if (diInfoProxy.houseRepair) ++trueCount;
            if (diInfoProxy.houseRepairSum) ++trueCount;

            count = trueCount;

            return (trueCount / 17M) * 100;
        }

        private decimal GetPercentByRo(DiRealtyObjectProxy diRealtyObject, int countByMo)
        {
            int trueCount = 0;
            if (diRealtyObject != null)
            {
                if (diRealtyObject.paymentOfClaims) ++trueCount;
                if (diRealtyObject.mkdManagement) ++trueCount;
                if (diRealtyObject.finActivityCommunalService) ++trueCount;
                if (diRealtyObject.finActivityRepair) ++trueCount;
                if (diRealtyObject.service17Provider) ++trueCount;
                if (diRealtyObject.service17Profit) ++trueCount;
                if (diRealtyObject.service18Provider) ++trueCount;
                if (diRealtyObject.service18Profit) ++trueCount;
                if (diRealtyObject.service19Provider) ++trueCount;
                if (diRealtyObject.service19Profit) ++trueCount;
                if (diRealtyObject.service20Provider) ++trueCount;
                if (diRealtyObject.service20Profit) ++trueCount;
                if (diRealtyObject.service21Provider) ++trueCount;
                if (diRealtyObject.service21Profit) ++trueCount;
                if (diRealtyObject.service22Provider) ++trueCount;
                if (diRealtyObject.service22Profit) ++trueCount;
            }
            return ((countByMo + trueCount) / 33M) * 100;
        }
    }

    internal sealed class DiRealtyObjectProxy
    {
        public long disclosureInfoId;

        public long realtyObjectId;

        public long diRealObjId;

        public bool paymentOfClaims;

        public bool mkdManagement;

        public bool finActivityCommunalService;

        public bool finActivityRepair;

        public bool service17Provider;

        public bool service17Profit;

        public bool service18Provider;

        public bool service18Profit;

        public bool service19Provider;

        public bool service19Profit;

        public bool service20Provider;

        public bool service20Profit;

        public bool service21Provider;

        public bool service21Profit;

        public bool service22Provider;

        public bool service22Profit;
    }

    internal sealed class DiInfoProxy
    {
        public long diInfoId;

        public long municipality;

        public string manOrgName;

        public long manOrgId;

        public bool director;

        public bool jurAddress;

        public bool factAddress;

        public bool workMode;

        public bool workersCount;

        public bool fileProjectContract;

        public bool adminResponsibility;

        public bool membershipUnions;

        public bool finActivityManagRealityObj;

        public bool terminateContract;

        public bool finActivityRepairCategory;

        public bool finStatements;

        public bool claimsPayment;

        public bool managementByCategory;

        public bool publicService;

        public bool houseRepair;

        public bool houseRepairSum;
    }
}