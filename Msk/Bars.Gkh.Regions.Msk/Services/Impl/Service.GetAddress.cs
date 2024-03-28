namespace Bars.Gkh.Regions.Msk.Services.Impl
{
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Services.DataContracts;
    using Bars.Gkh.Regions.Msk.Entities;

    public partial class Service
    {
        public GetAddressResponse GetAddress()
        {
            var roInfoDomain = Container.ResolveDomain<RealityObjectInfo>();

            try
            {
                var data = roInfoDomain.GetAll()
                    .OrderBy(x => x.Okrug)
                    .ThenBy(x => x.Raion)
                    .ThenBy(x => x.Address)
                    .Select(x => new GetAddressRecord
                    {
                        Uid = x.Uid,
                        Address = x.Okrug + ", " + x.Raion + ", " + x.Address
                    })
                    .ToArray();

                return new GetAddressResponse
                {
                    AddressRecords = data
                };
            }
            finally
            {
                Container.Release(roInfoDomain);
            }
        }
    }
}