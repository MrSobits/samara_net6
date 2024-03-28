// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FillFinanceDataRatingCr.cs" company="BarsGroup">
//   BarsGroup
// </copyright>
// <summary>
//   Defines the FillFinanceDataRatingCr type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhDi.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Заполнение финансовых показателей для рейтинга УК
    /// </summary>
    public class FillFinanceDataRatingCr : BasePrintForm
    {
        #region параметры

        /// <summary>
        /// The municipality ids.
        /// </summary>
        private List<long> municipalityIds = new List<long>();

        /// <summary>
        /// The period di id.
        /// </summary>
        private long periodDiId;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FillFinanceDataRatingCr"/> class.
        /// </summary>
        public FillFinanceDataRatingCr() : base(new ReportTemplateBinary(Properties.Resources.FillFinanceDataRatingCr))
        {
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Заполнение финансовых показателей для рейтинга УК";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "Заполнение финансовых показателей для рейтинга УК";
            }
        }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Раскрытие информации о деятельности УК";
            }
        }

        /// <summary>
        /// Gets the parameters controller.
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.FillFinanceDataRatingCr";
            }
        }

        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.Di.FillFinanceDataRatingCr";
            }
        }

        /// <summary>
        /// The set user parameters.
        /// </summary>
        /// <param name="baseParams">
        /// The base parameters.
        /// </param>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.periodDiId = baseParams.Params.GetAs<long>("periodDi");

            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        /// <summary>
        /// The prepare report.
        /// </summary>
        /// <param name="reportParams">
        /// The report parameters.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var servicePeriodDi = this.Container.Resolve<IDomainService<PeriodDi>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var serviceManagingOrganization = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var serviceDisclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            var serviceFinActivityManagCategory = this.Container.Resolve<IDomainService<FinActivityManagCategory>>();
            var serviceFinActivityCommunalService = this.Container.Resolve<IDomainService<FinActivityCommunalService>>();
            var serviceFinActivity = this.Container.Resolve<IDomainService<FinActivity>>();
            var serviceFinActivityRepairCategory = this.Container.Resolve<IDomainService<FinActivityRepairCategory>>();
            var serviceFinActivityRepairSource = this.Container.Resolve<IDomainService<FinActivityRepairSource>>();
            var serviceManOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var period = servicePeriodDi.Load(this.periodDiId);

            var municipalityDict = serviceMunicipality.GetAll()
                .Where(x => this.municipalityIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Id, x => x.Name);

            var manOrgsQuery = serviceManagingOrganization.GetAll()
                .Where(x => x.ActivityDateEnd == null || x.ActivityDateEnd > period.DateStart)
                .Where(x => x.TypeManagement != TypeManagementManOrg.Other);

            var manOrgIdsQuery = manOrgsQuery.Select(x => x.Id);

            var manageCategoryIncomeManagingQuery = serviceFinActivityManagCategory.GetAll().Where(x => x.IncomeManaging.HasValue);
            var manageCategoryExpenseManagingQuery = serviceFinActivityManagCategory.GetAll().Where(x => x.ExpenseManaging.HasValue);
            var manageCategoryDebtPopulationEndQuery = serviceFinActivityManagCategory.GetAll().Where(x => x.DebtPopulationEnd.HasValue);
            var manageCategoryDebtPopulationStartQuery = serviceFinActivityManagCategory.GetAll().Where(x => x.DebtPopulationStart.HasValue);
            var manageCategoryExactPopulationQuery = serviceFinActivityManagCategory.GetAll().Where(x => x.ExactPopulation.HasValue);
            var manageCategoryIncomeUsingGeneralPropertyQuery = serviceFinActivityManagCategory.GetAll().Where(x => x.IncomeUsingGeneralProperty.HasValue);
            
            var manOrgRobjectsQuery = serviceManOrgContractRealityObject.GetAll()
                                    .Where(x => x.ManOrgContract.StartDate == null || period.DateEnd == null || x.ManOrgContract.StartDate <= period.DateEnd)
                                    .Where(x => x.ManOrgContract.EndDate == null || period.DateStart == null || x.ManOrgContract.EndDate >= period.DateStart);

            var communalServiceIncomeFromProvidingQuery = serviceFinActivityCommunalService.GetAll().Where(x => x.IncomeFromProviding.HasValue);
            var communalServiceDebtPopulationEndQuery = serviceFinActivityCommunalService.GetAll().Where(x => x.DebtPopulationEnd.HasValue);
            var communalServiceDebtPopulationStartQuery = serviceFinActivityCommunalService.GetAll().Where(x => x.DebtPopulationStart.HasValue);
            var communalServiceDebtManOrgCommunalServiceQuery = serviceFinActivityCommunalService.GetAll().Where(x => x.DebtManOrgCommunalService.HasValue);
            var communalServiceExactQuery = serviceFinActivityCommunalService.GetAll().Where(x => x.Exact.HasValue);
            var communalServicePaidByMeteringDeviceQuery = serviceFinActivityCommunalService.GetAll().Where(x => x.PaidByMeteringDevice.HasValue);
            var communalServicePaidByGeneralNeedsQuery = serviceFinActivityCommunalService.GetAll().Where(x => x.PaidByGeneralNeeds.HasValue);
            var communalServicePaymentByClaimQuery = serviceFinActivityCommunalService.GetAll().Where(x => x.PaymentByClaim.HasValue);

            var communalServiceWorkByRepairQuery = serviceFinActivityRepairCategory.GetAll().Where(x => x.WorkByRepair.HasValue);
            var communalServiceWorkByBeautificationQuery = serviceFinActivityRepairCategory.GetAll().Where(x => x.WorkByBeautification.HasValue);
            var communalServicePeriodServiceQuery = serviceFinActivityRepairCategory.GetAll().Where(x => x.PeriodService.HasValue);
            var communalServiceSourceSumQuery = serviceFinActivityRepairSource.GetAll().Where(x => x.Sum.HasValue);
            
            var valueBlankActiveQuery = serviceFinActivity.GetAll().Where(x => x.ValueBlankActive.HasValue);
            var finActivityPlaintPaidQuery = serviceFinActivity.GetAll().Where(x => x.ClaimDamage.HasValue || x.FailureService.HasValue || x.NonDeliveryService.HasValue);

            var manOrgsByMunicipalityDict = manOrgsQuery
                .Select(x => new
                {
                    municipality = x.Contragent != null && x.Contragent.Municipality != null ? x.Contragent.Municipality.Id : -1,
                    manOrgName = x.Contragent != null ? x.Contragent.Name : string.Empty,
                    manOrgId = x.Id,
                    col4 = x.CountSrf.HasValue ? 1 : 0,
                    col5 = x.CountMo.HasValue ? 1 : 0,
                    col6 = x.CountOffices.HasValue ? 1 : 0,
                    col10 = manOrgRobjectsQuery.Where(y => y.ManOrgContract.ManagingOrganization.Id == x.Id)
                                                .Any(y => y.RealityObject != null)
                            &&
                            manOrgRobjectsQuery.Where(y => y.ManOrgContract.ManagingOrganization.Id == x.Id)
                                                .All(y => y.RealityObject.NumberLiving.HasValue) ? 1 : 0
                })
                .AsEnumerable()
                .GroupBy(x => x.municipality)
                .ToDictionary(x => x.Key, x => x.ToList());

            var disclosureInfoDict = serviceDisclosureInfo.GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ManagingOrganization.Contragent.Municipality.Id))
                .Where(x => x.PeriodDi.Id == this.periodDiId)
                .Where(x => manOrgIdsQuery.Contains(x.ManagingOrganization.Id))
                .Select(x => new
                {
                    manOrgId = x.ManagingOrganization.Id,
                    col7 = x.AdminPersonnel.HasValue || x.Engineer.HasValue || x.Work.HasValue ? 1 : 0,
                    col8 = x.DismissedAdminPersonnel.HasValue || x.DismissedEngineer.HasValue || x.DismissedWork.HasValue ? 1 : 0,
                    col9 = x.UnhappyEventCount.HasValue ? 1 : 0,
                    col11 = manageCategoryIncomeManagingQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col12 = communalServiceIncomeFromProvidingQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col13 = manageCategoryExpenseManagingQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col14 = manageCategoryDebtPopulationEndQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col15 = manageCategoryDebtPopulationStartQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col16 = communalServiceDebtPopulationEndQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col17 = communalServiceDebtPopulationStartQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col18 = communalServiceDebtManOrgCommunalServiceQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col19 = manageCategoryExactPopulationQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col20 = communalServiceExactQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col21 = valueBlankActiveQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col22 = communalServiceWorkByRepairQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col23 = communalServiceWorkByBeautificationQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col24 = communalServiceSourceSumQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col25 = communalServicePaidByMeteringDeviceQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col26 = communalServicePaidByGeneralNeedsQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col27 = manageCategoryIncomeUsingGeneralPropertyQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col28 = communalServicePeriodServiceQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col29 = finActivityPlaintPaidQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0,
                    col30 = communalServicePaymentByClaimQuery.Any(y => y.DisclosureInfo.Id == x.Id) ? 1 : 0
                })
                .AsEnumerable()
                .GroupBy(x => x.manOrgId)
                .ToDictionary(x => x.Key, x => x.First());
            
            var counter = 0;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");
            var sectionMo = section.ДобавитьСекцию("СекцияМо");
            foreach (var municipality in municipalityDict.OrderBy(x => x.Value))
            {
                double averageMo = 0;
                section.ДобавитьСтроку();
                section["МО"] = municipality.Value;

                if (!manOrgsByMunicipalityDict.ContainsKey(municipality.Key))
                {
                    section["СреднийПроцентМО"] = string.Empty;
                    continue;
                }

                foreach (var manOrgInfo in manOrgsByMunicipalityDict[municipality.Key].OrderBy(x => x.manOrgName))
                {
                    sectionMo.ДобавитьСтроку();
                    sectionMo["номер"] = ++counter;
                    sectionMo["НаименованиеУО"] = manOrgInfo.manOrgName;

                    var sum = manOrgInfo.col4 + manOrgInfo.col5 + manOrgInfo.col6 + manOrgInfo.col10;

                    if (disclosureInfoDict.ContainsKey(manOrgInfo.manOrgId))
                    {
                        var diInfo = disclosureInfoDict[manOrgInfo.manOrgId];
                        sectionMo["col7"] = diInfo.col7;
                        sectionMo["col8"] = diInfo.col8;
                        sectionMo["col9"] = diInfo.col9;
                        sectionMo["col11"] = diInfo.col11;
                        sectionMo["col12"] = diInfo.col12;
                        sectionMo["col13"] = diInfo.col13;
                        sectionMo["col14"] = diInfo.col14;
                        sectionMo["col15"] = diInfo.col15;
                        sectionMo["col16"] = diInfo.col16;
                        sectionMo["col17"] = diInfo.col17;
                        sectionMo["col18"] = diInfo.col18;
                        sectionMo["col19"] = diInfo.col19;
                        sectionMo["col20"] = diInfo.col20;
                        sectionMo["col21"] = diInfo.col21;
                        sectionMo["col22"] = diInfo.col22;
                        sectionMo["col23"] = diInfo.col23;
                        sectionMo["col24"] = diInfo.col24;
                        sectionMo["col25"] = diInfo.col25;
                        sectionMo["col26"] = diInfo.col26;
                        sectionMo["col27"] = diInfo.col27;
                        sectionMo["col28"] = diInfo.col28;
                        sectionMo["col29"] = diInfo.col29;
                        sectionMo["col30"] = diInfo.col30;

                        sum += diInfo.col7 + diInfo.col8 + diInfo.col9 + diInfo.col11 + diInfo.col12 + diInfo.col13
                            + diInfo.col14 + diInfo.col15 + diInfo.col16 + diInfo.col17 + diInfo.col18 + diInfo.col19 
                            + diInfo.col20 + diInfo.col21 + diInfo.col22 + diInfo.col23 + diInfo.col24 + diInfo.col25 
                            + diInfo.col26 + diInfo.col27 + diInfo.col28 + diInfo.col29 + diInfo.col30;
                    }
                    else
                    {
                        Enumerable.Range(7, 24).ForEach(i => sectionMo["col" + i] = "0");
                    }

                    sectionMo["col4"] = manOrgInfo.col4;
                    sectionMo["col5"] = manOrgInfo.col5;
                    sectionMo["col6"] = manOrgInfo.col6;
                    sectionMo["col10"] = manOrgInfo.col10;

                    var average = (sum / 27.0) * 100;
                    sectionMo["ПроцентРаскрытойИнформации"] = string.Format("{0:0.##}%", average);
                    
                    averageMo += average;
                }

                averageMo /= manOrgsByMunicipalityDict[municipality.Key].Count;
                section["СреднийПроцентМО"] = string.Format("{0:0.##}%", averageMo);
            }
        }
    }
}
