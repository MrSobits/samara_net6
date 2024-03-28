using Bars.Gkh.Utils.AddressPattern;

namespace Bars.Gkh.Regions.Tula.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Utils;
    using Entities;

    public class RealityObjectFixerController : BaseController
    {
        public IDomainService<RealityObject> RoDomain { get; set; }

        public IRepository<RealityObject> RoRepo { get; set; }

        public IDomainService<Municipality> MunDomain { get; set; }

        private Dictionary<string, long> _fiasCache;

        public Dictionary<string, long> FiasCache
        {
            get
            {
                return _fiasCache
                       ?? (_fiasCache =
                           MunDomain.GetAll()
                               .Select(x => new {x.FiasId, x.Id})
                               .ToList()
                               .GroupBy(x => x.FiasId)
                               .ToDictionary(x => x.Key, x => x.FirstOrDefault().Return(y => y.Id)));
            }
        }

        public ActionResult FixAddresses()
        {
            var isession = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            foreach (var x in RoDomain.GetAll())
            {
                x.Municipality = GetMunicipality(x.FiasAddress);
                if (x.FiasAddress != null)
                {
                    x.Address = x.Municipality != null ? Container.Resolve<IAddressPattern>().FormatShortAddress(x.Municipality, x.FiasAddress) : x.FiasAddress.AddressName;
                }

                isession.Update(x);
            }

            return JsSuccess();
        }

        private Municipality GetMunicipality(FiasAddress address)
        {
            if (address == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(address.AddressGuid))
            {
                return null;
            }

            var guidMass = address.AddressGuid.Split('#');

            Municipality result = null;

            foreach (var s in guidMass)
            {
                var t = s.Split('_');

                Guid g;

                Guid.TryParse(t[1], out g);

                if (g != Guid.Empty)
                {
                    var mcp = FiasCache.ContainsKey(g.ToString()) ? new Municipality{Id = FiasCache[g.ToString()], FiasId = g.ToString()} : null;
                    if (mcp != null)
                    {
                        result = mcp;
                    }
                }
            }

            return result;
        }
    }
}