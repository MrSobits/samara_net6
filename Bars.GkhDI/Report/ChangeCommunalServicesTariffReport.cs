namespace Bars.GkhDi.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using Bars.GkhDi.Properties;

    using Castle.Windsor;

    public class ChangeCommunalServicesTariffReport : BasePrintForm
    {
        private List<long> municipalities;

        private readonly List<TypeGroupServiceDi> serviceType = new List<TypeGroupServiceDi>();

        private bool transmitOrg = true;

        protected DateTime startDate = DateTime.MinValue;

        protected DateTime endDate = DateTime.MinValue;

        public IWindsorContainer Container { get; set; }

        public ChangeCommunalServicesTariffReport()
            : base(new ReportTemplateBinary(Resources.ChangeCommunalServicesTariff))
        {
        }

        public override string Name
        {
            get
            {
                return "Изменение тарифов на коммунальные услуги";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Изменение тарифов на коммунальные услуги";
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
                return "B4.controller.report.ChangeCommunalServicesTariff";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.Di.ChangeCommunalServicesTariff";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalitiesParam = baseParams.Params["municipalityIds"].ToString();
            this.municipalities = !string.IsNullOrEmpty(municipalitiesParam)
                ? municipalitiesParam.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var serviceTypesParam = baseParams.Params["serviceType"].ToString();
            var serviceTypesList = !string.IsNullOrEmpty(serviceTypesParam)
                ? serviceTypesParam.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            this.transmitOrg = baseParams.Params["transmitOrg"].ToLong() != 20;
            this.startDate = baseParams.Params.GetAs<DateTime>("startDate");
            this.endDate = baseParams.Params.GetAs<DateTime>("endDate");

            foreach (var serviceTypeInt in serviceTypesList)
            {
                switch (serviceTypeInt)
                {
                    case 10:
                        this.serviceType.Add(TypeGroupServiceDi.Communal);
                        break;
                    case 20:
                        this.serviceType.Add(TypeGroupServiceDi.Housing);
                        break;
                    case 30:
                        this.serviceType.Add(TypeGroupServiceDi.Other);
                        break;
                }
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var periodDi = this.Container.Resolve<IDomainService<PeriodDi>>().GetAll()
                .Where(x => this.endDate >= x.DateStart && this.endDate <= x.DateEnd)
                .Select(x => new
                {
                    x.Id,
                    x.DateEnd,
                    x.DateStart
                })
                .FirstOrDefault();


            if (periodDi == null)
            {
                return;
            }

            if (this.startDate < periodDi.DateStart)
            {
                this.startDate = periodDi.DateStart.HasValue ? periodDi.DateStart.Value : this.startDate;
            }

            var templateServices =
                this.Container.Resolve<IDomainService<TemplateService>>()
                    .GetAll()
                    .WhereIf(this.serviceType.Count > 0, x => this.serviceType.Contains(x.TypeGroupServiceDi))
                    .Select(x => new {x.Name, x.Id, x.TypeGroupServiceDi})
                    .AsEnumerable()
                    .OrderBy(x => x.TypeGroupServiceDi)
                    .ToList();

            var vsection = reportParams.ComplexReportParams.ДобавитьСекцию("vsection");

            var columnNumber = 5;

            // заполнение вертикальной секции
            foreach (var templateService in templateServices)
            {
                vsection.ДобавитьСтроку();
                switch (templateService.TypeGroupServiceDi)
                {
                    case TypeGroupServiceDi.Communal:
                        vsection["groupServiceName"] = "Изменение тарифов на коммунальные услуги";
                        break;
                    case TypeGroupServiceDi.Housing:
                        vsection["groupServiceName"] = "Изменение тарифов на жилищные услуги";
                        break;
                    case TypeGroupServiceDi.Other:
                        vsection["groupServiceName"] = " Изменение тарифов на прочие услуги";
                        break;
                }

                vsection["serviceName"] = templateService.Name;
                vsection["tariff1"] = string.Format("$tariff1_{0}$", templateService.Id);
                vsection["tariff2"] = string.Format("$tariff2_{0}$", templateService.Id);
                vsection["change"] = string.Format("$change_{0}$", templateService.Id);

                var columnIndex = 0;
                for (var i = columnNumber; i < columnNumber + 9; i++)
                {
                    vsection["col" + columnIndex] = i;
                    ++columnIndex;
                }

                columnNumber += 9;
            }

            // учитываем действующие договоры управление
            var robjectFilter = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .WhereIf(this.municipalities.Count > 0,
                    x => this.municipalities.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(!this.transmitOrg,
                    x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.ManagingOrgJskTsj)
                .Where(
                    x =>
                        x.ManOrgContract.StartDate <= periodDi.DateEnd
                        && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= periodDi.DateStart))
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    moId = x.ManOrgContract.ManagingOrganization.Id
                });

            var diRoIdsQuery = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>().GetAll()
                .Where(x => x.DisclosureInfo.PeriodDi.Id == periodDi.Id)
                .Where(x => x.DisclosureInfo.ManagingOrganization.Contragent != null)
                .Where(
                    x =>
                        robjectFilter.Any(
                            y =>
                                y.roId == x.DisclosureInfoRealityObj.RealityObject.Id
                                && y.moId == x.DisclosureInfo.ManagingOrganization.Id))
                .WhereIf(this.municipalities.Count > 0,
                    x => this.municipalities.Contains(x.DisclosureInfoRealityObj.RealityObject.Municipality.Id));

            var diRoIds = diRoIdsQuery.Select(x => x.DisclosureInfoRealityObj.Id);

            // информация об УК и доме
            var realtyObjDataList = diRoIdsQuery
                .Select(x => new
                {
                    roId = x.DisclosureInfoRealityObj.RealityObject.Id,
                    x.DisclosureInfoRealityObj.RealityObject.Address,
                    muName = x.DisclosureInfoRealityObj.RealityObject.Municipality.Name,
                    moId = x.DisclosureInfo.ManagingOrganization.Id,
                    moName = x.DisclosureInfo.ManagingOrganization.Contragent.Name,
                    diRoId = x.DisclosureInfoRealityObj.Id
                })
                .OrderBy(x => x.muName)
                .ThenBy(x => x.moName)
                .ThenBy(x => x.Address)
                .ToList();

            var serviceQuery = this.Container.Resolve<IDomainService<BaseService>>().GetAll()
                .WhereIf(this.serviceType.Count > 0,
                    x => this.serviceType.Contains(x.TemplateService.TypeGroupServiceDi))
                .Where(x => diRoIds.Contains(x.DisclosureInfoRealityObj.Id));

            var baseServiceIdsQuery = serviceQuery.Select(x => x.Id);

            // информация об услуге
            // если в раскрытии информации в доме 2 одинаковых услуги - берем с более поздней датой создания
            var serviceData = serviceQuery
                .Select(x => new
                {
                    diRoId = x.DisclosureInfoRealityObj.Id,
                    tmplServiceId = x.TemplateService.Id,
                    baseServiceId = x.Id,
                    x.ObjectCreateDate
                })
                .AsEnumerable()
                .GroupBy(x => x.diRoId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.tmplServiceId).ToDictionary(
                        y => y.Key,
                        y => y.OrderByDescending(z => z.ObjectCreateDate)
                            .Select(z => z.baseServiceId)
                            .FirstOrDefault()));

            // информация о тарифах услуги
            var tariffData =
                this.Container.Resolve<IDomainService<TariffForConsumers>>()
                    .GetAll()
                    .Where(x => baseServiceIdsQuery.Contains(x.BaseService.Id))
                    .Where(x => x.Cost.HasValue)
                    .Select(x => new {baseServiceId = x.BaseService.Id, x.DateStart, x.ObjectCreateDate, x.Cost})
                    .AsEnumerable()
                    .GroupBy(x => x.baseServiceId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => new TariffInfoProxy
                        {
                            Cost = y.Cost,
                            StartDate = y.DateStart.HasValue ? y.DateStart.Value : y.ObjectCreateDate
                        })
                            .Where(y => this.startDate <= y.StartDate && this.endDate >= y.StartDate)
                            .OrderByDescending(y => y.StartDate)
                            .ToList());

            // формируем несколько списков с типом оказания услуги = Не предоставляется
            var communalServiceNotAvailable =
                this.Container.Resolve<IDomainService<CommunalService>>()
                    .GetAll()
                    .Where(x => baseServiceIdsQuery.Contains(x.Id))
                    .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable)
                    .Select(x => x.Id)
                    .ToList();

            var housingServiceNotAvailable =
                this.Container.Resolve<IDomainService<HousingService>>()
                    .GetAll()
                    .Where(x => baseServiceIdsQuery.Contains(x.Id))
                    .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable)
                    .Select(x => x.Id)
                    .ToList();

            var capServiceNotAvailable =
                this.Container.Resolve<IDomainService<CapRepairService>>()
                    .GetAll()
                    .Where(x => baseServiceIdsQuery.Contains(x.Id))
                    .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable)
                    .Select(x => x.Id)
                    .ToList();

            var repairServiceNotAvailable =
                this.Container.Resolve<IDomainService<RepairService>>()
                    .GetAll()
                    .Where(x => baseServiceIdsQuery.Contains(x.Id))
                    .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable)
                    .Select(x => x.Id)
                    .ToList();

            var num = 0;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            // заполнение основной (горизонтальной) секции
            foreach (var realtyObjData in realtyObjDataList)
            {
                section.ДобавитьСтроку();
                ++num;
                section["num"] = num;
                section["municipality"] = realtyObjData.muName;
                section["moname"] = realtyObjData.moName;
                section["address"] = realtyObjData.Address;

                var currentRoServices = serviceData.ContainsKey(realtyObjData.diRoId)
                    ? serviceData[realtyObjData.diRoId]
                    : new Dictionary<long, long>();

                foreach (var templateService in templateServices)
                {
                    // если в доме нет услуги - ничего не выводим
                    if (!currentRoServices.ContainsKey(templateService.Id))
                    {
                        continue;
                    }

                    var currentBaseServiceId = currentRoServices[templateService.Id];

                    // если услуга есть в списке непредоствляемых - выводим "-"
                    if (communalServiceNotAvailable.Contains(currentBaseServiceId)
                        || housingServiceNotAvailable.Contains(currentBaseServiceId)
                        || capServiceNotAvailable.Contains(currentBaseServiceId)
                        || repairServiceNotAvailable.Contains(currentBaseServiceId))
                    {
                        section[string.Format("tariff1_{0}", templateService.Id)] = "-";
                        section[string.Format("tariff2_{0}", templateService.Id)] = "-";
                        section[string.Format("change_{0}", templateService.Id)] = "-";
                        continue;
                    }

                    // Если в доме есть услуга, но не прописан тариф - выводим нули
                    if (!tariffData.ContainsKey(currentBaseServiceId))
                    {
                        section[string.Format("tariff1_{0}", templateService.Id)] = 0;
                        section[string.Format("tariff2_{0}", templateService.Id)] = 0;
                        section[string.Format("change_{0}", templateService.Id)] = 0;
                        continue;
                    }

                    var currentSeviceTariffs = tariffData[currentBaseServiceId];
                    this.FillServiceTariff(section, currentSeviceTariffs, templateService.Id);
                }
            }
        }

        private void FillServiceTariff(Section section, List<TariffInfoProxy> seviceTariffs, long serviceId)
        {
            if (seviceTariffs.Count == 0)
            {
                section[string.Format("tariff1_{0}", serviceId)] = 0;
                section[string.Format("tariff2_{0}", serviceId)] = 0;
                section[string.Format("change_{0}", serviceId)] = 0;
                return;
            }

            var tariff2 = seviceTariffs.FirstOrDefault();
            var tariffCost2 = tariff2 != null && tariff2.Cost.HasValue ? tariff2.Cost.Value : 0;

            var tariff1 = seviceTariffs.Count > 1 ? seviceTariffs[1] : null;
            var tariffCost1 = tariff1 != null && tariff1.Cost.HasValue ? tariff1.Cost.Value : 0;

            var changeTariff = tariffCost1 != 0 ? (tariffCost2 - tariffCost1) / tariffCost1 : 0;

            section[string.Format("tariff1_{0}", serviceId)] = tariffCost1;
            section[string.Format("tariff2_{0}", serviceId)] = tariffCost2;
            section[string.Format("change_{0}", serviceId)] = Math.Abs(changeTariff);
        }
    }

    class TariffInfoProxy
    {
        public DateTime? StartDate { get; set; }
        public Decimal? Cost { get; set; }
    }
}