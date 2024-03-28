namespace Bars.Gkh.Gis.Controllers.GisAddressMatching
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Gis.DomainService.Register.HouseRegisterRegister;
    using DomainService.GisAddressMatching;

    public class GisAddressMatchingController : BaseController
    {
        protected IGisAddressMatchingService Service;
        protected IAddressService AddressService;
        protected IHouseRegisterService HouseRegisterService;

        public GisAddressMatchingController(
            IGisAddressMatchingService service,
            IAddressService addressService,
            IHouseRegisterService houseRegisterService
            )
        {
            Service = service;
            AddressService = addressService;
            HouseRegisterService = houseRegisterService;
        }

        public ActionResult GetAddresses(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = AddressService.GetAddresses(baseParams)
                .AsQueryable()
                .Filter(loadParams, Container);

            return new JsonListResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public ActionResult GetImportAddresses(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = AddressService.GetImportAddresses()
                .Filter(loadParams, Container);

            var result = data.Order(loadParams).Paging(loadParams).ToList();

            return new JsonListResult(result, data.Count());
        }


        public ActionResult GetFiasAdresses(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = Container.Resolve<IDomainService<FiasAddress>>()
                .GetAll()
                .Select(x => new
                {
                    x.Id,
                    CityName = x.PlaceName,
                    RegionName = x.PlaceAddressName,
                    x.StreetName,
                    Address = x.AddressName,
                    Number = x.House
                })
                .Filter(loadParams, Container);

            return new JsonListResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
        
        public ActionResult GetAdresses(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = Container.Resolve<IDomainService<Gkh.Entities.RealityObject>>()
                .GetAll()
                .Select(x => new
                {
                    x.FiasAddress.Id,
                    CityName = x.FiasAddress.PlaceName,
                    RegionName = x.FiasAddress.PlaceAddressName,
                    x.FiasAddress.StreetName,
                    Address = x.FiasAddress.AddressName,
                    Number = x.FiasAddress.House,
                    x.FiasAddress.Letter,
                    x.FiasAddress.Housing,
                    x.FiasAddress.Building

                })
                .Filter(loadParams, Container);

            return new JsonListResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public ActionResult ManualMathAddress(BaseParams baseParams)
        {
            var result = (BaseDataResult) Service.ManualMathAddress(baseParams);
            return new JsonNetResult(new { success = result.Success, message = result.Message });
        }

        public ActionResult ManualBillingAddressMatch(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.ManualBillingAddressMatch(baseParams);
            return new JsonNetResult(new { success = result.Success, message = result.Message });
        }

        public ActionResult MassManualMathAddress(BaseParams baseParams)
        {
            var result = (BaseDataResult) Service.MassManualMathAddress(baseParams);
            return new JsonNetResult(new {success = result.Success, message = result.Message});
        }

        public ActionResult ManualBillingAddressMismatch(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.ManualBillingAddressMismatch(baseParams);
            return new JsonNetResult(new { success = result.Success, message = result.Message });
        }

        /// <summary>
        /// Опеределить доступнось массового сопоставления
        /// </summary>
        public ActionResult SimilarAddressesDetected(BaseParams baseParams)
        {
            var result = (BaseDataResult) Service.DetectSimilarAddresses(baseParams);
            return new JsonNetResult(result.Data);
        }

        /// <summary>
        /// Присвоить типы домам
        /// </summary>
        public ActionResult SaveHouseType(BaseParams baseParams)
        {
            var result = (BaseDataResult) HouseRegisterService.SaveHouseTypes(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Присвоить Мо домам
        /// </summary>
        public ActionResult SaveHouseMunicipality(BaseParams baseParams)
        {
            var result = (BaseDataResult) HouseRegisterService.SaveHouseMunicipality(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}