namespace Bars.Gkh.Gis.DomainService.Analysis.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.DomainService.Indicator;
    using Bars.Gkh.Gis.DomainService.Indicator.Impl;
    using Bars.Gkh.Gis.DomainService.RealEstate;
    using System.Linq;
    using B4.Modules.FIAS;

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Gis.Entities.Register.HouseServiceRegister;
    using Bars.Gkh.Gis.Enum;
    using Entities.RealEstate.GisRealEstateType;

    public class RegressionAnalysisService : IRegressionAnalysisService
    {
        protected IRepository<GisRealEstateType> RealEstateTypeRepository;
        protected IRepository<GisRealEstateTypeGroup> RealEstateTypeGroupRepository;
        protected IIndicatorService IndicatorService;
        protected IRealEstateTypeCommonParamService RealEstateTypeCommonParamService;
        protected IRepository<ServiceDictionary> ServiceDictionaryRepository;
        protected IRepository<HouseServiceRegister> HouseServiceRegisterRepository;
        protected IRepository<Fias> FiasRepository;

        public RegressionAnalysisService(
            IRepository<GisRealEstateType> realEstateTypeRepository,
            IRepository<GisRealEstateTypeGroup> realEstateTypeGroupRepository,
            IIndicatorService indicatorService,
            IRealEstateTypeCommonParamService realEstateTypeCommonParamService,
            IRepository<ServiceDictionary> serviceDictionaryRepository,
            IRepository<HouseServiceRegister> houseServiceRegisterRepository,
            IRepository<Fias> fiasRepository
            )
        {
            RealEstateTypeRepository = realEstateTypeRepository;
            RealEstateTypeGroupRepository = realEstateTypeGroupRepository;
            IndicatorService = indicatorService;
            RealEstateTypeCommonParamService = realEstateTypeCommonParamService;
            ServiceDictionaryRepository = serviceDictionaryRepository;
            HouseServiceRegisterRepository = houseServiceRegisterRepository;
            FiasRepository = fiasRepository;
        }

        /// <summary>
        /// Список типов домов с группами
        /// исключая пучтые группы        
        /// </summary>
        /// <param name="baseParams">параметры</param>
        /// <returns>древовидная структура групп домов с типами домов</returns>
        public IDataResult GroupedTypeWithoutEmptyGroupList(BaseParams baseParams)
        {
            var filter = baseParams.Params.GetAs<string>("workName");

            var filteredTypes = RealEstateTypeRepository
                .GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.ToUpper().Contains(filter.ToUpper()));

            var data = RealEstateTypeGroupRepository
                .GetAll()
                .ToList()
                .Where(x => filteredTypes.Any(y => y.Group.Id == x.Id))
                .Select(x => new
                {
                    id = string.Format("{0}_{1}", "Group", x.Id),
                    EntityId = x.Id,
                    text = x.Name,
                    expanded = !string.IsNullOrEmpty(filter),
                    children = filteredTypes
                        .Where(y => y.Group == x)
                        .Select(y => new
                        {
                            id = string.Format("{0}_{1}", "Type", y.Id),
                            EntityId = y.Id,
                            text = y.Name,
                            @checked = false,
                            leaf = true
                        })
                });
            return new ListDataResult(data, data.Count());
        }

        /// <summary>
        /// Дерево индикаторов для регрессионного анализа
        /// </summary>
        /// <param name="baseParams">параметры</param>
        /// <returns>дерево индикаторов</returns>
        public IDataResult IndicatorsRegressionAnalysis(BaseParams baseParams)
        {
            var filter = baseParams.Params.GetAs<string>("workName");

            var indicatorTree = IndicatorService.GetIndicatorTree(baseParams);

            //фильтр по услугам для регрессионного анализа
            indicatorTree = indicatorTree
                //.Where(x =>
                //    x.GroupIndicator == TypeGroupIndicators.ColdWster
                //    ||
                //    x.GroupIndicator == TypeGroupIndicators.HotWater
                //    ||
                //    x.GroupIndicator == TypeGroupIndicators.OutWater
                //    ||
                //    x.GroupIndicator == TypeGroupIndicators.Heating
                //    ||
                //    x.GroupIndicator == TypeGroupIndicators.Electricity
                //    ||
                //    x.GroupIndicator == TypeGroupIndicators.AllOtherServices
                //    ||
                //    x.GroupIndicator == TypeGroupIndicators.AllWithOdn
                //    ||
                //    x.GroupIndicator == TypeGroupIndicators.AllWithoutOdn)
                .WhereIf(filter.IsNotEmpty(),
                    x => x.Indicators.Keys.Any(y => y.GetEnumMeta().Display.ToUpper().Contains(filter.ToUpper())))
                .ToList();

            var data = indicatorTree
                .Select(x => new
                {
                    id = string.Format("{0}_{1}", "Service", x.Service.Code),
                    EntityId = x.Service.Code,
                    text = x.Service.Name,
                    expanded = !string.IsNullOrEmpty(filter),
                    @checked = false,
                    children = x.Indicators
                        .WhereIf(!string.IsNullOrEmpty(filter),
                            y => y.Key.GetEnumMeta().Display.ToUpper().Contains(filter.ToUpper()))
                        .ToList()
                        .Select(y => new
                        {
                            id = string.Format("{0}_{1}_{2}", "Indicator", (int) y.Key, x.Service.Code),
                            EntityId = (int) y.Key,
                            text =
                                ((GisTypeIndicator) Enum.Parse(typeof (GisTypeIndicator), y.Key.ToString())).GetEnumMeta()
                                    .Display,
                            leaf = true,
                            @checked = false
                        })
                });
            return new ListDataResult(data, data.Count());
        }

        /// <summary>
        /// Регрессионный анализ
        /// возвращает данные для отображения графика
        /// </summary>
        /// <param name="baseParams">параметры</param>
        /// <returns>список объектов</returns>
        public IDataResult ChartRegressionAnalysis(BaseParams baseParams)
        {
            var houseType = baseParams.Params.GetAs<string>("houseType");
            var indicators = baseParams.Params.GetAs<List<string>>("indicators");
            var dateBegin = baseParams.Params.GetAs<DateTime>("dateBegin");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");            
            dateBegin = new DateTime(dateBegin.Year, dateBegin.Month, 1);
            dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, DateTime.DaysInMonth(dateEnd.Year, dateEnd.Month));
            var areaGuid = baseParams.Params.GetAs<string>("areaGuid");
            //var placeGuid = baseParams.Params.GetAs<string>("placeGuid");
            //var streetGuidList = baseParams.Params.GetAs<List<string>>("streetGuid"); 
            //streetGuidList.RemoveAll(x => x == "");

            if (string.IsNullOrEmpty(houseType) || indicators == null || indicators.Count == 0)
            {
                return BaseDataResult.Error("Некорректные параметры");
            }

            //todo переделдать что бы с клиента приходили чистые id
            //todo предположим я таки получил чистый id
            var houseTypeId = string.IsNullOrEmpty(houseType) ? 0 : houseType.Split('_')[1].ToLong();
            //todo а также получил список индикаторов            
            var serviceIndicatorsProxy = indicators
                .Select(x => new ServiceIndicatorProxy
                {
                    ServiceId =
                        ServiceDictionaryRepository.GetAll()
                            .FirstOrDefault(y => y.Code == x.Split('_', System.StringSplitOptions.None)[2].ToInt())
                            .Return(y => y.Id),
                    IndicatorGroup = x.Split('_')[2].ToInt(),
                    Indicator = x.Split('_')[1].ToInt(),
                    FieldName = x
                })
                .ToList();


            //список домов по параметрам из типа дома - берем дома только от УК
            var housesByType =
                RealEstateTypeCommonParamService.GetHouseRegistersByRealEstateType(
                    RealEstateTypeRepository.Get(houseTypeId))
                    .Join(FiasRepository.GetAll(), x => x.FiasAddress.PlaceGuidId, y => y.AOGuid, (x, y) => new { house = x, fias = y })
                .WhereIf(!areaGuid.IsEmpty(),
                    x =>
                        x.house.FiasAddress != null && x.house.FiasAddress.PlaceGuidId == areaGuid
                        || x.fias != null && x.fias.ParentGuid == areaGuid
                        || x.fias != null && x.fias.MirrorGuid == areaGuid)
                    .Where(x => x.house.ManOrgs != null && x.house.ManOrgs != "")
                    .Select(x => x.house)
                    //.WhereIf(placeGuid != "" || areaGuid != "",
                    //    x => x.FiasAddress.PlaceGuidId == (placeGuid != "" ? placeGuid : areaGuid))
                    //.WhereIf(streetGuidList != null, x => streetGuidList.Contains(x.FiasAddress.StreetGuidId))
                    .ToList();

            //данные начислений по домам
            var houseServicesList = HouseServiceRegisterRepository
                .GetAll()
                .Where(x =>
                    housesByType.Contains(x.House)
                    &&
                    x.CalculationDate.Date >= dateBegin.Date
                    &&
                    x.CalculationDate.Date <= dateEnd.Date)
                .GroupBy(x => x.Service.Id)
                .ToDictionary(x => x.Key, x => x);


            var data = new List<Object>();
            var currentDate = dateBegin;
            while (currentDate <= dateEnd)
            {
                var exo = new ExpandoObject() as IDictionary<string, object>;
                exo.Add("month", currentDate.Date.ToString("Y"));
                var fAdd = false;
                foreach (var indicatorProxy in serviceIndicatorsProxy)
                {
                    ////список услуг для индикатора
                    //var servicesForIndicator =
                    //    IndicatorService.GetServicesByIndicatorsGroup(
                    //        (TypeGroupIndicators)
                    //            Enum.Parse(typeof (TypeGroupIndicators), indicatorProxy.IndicatorGroup.ToString()))
                    //        .Select(x => x.Id)
                    //        .ToList();

                    //фильтр по услугам индикатора + месяц начислений
                    var houseServicesFilter =
                        houseServicesList
                            //.Where(x => servicesForIndicator.Contains(x.Key))
                            .Where(x => x.Key == indicatorProxy.ServiceId)
                            .SelectMany(x => x.Value)
                            .Where(
                                x =>
                                    x.CalculationDate.Year == currentDate.Year
                                    &&
                                    x.CalculationDate.Month == currentDate.Month)
                            .ToList();
                    exo.Add(indicatorProxy.FieldName,
                        Math.Round(houseServicesFilter
                            .Select(
                                x => GetIndicatorValue(x, indicatorProxy.Indicator).ToDouble()).Sum(), 2)
                        );
                }
                data.Add(exo);
                currentDate = currentDate.AddMonths(1);
            }

            return new ListDataResult(data, data.Count);
        }

        /// <summary>
        /// Получить по индикатору соответсвующее
        /// значение свойства сущности
        /// </summary>
        /// <param name="houseServiceRegister">сущность</param>
        /// <param name="indicator">значение индикатора</param>
        /// <returns>значение свойства сущности</returns>
        private object GetIndicatorValue(HouseServiceRegister houseServiceRegister, int indicator)
        {
            switch (indicator)
            {
                case (int) GisTypeIndicator.Volume:
                {
                    return houseServiceRegister.TotalVolume;
                }
                case (int) GisTypeIndicator.Charge:
                case (int) GisTypeIndicator.SummaryCharge:
                {
                    return houseServiceRegister.Charge;
                }
                case (int) GisTypeIndicator.Payment:
                case (int) GisTypeIndicator.SummaryPayment:
                {
                    return houseServiceRegister.Payment;
                }
                case (int) GisTypeIndicator.CoefOdn:
                {
                    return houseServiceRegister.CoefOdn;
                }
                case (int) GisTypeIndicator.DistibutedVolume:
                {
                    return houseServiceRegister.VolumeDistributed;
                }
                case (int) GisTypeIndicator.NotDistibutedVolume:
                {
                    return houseServiceRegister.VolumeNotDistributed;
                }
                default:
                {
                    return null;
                }
            }
        }
    }
}