namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.Reforma;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Reforma.Utils.Validation;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Core.Internal;

    /// <summary>
    ///     Сборщик данных по жилома дому
    /// </summary>
    public partial class RobjectDataCollector
    {
        /// <summary>
        ///     Собирает данные по жилому дому
        /// </summary>
        /// <param name="currentProfile">Текущая анкета жилого дома</param>
        /// <param name="refRobject">Жилой дом</param>
        /// <param name="period">Период раскрытия</param>
        /// <param name="manOrgId">Id УК</param>
        /// <returns>Результат</returns>
        public IDataResult<CollectHouseProfile988DataResult> CollectHouseProfile988Data(
            HouseProfileData988 currentProfile,
            RefRealityObject refRobject,
            PeriodDi period,
            long manOrgId)
        {
            var useMerge = ((Dictionary<string, object>) this.syncService.GetParams().Data).Get("NullIsNotData").ToBool();

            var collectedFiles = new List<ICollectedFile<HouseProfileData988>>();
            var result = useMerge ? this.Clone(currentProfile) : currentProfile;
            
            var di = manOrgId > 0 ? this.GetRobjectDisclosure(refRobject, period, manOrgId) : this.GetRobjectDisclosure(refRobject, period);

            var reportingPeriodDomain = this.Container.ResolveDomain<ReportingPeriodDict>();
            ReportingPeriodDict reportingPeriod;

            using (this.Container.Using(reportingPeriodDomain))
            {
                reportingPeriod = reportingPeriodDomain.GetAll().FirstOrDefault(x => x.PeriodDi.Id == period.Id);
            }

            this.FillManagementContract988(result, collectedFiles, refRobject.RealityObject, reportingPeriod, manOrgId);
       
            this.FillDataByRobjectOverhaul988(result, refRobject.RealityObject);

            if (di != null)
            {
                this.FillDataByRobjectPassport988(result, di);
                this.FillDataByRobjectAlarm988(result, refRobject.RealityObject);
                this.FillDataByRobjectStructElements988(result, di);
                this.FillDataByRobjectEngineerSystems988(result, di);
                this.FillDataByRobjectDevices988(result, di);
                this.FillDataByRobjectLifts988(result, di);
                this.FillDataByRobjectServices988(result, di);
                this.FillDataByRobjectHouseCommunalServices988(result, di);
                this.FillDataByRobjectCommonProperty988(result, di);
                this.FillDataByRobjectCommonMeetting988(result, collectedFiles, di, reportingPeriod);
                this.FillDataByRobjectReport988(result, di);

                if (useMerge)
                {
                    this.merger.AggressiveMode = true;
                    this.merger.Apply(currentProfile, result);
                    result = currentProfile;
                }
            }
            else
            {
                return new GenericDataResult<CollectHouseProfile988DataResult>(message: "По указанному дому в текущем периоде не ведётся раскрытие информации")
                {
                    Success = false
                };
            }

            return new GenericDataResult<CollectHouseProfile988DataResult>(new CollectHouseProfile988DataResult(result, collectedFiles.ToArray()));
        }

        private void FillDataByRobjectCommonMeetting988(HouseProfileData988 result, List<ICollectedFile<HouseProfileData988>> collectedFiles, DisclosureInfoRealityObj di, ReportingPeriodDict period)
        {
            var documentRoProtocolDomain = this.Container.ResolveDomain<DocumentsRealityObjProtocol>();
            try
            {
                var data = documentRoProtocolDomain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == di.Id && x.Year >= di.PeriodDi.DateStart.Value.Year - 1 && x.Year <= di.PeriodDi.DateEnd.Value.Year)
                    .ToList();

                var existProtocols = result.common_meetings?.Where(x => x.protocol_file_id.HasValue).ToDictionary(x => x.protocol_file_id, x => x.id);
                result.common_meetings = data.Select(
                    (x, i) =>
                        {
                            if (x.File != null)
                            {
                                collectedFiles.Add(
                                    new CollectedFile<HouseProfileData988>(this.Container, x.File, period,
                                        (profile, id) =>
                                        {
                                            profile.common_meetings[i].id = existProtocols?.Get(id);
                                            profile.common_meetings[i].protocol_file_id = id;
                                        }));
                            }

                            return new HouseCommonMeeting {protocol_date = x.DocDate, protocol_number = x.DocNum};
                        }).ToArray();
            }
            finally
            {
                this.Container.Release(documentRoProtocolDomain);
            }
        }

        private void FillDataByRobjectOverhaul988(HouseProfileData988 result, RealityObject robject)
        {
            if (!this.Container.Kernel.HasComponent(typeof(IRealityObjectBothProtocolProvider)))
            {
                return;
            }

            var provider = this.Container.Resolve<IRealityObjectBothProtocolProvider>();
            try
            {
                var dataResult = provider.GetData(robject.Id);
                var data = dataResult.Data as RealityObjectBothProtocolData;
                if (dataResult.Success && data != null)
                {
                    result.overhaul = new HouseOverhaul
                                          {
                                              provider_inn = data.ProviderInn,
                                              provider_name = data.ProviderName,
                                              common_meeting_protocol_date = data.CommonMeetingProtocolDate,
                                              common_meeting_protocol_number = data.CommonMeetingProtocolNumber,
                                              payment_amount_for_1sm = data.PaymentAmount
                                          };
                }
            }
            finally
            {
                this.Container.Release(provider);
            }
        }

        private void FillDataByRobjectAlarm988(HouseProfileData988 result, RealityObject robject)
        {
            var service = this.Container.ResolveDomain<EmergencyObject>();
            try
            {
                var emergencyObject = service.GetAll().FirstOrDefault(x => x.RealityObject.Id == robject.Id);
                if (emergencyObject != null && result.is_alarm)
                {
                    result.alarm_info = new HouseAlarm
                    {
                        document_date = emergencyObject.DocumentDate,
                        document_number = emergencyObject.DocumentNumber,
                        reason = null,
                        failure = null
                    };

                    emergencyObject.NotNullOrEmpty(x => x.ReasonInexpedient.Name);
                    switch (emergencyObject.ReasonInexpedient.Name.ToLower())
                    {
                        case "физический износ":
                            result.alarm_info.reason = 1;
                            break;
                        case "влияние окружающей среды":
                            result.alarm_info.reason = 2;
                            break;
                        case "природные катастрофы":
                            result.alarm_info.reason = 3;
                            break;
                        case "причины техногенного характера":
                            result.alarm_info.reason = 4;
                            break;
                        case "пожар":
                            result.alarm_info.reason = 5;
                            break;
                        case "иное":
                            result.alarm_info.reason = 6;
                            break;
                    }

                    if (result.alarm_info.reason == 6)
                    {
                        result.alarm_info.reason_other = emergencyObject.ReasonInexpedient.Name;
                    }
                }
                else if (result.alarm_info != null && result.is_alarm)
                {
                    result.alarm_info.failure = result.alarm_info.failure ?? new HouseAlarmFailure
                    {
                        // Ошибочный перевод статуса в аварийный (заглушка)
                        reason = 1
                    };
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillDataByRobjectReport988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            result.report = new HouseReport
                                {
                                    common = new HouseReportCommon(),
                                    communal_service = new HouseReportCommunalService(),
                                    claims_to_consumers = new HouseReportClaimsToConsumers(),
                                    house_report_quality_of_work_claims = new HouseReportQualityOfWorkClaims()
                                };

            result.report.common.cash_balance_beginning_period_consumers_overpayment = di.AdvancePayments;
            result.report.common.cash_balance_beginning_period = di.CarryOverFunds;
            result.report.common.cash_balance_beginning_period_consumers_arrears = di.Debt;
            result.report.common.charged_for_services = di.ChargeForMaintenanceAndRepairsAll;
            result.report.common.charged_for_maintenance_of_house = di.ChargeForMaintenanceAndRepairsMaintanance;
            result.report.common.charged_for_maintenance_work = di.ChargeForMaintenanceAndRepairsRepairs;
            result.report.common.charged_for_management_service = di.ChargeForMaintenanceAndRepairsManagement;
            result.report.common.received_cash = di.ReceivedCashAll;
            result.report.common.received_cash_from_owners = di.ReceivedCashFromOwners;
            result.report.common.received_target_payment_from_owners = di.ReceivedCashFromOwnersTargeted;
            result.report.common.received_subsidies = di.ReceivedCashAsGrant;
            result.report.common.received_from_use_of_common_property = di.ReceivedCashFromUsingCommonProperty;
            result.report.common.received_from_other = di.ReceivedCashFromOtherTypeOfPayments;
            result.report.common.cash_total = di.CashBalanceAll;
            result.report.common.cash_balance_ending_period_consumers_overpayment = di.CashBalanceAdvancePayments;
            result.report.common.cash_balance_ending_period = di.CashBalanceCarryOverFunds;
            result.report.common.cash_balance_ending_period_consumers_arrears = di.CashBalanceDebt;


            result.report.communal_service.balance_beginning_period_consumers_overpayment = di.ComServStartAdvancePay;
            result.report.communal_service.balance_beginning_period = di.ComServStartCarryOverFunds;
            result.report.communal_service.balance_beginning_period_consumers_arrears = di.ComServStartDebt;
            result.report.communal_service.balance_ending_period_consumers_overpayment = di.ComServEndAdvancePay;
            result.report.communal_service.balance_ending_period = di.ComServEndCarryOverFunds;
            result.report.communal_service.balance_ending_period_consumers_arrears = di.ComServEndDebt;
            result.report.communal_service.claims_received_count = di.ComServReceivedPretensionCount;
            result.report.communal_service.claims_satisfied_count = di.ComServApprovedPretensionCount;
            result.report.communal_service.claims_denied_count = di.ComServNoApprovedPretensionCount;
            result.report.communal_service.produced_recalculation_amount = di.ComServPretensionRecalcSum;

            result.report.claims_to_consumers.sent_claims_count = di.SentPretensionCount;
            result.report.claims_to_consumers.filed_actions_count = di.SentPetitionCount;
            result.report.claims_to_consumers.received_cash_amount = di.ReceiveSumByClaimWork;

            result.report.house_report_quality_of_work_claims.claims_received_count = di.ReceivedPretensionCount;
            result.report.house_report_quality_of_work_claims.claims_satisfied_count = di.ApprovedPretensionCount;
            result.report.house_report_quality_of_work_claims.claims_denied_count = di.NoApprovedPretensionCount;
            result.report.house_report_quality_of_work_claims.produced_recalculation_amount = di.PretensionRecalcSum;


        }

        private void FillDataByRobjectCommonProperty988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            var service = this.Container.ResolveDomain<InfoAboutUseCommonFacilities>();
            try
            {
                var infos = service.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == di.Id).ToArray();
                var commonProperties = new List<HouseCommonProperty>();
                foreach (var info in infos)
                {
                    var commonProperty = new HouseCommonProperty();
                    commonProperties.Add(commonProperty);

                    commonProperty.name = info.KindCommomFacilities;
                    commonProperty.function = info.AppointmentCommonFacilities;
                    commonProperty.area = info.AreaOfCommonFacilities;

                    commonProperty.rent = new HouseCommonPropertyRent
                                              {
                                                  provider_inn = info.Inn,
                                                  provider_name = info.Lessee,
                                                  contract_number = info.ContractNumber,
                                                  contract_date = info.ContractDate,
                                                  contract_start_date = info.DateStart,
                                                  cost_per_month = info.CostByContractInMonth,
                                                  common_meeting_protocol_date = info.From,
                                                  common_meeting_protocol_number = info.Number
                                              };
                }

                result.common_properties = commonProperties.ToArray();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private void FillDataByRobjectHouseCommunalServices988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            var communalServiceService = this.Container.ResolveDomain<CommunalService>();
            try
            {
                var communalServices = communalServiceService.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == di.Id)
                    .ToArray();

                var houseCommunalServices = new List<HouseCommunalService>();
                foreach (var communalService in communalServices)
                {
                    if (communalService.TemplateService.Name == null)
                    {
                        continue;
                    }

                    var type = 0;
                    switch (communalService.TemplateService.Name.ToLower())
                    {
                        case "газоснабжение":
                            type = 6;
                            break;
                        case "отопление":
                            type = 5;
                            break;
                        case "электроснабжение":
                            type = 4;
                            break;
                        case "водоотведение":
                            type = 3;
                            break;
                        case "горячее водоснабжение":
                            type = 2;
                            break;
                        case "холодное водоснабжение":
                            type = 1;
                            break;
                        case "холодная вода для нужд гвс":
                            type = 7;
                            break;
                        case "тепловая энергия для подогрева холодной воды для нужд гвс":
                            type = 8;
                            break;
                        case "газоснабжение для подогрева холодной воды для нужд гвс":
                            type = 9;
                            break;
                        case "обращение с твердыми коммунальными отходами":
                            type = 11;
                            break;
                    }

                    var houseCommunalService = result.communal_services.Return(d => d.FirstOrDefault(x => x.type == type)) 
                        ?? new HouseCommunalService { type = type, is_default = type <= 6 };

                    houseCommunalServices.Add(houseCommunalService);
                    this.FillDataByRobjectHouseCommunalService988(houseCommunalService, communalService);
                }

                houseCommunalServices.AddRange(
                    result.communal_services?
                        .Where(x => new int?[] { 1, 2, 3, 4, 5, 6, 11 }.Except(houseCommunalServices.Select(y => y.type)).Contains(x.type)) 
                    ?? Enumerable.Empty<HouseCommunalService>());

                result.communal_services = houseCommunalServices.ToArray();
            }
            finally
            {
                this.Container.Release(communalServiceService);
            }
        }

        private void FillDataByRobjectHouseCommunalService988(HouseCommunalService result, CommunalService communalService)
        {
            var providerService = this.Container.ResolveDomain<ProviderService>();
            var tariffService = this.Container.ResolveDomain<TariffForConsumers>();
            var npaService = this.Container.ResolveDomain<ConsumptionNormsNpa>();
            var repairServiceService = this.Container.ResolveDomain<RepairService>();
            var infoAboutPaymentCommunalDomain = this.Container.ResolveDomain<InfoAboutPaymentCommunal>();
            var diRelationsDomain = this.Container.ResolveDomain<DisclosureInfoRelation>();

            try
            {
                result.filling_fact = null;
                switch (communalService.TypeOfProvisionService)
                {
                    case TypeOfProvisionServiceDi.ServiceProvidedMo:
                        result.filling_fact = 1;
                        break;
                    case TypeOfProvisionServiceDi.ServiceProvidedWithoutMo:
                        result.filling_fact = 1;
                        result.service_method = 3;
                        break;
                    case TypeOfProvisionServiceDi.ServiceNotAvailable:
                        result.filling_fact = 2;
                        break;
                }

                if (result.filling_fact == 1 && result.service_method != 3)
                {
                    var managingOrgType = diRelationsDomain.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == communalService.DisclosureInfoRealityObj.Id)
                        .Select(x => x.DisclosureInfo.ManagingOrganization.TypeManagement)
                        .FirstOrDefault();

                    result.service_method = managingOrgType == TypeManagementManOrg.UK
                           ? 1
                           : managingOrgType == TypeManagementManOrg.JSK || managingOrgType == TypeManagementManOrg.TSJ
                               ? (int?)2
                               : null;
                }

                if (result.filling_fact == 2 || result.filling_fact == 1 && result.service_method == 3)
                {
                    result.volumes_report = null;
                }
                else
                {
                    var unitMeasure = communalService.UnitMeasure ?? communalService.TemplateService.UnitMeasure;

                    var infoAboutPaymentCommunal = infoAboutPaymentCommunalDomain.GetAll()
                        .FirstOrDefault(x => x.DisclosureInfoRealityObj.Id == communalService.DisclosureInfoRealityObj.Id 
                        && x.BaseService.Id == communalService.Id);

                    result.volumes_report = new HouseCommunalServiceVolumesReport
                    {
                        unit_of_measurement = this.ConvertUnitOfMeasure(unitMeasure.Name),
                        total_volume = infoAboutPaymentCommunal?.TotalConsumption,
                        accrued_consumer = infoAboutPaymentCommunal?.Accrual,
                        cash_to_provider_payment = infoAboutPaymentCommunal?.AccrualByProvider,
                        paid_by_consumers_amount = infoAboutPaymentCommunal?.Payed,
                        consumer_arrears = infoAboutPaymentCommunal?.Debt,
                        paid_to_supplier_amount = infoAboutPaymentCommunal?.PayedToProvider,
                        arrear_to_supplier_amount = infoAboutPaymentCommunal?.DebtToProvider,
                        total_penalties = infoAboutPaymentCommunal?.ReceivedPenaltySum
                    };
                }

                if (result.filling_fact == 2)
                {
                    return;
                }


                if (communalService.TariffForConsumers != null)
                {
                    result.tariff_description = communalService.TariffForConsumers.ToString();
                }

                result.supplied_via_management_organization = communalService.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo;

                if (result.supplied_via_management_organization.GetValueOrDefault())
                {
                    result.provider_inn = null;
                    result.provider_name = null;
                    result.provider_additional_info = null;
                    result.supply_contract_date = null;
                    result.supply_contract_number = null;
                }
                else
                {
                    var provider = providerService.GetAll().FirstOrDefault(x => x.BaseService.Id == communalService.Id);

                    result.provider_inn = communalService.Provider.Inn;
                    result.provider_name = communalService.Provider.Name;
                    if (provider != null)
                    {
                        result.provider_additional_info = provider.Description;
                        result.supply_contract_date = provider.DateStartContract;
                        result.supply_contract_number = provider.NumberContract;
                    }
                }

                var costs = new List<HouseCommunalServiceCost>();
                foreach (var tariff in tariffService.GetAll().Where(x => x.BaseService.Id==communalService.Id).ToArray())
                {
                    var cost = new HouseCommunalServiceCost();
                    costs.Add(cost);

                    cost.tariff = tariff.Cost;
                    cost.tariff_start_date = tariff.DateStart;
                    var unitMeasure = communalService.UnitMeasure ?? communalService.TemplateService.UnitMeasure;
                    cost.unit_of_measurement = this.ConvertUnitOfMeasure(unitMeasure.Name);
                }

                result.costs = costs.ToArray();

                var npa = npaService.GetAll().OrderByDescending(x => x.NpaDate).FirstOrDefault(x => x.BaseService.Id == communalService.Id);
                if (npa != null)
                {
                    result.legal_act_of_tariff_date = npa.NpaDate;
                    result.legal_act_of_tariff_number = npa.NpaNumber;
                    result.legal_act_of_tariff_org_name = npa.NpaAcceptor;
                }
                
                decimal consumptionNorm;
                result.consumption_norm = communalService.ConsumptionNormLivingHouse;
                
                result.consumption_norm_unit_of_measurement = null;
                if (communalService.UnitMeasureLivingHouse != null)
                {
                    result.consumption_norm_unit_of_measurement = this.ConvertUnitOfMeasure(communalService.UnitMeasureLivingHouse.Name)
                                                                  ?? this.ConvertUnitOfMeasure(communalService.UnitMeasureLivingHouse.ShortName);
                }

                result.consumption_norm_additional_info = communalService.AdditionalInfoLivingHouse;

                if (decimal.TryParse(communalService.ConsumptionNormHousing, out consumptionNorm))
                {
                    result.consumption_norm_on_common_needs = consumptionNorm;
                }

                result.consumption_norm_on_common_needs_unit_of_measurement = null;
                if (communalService.UnitMeasureHousing != null)
                {
                    result.consumption_norm_on_common_needs_unit_of_measurement = this.ConvertUnitOfMeasure(communalService.UnitMeasureHousing.Name)
                                                                                  ?? this.ConvertUnitOfMeasure(communalService.UnitMeasureHousing.ShortName);
                }

                result.consumption_norm_on_common_needs_additional_info = communalService.AdditionalInfoHousing;

                result.normative_acts =
                    npaService.GetAll()
                              .Where(x => x.BaseService.Id == communalService.Id)
                              .Select(x => new HouseCommunalServiceNormativeAct { document_date = x.NpaDate, document_number = x.NpaNumber, document_organization_name = x.NpaAcceptor })
                              .ToArray();

                var repairService = repairServiceService.GetAll().FirstOrDefault(x => x.Id == communalService.Id);
                if (repairService != null)
                {
                    // result.stop_reason_type = repairService.DateEnd.HasValue ? 1 : (int?)null;
                    result.stop_reason = repairService.RejectCause;
                    result.date_stop = repairService.DateEnd;
                }
            }
            finally
            {
                this.Container.Release(providerService);
                this.Container.Release(tariffService);
                this.Container.Release(npaService);
                this.Container.Release(infoAboutPaymentCommunalDomain);
                this.Container.Release(diRelationsDomain);
            }
        }

        private void FillDataByRobjectServices988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            var serviceDomain = this.Container.ResolveDomain<BaseService>();
            var housingServiceDomain = this.Container.ResolveDomain<HousingService>();
            var capRepairServiceDomain = this.Container.ResolveDomain<CapRepairService>();
            var repairServiceDomain = this.Container.ResolveDomain<RepairService>();
            var controlServiceDomain = this.Container.ResolveDomain<ControlService>();
            try
            {
                var services = serviceDomain.GetAll()
                    .Where(x => x.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Housing)
                    .Where(x => x.DisclosureInfoRealityObj.Id == di.Id)
                    .Where(x => 
                        housingServiceDomain.GetAll().Any(y => y.Id == x.Id && 
                            (y.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo 
                                || y.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo)) ||

                        capRepairServiceDomain.GetAll().Any(y => y.Id == x.Id &&
                            (y.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo 
                                || y.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo)) ||

                        repairServiceDomain.GetAll().Any(y => y.Id == x.Id &&
                            (y.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo 
                                || y.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo)) || 

                        controlServiceDomain.GetAll().Any(y => x.Id == y.Id)
                    );


                var existsServices = result.services ?? new HouseService[0];
                var houseServices = new List<HouseService>();

                foreach (var service in services)
                {
                    string name;
                    var nameId = RobjectDataCollector.GetServiceName(service, out name);

                    var houseService = existsServices.FirstOrDefault(x => x.name == nameId && (x.name == 14 && x.name_other == name || x.name_other == null)) ??
                        new HouseService
                        {
                            name = nameId,
                            name_other = nameId == 14 ? name : null
                        };

                    houseServices.Add(houseService);
                    this.FillDataByRobjectService988(houseService, service, di);
                    this.FillDataByRobjectServiceReport988(houseService.report, service);
                }


                var oldServices = existsServices.Except(houseServices);
                var enumerable = oldServices as IList<HouseService> ?? oldServices.ToList();
                foreach (var service in enumerable.Where(x => !x.stop_reason_type.HasValue))
                {
                    // Услуга была включена в список предоставляемых услуг по ошибке
                    service.stop_reason_type = 2;
                    service.date_stop = null;
                    service.stop_reason = null;
                    service.report = null;
                }

                result.services = houseServices.Union(enumerable).ToArray();
            }
            finally
            {
                this.Container.Release(serviceDomain);
                this.Container.Release(capRepairServiceDomain);
                this.Container.Release(repairServiceDomain);
                this.Container.Release(controlServiceDomain);
                this.Container.Release(housingServiceDomain);
            }
        }

        private void FillDataByRobjectServiceReport988(HouseServiceReport report, BaseService service)
        {
            var baseServiceDomain = this.Container.ResolveDomain<BaseService>();
            var tarifDomain = this.Container.ResolveDomain<TariffForConsumers>();
            var workCapRepairDomain = this.Container.ResolveDomain<WorkCapRepair>();
            var costItemDomain = this.Container.ResolveDomain<CostItem>();
            try
            {
                var volumes = new List<HouseServiceReportVolume>();
                List<KeyValuePair<string, decimal?>> volumesDict;

                if (service.TemplateService.KindServiceDi == KindServiceDi.Housing)
                {
                    volumesDict = costItemDomain.GetAll()
                        .Where(x => x.BaseService.Id == service.Id)
                        .GroupBy(x => x.Name)
                        .ToDictionary(x => x.Key, x => x.Where(y => y.Cost != null).Select(y => y.Cost).FirstOrDefault())
                        .ToList();
                }
                else
                {
                    volumesDict = tarifDomain.GetAll()
                        .Where(x => x.BaseService.Id == service.Id)
                        .AsEnumerable()
                        .Select(x => new KeyValuePair<string, decimal?>(service.TemplateService.Name,  x.Cost))
                        .ToList();
                }

                report.fact_cost_per_unit = service.Profit;

                foreach (var kvp in volumesDict)
                {
                    var volume = new HouseServiceReportVolume();
                    volumes.Add(volume);

                    volume.name = kvp.Key;

                    volume.unit_of_measurement = null;
                    if (service.UnitMeasure != null)
                    {
                        volume.unit_of_measurement = this.ConvertUnitOfMeasure(service.UnitMeasure.Name) ?? this.ConvertUnitOfMeasure(service.UnitMeasure.ShortName);
                    }

                    volume.cost_per_unit = kvp.Value;

                    var housingService = service as HousingService;
                    if (housingService != null)
                    {
                        volume.periodicity = null;
                        if (housingService.Periodicity != null)
                        {
                            RobjectDataCollector.GetPeriodicyValue(housingService, volume);
                        }
                    }
                }

                report.volumes = volumes.ToArray();
            }
            finally
            {
                this.Container.Release(baseServiceDomain);
                this.Container.Release(tarifDomain);
                this.Container.Release(workCapRepairDomain);
                this.Container.Release(costItemDomain);
            }
        }

        private static void GetPeriodicyValue(HousingService housingService, HouseServiceReportVolume volume)
        {
            switch (housingService.Periodicity.Name.ToLower())
            {
                case "ежедневно":
                    volume.periodicity = 1;
                    break;
                case "1 сутки":
                    volume.periodicity = 1;
                    break;
                case "6 лет":
                    volume.periodicity = 5;
                    break;
                case "10-30 лет":
                    volume.periodicity = 5;
                    break;
                case "6-8 лет":
                    volume.periodicity = 5;
                    break;
                case "от 10 лет":
                    volume.periodicity = 5;
                    break;
                case "40 лет":
                    volume.periodicity = 5;
                    break;
                case "15-40 лет":
                    volume.periodicity = 5;
                    break;
                case "1 раз в сутки":
                    volume.periodicity = 6;
                    break;
                case "3 раза в сутки":
                    volume.periodicity = 7;
                    break;
                case "2 раза в сутки":
                    volume.periodicity = 7;
                    break;
                case "1 раз в неделю":
                    volume.periodicity = 8;
                    break;
                case "2 раза в неделю":
                    volume.periodicity = 9;
                    break;
                case "до 7 суток":
                    volume.periodicity = 9;
                    break;
                case "1 раз в месяц":
                    volume.periodicity = 10;
                    break;
                case "2 раза в месяц":
                    volume.periodicity = 11;
                    break;
                case "до 30 суток":
                    volume.periodicity = 11;
                    break;
                case "1 раз в год":
                    volume.periodicity = 14;
                    break;
                case "2 раза в год":
                    volume.periodicity = 15;
                    break;
                case "1 раз в сутки во время гололеда":
                    volume.periodicity = 16;
                    break;
                case "1 раз в двое суток в дни без снегопада":
                    volume.periodicity = 17;
                    break;
                case "1 раз в трое суток во время гололеда":
                    volume.periodicity = 18;
                    break;
                case "1 раз в сутки в дни снегопада":
                    volume.periodicity = 20;
                    break;
                case "2 раза в холодный период":
                    volume.periodicity = 21;
                    break;
                case "по мере необходимости, при проведении текущего и капитального ремонта":
                    volume.periodicity = 24;
                    break;
                case "при проведении текущего и капитального ремонта":
                    volume.periodicity = 25;
                    break;
                case "в малых объемах при подготовке к зиме":
                    volume.periodicity = 26;
                    break;
                case "при подготовке к зиме":
                    volume.periodicity = 26;
                    break;
                case "по мере необходимости":
                    volume.periodicity = 27;
                    break;
                case "немедленно":
                    volume.periodicity = 27;
                    break;
                case "немедленное принятие мер с недопущением развития утраты несущей способности":
                    volume.periodicity = 27;
                    break;
                case "по мере необходимости, в кратчайшие сроки":
                    volume.periodicity = 27;
                    break;
                case "немедленное принятие мер, с дальнейшим выполнением работ по проекту":
                    volume.periodicity = 27;
                    break;
                case "немедленно с принятием мер безопасности":
                    volume.periodicity = 27;
                    break;
                case "по графику":
                    volume.periodicity = 28;
                    break;
                case "по графику, но не реже 1 раза в месяц":
                    volume.periodicity = 27;
                    break;
                case "по мере выявления":
                    volume.periodicity = 29;
                    break;
                case "по мере выявления, 1 сутки":
                    volume.periodicity = 27;
                    break;
                case "по мере выявления, не допуская их дальнейшего развития":
                    volume.periodicity = 27;
                    break;
                case "по мере выявления дефектов, не нарушая целостности конструкции":
                    volume.periodicity = 27;
                    break;
                case "2 раза в двое суток":
                    volume.periodicity = 32;
                    break;
                case "1 раз в двое суток":
                    volume.periodicity = 32;
                    break;
                case "1 раз в трое суток":
                    volume.periodicity = 32;
                    break;
                case "5 раз в теплый период":
                    volume.periodicity = 34;
                    break;
                case "3 раза в теплый период":
                    volume.periodicity = 34;
                    break;
                case "1 раз в двое суток (50% территорий)":
                    volume.periodicity = 36;
                    break;
                case "3 часа":
                    volume.periodicity = 36;
                    break;
                case "1-3 суток":
                    volume.periodicity = 36;
                    break;
                case "по результатам технического освидетельствования":
                    volume.periodicity = 36;
                    break;
                case "согласно проектных решений":
                    volume.periodicity = 36;
                    break;
                case "7 суток":
                    volume.periodicity = 36;
                    break;
                case "3-8 лет":
                    volume.periodicity = 36;
                    break;
                case "в кратчайшие сроки":
                    volume.periodicity = 36;
                    break;
                case "5 суток":
                    volume.periodicity = 36;
                    break;
            }
        }

        private void FillDataByRobjectService988(HouseService houseService, BaseService service, DisclosureInfoRealityObj di)
        {
            var repairServiceService = this.Container.ResolveDomain<RepairService>();
            var workCapRepairDomain = this.Container.ResolveDomain<WorkCapRepair>();
            var tariffForConsumersDomain = this.Container.ResolveDomain<TariffForConsumers>();
            var costItemDomain = this.Container.ResolveDomain<CostItem>();
            try
            {
                var repairService = repairServiceService.GetAll().FirstOrDefault(x => x.Id == service.Id);

                if (repairService != null && !houseService.stop_reason_type.HasValue)
                {
                    houseService.stop_reason_type = repairService.DateEnd.HasValue ? (int?)1 : null;
                    houseService.date_stop = repairService.DateEnd;
                    houseService.stop_reason = repairService.DateEnd.HasValue
                        ? repairService.RejectCause.IsNotEmpty()
                            ? repairService.RejectCause
                            : "Срок действия предоставления услуги истек"
                        : null;
                }

                var listData = new List<HouseServiceCost>();

                var errorMessage = $"Для услуги \"{service.TemplateService.Name}\" не заполнено обязательное поле";
                switch (service.TemplateService.KindServiceDi)
                {
                    case KindServiceDi.Housing:
                        var housingTariff = costItemDomain.GetAll()
                            .FirstOrDefault(x => x.BaseService.Id == service.Id && x.Sum != null);

                        housingTariff.HasValue(x => x.Sum, errorMessage);
                        listData.Add(new HouseServiceCost
                        {
                            year = di.PeriodDi.DateStart.GetValueOrDefault(DateTime.UtcNow).Year,
                            plan_cost_per_unit = housingTariff?.Sum
                        });
                        break;

                    case KindServiceDi.Repair:
                        repairService.HasValue(x => x.SumFact, errorMessage);
                        listData.Add(new HouseServiceCost
                        {
                            year = di.PeriodDi.DateStart.GetValueOrDefault(DateTime.UtcNow).Year,
                            plan_cost_per_unit = repairService?.SumFact
                        });
                        break;

                    case KindServiceDi.CapitalRepair:
                        var capRepairTariff = workCapRepairDomain.GetAll()
                            .FirstOrDefault(x => x.BaseService.Id == service.Id && x.PlannedCost != null);

                        capRepairTariff.HasValue(x => x.PlannedCost, errorMessage);
                        listData.Add(
                            new HouseServiceCost
                            {
                                year = di.PeriodDi.DateStart.GetValueOrDefault(DateTime.UtcNow).Year,
                                plan_cost_per_unit = capRepairTariff.PlannedCost
                            });
                        
                        break;

                    case KindServiceDi.Managing:
                        var managingTariff = tariffForConsumersDomain.GetAll()
                            .FirstOrDefault(x => x.BaseService.Id == service.Id && x.Cost != null);

                        managingTariff.HasValue(x => x.Cost, errorMessage);
                        listData.Add(
                            new HouseServiceCost
                            {
                                year = di.PeriodDi.DateStart.GetValueOrDefault(DateTime.UtcNow).Year,
                                plan_cost_per_unit = managingTariff.Cost
                            });
                        
                        break;
                }

                houseService.costs = listData.ToArray();

                houseService.report = new HouseServiceReport();
            }
            finally
            {
                this.Container.Release(repairServiceService);
                this.Container.Release(workCapRepairDomain);
                this.Container.Release(tariffForConsumersDomain);
                this.Container.Release(costItemDomain);
            }
        }

        /// <summary>
        /// Сопоставление типа услуги в нашей системе с Реформой.
        /// <remarks>Если в Реформе отсутствуует нужный тип, то мы отправляем также наименование услуги в нашей системе</remarks>
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <param name="name">Наименование услуги (выходной параметр)</param>
        /// <returns></returns>
        private static int GetServiceName(BaseService service, out string name)
        {
            name = service?.TemplateService?.Name;
            if (name != null)
            {
                switch (name.ToLower())
                {
                    case "управление жилым домом":
                        return 1;
                    case "уборка внутридомовых мест общего пользования":
                        return 2;
                    case "вывоз тбо":
                        return 3;
                    case "вывоз жбо":
                        return 14;
                    case "утилизация твердых бытовых отходов":
                        return 14;
                    case "текущий ремонт и содержание внутридомовых инженерных сетей водоснабжения и водоотведения":
                        return 14;
                    case "текущий ремонт и содержание внутридомовых инженерных сетей центрального отопления":
                        return 14;
                    case "текущий ремонт и содержание внутридомовых инженерных сетей электроснабжения":
                        return 14;
                    case "обслуживание мусоропроводов":
                        return 6;
                    case "содержание лифтов":
                        return 7;
                    case "пользование лифтом":
                        return 14;
                    case "обслуживание, ремонт технических средств и систем пожаротушения и дымоудаления":
                        return 9;
                    case "текущий ремонт и содержание внутридомовых инженерных сетей газоснабжения":
                        return 10;
                    case "дератизация":
                        return 12;
                    case "текущий ремонт жилого здания и благоустройство территории":
                        return 13;
                }
            }
            return 14;
        }

        private void FillDataByRobjectDevices988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            var devices = this.GetRobjectDevices(di);
            var houseMeteringDevices = new List<HouseMeteringDevice>();
            foreach (var device in devices)
            {
                if (device.TypeCommResourse.IsNullOrEmpty())
                {
                    continue;
                }

                var type = 0;
                switch (device.TypeCommResourse.ToLower())
                {
                    case "газоснабжение":
                        type = 6;
                        break;
                    case "отопление":
                        type = 5;
                        break;
                    case "электроснабжение":
                        type = 4;
                        break;
                    case "водоотведение":
                        type = 3;
                        break;
                    case "горячее водоснабжение":
                        type = 2;
                        break;
                    case "холодное водоснабжение":
                        type = 1;
                        break;
                    case "холодная вода для нужд гвс":
                        type = 7;
                        break;
                    case "тепловая энергия для подогрева холодной воды для нужд гвс":
                        type = 8;
                        break;
                    case "газоснабжение для подогрева холодной воды для нужд гвс":
                        type = 9;
                        break;
                }

                var meteringDevice = result.metering_devices.Return(d => d.FirstOrDefault(x => x.communal_resource_type == type)) ?? 
                    new HouseMeteringDevice { communal_resource_type = type, is_default = type <= 6 };
                houseMeteringDevices.Add(meteringDevice);
                this.FillDataByRobjectDevice988(meteringDevice, device);
            }

            if (result.metering_devices.IsNotEmpty())
            {
                houseMeteringDevices.AddRange(result.metering_devices.Where(x => new int?[] { 1, 2, 3, 4, 5, 6 }.Except(houseMeteringDevices.Select(y => y.communal_resource_type)).Contains(x.communal_resource_type)));
            }

            result.metering_devices = houseMeteringDevices.ToArray();
        }

        private void FillDataByRobjectDevice988(HouseMeteringDevice meteringDevice, DisclosureInfoRealityObjService.RealtyObjectDevice device)
        {
            meteringDevice.availability = null;
            if (device.ExistMeterDevice != null)
            {
                switch (device.ExistMeterDevice.ToLower())
                {
                    case "отсутствует, установка не требуется":
                        meteringDevice.availability = 1;
                        break;
                    case "отсутствует, требуется установка":
                        meteringDevice.availability = 2;
                        break;
                    case "установлен":
                        meteringDevice.availability = 3;
                        break;
                }
            }

            meteringDevice.meter_type = null;
            if (device.InterfaceType != null)
            {
                switch (device.InterfaceType.ToLower())
                {
                    case "без интерфейса передачи данных":
                        meteringDevice.meter_type = 1;
                        break;

                    case "с интерфейсом передачи данных":
                        meteringDevice.meter_type = 2;
                        break;
                }
            }

            meteringDevice.unit_of_measurement = this.ConvertUnitOfMeasure(device.UnutOfMeasure);

            meteringDevice.commissioning_date = null;
            if (device.InstallDate != null)
            {
                DateTime installDate;
                if (DateTime.TryParse(device.InstallDate, out installDate))
                {
                    meteringDevice.commissioning_date = installDate;
                }
            }

            meteringDevice.calibration_date = null;
            if (device.CheckDate != null)
            {
                DateTime checkDate;
                if (DateTime.TryParse(device.CheckDate, out checkDate))
                {
                    meteringDevice.calibration_date = checkDate;
                }
            }
        }

        private void FillDataByRobjectPassport988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            var passport = this.GetRobjectPassport(di);

            result.area_total = passport.AreaMkd;
            result.area_residential = passport.AreaLiving;
            result.area_non_residential = passport.AreaNotLivingPremises;
            result.area_common_property = passport.AreaOfAllNotLivingPremises;
            result.is_alarm = di.RealityObject.ConditionHouse == ConditionHouse.Emergency;

            switch (passport.RawTypeOfFormingCr)
            {
                case CrFundFormationType.RegOpAccount:
                    result.method_of_forming_overhaul_fund = 3;
                    break;
                case CrFundFormationType.SpecialRegOpAccount:
                    result.method_of_forming_overhaul_fund = 2;
                    break;
                case CrFundFormationType.SpecialAccount:
                    result.method_of_forming_overhaul_fund = 1;
                    break;
                case CrFundFormationType.Unknown:
                    result.method_of_forming_overhaul_fund = 4;
                    break;
                default:
                    result.method_of_forming_overhaul_fund = null;
                    break;
            }

            result.exploitation_start_year = passport.DateCommissioning?.Year;

            result.project_type = passport.TypeOfProject;
            result.built_year = passport.BuildYear;

            switch (passport.TypeHouse)
            {
                case TypeHouse.BlockedBuilding:
                    result.house_type = 2;
                    break;
                case TypeHouse.Individual:
                    result.house_type = 4;
                    break;
                case TypeHouse.ManyApartments:
                    result.house_type = 1;
                    break;
                case TypeHouse.SocialBehavior:
                    result.house_type = 3;
                    break;
                default:
                    result.house_type = null;
                    break;
            }

            result.floor_count_max = passport.MaximumFloors;
            result.floor_count_min = passport.Floors;
            result.entrance_count = passport.NumberEntrances;
            if (result.entrance_count == 0)
            {
                result.entrance_count = null;
            }

            result.elevators_count = passport.NumberLifts;
            result.flats_count = passport.AllNumberOfPremises;
            result.living_quarters_count = passport.NumberApartments;
            result.not_living_quarters_count = passport.NumberNonResidentialPremises;
            result.area_land = passport.DocumentBasedArea;
            result.parking_square = passport.ParkingArea;

            result.energy_efficiency = null;
            switch (passport.EnergyEfficiencyClass.ToLower())
            {
                case "не присвоен":
                    result.energy_efficiency = 1;
                    break;
                case "a":
                    result.energy_efficiency = 2;
                    break;
                case "b++":
                    result.energy_efficiency = 3;
                    break;
                case "b+":
                    result.energy_efficiency = 4;
                    break;
                case "c":
                    result.energy_efficiency = 5;
                    break;
                case "d":
                    result.energy_efficiency = 6;
                    break;
                case "e":
                    result.energy_efficiency = 7;
                    break;
                case "b":
                    result.energy_efficiency = 8;
                    break;
            }

            result.has_playground = passport.ChildrenArea.ToLower() == "имеется" ? true : (passport.ChildrenArea.ToLower() == "не имеется" ? false : (bool?)null);

            result.has_sportsground = passport.SportArea.ToLower() == "имеется" ? true : (passport.SportArea.ToLower() == "не имеется" ? false : (bool?)null);

            result.other_beautification = passport.OtherArea;

            if (!string.IsNullOrEmpty(passport.CadastreNumber))
            {
                result.cadastral_numbers = new[] { new HouseCadastralNumber { cadastral_number = passport.CadastreNumber } };
            }
        }

        private void FillDataByRobjectStructElements988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            var structElements = this.GetRobjectStructElements(di);

            result.foundation_type = null;
            if (structElements.BasementType != null)
            {
                switch (structElements.BasementType.ToLower())
                {
                    // TODO если появятся типы фундамента: Бетонные столбы, Свайный, Иной, то добавить в switch
                    case "ленточный":
                        result.foundation_type = 1;
                        break;
                    case "столбчатый":
                        result.foundation_type = 5;
                        break;
                    case "сплошной":
                        result.foundation_type = 6;
                        break;
                    case "сборный":
                        result.foundation_type = 7;
                        break;
                    default:
                        result.foundation_type = 8;
                        break;
                }
            }

            result.floor_type = structElements.TypeFloorReformaCode;
            result.wall_material = structElements.TypeWallsReformaCode;
            

            result.area_basement = null;
            if (!string.IsNullOrEmpty(structElements.BasementArea))
            {
                decimal areaBasement;
                if (decimal.TryParse(structElements.BasementArea, out areaBasement))
                {
                    result.area_basement = areaBasement;
                }
            }

            result.chute_type = structElements.ConstructionChuteReformaCode;

            result.chute_count = null;
            if (!string.IsNullOrEmpty(structElements.ChutesNumber))
            {
                int chutesNumber;
                if (int.TryParse(structElements.ChutesNumber, out chutesNumber))
                {
                    result.chute_count = chutesNumber;
                }
            }

            if (structElements.Facades != null)
            {
                result.facades = structElements.Facades.Where(x => x.FacadeType != null).Select(x =>
                {
                    var facade = new HouseFacade();
                    switch (x.FacadeType.ToLower())
                    {
                        case "соответствует материалу стен":
                            facade.type = 1;
                            break;
                        case "оштукатуренный":
                            facade.type = 2;
                            break;
                        case "облицованный плиткой":
                            facade.type = 4;
                            break;
                        case "окрашенный":
                            facade.type = 3;
                            break;
                        case "сайдинг":
                            facade.type = 6;
                            break;
                    }
                    return facade;
                }).Where(x => x.type != null).ToArray();
            }

            if (structElements.RoofTypes != null)
            {
                result.roofs = structElements.RoofTypes.Select(x =>
                {
                    var roof = new HouseRoof();
                    if (x.RoofType != null)
                    {
                        switch (x.RoofType.ToLower())
                        {
                            case "плоская":
                            case "вальмовая сложной формы":
                            case "полувальмовая":
                            case "шатровая":
                            case "вальмовая":
                                roof.roof_type = 1;
                                break;
                            case "скатная":
                            case "двускатная":
                            case "односкатная":
                                roof.roof_type = 2;
                                break;
                        }
                    }

                    if (x.RoofingType != null)
                    {
                        switch (x.RoofingType.ToLower())
                        {
                            case "шифер":
                            case "шиферная":
                                roof.roofing_type = 1;
                                break;
                            case "сталь оцинкованная":
                                roof.roofing_type = 2;
                                break;
                            case "черепица":
                            case "металлическая":
                                roof.roofing_type = 3;
                                break;
                            case "сталь черная":
                                roof.roofing_type = 4;
                                break;
                            case "рубероид":
                                roof.roofing_type = 5;
                                break;
                            case "мягкая (наплавляемая)":
                            case "наплавляемый материал":
                                roof.roofing_type = 6;
                                break;
                            default:
                                roof.roofing_type = 7;
                                break;
                        }
                    }

                    return roof;
                }).Where(x => x.roof_type != null || x.roofing_type != null).ToArray();
            }
        }

        private void FillDataByRobjectEngineerSystems988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            var engineerSystems = this.GetRobjectEngineerSystems(di);
            result.electrical_type = engineerSystems.TypePowerReformaCode;

            result.electrical_entries_count = null;
            if (engineerSystems.TypePowerPoints != null)
            {
                int powerPoints;
                if (int.TryParse(engineerSystems.TypePowerPoints, out powerPoints))
                {
                    result.electrical_entries_count = powerPoints;
                }
            }

            result.heating_type = engineerSystems.TypeHeatingReformaCode;
            result.hot_water_type = engineerSystems.TypeHotWaterReformaCode;
            result.cold_water_type = engineerSystems.TypeColdWaterReformaCode;
            result.sewerage_type = engineerSystems.TypeSewageReformaCode;

            result.sewerage_cesspools_volume = null;
            if (engineerSystems.SewageVolume != null)
            {
                decimal sewerageVolume;
                if (decimal.TryParse(engineerSystems.SewageVolume, out sewerageVolume))
                {
                    result.sewerage_cesspools_volume = sewerageVolume;
                }
            }

            result.gas_type = engineerSystems.TypeGasReformaCode;
            result.ventilation_type = engineerSystems.TypeVentilationReformaCode;
            result.firefighting_type = engineerSystems.FirefightingReformaCode;
            result.drainage_type = engineerSystems.TypeDrainageReformaCode;
        }

        private void FillDataByRobjectLifts988(HouseProfileData988 result, DisclosureInfoRealityObj di)
        {
            var lifts = this.GetRobjectLifts(di);
            var houseLifts = new List<HouseLift>();

            foreach (var lift in lifts)
            {
                var houseLift = new HouseLift();

                houseLift.porch_number = lift.EntranceNumber;

                houseLift.type = null;
                var liftType = lift.Type.FirstOrDefault();
                if (liftType != null)
                {
                    switch (liftType.ToLower())
                    {
                        case "пассажирский":
                            houseLift.type = 1;
                            break;
                        case "грузовой":
                            houseLift.type = 2;
                            break;
                        case "грузо-пассажирский":
                            houseLift.type = 3;
                            break;
                    }
                }

                houseLift.commissioning_year = null;
                if (lift.CommissioningYear != null)
                {
                    int commissioningYear;
                    if (int.TryParse(lift.CommissioningYear, out commissioningYear))
                    {
                        houseLift.commissioning_year = commissioningYear;
                    }
                }

                houseLifts.Add(houseLift);
            }

            result.lifts = houseLifts.ToArray();
        }

        private void FillManagementContract988(
            HouseProfileData988 result,
            List<ICollectedFile<HouseProfileData988>> collectedFiles,
            RealityObject robject,
            ReportingPeriodDict reportingPeriod,
            long manOrgId)
        {
            var service = this.Container.Resolve<IRobjectService>();
            var refService = this.Container.ResolveDomain<RefRealityObject>();
            try
            {
                var management = service.GetManagingHistory(robject.Id).LastOrDefault(x => x.ManOrgId == manOrgId);
                
                if (management == null)
                {
                    return;
                }

                var contract = new HouseManagementContract
                {
                    date_start = management.DateStart,
                    management_reason = management.ContractFoundation > 0
                        ? management.ContractFoundation.GetDisplayName()
                        : management.DocumentName.IsNotEmpty()
                            ? management.DocumentName
                            : management.DocumentFile?.Name,

                    confirm_method_document_date = management.DocumentDate,
                    confirm_method_document_number = management.DocumentNumber,
                    management_contract_date = management.DateStart
                };

                contract.confirm_method_document_name = contract.management_reason;
                result.management_contract = contract;

                collectedFiles.Add(
                    new CollectedFile<HouseProfileData988>(
                        this.Container,
                        management.DocumentFile,
                        reportingPeriod,
                        (house, id) => house.management_contract.management_contract_files = new[] { id }));
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(refService);
            }
        }

        private int? ConvertUnitOfMeasure(string measure)
        {
            int? value = null;
            if (measure != null)
            {
                switch (measure.ToLower())
                {
                    case "кв.м":
                        value = 1;
                        break;
                    case "пог.м":
                        value = 2;
                        break;
                    case "шт.":
                        value = 3;
                        break;
                    case "куб.м":
                    case "куб.метр":
                        value = 4;
                        break;
                    case "гкал":
                        value = 5;
                        break;
                    case "гкал/кв.м":
                        value = 6;
                        break;
                    case "гкал/час":
                        value = 7;
                        break;
                    case "гкал*час/кв.м":
                        value = 8;
                        break;
                    case "гкал/год":
                        value = 9;
                        break;
                    case "чел.":
                        value = 10;
                        break;
                    case "ед.":
                        value = 11;
                        break;
                    case "руб.":
                        value = 12;
                        break;
                    case "%":
                        value = 13;
                        break;
                    case "°с*сут":
                        value = 14;
                        break;
                    case "км":
                        value = 15;
                        break;
                    case "куб.м/сут.":
                        value = 16;
                        break;
                    case "куб.м/квартира":
                        value = 17;
                        break;
                    case "куб.м/чел.в мес.":
                        value = 18;
                        break;
                    case "вт/куб.м":
                        value = 19;
                        break;
                    case "квт":
                        value = 20;
                        break;
                    case "ква":
                        value = 21;
                        break;
                    case "вт/(куб.м*°с)":
                        value = 22;
                        break;
                    case "час":
                        value = 23;
                        break;
                    case "дн.":
                        value = 24;
                        break;
                    case "тыс.руб.":
                        value = 25;
                        break;
                    case "м":
                        value = 26;
                        break;
                    case "кг":
                        value = 27;
                        break;
                    case "кг/куб.м":
                        value = 28;
                        break;
                    case "мвт":
                        value = 29;
                        break;
                    case "квт/куб.м":
                        value = 30;
                        break;
                    case "квт/ч":
                    case "квт/час":
                        value = 31;
                        break;
                    case "квт*ч":
                        value = 32;
                        break;
                    case "руб/куб.м":
                        value = 33;
                        break;
                    case "куб.м/кв.м":
                        value = 34;
                        break;
                    case "квт.ч/кв.м":
                        value = 35;
                        break;
                    case "руб./гкал":
                        value = 36;
                        break;
                    case "руб./квт.ч":
                        value = 37;
                        break;
                    case "гкал/чел.":
                        value = 38;
                        break;
                    case "гкал/куб.м":
                        value = 39;
                        break;
                    case "квт/чел.":
                        value = 40;
                        break;
                    case "квт*ч/чел.в мес.":
                        value = 41;
                        break;
                    case "руб./1000куб.м.":
                        value = 42;
                        break;
                    case "куб.м/кв.м общ. имущества в мес.":
                        value = 43;
                        break;
                    case "квт*ч/кв.м общ. имущества в мес.":
                        value = 44;
                        break;
                    case "квт/кв.м":
                        value = 45;
                        break;
                    case "гкал/кв.м в мес.":
                        value = 46;
                        break;
                    case "гкал/кв.м. в год":
                        value = 47;
                        break;
                    case "руб./чел. в мес.":
                        value = 48;
                        break;
                    case "руб./кв.м":
                        value = 49;
                        break;
                    case "руб./кг":
                        value = 50;
                        break;
                    case "1 пролёт":
                        value = 51;
                        break;
                    case "10 погонных метров":
                        value = 52;
                        break;
                    case "10 шт.":
                        value = 53;
                        break;
                    case "100 кв.м.":
                        value = 54;
                        break;
                    case "100 куб.м.":
                        value = 55;
                        break;
                    case "100 погонных метров":
                        value = 56;
                        break;
                    case "100 шт.":
                        value = 57;
                        break;
                    case "1000 кв.м.":
                        value = 58;
                        break;
                    case "1 м фальца":
                        value = 59;
                        break;
                    case "квартира":
                        value = 60;
                        break;
                    case "комплекс работ/мес.":
                        value = 61;
                        break;
                    case "кв.м./мес.":
                        value = 62;
                        break;
                    case "погонный метр/мес.":
                        value = 63;
                        break;
                    case "шт./мес":
                        value = 64;
                        break;
                    case "этаж/мес.":
                        value = 65;
                        break;
                    case "куб.м./мес.":
                    case "м3/мес.":
                        value = 66;
                        break;
                    case "1000 куб.м.":
                        value = 67;
                        break;
                    case "руб./ч.":
                        value = 68;
                        break;
                    case "руб./шт.":
                        value = 69;
                        break;
                }
            }

            return value;
        }

        private DisclosureInfoRealityObjService.RealtyObjectPassport GetRobjectPassport(DisclosureInfoRealityObj robjectDisclosure)
        {
            var service = this.Container.Resolve<IDisclosureInfoRealityObjService>();
            try
            {
                var result = service.GetRealtyObjectPassport(robjectDisclosure.Id);
                if (!result.Success)
                {
                    throw new ReformaValidationException(result.Message);
                }

                return (DisclosureInfoRealityObjService.RealtyObjectPassport)result.Data;
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private DisclosureInfoRealityObjService.RealtyObjectStructElements GetRobjectStructElements(DisclosureInfoRealityObj robjectDisclosure)
        {
            var service = this.Container.Resolve<IDisclosureInfoRealityObjService>();
            try
            {
                var result = service.GetRealtyObjectStructElements(robjectDisclosure.Id);
                if (!result.Success)
                {
                    throw new ReformaValidationException(result.Message);
                }

                return (DisclosureInfoRealityObjService.RealtyObjectStructElements)result.Data;
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private DisclosureInfoRealityObjService.RealtyObjectEngineerSystems GetRobjectEngineerSystems(DisclosureInfoRealityObj robjectDisclosure)
        {
            var service = this.Container.Resolve<IDisclosureInfoRealityObjService>();
            try
            {
                var result = service.GetRealtyObjectEngineerSystems(robjectDisclosure.Id);
                if (!result.Success)
                {
                    throw new ReformaValidationException(result.Message);
                }

                return (DisclosureInfoRealityObjService.RealtyObjectEngineerSystems)result.Data;
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private List<DisclosureInfoRealityObjService.RealtyObjectDevice> GetRobjectDevices(DisclosureInfoRealityObj robjectDisclosure)
        {
            var service = this.Container.Resolve<IDisclosureInfoRealityObjService>();
            try
            {
                var result = service.GetRealtyObjectDevices(robjectDisclosure.Id);
                if (!result.Success)
                {
                    throw new ReformaValidationException(result.Message);
                }

                return (List<DisclosureInfoRealityObjService.RealtyObjectDevice>)result.Data;
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private List<DisclosureInfoRealityObjService.RealtyObjectLift> GetRobjectLifts(DisclosureInfoRealityObj robjectDisclosure)
        {
            var service = this.Container.Resolve<IDisclosureInfoRealityObjService>();
            try
            {
                var result = service.GetRealtyObjectLifts(robjectDisclosure.Id);
                if (!result.Success)
                {
                    throw new ReformaValidationException(result.Message);
                }

                return (List<DisclosureInfoRealityObjService.RealtyObjectLift>)result.Data;
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private DisclosureInfoRealityObj GetRobjectDisclosure(RefRealityObject robject, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            try
            {
                return service.GetAll().FirstOrDefault(x =>
                    x.RealityObject.Id == robject.RealityObject.Id && x.PeriodDi.Id == period.Id &&
                    x.ManagingOrganization.Contragent.Inn == robject.RefManagingOrganization.Inn);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private DisclosureInfoRealityObj GetRobjectDisclosure(RefRealityObject robject, PeriodDi period, long? manOrgId)
        {
            var service = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            try
            {
                return service.GetAll().FirstOrDefault(x =>
                    x.RealityObject.Id == robject.RealityObject.Id && x.PeriodDi.Id == period.Id &&
                    x.ManagingOrganization.Id == manOrgId);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}