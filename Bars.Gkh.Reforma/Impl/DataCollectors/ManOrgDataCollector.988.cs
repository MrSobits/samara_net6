namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Reforma.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    ///     Сервис сбора информации для отправки в Реформу
    /// </summary>
    public partial class ManOrgDataCollector
    {
        /// <summary>
        ///     Собирает профиль УО по 988 формам
        /// </summary>
        /// <param name="profileData">Профиль УО на Реформе</param>
        /// <param name="organization">УО</param>
        /// <param name="period">Период раскрытия</param>
        /// <returns></returns>
        public IDataResult<CollectCompanyProfile988DataResult> CollectCompanyProfile988Data(CompanyProfileData988 profileData, ManagingOrganization organization, PeriodDi period)
        {
            var commonDataResult = this.CollectNewCompanyProfileData(organization, period);
            if (!commonDataResult.Success)
            {
                return new GenericDataResult<CollectCompanyProfile988DataResult>(message: commonDataResult.Message)
                {
                    Success = false
                };
            }

            var commonData = commonDataResult.Data;
            var collectedFiles = new List<ICollectedFile<CompanyProfileData988>>();

            if (profileData.IsNull())
            {
                profileData = new CompanyProfileData988();
            }

            profileData.okopf = commonData.okopf;
            profileData.ogrn = commonData.ogrn;
            profileData.surname = commonData.surname;
            profileData.firstname = commonData.firstname;
            profileData.middlename = commonData.middlename;
            profileData.legal_address = commonData.legal_address;
            profileData.actual_address = commonData.actual_address;
            profileData.post_address = commonData.post_address;
            profileData.phone = commonData.phone;
            profileData.email = commonData.email;
            profileData.site = commonData.site;
            profileData.proportion_sf = commonData.proportion_sf;
            profileData.proportion_mo = commonData.proportion_mo;
            profileData.work_time = this.GetWorkTimeData(organization);

            this.FillDispatcher988Data(profileData, organization);

            var disclosureInfo = this.GetDisclosure(organization, period);
            var reportingPeriodDomain = this.Container.ResolveDomain<ReportingPeriodDict>();
            ReportingPeriodDict reportingPeriod;

            using (this.Container.Using(reportingPeriodDomain))
            {
                reportingPeriod = reportingPeriodDomain.GetAll().FirstOrDefault(x => x.PeriodDi.Id == period.Id);
            }

            if (disclosureInfo != null)
            {
                this.FillRating988Data(profileData, collectedFiles, disclosureInfo, reportingPeriod);
                this.FillMoneyByManagRobject988Data(profileData, disclosureInfo);
                this.FillCommunalMoney988Data(profileData, disclosureInfo);

                this.FillFinActivity988Docs(collectedFiles, disclosureInfo, reportingPeriod);
                this.FillFinActivity988DocsByYear(collectedFiles, disclosureInfo, reportingPeriod);

                this.FillDisturbances988Data(profileData, collectedFiles, disclosureInfo, reportingPeriod);
                this.FillLicenses988Data(profileData, collectedFiles, disclosureInfo, reportingPeriod);
            }
            else
            {
                return new GenericDataResult<CollectCompanyProfile988DataResult>(message: "По указанной УО в текущем периоде не ведётся раскрытие информации")
                {
                    Success = false
                };
            }

            return
                new GenericDataResult<CollectCompanyProfile988DataResult>(
                    new CollectCompanyProfile988DataResult(profileData, collectedFiles.ToArray()));
        }

        private void FillCommunalMoney988Data(CompanyProfileData988 result, DisclosureInfo disclosureInfo)
        {
            var service = this.Container.ResolveDomain<FinActivityCommunalService>();
            try
            {
                var data =
                    service.GetAll()
                           .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                           .Select(x => new { x.TypeServiceDi, x.DebtManOrgCommunalService })
                           .ToArray();

                foreach (var communalService in data)
                {
                    switch (communalService.TypeServiceDi)
                    {
                        case TypeServiceDi.Heating:
                            result.debt_for_thermal_energy = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.Electrical:
                            result.debt_for_electrical_energy = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.Gas:
                            result.debt_for_gas = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.HotWater:
                            result.debt_for_hot_water = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.ColdWater:
                            result.debt_for_cold_water = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.Wastewater:
                            result.debt_for_sewerage = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.OtherSource:
                            result.debt_for_other = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.Summury:
                            result.debt_for_ku = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.ThermalEnergyForHeating:
                            result.debt_for_thermal_energy_by_heating = communalService.DebtManOrgCommunalService;
                            break;
                        case TypeServiceDi.ThermalEnergyForNeedsOfHotWater:
                            result.debt_for_thermal_energy_by_hot_water = communalService.DebtManOrgCommunalService;
                            break;
                    }
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillDispatcher988Data(CompanyProfileData988 result, ManagingOrganization org)
        {
            result.dispatcher_address = org.DispatchAddress.ToReformaFias(this.Container);
            result.dispatcher_phone = org.DispatchPhone;
            result.dispatcher_work_time = this.GetWorkTimeData(org, TypeMode.DispatcherWork);
        }

        private void FillDisturbances988Data(CompanyProfileData988 result, List<ICollectedFile<CompanyProfileData988>> collectedFiles, DisclosureInfo disclosureInfo, ReportingPeriodDict period)
        {
            var diService = this.Container.ResolveDomain<DisclosureInfo>();
            var adminRespService = this.Container.ResolveDomain<AdminResp>();
            var actionService = this.Container.ResolveDomain<Actions>();
            
            try
            {
                if (disclosureInfo.AdminResponsibility != YesNoNotSet.Yes)
                {
                    result.disturbances = new Disturbance[0];
                    return;
                }

                var existsDisturbances = result.disturbances?.Where(x => x.document_file_id.HasValue).ToDictionary(x => x.document_file_id, x => x.id);

                var adminResps = adminRespService.GetAll().Where(x => x.DisclosureInfo.Id == disclosureInfo.Id).ToArray();
                var actionsDict = actionService.GetAll()
                    .Where(x => x.AdminResp.DisclosureInfo.Id == disclosureInfo.Id)
                    .Select(x => new
                    {
                        x.AdminResp.Id,
                        x.Action
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Action).ToArray());

                var disturbances = new List<Disturbance>(adminResps.Length);
                for (var i = 0; i < adminResps.Length; i++)
                {
                    var adminResp = adminResps[i];
                    var disturbance = new Disturbance();
                    disturbances.Add(disturbance);

                    disturbance.date_of_responsibility = adminResp.DateImpositionPenalty;
                    disturbance.person_type = adminResp.TypePerson.Return(x =>
                    {
                        switch (x)
                        {
                            case TypePersonAdminResp.JurPerson:
                                return 1;
                            case TypePersonAdminResp.PhisicalPerson:
                                return 2;
                            default:
                                return (int?) null;
                        }
                    });

                    if (adminResp.TypePerson == TypePersonAdminResp.PhisicalPerson)
                    {
                        disturbance.person_fio = adminResp.Fio;
                        disturbance.person_position = adminResp.Position;
                    }

                    disturbance.subject_of_disturbance = adminResp.TypeViolation;
                    disturbance.supervisory_authority_name = adminResp.SupervisoryOrg.Return(x => x.Name);
                    disturbance.number_of_disturbance = adminResp.AmountViolation;
                    disturbance.amount_of_penalty = adminResp.SumPenalty;
                    disturbance.document_name = adminResp.DocumentName;
                    disturbance.document_date = adminResp.DateFrom;
                    disturbance.document_number = adminResp.DocumentNum;
                    if (adminResp.File != null)
                    {
                        var i1 = i;
                        collectedFiles.Add(new CollectedFile<CompanyProfileData988>(
                            this.Container, adminResp.File, period,
                            (profile, id) =>
                            {
                                profile.disturbances[i1].document_file_id = id;
                                profile.disturbances[i1].id = existsDisturbances?.Get(id);
                            }));
                    }

                    disturbance.measures_to_eliminate = string.Join("; ", actionsDict.Get(adminResp.Id, defValue: new string[0]));
                }

                result.disturbances = disturbances.ToArray();
            }
            finally
            {
                this.Container.Release(diService);
                this.Container.Release(adminRespService);
                this.Container.Release(actionService);
            }
        }

        private void FillFinActivity988Docs(List<ICollectedFile<CompanyProfileData988>> collectedFiles, DisclosureInfo disclosureInfo, ReportingPeriodDict period)
        {
            var service = this.Container.ResolveDomain<FinActivityDocs>();
            try
            {
                var docs = service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfo.Id);
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
                    collectedFiles.Add(new CollectedFilesCollection<CompanyProfileData988>(
                        this.Container, files.ToArray(), period, (profile, ids) => profile.annual_financial_statement_files = ids));
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillFinActivity988DocsByYear(List<ICollectedFile<CompanyProfileData988>> collectedFiles, DisclosureInfo disclosureInfo, ReportingPeriodDict period)
        {
            var service = this.Container.ResolveDomain<FinActivityDocByYear>();
            try
            {
                var docs = service.GetAll()
                    .Where(x => 
                        x.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id
                        && x.Year <= disclosureInfo.PeriodDi.DateStart.Value.Year 
                        && x.Year >= disclosureInfo.PeriodDi.DateStart.Value.Year - 2)
                    .Where(x => x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome || x.TypeDocByYearDi == TypeDocByYearDi.ReportEstimateIncome)
                    .Where(x => x.File != null)
                    .Select(x => new
                    {
                        x.TypeDocByYearDi,
                        x.File
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.TypeDocByYearDi)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.File).ToArray());

                collectedFiles.Add(new CollectedFilesCollection<CompanyProfileData988>(this.Container, docs.Get(TypeDocByYearDi.EstimateIncome) ?? new FileInfo[0], 
                    period, (profile, ids) => profile.revenues_expenditures_estimates_files = ids));

                if (docs.ContainsKey(TypeDocByYearDi.ReportEstimateIncome) && docs[TypeDocByYearDi.ReportEstimateIncome].Length > 0)
                {
                    collectedFiles.Add(
                        new CollectedFilesCollection<CompanyProfileData988>(this.Container, docs[TypeDocByYearDi.ReportEstimateIncome], period, (profile, ids) => profile.performance_report_files = ids));
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillLicenses988Data(CompanyProfileData988 result, List<ICollectedFile<CompanyProfileData988>> collectedFiles, DisclosureInfo disclosureInfo, ReportingPeriodDict period)
        {
            var diService = this.Container.ResolveDomain<DisclosureInfo>();
            var disclosureInfoLicenseService = this.Container.ResolveDomain<DisclosureInfoLicense>();

            try
            {
                var currentLicences = result.licenses?.Where(x => x.license_document_file_id.HasValue).ToDictionary(x => x.license_document_file_id, x => x.id);

                if (disclosureInfo.HasLicense != YesNoNotSet.Yes)
                {
                    result.licenses = new License[0];
                    return;
                }

                var disclosureInfoLicenses = disclosureInfoLicenseService.GetAll().Where(x => x.DisclosureInfo.Id == disclosureInfo.Id).ToArray();

                var licenses = new List<License>(disclosureInfoLicenses.Length);
                for (var i = 0; i < disclosureInfoLicenses.Length; i++)
                {
                    var disclosureInfoLicense = disclosureInfoLicenses[i];
                    var license = new License();
                    licenses.Add(license);

                    license.license_number = disclosureInfoLicense.LicenseNumber;
                    license.license_issuing_authority = disclosureInfoLicense.LicenseOrg;
                    license.license_receipt_date = disclosureInfoLicense.DateReceived;
                    if (disclosureInfoLicense.LicenseDoc != null)
                    {
                        var i1 = i;
                        collectedFiles.Add(new CollectedFile<CompanyProfileData988>(
                            this.Container, disclosureInfoLicense.LicenseDoc, period,
                            (profile, id) =>
                            {
                                profile.licenses[i1].license_document_file_id = id;
                                profile.licenses[i1].id = currentLicences?.Get(id);
                            }));
                    }
                }

                result.licenses = licenses.ToArray();
            }
            finally
            {
                this.Container.Release(diService);
                this.Container.Release(disclosureInfoLicenseService);
            }
        }

        private void FillMoneyByManagRobject988Data(CompanyProfileData988 result, DisclosureInfo disclosureInfo)
        {
            var service = this.Container.ResolveDomain<FinActivityManagRealityObj>();
            try
            {
                var data = service.GetAll().Where(x => x.DisclosureInfo.Id == disclosureInfo.Id);
                result.income_of_mng = data.Sum(x => x.SumIncomeManage);
                result.spending_of_mng = data.Sum(x => x.SumFactExpense);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillRating988Data(CompanyProfileData988 result, List<ICollectedFile<CompanyProfileData988>> collectedFiles, DisclosureInfo disclosureInfo, ReportingPeriodDict period)
        {
            result.staff_regular_total = 
                disclosureInfo.ManagingOrganization.NumberEmployees ?? 0 + 
                disclosureInfo.Engineer ?? 0 + 
                disclosureInfo.Work ?? 0 + 
                disclosureInfo.AdminPersonnel ?? 0;

            result.staff_regular_administrative = disclosureInfo.AdminPersonnel;
            result.staff_regular_engineers = disclosureInfo.Engineer;
            result.staff_regular_labor = disclosureInfo.Work;

            if (disclosureInfo.MembershipUnions == YesNoNotSet.Yes)
            {
                result.membership_information = this.GetParticipationInInAssociations(disclosureInfo.ManagingOrganization, disclosureInfo.PeriodDi);
            }

            var typeManagement = disclosureInfo.ManagingOrganization.TypeManagement;
            var needToAttachChartFile = typeManagement == TypeManagementManOrg.TSJ || typeManagement == TypeManagementManOrg.JSK;

            if (needToAttachChartFile && disclosureInfo.ManagingOrganization.DispatchFile != null)
            {
                collectedFiles.Add(new CollectedFile<CompanyProfileData988>(
                    this.Container, disclosureInfo.ManagingOrganization.DispatchFile, period, (profile, id) => profile.chart_file_id = id));
            }
        }
    }
}