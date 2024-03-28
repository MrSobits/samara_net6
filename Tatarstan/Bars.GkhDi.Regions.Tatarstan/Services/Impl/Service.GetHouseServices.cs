namespace Bars.GkhDi.Regions.Tatarstan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    using Bars.GkhDi.Services;
    using Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo;

    public partial class Service
    {
        public virtual GetHouseServicesResponse GetHouseServices(string houseId, string periodId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }

            var idHouse = houseId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idHouse == 0 || idPeriod == 0)
            {
                return new GetHouseServicesResponse { Result = Result.DataNotFound };
            }

            var disclosureInfoRealityObjectsDomain = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();
            var tariffForRsoDomain = this.Container.Resolve<IDomainService<TariffForRso>>();
            var tariffForConsumersDomain = this.Container.Resolve<IDomainService<TariffForConsumers>>();
            var baseServiceDomain = this.Container.Resolve<IDomainService<BaseService>>();
            var additionalServiceDomain = this.Container.Resolve<IDomainService<AdditionalService>>();
            var housingServiceDomain = this.Container.Resolve<IDomainService<HousingService>>();
            var communalServiceDomain = this.Container.Resolve<IDomainService<CommunalService>>();
            var workRepairTechServDomain = this.Container.Resolve<IDomainService<WorkRepairTechServ>>();
            var repairServiceDomain = this.Container.Resolve<IDomainService<RepairService>>();
            var workRepairDetailTatDomain = this.Container.Resolve<IDomainService<WorkRepairDetailTat>>();
            var workRepairListDomain = this.Container.Resolve<IDomainService<WorkRepairList>>();
            var manOrgContractRealityObjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var manOrgContractOwnersDomain = this.Container.Resolve<IDomainService<ManOrgContractOwners>>();
            var disclosureInfoRelationDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();

            using (this.Container.Using(disclosureInfoRealityObjectsDomain, tariffForRsoDomain, tariffForConsumersDomain, baseServiceDomain,
                additionalServiceDomain, housingServiceDomain, communalServiceDomain, workRepairTechServDomain, repairServiceDomain, workRepairDetailTatDomain, workRepairListDomain,
                manOrgContractRealityObjectDomain, manOrgContractOwnersDomain, disclosureInfoRelationDomain))
            {
                var diRealObjects = disclosureInfoRealityObjectsDomain.GetAll()
                    .Where(x => x.RealityObject.Id == idHouse && x.PeriodDi.Id == idPeriod);

                if (!diRealObjects.Any())
                {
                    return new GetHouseServicesResponse { Result = Result.DataNotFound };
                }

                List<ManagingOrgItem> managingOrgItems = null;
                foreach (var diRealObj in diRealObjects)
                {
                    var tariffRsoDict = tariffForRsoDomain
                        .GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .GroupBy(x => x.BaseService.Id)
                        .ToDictionary(x => x.Key, y => y.Where(z => z.DateStart < DateTime.Now).OrderByDescending(x => x.DateStart).FirstOrDefault());

                    var tariffForConsumers = tariffForConsumersDomain
                      .GetAll()
                      .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                      .GroupBy(x => x.BaseService.Id)
                      .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.DateStart).FirstOrDefault());

                    var managServs = baseServiceDomain.GetAll()
                        .Where(x => x.TemplateService.KindServiceDi == KindServiceDi.Managing && x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            x.Id,
                            TemplateServiceName = x.TemplateService.Name,
                            ProviderName = x.Provider.Name,
                            UnitMeasureName = x.UnitMeasure.Name
                        })
                        .AsEnumerable()
                        .Select(x => new ManagServ
                        {
                            Id = x.Id,
                            Name = x.TemplateServiceName,
                            Provider = x.ProviderName,
                            Measure = x.UnitMeasureName,
                            Cost = tariffForConsumers.ContainsKey(x.Id)
                                ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                : string.Empty
                        })
                        .ToArray();

                    var comServs = communalServiceDomain.GetAll()
                       .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                       .Select(x => new
                           {
                               x.Id,
                               TemplateServiceName = x.TemplateService.Name,
                               ProviderName = x.Provider.Name,
                               UnitMeasureName = x.UnitMeasure.Name,
                               x.VolumePurchasedResources
                           }).AsEnumerable()
                       .Select(x => new ComServ
                       {
                           Id = x.Id,
                           NameService = x.TemplateServiceName,
                           Provider = x.ProviderName,
                           Measure = x.UnitMeasureName,
                           Cost = tariffForConsumers.ContainsKey(x.Id)
                               ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat)
                               : string.Empty,
                           NameResOrg = x.ProviderName,
                           TypeUtilities = x.TemplateServiceName,
                           ResSize = x.VolumePurchasedResources.HasValue
                               ? x.VolumePurchasedResources.Value.RoundDecimal(2).ToString(numberformat)
                               : string.Empty,
                           Tariff = tariffForConsumers.ContainsKey(x.Id)
                               ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat)
                               : string.Empty,
                           DataNum = tariffRsoDict.ContainsKey(x.Id) && tariffRsoDict[x.Id] != null
                               ? tariffRsoDict[x.Id].NumberNormativeLegalAct
                               : string.Empty,
                           DataDate = tariffRsoDict.ContainsKey(x.Id) && tariffRsoDict[x.Id] != null
                               ? tariffRsoDict[x.Id].DateNormativeLegalAct.ToDateTime().ToShortDateString()
                               : string.Empty,
                           TarifSetOrg = tariffRsoDict.ContainsKey(x.Id) && tariffRsoDict[x.Id] != null
                               ? tariffRsoDict[x.Id].OrganizationSetTariff
                               : string.Empty,
                       })
                       .ToArray();

                    var works = workRepairTechServDomain.GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            BaseServiceId = x.BaseService.Id,
                            x.Id,
                            GroupName = x.WorkTo.GroupWorkTo.Name,
                            x.WorkTo.Name
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.BaseServiceId)
                        .ToDictionary(
                            x => x.Key,
                            y => new
                            {
                                NameGroup = String.Join(", ", y.Select(x => x.GroupName).Distinct()),
                                Works = y.Select(x => new Work { Id = x.Id, Name = x.Name }).ToArray()
                            });


                    var worksToDict = repairServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Where(x => x.SumWorkTo != null)
                        .Select(x => new
                        {
                            x.Id,
                            x.SumWorkTo,
                            x.SumFact,
                            x.DateStart,
                            x.DateEnd
                        })
                        .AsEnumerable()
                        .Where(x => works.ContainsKey(x.Id))
                        .Select(x =>
                        {
                            var dateStart = x.DateStart.HasValue ? x.DateStart.ToDateTime().ToShortDateString() : string.Empty;
                            var dateFinish = x.DateEnd.HasValue ? x.DateEnd.ToDateTime().ToShortDateString() : string.Empty;
                            var worksData = works[x.Id];

                            return new
                            {
                                x.Id,
                                ServiceWorkToRt = new
                                {
                                    planCost = x.SumWorkTo,
                                    factCost = x.SumFact,
                                    dateStart,
                                    dateFinish,
                                    worksData.NameGroup,
                                    worksData.Works
                                }
                            };
                        })
                        .ToDictionary(x => x.Id, x => x.ServiceWorkToRt);

                    var worksRepairDetail = workRepairDetailTatDomain.GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            GroupWorkPprId = x.WorkPpr.GroupWorkPpr.Id,
                            x.WorkPpr.Name,
                            UnitMeasure = x.UnitMeasure.Name,
                            x.PlannedVolume,
                            x.FactVolume
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.GroupWorkPprId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.Select(x => new Detail
                            {
                                Name = x.Name,
                                FactSize = x.FactVolume.HasValue ? x.FactVolume.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                PlanSize = x.PlannedVolume.HasValue ? x.PlannedVolume.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                Izm = x.UnitMeasure ?? string.Empty
                            })
                                .ToArray());

                    var worksPprByServiceDict = workRepairListDomain.GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            x.Id,
                            BaseServiceId = x.BaseService.Id,
                            GroupWorkPprId = x.GroupWorkPpr.Id,
                            NameWork = x.GroupWorkPpr.Name,
                            x.PlannedCost,
                            x.FactCost,
                            x.DateStart,
                            x.DateEnd
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.BaseServiceId,
                            ServiceWorkPprRt = new
                            {
                                x.Id,
                                x.NameWork,
                                PlanCost = x.PlannedCost,
                                FactSum = x.FactCost,
                                StartDate = x.DateStart.HasValue ? x.DateStart.ToDateTime().ToShortDateString() : string.Empty,
                                FinishDate = x.DateEnd.HasValue ? x.DateEnd.ToDateTime().ToShortDateString() : string.Empty,
                                Details = worksRepairDetail.ContainsKey(x.GroupWorkPprId) ? worksRepairDetail[x.GroupWorkPprId] : null
                            }
                        })
                        .GroupBy(x => x.BaseServiceId)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.ServiceWorkPprRt).ToList());

                    var housingServ = baseServiceDomain.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Where(x => x.TemplateService.KindServiceDi == KindServiceDi.Repair)
                        .Select(x => new { x.Id, x.TemplateService.Name })
                        .ToList();

                    var housingServs = housingServ.Select(x =>
                    {
                        var result = new HousingServ { Id = x.Id, NameService = x.Name };

                        var factCost = 0m;
                        var planCost = 0m;

                        if (worksToDict.ContainsKey(x.Id))
                        {
                            var worksTo = worksToDict[x.Id];

                            factCost = worksTo.factCost ?? 0;
                            planCost = worksTo.planCost ?? 0;

                            result.WorksTo = new ServiceWorkTo
                            {
                                DateFinish = worksTo.dateFinish,
                                DateStart = worksTo.dateStart,
                                FactCost = worksTo.factCost.HasValue ? worksTo.factCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                PlanCost = worksTo.planCost.HasValue ? worksTo.planCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                Works = worksTo.Works,
                                NameGroup = worksTo.NameGroup
                            };
                        }

                        if (worksPprByServiceDict.ContainsKey(x.Id))
                        {
                            var worksPpr = worksPprByServiceDict[x.Id];

                            worksPpr.ForEach(y =>
                            {
                                factCost += y.FactSum ?? 0;
                                planCost += y.PlanCost ?? 0;
                            });

                            result.WorksPpr = worksPpr
                                .Select(y => new ServiceWorkPpr
                                {
                                    Id = y.Id,
                                    Details = y.Details,
                                    NameWork = y.NameWork,
                                    StartDate = y.StartDate,
                                    FinishDate = y.FinishDate,
                                    FactSum = y.FactSum.HasValue ? y.FactSum.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                    PlanCost = y.PlanCost.HasValue ? y.PlanCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty
                                })
                                .ToArray();
                        }

                        result.FactCost = factCost != 0 ? factCost.RoundDecimal(2).ToString(numberformat) : string.Empty;
                        result.PlanCost = planCost != 0 ? planCost.RoundDecimal(2).ToString(numberformat) : string.Empty;

                        return result;
                    });

                    var managingOrgItem = new ManagingOrgItem
                    {
                        ManagServs = managServs,
                        HousingServs = housingServs.ToArray(),
                        ComServs = comServs
                    };

                    if (managingOrgItems == null)
                    {
                        managingOrgItems = new List<ManagingOrgItem>();
                    }

                    managingOrgItems.Add(managingOrgItem);
                    this.UpdateManageOrgItemProperties(managingOrgItem, diRealObj, idPeriod, idHouse, numberformat,
                        manOrgContractRealityObjectDomain, manOrgContractOwnersDomain, disclosureInfoRelationDomain);
                }

                return new GetHouseServicesResponse
                {
                    ManagingOrgItems = managingOrgItems?.ToArray(),
                    Result = Result.NoErrors
                };
            }
        }

        /// <summary>
        /// Обновляет значения свойств переданного экземпляра <see cref="ManagingOrgItem"/>
        /// </summary>
        /// <param name="managingOrgItem">Обновляемый экземпляра <see cref="ManagingOrgItem"/></param>
        /// <param name="diRealObj">Объект недвижимости деятельности управляющей организации в периоде раскрытия информации.</param>
        /// <param name="periodId">Уникальный идентификатор периода.</param>
        /// <param name="houseId">Уникальный идентификатор дома.</param>
        /// <param name="numberFormat">Числовой формат.</param>
        /// <param name="manOrgContractRealityObjectDomain">Домен "Жилой дом договора управляющей организации".</param>
        /// <param name="manOrgContractOwnersDomain">Домен "Управление домом, договор УК с собственниками".</param>
        /// <param name="disclosureInfoRelationDomain">Домен "Связь деятельности УО и деятельности УО в доме".</param>
        private void UpdateManageOrgItemProperties(ManagingOrgItem managingOrgItem, DisclosureInfoRealityObj diRealObj, long periodId, long houseId, NumberFormatInfo numberFormat,
          IDomainService<ManOrgContractRealityObject> manOrgContractRealityObjectDomain,
          IDomainService<ManOrgContractOwners> manOrgContractOwnersDomain, IDomainService<DisclosureInfoRelation> disclosureInfoRelationDomain)
        {
            var disclosureInfoRelation = disclosureInfoRelationDomain
              .FirstOrDefault(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id && x.DisclosureInfoRealityObj.PeriodDi.Id == periodId);

            if (disclosureInfoRelation == null)
            {
                return;
            }

            var managingOrg = disclosureInfoRelation.DisclosureInfo?.ManagingOrganization;
            var realityObject = disclosureInfoRelation.DisclosureInfoRealityObj?.RealityObject;

            // Договора дома с ук
            var contractManOrg = manOrgContractRealityObjectDomain
                .FirstOrDefault(x => x.RealityObject.Id == houseId && x.ManOrgContract.ManagingOrganization.Id == managingOrg.Id)?.ManOrgContract;

            managingOrgItem.Id = managingOrg?.Id ?? default(long);
            managingOrgItem.Name = managingOrg?.Contragent.Name.ToStr();
            managingOrgItem.JuridicalAddress = managingOrg?.Contragent.JuridicalAddress.ToStr();
            managingOrgItem.Ogrn = managingOrg?.Contragent.Ogrn.ToStr();
            managingOrgItem.YearRegistration = (managingOrg?.Contragent.DateRegistration.HasValue ?? false) && managingOrg.Contragent.DateRegistration.Value.Date != DateTime.MinValue.Date
                ? managingOrg.Contragent.DateRegistration.Value.Year.ToStr()
                : string.Empty;
            managingOrgItem.OgrnRegistration = managingOrg?.Contragent.OgrnRegistration.ToStr();
            managingOrgItem.PostAddress = managingOrg?.Contragent.MailingAddress.ToStr();
            managingOrgItem.Phone = managingOrg?.Contragent.Phone.ToStr();
            managingOrgItem.Email = managingOrg?.Contragent.Email.ToStr();
            managingOrgItem.Suite = managingOrg?.Contragent.OfficialWebsite.ToStr();
            managingOrgItem.ContractStart = contractManOrg?.StartDate != null && contractManOrg.StartDate.Value.Date != DateTime.MinValue.Date
                ? contractManOrg.StartDate.GetValueOrDefault().ToShortDateString()
                : string.Empty;
            managingOrgItem.ContractEnd = contractManOrg?.EndDate != null && contractManOrg.EndDate.Value.Date != DateTime.MinValue.Date
                ? contractManOrg.EndDate.GetValueOrDefault().ToShortDateString()
                : string.Empty;
            managingOrgItem.MkdArea = realityObject?.AreaMkd.HasValue ?? false ? realityObject.AreaMkd.Value.RoundDecimal(2).ToString(numberFormat) : string.Empty;
            managingOrgItem.ContractType = contractManOrg?.TypeContractManOrgRealObj.GetEnumMeta().Display ?? string.Empty;
            managingOrgItem.ManagingType = managingOrg?.TypeManagement.GetEnumMeta().Display ?? string.Empty;
            managingOrgItem.DocumentDate = contractManOrg?.DocumentDate.GetValueOrDefault().ToShortDateString() ?? string.Empty;
            managingOrgItem.TerminateReason = contractManOrg?.TerminateReason.ToStr();
            managingOrgItem.TerminationDate = contractManOrg?.TerminationDate.ToStr();
            managingOrgItem.DocumentNumber = contractManOrg?.DocumentNumber.ToStr();
            managingOrgItem.DocumentName = contractManOrg?.Return(contract =>
            {
                if (contract == null)
                {
                    return string.Empty;
                }

                switch (contract.ManagingOrganization.TypeManagement)
                {
                    case TypeManagementManOrg.UK:
                    case TypeManagementManOrg.TSJ:
                        return contract.FileInfo?.Name;
                    case TypeManagementManOrg.JSK:
                        return contract.DocumentName;
                    default:
                        return string.Empty;
                }
            }) ?? string.Empty;

            managingOrgItem.ContractFoundation = contractManOrg?.Return(contract =>
            {
                if (contract == null)
                {
                    return string.Empty;
                }

                switch (contract.ManagingOrganization.TypeManagement)
                {
                    case TypeManagementManOrg.UK:
                    case TypeManagementManOrg.TSJ:
                        var contractFoundation = (ManOrgContractOwnersFoundation?)manOrgContractOwnersDomain
                            .GetAll()
                            .Where(y => y.Id == contractManOrg.Id)
                            .Select(y => y.ContractFoundation)
                            .FirstOrDefault();
                        return contractFoundation?.GetAttribute<DisplayAttribute>()?.Value ?? string.Empty;
                    case TypeManagementManOrg.JSK:
                        return contract.DocumentName;
                    default:
                        return string.Empty;
                }
            }) ?? string.Empty;
        }
    }
}
