using Castle.Windsor;

namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Domain;

    using Gkh.Entities;

    public class LocalityAddressByMoController : BaseController
    {
        public IWindsorContainer Container { get; set; }

        private readonly IDomainService<RealityObject> _realObjDomain;

        public LocalityAddressByMoController(
            IDomainService<RealityObject> realObjDomain)
        {
            _realObjDomain = realObjDomain;
        }

        public ActionResult ListLocalityByMo(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var moId = baseParams.Params.GetAs<long>("moId");

            if (moId == 0)
            {
                return JsonListResult.EmptyList;
            }

            var placeNames =
                _realObjDomain.GetAll()
                    .Where(x => x.MoSettlement.Id == moId)
                    .Where(x => x.FiasAddress != null)
                    .Select(x => x.FiasAddress.PlaceName)
                    .Distinct();

            var list = new List<Locality>();
            foreach (var placeName in placeNames)
            {
                var locality = new Locality
                                   {
                                       Id = placeName,
                                       Name = placeName
                                   };
                list.Add(locality);
            }

            return new JsonListResult(list.AsQueryable().Order(loadParams).Paging(loadParams), list.Count());
        }

        public ActionResult ListAddressByLocality(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var moId = baseParams.Params.GetAs<long>("moId");
            var locality = baseParams.Params.GetAs<string>("locality");

            if (moId == 0)
            {
                return JsonListResult.EmptyList;
            }

            var addressNameFilter = loadParams != null ? loadParams.ComplexFilter : null;

            var addresses =
                _realObjDomain.GetAll()
                    .Where(x => x.MoSettlement.Id == moId)
                    .WhereIf(
                        locality != string.Empty,
                        x => x.FiasAddress != null && x.FiasAddress.PlaceName == locality)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Name = !x.FiasAddress.Housing.IsEmpty()
                            ? x.FiasAddress.StreetName.Append(", д. ").Append(x.FiasAddress.House).Append(x.FiasAddress.Housing)
                            : x.FiasAddress.StreetName.Append(", д. ").Append(x.FiasAddress.House)
                    })
                    .WhereIf(addressNameFilter != null, x => x.Name.ToLower().Contains(
                        addressNameFilter.Value.ToString().ToLower()))
                    .ToList();

            return new JsonListResult(addresses.AsQueryable().Order(loadParams).Paging(loadParams), addresses.Count());
        }


        public ActionResult ListLocality(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var moIds = baseParams.Params.GetAs("moIds", string.Empty).ToLongArray();

            var list =
                _realObjDomain.GetAll()
                    .WhereIf(moIds.Any(), x => moIds.Contains(x.MoSettlement.Id))
                    .Where(x => x.FiasAddress != null)
                    .Select(x => new
                    {
                        Id = x.FiasAddress.PlaceName,
                        Name = x.FiasAddress.PlaceName,
                        Settlement = x.MoSettlement.Name,
                        Municipality = x.Municipality.Name
                    })
                    .AsEnumerable()
                    .Distinct();

            return new JsonListResult(list.AsQueryable().Filter(loadParams, Container).Order(loadParams).Paging(loadParams), list.Count());
        }


        public class Locality
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}