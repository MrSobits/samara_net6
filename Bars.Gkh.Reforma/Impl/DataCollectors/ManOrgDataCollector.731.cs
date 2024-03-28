namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    ///     Сервис сбора информации для отправки в Реформу
    /// </summary>
    public partial class ManOrgDataCollector
    {
        /// <summary>
        ///     Сбор данных по УО для выгрузки изменений
        /// </summary>
        /// <param name="currentProfile">Текущий профиль УО</param>
        /// <param name="organization">УО</param>
        /// <param name="period">Период раскрытия</param>
        /// <returns></returns>
        public IDataResult<CollectCompanyProfileDataResult> CollectCompanyProfileData(CompanyProfileData currentProfile, ManagingOrganization organization, PeriodDi period)
        {
            var commonDataResult = this.CollectNewCompanyProfileData(organization, period);
            if (!commonDataResult.Success)
            {
                return new GenericDataResult<CollectCompanyProfileDataResult>(message: commonDataResult.Message) { Success = false };
            }
            var contact = this.Container.ResolveDomain<ContragentContact>().GetAll().FirstOrDefault(x => x.Contragent.Id == organization.Contragent.Id && x.Position.Code == "1");
            var commonData = commonDataResult.Data;

            var useMerge = ((Dictionary<string, object>)this._syncService.GetParams().Data).Get("NullIsNotData").ToBool();

            var collectedFiles = new List<ICollectedFile<CompanyProfileData>>();
            var profile = useMerge ? new CompanyProfileData() : currentProfile;

            profile.okopf = commonData.okopf;
            profile.surname = commonData.surname;
            profile.middlename = commonData.middlename;
            profile.firstname = commonData.firstname;
            profile.position = contact != null ? contact.Position.Name : "";
            profile.ogrn = commonData.ogrn;
            profile.date_assignment_ogrn = organization.Contragent.DateRegistration ?? new DateTime();
            profile.name_authority_assigning_ogrn = organization.Contragent.OgrnRegistration;
            profile.legal_address = commonData.legal_address;
            profile.actual_address = commonData.actual_address;
            profile.post_address = commonData.post_address;

            profile.phone = commonData.phone;
            profile.email = commonData.email;
            profile.site = commonData.site;
            profile.proportion_sf = commonData.proportion_sf;
            profile.proportion_mo = commonData.proportion_mo;

            profile.work_time = this.GetWorkTimeData(organization);

            var di = this.GetDisclosure(organization, period);
            if (di != null)
            {
                // result.additional_info не выгружаем

                // result.srf_count не выгружаем
                // result.mo_count не выгружаем
                // result.offices_count не выгружаем
                this.FillRatingData(profile, organization, period);

                // result.tsg_management_members не выгружаем
                // result.audit_commision_members не выгружаем
                // result.additional_info_freeform не выгружаем
                // result.residents_count не выгружаем
                this.FillHouseCountersData(profile, organization, period);

                // result.sum_sq_houses_under_mng_start_period не выгружаем
                // result.avg_time_service_mkd не выгружаем

                // result.income_of_mng = commonData.income_of_mng;
                // result.income_of_usage = commonData.income_of_usage;
                // result.spending_of_mng = commonData.spending_of_mng;
                // result.debt_for_mng = commonData.debt_for_mng;
                this.FillMoneyByCategoryData(profile, organization, period);

                // result.income_of_ku = commonData.income_of_ku;
                // result.debt_owners_for_ku = commonData.debt_owners_for_ku;
                // result.debt_uo_for_ku = commonData.debt_uo_for_ku;
                // result.debt_owners_communal = commonData.debt_owners_communal;
                // result.payed_ku_by_statements = commonData.payed_ku_by_statements;
                // result.payed_ku_by_needs = commonData.payed_ku_by_needs;
                // result.claims_by_rso = commonData.claims_by_rso;
                this.FillCommunalMoneyData(profile, organization, period);

                // result.net_assets = commonData.net_assets;
                // result.annual_financial_statements = commonData.annual_financial_statements;
                this.FillCommonMoneyData(profile, organization, period);

                // result.claims_by_contracts_mng не выгружаем
                // result.revenues_expenditures_estimates не выгружаем
                // result.performance_report не выгружаем
                // result.members_meetings_minutes не выгружаем
                // result.audit_commision_report не выгружаем
                // result.audit_report не выгружаем
                // result.debt_owners не выгружаем
                // result.charged_for_mng не выгружаем
                // result.charged_for_resources не выгружаем
                // result.management_contract не выгружаем
                // result.services_cost не выгружаем
                // result.tariffs не выгружаем

                // result.spending_repair = commonData.spending_repair;
                // result.spending_beauty = commonData.spending_beauty;
                // result.spending_repair_invests = commonData.spending_repair_invests;
                this.FillRepairMoneyData(profile, organization, period);

                var reportingPeriodDomain = this.Container.ResolveDomain<ReportingPeriodDict>();
                ReportingPeriodDict reportingPeriod;

                using (this.Container.Using(reportingPeriodDomain))
                {
                    reportingPeriod = reportingPeriodDomain.GetAll().FirstOrDefault(x => x.PeriodDi.Id == period.Id);
                }

                this.FillFinActivityDocs(collectedFiles, di, reportingPeriod);
                this.FillProtocolDocs(collectedFiles, di, reportingPeriod);
                this.FillDocuments(collectedFiles, di, reportingPeriod);
            }

            if (useMerge)
            {
                this._merger.Apply(currentProfile, profile);
                profile = currentProfile;
            }

            return new GenericDataResult<CollectCompanyProfileDataResult>(new CollectCompanyProfileDataResult(profile, collectedFiles.ToArray()));
        }

        private void FillRatingData(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<DisclosureInfo>();
            var infoService = this.Container.Resolve<IDisclosureInfoService>();
            try
            {
                var data = service.GetAll().First(x => x.ManagingOrganization.Id == org.Id && x.PeriodDi.Id == period.Id);
                result.staff_regular_total = data.Engineer + data.Work + data.AdminPersonnel;
                result.staff_regular_administrative = data.AdminPersonnel;
                result.staff_regular_engineers = data.Engineer;
                result.staff_regular_labor = data.Work;
                result.count_dismissed = new CountDismissed
                                             {
                                                 count_dismissed = data.DismissedWork + data.DismissedAdminPersonnel + data.DismissedEngineer,
                                                 count_dismissed_admins = data.DismissedAdminPersonnel,
                                                 count_dismissed_engineers = data.DismissedEngineer,
                                                 count_dismissed_workers = data.DismissedWork
                                             };

                result.audit_commision_members = infoService.GetPositionByCode(org.Contragent.Id, period, new List<string> { "5" });
                result.tsg_management_members = infoService.GetPositionByCode(org.Contragent.Id, period, new List<string> { "6" });

                result.accidents_count = data.UnhappyEventCount;
                if (data.MembershipUnions == YesNoNotSet.Yes)
                {
                    result.participation_in_associations = this.GetParticipationInInAssociations(org, period);
                }

                if (data.AdminResponsibility == YesNoNotSet.Yes)
                {
                    this.FillProsecution(result, org, period);
                }
                else if (data.AdminResponsibility == YesNoNotSet.No)
                {
                    result.prosecute_count = 0;
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillHouseCountersData(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<ManOrgContractRealityObject>();
            try
            {
                var data =
                    service.GetAll()
                           .Where(x => x.ManOrgContract.ManagingOrganization.Id == org.Id)
                           .Select(x => new { x.RealityObject.AreaMkd, x.ManOrgContract.StartDate, x.ManOrgContract.EndDate })
                           .Select(x => new { x.StartDate, x.EndDate, x.AreaMkd });

                // выборка на период (как в гриде)
                var dataReportDate = data.Where(x => x.StartDate <= period.DateEnd && (!x.EndDate.HasValue || x.EndDate >= period.DateStart));

                // выборка на начало периода
                var dataStartDate = data.Where(x => x.StartDate <= period.DateStart && (!x.EndDate.HasValue || x.EndDate >= period.DateStart));

                result.count_houses_under_mng_report_date = new CountHousesUnderMngReportDate { count_houses_under_mng_report_date = dataReportDate.Count() };
                result.count_houses_under_mng_start_period = new CountHousesUnderMngStartPeriod { count_houses_under_mng_start_period = dataStartDate.Count() };
                result.sum_sq_houses_under_mng_report_date = new SumSqHousesUnderMngReportDate { sum_sq_houses_under_mng_report_date = (float)dataReportDate.SafeSum(x => x.AreaMkd ?? 0) };
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillMoneyByCategoryData(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<FinActivityManagCategory>();
            try
            {
                var data = service.GetAll().Where(x => x.DisclosureInfo.ManagingOrganization.Id == org.Id && x.DisclosureInfo.PeriodDi.Id == period.Id);

                result.income_of_mng = new IncomeOfMng();
                result.income_of_usage = new IncomeOfUsage();
                result.spending_of_mng = new SpendingOfMng();
                result.debt_for_mng = new DebtForMng();

                foreach (var category in data)
                {
                    switch (category.TypeCategoryHouseDi)
                    {
                        case TypeCategoryHouseDi.To25:
                            result.income_of_mng.by_houses_25 = (float?)category.IncomeManaging;
                            result.income_of_usage.by_houses_25 = (float?)category.IncomeUsingGeneralProperty;
                            result.spending_of_mng.by_houses_25 = (float?)category.ExpenseManaging;
                            result.debt_for_mng.by_houses_25 = (float?)category.DebtPopulationEnd;
                            break;
                        case TypeCategoryHouseDi.From26To50:
                            result.income_of_mng.by_houses_26_50 = (float?)category.IncomeManaging;
                            result.income_of_usage.by_houses_26_50 = (float?)category.IncomeUsingGeneralProperty;
                            result.spending_of_mng.by_houses_26_50 = (float?)category.ExpenseManaging;
                            result.debt_for_mng.by_houses_26_50 = (float?)category.DebtPopulationEnd;
                            break;
                        case TypeCategoryHouseDi.From51To75:
                            result.income_of_mng.by_houses_51_75 = (float?)category.IncomeManaging;
                            result.income_of_usage.by_houses_51_75 = (float?)category.IncomeUsingGeneralProperty;
                            result.spending_of_mng.by_houses_51_75 = (float?)category.ExpenseManaging;
                            result.debt_for_mng.by_houses_51_75 = (float?)category.DebtPopulationEnd;
                            break;
                        case TypeCategoryHouseDi.From76:
                            result.income_of_mng.by_houses_76 = (float?)category.IncomeManaging;
                            result.income_of_usage.by_houses_76 = (float?)category.IncomeUsingGeneralProperty;
                            result.spending_of_mng.by_houses_76 = (float?)category.ExpenseManaging;
                            result.debt_for_mng.by_houses_76 = (float?)category.DebtPopulationEnd;
                            break;
                        case TypeCategoryHouseDi.CrashHouse:
                            result.income_of_mng.by_houses_alarm = (float?)category.IncomeManaging;
                            result.income_of_usage.by_houses_alarm = (float?)category.IncomeUsingGeneralProperty;
                            result.spending_of_mng.by_houses_alarm = (float?)category.ExpenseManaging;
                            result.debt_for_mng.by_houses_alarm = (float?)category.DebtPopulationEnd;
                            break;
                        case TypeCategoryHouseDi.Summury:
                            result.income_of_mng.income_of_mng = (float?)category.IncomeManaging;
                            result.income_of_usage.income_of_usage = (float?)category.IncomeUsingGeneralProperty;
                            result.spending_of_mng.spending_of_mng = (float?)category.ExpenseManaging;
                            result.debt_for_mng.debt_for_mng = (float?)category.DebtPopulationEnd;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillCommunalMoneyData(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<FinActivityCommunalService>();
            try
            {
                var data = service.GetAll().Where(x => x.DisclosureInfo.ManagingOrganization.Id == org.Id && x.DisclosureInfo.PeriodDi.Id == period.Id).ToArray();

                result.income_of_ku = new IncomeOfKu();
                result.debt_owners_for_ku = new DebtOwnersForKu();
                result.debt_uo_for_ku = new DebtUoForKu();
                result.payed_ku_by_statements = new PayedKuByStatements();
                result.payed_ku_by_needs = new PayedKuByNeeds();
                result.claims_by_rso = new ClaimsByRso();

                foreach (var communalService in data)
                {
                    switch (communalService.TypeServiceDi)
                    {
                        case TypeServiceDi.Heating:
                            result.income_of_ku.by_heating = (float?)communalService.IncomeFromProviding;
                            result.debt_owners_for_ku.by_heating = (float?)communalService.DebtPopulationEnd;
                            result.debt_uo_for_ku.by_heating = (float?)communalService.DebtManOrgCommunalService;
                            result.payed_ku_by_statements.by_heating = (float?)communalService.PaidByMeteringDevice;
                            result.payed_ku_by_needs.by_heating = (float?)communalService.PaidByGeneralNeeds;
                            result.claims_by_rso.by_heating = (float?)communalService.PaymentByClaim;
                            break;
                        case TypeServiceDi.Electrical:
                            result.income_of_ku.by_electro = (float?)communalService.IncomeFromProviding;
                            result.debt_owners_for_ku.by_electro = (float?)communalService.DebtPopulationEnd;
                            result.debt_uo_for_ku.by_electro = (float?)communalService.DebtManOrgCommunalService;
                            result.payed_ku_by_statements.by_electro = (float?)communalService.PaidByMeteringDevice;
                            result.payed_ku_by_needs.by_electro = (float?)communalService.PaidByGeneralNeeds;
                            result.claims_by_rso.by_electro = (float?)communalService.PaymentByClaim;
                            break;
                        case TypeServiceDi.Gas:
                            result.income_of_ku.by_gaz = (float?)communalService.IncomeFromProviding;
                            result.debt_owners_for_ku.by_gaz = (float?)communalService.DebtPopulationEnd;
                            result.debt_uo_for_ku.by_gaz = (float?)communalService.DebtManOrgCommunalService;
                            result.payed_ku_by_statements.by_gaz = (float?)communalService.PaidByMeteringDevice;
                            result.payed_ku_by_needs.by_gaz = (float?)communalService.PaidByGeneralNeeds;
                            result.claims_by_rso.by_gaz = (float?)communalService.PaymentByClaim;
                            break;
                        case TypeServiceDi.HotWater:
                            result.income_of_ku.by_hot_water = (float?)communalService.IncomeFromProviding;
                            result.debt_owners_for_ku.by_hot_water = (float?)communalService.DebtPopulationEnd;
                            result.debt_uo_for_ku.by_hot_water = (float?)communalService.DebtManOrgCommunalService;
                            result.payed_ku_by_statements.by_hot_water = (float?)communalService.PaidByMeteringDevice;
                            result.payed_ku_by_needs.by_hot_water = (float?)communalService.PaidByGeneralNeeds;
                            result.claims_by_rso.by_hot_water = (float?)communalService.PaymentByClaim;
                            break;
                        case TypeServiceDi.ColdWater:
                            result.income_of_ku.by_cold_water = (float?)communalService.IncomeFromProviding;
                            result.debt_owners_for_ku.by_cold_water = (float?)communalService.DebtPopulationEnd;
                            result.debt_uo_for_ku.by_cold_water = (float?)communalService.DebtManOrgCommunalService;
                            result.payed_ku_by_statements.by_cold_water = (float?)communalService.PaidByMeteringDevice;
                            result.payed_ku_by_needs.by_cold_water = (float?)communalService.PaidByGeneralNeeds;
                            result.claims_by_rso.by_cold_water = (float?)communalService.PaymentByClaim;
                            break;
                        case TypeServiceDi.Wastewater:
                            result.income_of_ku.by_sewerage = (float?)communalService.IncomeFromProviding;
                            result.debt_owners_for_ku.by_sewerage = (float?)communalService.DebtPopulationEnd;
                            result.debt_uo_for_ku.by_sewerage = (float?)communalService.DebtManOrgCommunalService;
                            result.claims_by_rso.by_sewerage = (float?)communalService.PaymentByClaim;
                            break;
                        case TypeServiceDi.Summury:
                            result.income_of_ku.income_of_ku = (float?)communalService.IncomeFromProviding;
                            result.debt_owners_for_ku.debt_owners_for_ku = (float?)communalService.DebtPopulationEnd;
                            result.debt_uo_for_ku.debt_uo_for_ku = (float?)communalService.DebtManOrgCommunalService;
                            result.payed_ku_by_statements.payed_ku_by_statements = (float?)communalService.PaidByMeteringDevice;
                            result.payed_ku_by_needs.payed_ku_by_needs = (float?)communalService.PaidByGeneralNeeds;
                            result.claims_by_rso.claims_by_rso = (float?)communalService.PaymentByClaim;
                            break;
                    }
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillCommonMoneyData(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<FinActivity>();
            try
            {
                var data = service.GetAll().FirstOrDefault(x => x.DisclosureInfo.ManagingOrganization.Id == org.Id && x.DisclosureInfo.PeriodDi.Id == period.Id);
                if (data == null)
                {
                    return;
                }

                result.net_assets = (float?)data.ValueBlankActive;
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillRepairMoneyData(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            this.FillRepairMoneyByCategoryData(result, org, period);
            this.FillRepairMoneyBySourcesData(result, org, period);
        }

        private void FillRepairMoneyByCategoryData(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<FinActivityRepairCategory>();
            try
            {
                var data = service.GetAll().Where(x => x.DisclosureInfo.ManagingOrganization.Id == org.Id && x.DisclosureInfo.PeriodDi.Id == period.Id).ToArray();

                result.spending_repair = new SpendingRepair();
                result.spending_beauty = new SpendingBeauty();

                foreach (var category in data)
                {
                    switch (category.TypeCategoryHouseDi)
                    {
                        case TypeCategoryHouseDi.To25:
                            result.spending_repair.by_houses_25 = (float?)category.WorkByRepair;
                            result.spending_beauty.by_houses_25 = (float?)category.WorkByBeautification;
                            break;
                        case TypeCategoryHouseDi.From26To50:
                            result.spending_repair.by_houses_26_50 = (float?)category.WorkByRepair;
                            result.spending_beauty.by_houses_26_50 = (float?)category.WorkByBeautification;
                            break;
                        case TypeCategoryHouseDi.From51To75:
                            result.spending_repair.by_houses_51_75 = (float?)category.WorkByRepair;
                            result.spending_beauty.by_houses_51_75 = (float?)category.WorkByBeautification;
                            break;
                        case TypeCategoryHouseDi.From76:
                            result.spending_repair.by_houses_76 = (float?)category.WorkByRepair;
                            result.spending_beauty.by_houses_76 = (float?)category.WorkByBeautification;
                            break;
                        case TypeCategoryHouseDi.CrashHouse:
                            result.spending_repair.by_houses_alarm = (float?)category.WorkByRepair;
                            result.spending_beauty.by_houses_alarm = (float?)category.WorkByBeautification;
                            break;
                        case TypeCategoryHouseDi.Summury:
                            result.spending_repair.spending_repair = (float?)category.WorkByRepair;
                            result.spending_beauty.spending_beauty = (float?)category.WorkByBeautification;
                            break;
                    }
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillRepairMoneyBySourcesData(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<FinActivityRepairSource>();
            try
            {
                var data = service.GetAll().Where(x => x.DisclosureInfo.ManagingOrganization.Id == org.Id && x.DisclosureInfo.PeriodDi.Id == period.Id).ToArray();
                result.spending_repair_invests = new SpendingRepairInvests();
                foreach (var source in data)
                {
                    switch (source.TypeSourceFundsDi)
                    {
                        case TypeSourceFundsDi.Subsidy:
                            result.spending_repair_invests.subsidy = (float?)source.Sum;
                            break;
                        case TypeSourceFundsDi.Credit:
                            result.spending_repair_invests.credits = (float?)source.Sum;
                            break;
                        case TypeSourceFundsDi.FinanceByContractLeasing:
                            result.spending_repair_invests.fin_lising = (float?)source.Sum;
                            break;
                        case TypeSourceFundsDi.FinanceByContractEnergyService:
                            result.spending_repair_invests.fin_service = (float?)source.Sum;
                            break;
                        case TypeSourceFundsDi.PurposeContributionInhabitant:
                            result.spending_repair_invests.contributions_residents = (float?)source.Sum;
                            break;
                        case TypeSourceFundsDi.OtherSource:
                            result.spending_repair_invests.other_sources = (float?)source.Sum;
                            break;
                        case TypeSourceFundsDi.Summury:
                            result.spending_repair_invests.spending_repair_invests = (float?)source.Sum;
                            break;
                    }
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillProsecution(CompanyProfileData result, ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<AdminResp>();
            try
            {
                var data = service.GetAll().Where(x => x.DisclosureInfo.ManagingOrganization.Id == org.Id && x.DisclosureInfo.PeriodDi.Id == period.Id);

                // result.prosecute_copies_of_documents = TODO: че это за херня вообще?
                result.prosecute_count = data.Count();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillFinActivityDocs(List<ICollectedFile<CompanyProfileData>> collectedFiles, DisclosureInfo disclosureInfo, ReportingPeriodDict period)
        {
            var docsService = this.Container.ResolveDomain<FinActivityDocs>();
            var byYearService = this.Container.ResolveDomain<FinActivityDocByYear>();
            try
            {
                var docs = docsService.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfo.Id);
                if (docs == null)
                {
                    return;
                }

                var files = new List<FileInfo>();
                if (docs.BookkepingBalance != null)
                {
                    files.Add(docs.BookkepingBalance);
                }

                if (docs.BookkepingBalanceAnnex != null)
                {
                    files.Add(docs.BookkepingBalanceAnnex);
                }

                if (files.Count > 0)
                {
                    collectedFiles.Add(new CollectedFilesCollection<CompanyProfileData>(
                        this.Container, files.ToArray(), period, (profile, ids) => profile.annual_financial_statements_files = ids));
                }

                var byYearDocs =
                    byYearService.GetAll()
                                 .Where(
                                     x =>
                                     x.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id && x.Year <= disclosureInfo.PeriodDi.DateStart.Value.Year
                                     && x.Year >= disclosureInfo.PeriodDi.DateStart.Value.Year - 2 && x.File != null)
                                 .Select(x => new { x.TypeDocByYearDi, x.File })
                                 .AsEnumerable()
                                 .GroupBy(x => x.TypeDocByYearDi)
                                 .ToDictionary(x => x.Key, x => x.Select(y => y.File).ToArray());

                if (byYearDocs.ContainsKey(TypeDocByYearDi.EstimateIncome) && byYearDocs[TypeDocByYearDi.EstimateIncome].Length > 0)
                {
                    collectedFiles.Add(
                        new CollectedFilesCollection<CompanyProfileData>(
                            this.Container,
                            byYearDocs[TypeDocByYearDi.EstimateIncome],
                            period,
                            (profile, ids) => profile.revenues_expenditures_estimates_files = ids));
                }

                if (byYearDocs.ContainsKey(TypeDocByYearDi.ReportEstimateIncome) && byYearDocs[TypeDocByYearDi.ReportEstimateIncome].Length > 0)
                {
                    collectedFiles.Add(
                        new CollectedFilesCollection<CompanyProfileData>(
                            this.Container, byYearDocs[TypeDocByYearDi.ReportEstimateIncome], period, (profile, ids) => profile.performance_report_files = ids));
                }

                if (byYearDocs.ContainsKey(TypeDocByYearDi.ConclusionRevisory) && byYearDocs[TypeDocByYearDi.ConclusionRevisory].Length > 0)
                {
                    collectedFiles.Add(
                        new CollectedFilesCollection<CompanyProfileData>(this.Container, byYearDocs[TypeDocByYearDi.ConclusionRevisory], period, (profile, ids) => profile.audit_commision_report_files = ids));
                }
            }
            finally
            {
                this.Container.Release(docsService);
                this.Container.Release(byYearService);
            }
        }

        private void FillProtocolDocs(List<ICollectedFile<CompanyProfileData>> collectedFiles, DisclosureInfo disclosureInfo, ReportingPeriodDict period)
        {
            var roDocsService = this.Container.ResolveDomain<DocumentsRealityObjProtocol>();
            var manOrgContractService = this.Container.ResolveDomain<ManOrgContractRealityObject>();
            try
            {
                var periodStart = disclosureInfo.PeriodDi.DateStart;
                var periodEnd = disclosureInfo.PeriodDi.DateEnd;

                var protocols =
                    roDocsService.GetAll()
                                 .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfo.Id && x.Year >= periodStart.Value.Year - 1 && x.Year <= periodEnd.Value.Year && x.File != null && x.File.Size > 0)
                                 .Select(x => x.File)
                                 .ToArray();

                collectedFiles.Add(new CollectedFilesCollection<CompanyProfileData>(this.Container, protocols, period, (profile, ids) => profile.members_meetings_minutes_files = ids));
            }
            finally
            {
                this.Container.Release(roDocsService);
                this.Container.Release(manOrgContractService);
            }
        }

        private void FillDocuments(List<ICollectedFile<CompanyProfileData>> collectedFiles, DisclosureInfo disclosureInfo, ReportingPeriodDict period)
        {
            var service = this.Container.ResolveDomain<Documents>();
            try
            {
                var documents = service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfo.Id);
                if (documents == null)
                {
                    return;
                }

                if (documents.FileProjectContract != null)
                {
                    collectedFiles.Add(
                        new CollectedFilesCollection<CompanyProfileData>(this.Container, new[] { documents.FileProjectContract }, period, (profile, ids) => profile.management_contract_files = ids));
                }

                if (documents.FileCommunalService != null)
                {
                    collectedFiles.Add(
                        new CollectedFilesCollection<CompanyProfileData>(this.Container, new[] { documents.FileCommunalService }, period, (profile, ids) => profile.services_cost_files = ids));
                    collectedFiles.Add(
                        new CollectedFilesCollection<CompanyProfileData>(this.Container, new[] { documents.FileCommunalService }, period, (profile, ids) => profile.tariffs_files = ids));
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}