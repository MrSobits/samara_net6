using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh1468.Enums;

namespace Bars.Gkh1468.Controllers
{
    using Bars.Gkh.Enums;
    using Bars.Gkh1468.Entities;

    public class DesktopMapController : BaseController
    {
        public ActionResult GetContractors(BaseParams baseParams)
        {
            var muId = baseParams.Params["muId"].ToLong();
            var contragentTypeStr = baseParams.Params["contragentType"].ToStr();
            ContragentType? contragentType = null;

            if (!string.IsNullOrEmpty(contragentTypeStr))
            {
                ContragentType value;
                if (Enum.TryParse(contragentTypeStr, out value))
                {
                    contragentType = value;
                }
            }

            var data = Container.Resolve<IDomainService<HouseProviderPassport>>().GetAll()
                .Where(x => x.RealityObject.Municipality.Id == muId)
                .WhereIf(contragentType.HasValue, x => x.ContragentType == contragentType.Value)
                .Select(x => new
                {
                    x.Contragent.Id,
                    x.Contragent.Name,
                    x.Percent
                })
                .ToArray()
                .GroupBy(x => new { x.Id, x.Name })
                .Select(x => new { x.Key.Id, x.Key.Name, Percent = x.Average(y => y.Percent) })
                .ToArray();

            return new JsonListResult(data, data.Length);
        }

        public ActionResult GetPassports(BaseParams baseParams)
        {
            var muId = baseParams.Params["muId"].ToLong();
            var contragentId = baseParams.Params["contragentId"].ToLong();
            var typeRealObjStr = baseParams.Params["typeRealObj"].ToStr();
            TypeRealObj? typeRealObj = null;

            if (!string.IsNullOrEmpty(typeRealObjStr))
            {
                TypeRealObj value;
                if (Enum.TryParse(typeRealObjStr, out value))
                {
                    typeRealObj = value;
                }
            }

            var data =
                Container.Resolve<IDomainService<HouseProviderPassport>>()
                         .GetAll()
                .Where(x => x.Contragent.Id == contragentId)
                .Where(x => x.RealityObject.Municipality.Id == muId)
                         .WhereIf(
                                  typeRealObj.HasValue,
                             x =>
                                /*Эта хрень*/ typeRealObj.Value == TypeRealObj.RealityObject
                                /*из-за того*/    ? x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding || x.RealityObject.TypeHouse == TypeHouse.Individual
                                /*что в 1468*/    : typeRealObj.Value == TypeRealObj.Mkd
                                /*отличаются*/        ? x.RealityObject.TypeHouse == TypeHouse.ManyApartments
                                /*типы объектов*/     : x.RealityObject.TypeHouse == TypeHouse.NotSet || x.RealityObject.TypeHouse == TypeHouse.SocialBehavior)
                         .Select(x => new { x.Id, Name = x.RealityObject.FiasAddress.AddressName, x.Percent, x.HouseType, x.ReportMonth })
                .ToArray();

            return new JsonListResult(data, data.Length);
        }
    }
}