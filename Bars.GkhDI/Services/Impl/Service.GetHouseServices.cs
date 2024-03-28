namespace Bars.GkhDi.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo;

    using Entities;
    using Enums;
    using Gkh.Services.DataContracts;

    using TypeWork = Bars.GkhDi.Services.TypeWork;

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
            var workRepairDetailDomain = this.Container.Resolve<IDomainService<WorkRepairDetail>>();
            var workRepairListDomain = this.Container.Resolve<IDomainService<WorkRepairList>>();
            var manOrgContractRealityObjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var disclosureInfoRelationDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();

            using (this.Container.Using(disclosureInfoRealityObjectsDomain, tariffForRsoDomain, tariffForConsumersDomain, baseServiceDomain,
                additionalServiceDomain, housingServiceDomain, communalServiceDomain, workRepairTechServDomain, repairServiceDomain, workRepairDetailDomain, workRepairListDomain,
                manOrgContractRealityObjectDomain, disclosureInfoRelationDomain))
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
                        .Where(x => x.TemplateService.KindServiceDi == KindServiceDi.Managing && x.DisclosureInfoRealityObj.Id == diRealObj.Id).
                        Select(x => new
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

                    var housingServs = new List<HousingServ>();

                    housingServs.AddRange(baseServiceDomain.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id
                            && (x.TemplateService.KindServiceDi == KindServiceDi.Repair || x.TemplateService.KindServiceDi == KindServiceDi.CapitalRepair))
                        .Select(x => new
                        {
                            x.Id,
                            TemplateServiceName = x.TemplateService.Name,
                            ProviderName = x.Provider.Name,
                            UnitMeasureName = x.UnitMeasure.Name
                        })
                        .AsEnumerable()
                        .Select(x => new HousingServ
                        {
                            Id = x.Id,
                            Name = x.TemplateServiceName,
                            Provider = x.ProviderName,
                            Measure = x.UnitMeasureName,
                            Cost = tariffForConsumers.ContainsKey(x.Id)
                                ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                : string.Empty
                        })
                        .ToList());

                    housingServs.AddRange(additionalServiceDomain.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            x.Id,
                            TemplateServiceName = x.TemplateService.Name,
                            ProviderName = x.Provider.Name,
                            UnitMeasureName = x.UnitMeasure.Name,
                            PeriodicityName = x.Periodicity.Name
                        })
                        .AsEnumerable()
                        .Select(x => new HousingServ
                        {
                            Id = x.Id,
                            Name = x.TemplateServiceName,
                            Provider = x.ProviderName,
                            Measure = x.UnitMeasureName,
                            Cost = tariffForConsumers.ContainsKey(x.Id)
                                ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                : string.Empty,
                            Periodicity = x.PeriodicityName
                        })
                        .ToList());

                    housingServs.AddRange(housingServiceDomain.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            x.Id,
                            TemplateServiceName = x.TemplateService.Name,
                            ProviderName = x.Provider.Name,
                            UnitMeasureName = x.UnitMeasure.Name,
                            PeriodicityName = x.Periodicity.Name
                        })
                        .AsEnumerable()
                        .Select(x => new HousingServ
                        {
                            Id = x.Id,
                            Name = x.TemplateServiceName,
                            Provider = x.ProviderName,
                            Measure = x.UnitMeasureName,
                            Cost = tariffForConsumers.ContainsKey(x.Id)
                                ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                : string.Empty,
                            Periodicity = x.PeriodicityName
                        }).ToList());
                        
                    var comServs = communalServiceDomain.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            x.Id,
                            TemplateServiceName = x.TemplateService.Name,
                            ProviderName = x.Provider.Name,
                            UnitMeasureName = x.UnitMeasure.Name,
                            x.VolumePurchasedResources
                        })
                        .AsEnumerable()
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
                        .GroupBy(x => x.BaseService.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => new Work { Name = x.WorkTo.Name }).ToArray());

                    var worksTo = repairServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            x.Id,
                            TemplateServiceName = x.TemplateService.Name,
                            Cost = x.SumWorkTo.HasValue ? x.SumWorkTo.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                        })
                        .AsEnumerable()
                        .Select(x => new ServiceWorkTo
                        {
                            Id = x.Id,
                            NameServ = x.TemplateServiceName,
                            Cost = x.Cost,
                            Works = works.ContainsKey(x.Id) ? works[x.Id] : null
                        })
                        .Where(x => !string.IsNullOrEmpty(x.Cost) || (x.Works != null && x.Works.Any()))
                        .ToArray();

                    var worksRepairDetail = workRepairDetailDomain
                        .GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .GroupBy(x => x.WorkPpr.GroupWorkPpr.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => new TypeWork { Name = x.WorkPpr.Name }).ToArray());

                    var worksPpr = workRepairListDomain
                        .GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => new
                        {
                            x.Id,
                            GroupWorkPprName = x.GroupWorkPpr.Name,
                            PlanSize = x.PlannedVolume.HasValue ? x.PlannedVolume.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            PlanCost = x.PlannedCost.HasValue ? x.PlannedCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            FactSize = x.FactVolume.HasValue ? x.FactVolume.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            FactSum = x.FactCost.HasValue ? x.FactCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            StartDate = x.DateStart.HasValue ? x.DateStart.ToDateTime().ToShortDateString() : string.Empty,
                            FinishDate = x.DateEnd.HasValue ? x.DateEnd.ToDateTime().ToShortDateString() : string.Empty,
                            GroupWorkPprId = x.GroupWorkPpr.Id
                        })
                        .AsEnumerable()
                        .Select(x => new ServiceWorkPpr
                        {
                            Id = x.Id,
                            NameWork = x.GroupWorkPprName,
                            PlanSize = x.PlanSize,
                            PlanCost = x.PlanCost,
                            FactSize = x.FactSize,
                            FactSum = x.FactSum,
                            StartDate = x.StartDate,
                            FinishDate = x.FinishDate,
                            TypeWorks = worksRepairDetail.ContainsKey(x.GroupWorkPprId) ? worksRepairDetail[x.GroupWorkPprId] : null
                        })
                        .ToArray();

                    var managingOrgItem = new ManagingOrgItem
                    {
                        ManagServs = managServs,
                        HousingServs = housingServs.ToArray(),
                        ComServs = comServs,
                        WorksPpr = worksPpr,
                        WorksTo = worksTo
                    };

                    if (managingOrgItems == null)
                    {
                        managingOrgItems = new List<ManagingOrgItem>();
                    }

                    this.UpdateManageOrgItemProperties(managingOrgItem,
                        diRealObj,
                        idPeriod,
                        idHouse,
                        numberformat,
                        manOrgContractRealityObjectDomain,
                        disclosureInfoRelationDomain);
                    managingOrgItems.Add(managingOrgItem);
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
        /// <param name="disclosureInfoRelationDomain">Домен "Связь деятельности УО и деятельности УО в доме".</param>
        public void UpdateManageOrgItemProperties(
            ManagingOrgItem managingOrgItem,
            DisclosureInfoRealityObj diRealObj,
            long periodId,
            long houseId,
            NumberFormatInfo numberFormat,
            IDomainService<ManOrgContractRealityObject> manOrgContractRealityObjectDomain,
            IDomainService<DisclosureInfoRelation> disclosureInfoRelationDomain)
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
            managingOrgItem.YearRegistration = (managingOrg?.Contragent.DateRegistration.HasValue ?? false) &&
                managingOrg.Contragent.DateRegistration.Value.Date != DateTime.MinValue.Date
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
            managingOrgItem.MkdArea = (realityObject?.AreaMkd.HasValue ?? false) && numberFormat != null
                ? realityObject.AreaMkd.Value.RoundDecimal(2).ToString(numberFormat)
                : string.Empty;

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

            managingOrgItem.ContractFoundation = this.GetContractFoundation(contractManOrg) ?? string.Empty;
        }

        private string GetContractFoundation(ManOrgBaseContract contract)
        {
            IDomainService<ManOrgContractOwners> manOrgContractOwnersDomain;
            IDomainService<ManOrgJskTsjContract> manOrgContractJskTsjDomain;

            using (this.Container.Using(manOrgContractOwnersDomain = this.Container.ResolveDomain<ManOrgContractOwners>(),
                manOrgContractJskTsjDomain = this.Container.ResolveDomain<ManOrgJskTsjContract>()))
            {
                if (contract == null)
                {
                    return string.Empty;
                }

                switch (contract.ManagingOrganization.TypeManagement)
                {
                    case TypeManagementManOrg.UK:
                        var foundationOwners = (ManOrgContractOwnersFoundation?)manOrgContractOwnersDomain
                            .GetAll()
                            .Where(y => y.Id == contract.Id)
                            .Select(y => y.ContractFoundation)
                            .FirstOrDefault();
                        return foundationOwners?.GetAttribute<DisplayAttribute>()?.Value ?? string.Empty;
                    case TypeManagementManOrg.TSJ:
                    case TypeManagementManOrg.JSK:
                        var foundationManOrg = (ManOrgJskTsjContractFoundation?)manOrgContractJskTsjDomain
                            .GetAll()
                            .Where(y => y.Id == contract.Id)
                            .Select(y => y.ContractFoundation)
                            .FirstOrDefault();
                        return foundationManOrg?.GetAttribute<DisplayAttribute>()?.Value ?? string.Empty;
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
