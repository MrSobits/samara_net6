namespace Bars.Gkh.ViewModel.RealityObject
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class RoomViewModel: BaseViewModel<Room>
    {
        public override IDataResult List(IDomainService<Room> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var roId = baseParams.Params.GetAs<long>("realtyId");
            var entranceId = baseParams.Params.GetAsId("entranceId");

            var query = domain.GetAll()
                .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
                .WhereIf(entranceId > 0, x => x.Entrance.Id == entranceId)
                .Select(x => new
                {
                    x.Id,
                    x.Area,
                    x.Type,
                    x.OwnershipType,
                    x.RoomNum,
                    x.EntranceNum,
                    x.Notation,
                    Entrance = (int?)x.Entrance.Number,
                    x.ChamberNum
                })
                .Filter(loadParams, this.Container);

            var totalCount = query.Count();

            var orderByRoomNum = loadParams.Order.FirstOrDefault(x => x.Name == "RoomNum");
            var sortedData = orderByRoomNum != null
                ? orderByRoomNum.Asc
                    ? query.AsEnumerable().OrderBy(x => x.RoomNum, new NumericComparer()).AsQueryable()
                    : query.AsEnumerable().OrderByDescending(x => x.RoomNum, new NumericComparer()).AsQueryable()
                : query.Order(loadParams);

            var result = sortedData.Paging(loadParams).ToList();

            return new ListDataResult(result, totalCount);
        }
    }
}