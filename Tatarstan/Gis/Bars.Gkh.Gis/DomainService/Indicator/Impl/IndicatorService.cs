namespace Bars.Gkh.Gis.DomainService.Indicator.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;

    using Bars.Gkh.Entities.Dicts;

    using Entities.RealEstate.GisRealEstateType;
    using Entities.Register.HouseRegister;
    using Entities.Register.HouseServiceRegister;
    using Indicator;
    using RealEstate;
    using Castle.Windsor;
    using Entities.IndicatorServiceComparison;

    public class IndicatorService: IIndicatorService
    {
        protected IWindsorContainer Container; 
        protected IRepository<HouseRegister> HouseRegisterRepository;
        protected IRepository<HouseServiceRegister> HouseServiceRegisterRepository;
        protected IRepository<IndicatorServiceComparison> IndicatorServiceRepository;
        protected IRepository<ServiceDictionary> ServiceDictionaryRepository;
        protected IRepository<GisRealEstateType> RealEstateTypeRepository;
        protected IRealEstateTypeCommonParamService RealEstateTypeCommonParamService;
        
        /// <summary>
        /// Конструктор
        /// </summary>        
        public IndicatorService(
            IWindsorContainer container,
            IRepository<HouseRegister> houseRegisterRepository,
            IRepository<HouseServiceRegister> houseServiceRegisterRepository,
            IRepository<IndicatorServiceComparison> indicatorServiceRepository,
            IRepository<ServiceDictionary> serviceDictionaryRepository,
            IRepository<GisRealEstateType> realEstateTypeRepository,
            IRealEstateTypeCommonParamService realEstateTypeCommonParamService
            )
        {
            Container = container;
            HouseRegisterRepository = houseRegisterRepository;
            HouseServiceRegisterRepository = houseServiceRegisterRepository;
            IndicatorServiceRepository = indicatorServiceRepository;
            ServiceDictionaryRepository = serviceDictionaryRepository;
            RealEstateTypeRepository = realEstateTypeRepository;
            RealEstateTypeCommonParamService = realEstateTypeCommonParamService;
        }
        
        public List<IndicatorGroupProxy> GetIndicatorTree(BaseParams baseParams)
        {
            var indicators = IndicatorServiceRepository.GetAll().ToList();
            if (!indicators.Any())
            {
                return new List<IndicatorGroupProxy>();
            }

            var tree = indicators.GroupBy(x => x.Service).Select(x => new IndicatorGroupProxy
            {
                Service = x.Key,
                Indicators = indicators.Where(y => y.Service.Id == x.Key.Id).ToDictionary(y => y.GisTypeIndicator, z => z.Id)
            }).ToList();

            return tree;
        }

        /// <summary>
        /// Получить список услуг для выбранной группы индикаторов
        /// </summary>
        /// <param name="serviceDictionary">услуга</param>
        /// <returns>список услуг</returns>
        //public IEnumerable<ServiceDictionary> GetServicesByIndicatorsGroup(ServiceDictionary serviceDictionary)
        //{
        //    var query = ServiceDictionaryRepository
        //        .GetAll();

        //    switch (groupIndicators)
        //    {
        //        case TypeGroupIndicators.AllWithoutOdn:
        //        {
        //            query = query
        //                .Where(x =>
        //                    x.Code == (int)TypeGroupIndicators.HotWater
        //                    ||
        //                    x.Code == (int)TypeGroupIndicators.ColdWster
        //                    ||
        //                    x.Code == (int)TypeGroupIndicators.OutWater
        //                    ||
        //                    x.Code == (int)TypeGroupIndicators.Heating
        //                    ||
        //                    x.Code == (int)TypeGroupIndicators.Electricity
        //                );
        //            break;
        //        }
        //        case TypeGroupIndicators.AllWithOdn:
        //        {
        //            query = query
        //                .Where(x =>
        //                    x.Code == (int) TypeGroupIndicators.HotWaterOdn
        //                    ||
        //                    x.Code == (int) TypeGroupIndicators.ColdWsterOdn
        //                    ||
        //                    x.Code == (int) TypeGroupIndicators.OutWaterOdn
        //                    ||
        //                    x.Code == (int) TypeGroupIndicators.HeatingOdn
        //                    ||
        //                    x.Code == (int) TypeGroupIndicators.ElectricityOdn
        //                );
        //            break;
        //        }
        //        case TypeGroupIndicators.AllOtherServices:
        //        {

        //            var indicators =
        //                Enum.GetValues(typeof (TypeGroupIndicators))
        //                    .OfType<TypeGroupIndicators>()
        //                    .Select(x => (int) x)
        //                    .ToList();
        //            query = query
        //                .Where(x => !indicators.Contains(x.Code));
        //            break;
        //        }
        //        default:
        //        {
        //            query = query
        //                .Where(x => x.Code == (int) groupIndicators);
        //            break;
        //        }
        //    }
        //    return query.ToList();
        //}
    }

    /// <summary>
    /// Прокси класс индикаторов
    /// </summary>
    public class ServiceIndicatorProxy
    {
        public long ServiceId { set; get; }
        public int IndicatorGroup { set; get; }
        public int Indicator { set; get; }
        public string FieldName { set; get; }
    }
}
