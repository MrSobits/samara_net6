namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetHousesManOrgWithoutOpInf;

    public partial class Service
    {
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }

        public GetHousesManOrgWithoutOpResponse GetHousesManOrgWithoutOpInf(string moId, string startDateD, string startDateM, string startDateY, string endDateD, string endDateM, string endDateY)
        {
            var startDateStr = startDateD + "." + startDateM + "." + startDateY;
            var endDateStr = endDateD + "." + endDateM + "." + endDateY;
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(startDateStr, out startDate);
            DateTime.TryParse(endDateStr, out endDate);

            var housesManOrg = this.GetHousesManOrg(moId.ToLong(), startDate, endDate);
            var noHousesManOrg = this.GetNoHousesManOrg(moId.ToLong(), startDate, endDate);

            return new GetHousesManOrgWithoutOpResponse
            {
                HouseManOrg = housesManOrg,
                NoHousesManOrg = noHousesManOrg,
                Result = housesManOrg.Any() || noHousesManOrg.Any() ? Result.NoErrors : Result.DataNotFound
            };
        }

        private HouseManOrgProxy[] GetHousesManOrg(long moId, DateTime startDate, DateTime endDate)
        {
            return ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Where(x => x.ManOrgContract.ManagingOrganization.Id == moId)
                .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= endDate)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || (x.ManOrgContract.EndDate.Value > startDate && x.ManOrgContract.EndDate.Value > endDate))
                .Select(x => new 
                {
                    x.RealityObject.Id,
                    x.RealityObject.Address,
                    x.RealityObject.AreaLiving,
                    x.ManOrgContract.StartDate
                })
                .AsEnumerable()
                .Select(x => new HouseManOrgProxy
                {
                    Id = x.Id,
                    Address = x.Address,
                    AreaRooms = x.AreaLiving.ToStr(),
                    StartData = x.StartDate.HasValue ? x.StartDate.Value.ToString("dd.MM.yyyy") : string.Empty
                })
                .ToArray();
        }

        private NoHousesManOrgProxy[] GetNoHousesManOrg(long moId, DateTime startDate, DateTime endDate)
        {
            return ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Where(x => x.ManOrgContract.ManagingOrganization.Id == moId)
                .Where(x => x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate.Value <= endDate)
                .Where(x => x.ManOrgContract.EndDate.HasValue && x.ManOrgContract.EndDate.Value > startDate && x.ManOrgContract.EndDate.Value <= endDate)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.RealityObject.Address,
                    x.RealityObject.AreaLiving,
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.EndDate
                })
                .AsEnumerable()
                .Select(x => new NoHousesManOrgProxy
                {
                    Id = x.Id,
                    Address = x.Address,
                    AreaRooms = x.AreaLiving.ToStr(),
                    StartData = x.StartDate.HasValue ? x.StartDate.Value.ToString("dd.MM.yyyy") : string.Empty,
                    FinishData = x.EndDate.HasValue ? x.EndDate.Value.ToString("dd.MM.yyyy") : string.Empty,
                })
                .ToArray();
        }
    }
}