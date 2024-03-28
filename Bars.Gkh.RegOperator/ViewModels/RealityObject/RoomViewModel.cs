namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Utils;

    public class RoomViewModel : BaseViewModel<Room>
    {
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<Room> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var roId = baseParams.Params.GetAs<long>("realtyId");
            var ownerId = baseParams.Params.GetAs<long>("ownerId");
            var entranceId = baseParams.Params.GetAsId("entranceId");

            long[] alreadyAddedRooms = null;
            if (ownerId != 0)
            {
                alreadyAddedRooms = this.BasePersonalAccountDomain.GetAll()
                    .WhereIf(roId > 0, x => x.Room.RealityObject.Id == roId)
                    .Where(x => x.AccountOwner.Id == ownerId)
                    .GroupBy(x => x.Room.Id)
                    .Where(x => x.Sum(y => y.AreaShare) >= 1)
                    .Select(x => x.Key)
                    .ToArray();
            }

            var query = domain.GetAll()
                .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
                .WhereIf(alreadyAddedRooms.IsNotEmpty(), x => !alreadyAddedRooms.Contains(x.Id))
                .WhereIf(entranceId > 0, x => x.Entrance.Id == entranceId)
                .Select(x => new
                {
                    x.Id,
                    x.Area,
                    x.Type,
                    x.OwnershipType,
                    x.RoomNum,
                    x.EntranceNum,
                    x.CadastralNumber,
                    x.Notation,
                    Entrance = (int?) x.Entrance.Number,
                    x.ChamberNum
                })
                .Filter(loadParams, this.Container);

            var roomIds = query.Select(x => x.Id).ToList();
            var totalCount = query.Count();

            var accountsPerRoom = this.BasePersonalAccountDomain.GetAll()
                .Where(x => roomIds.Contains(x.Room.Id))
                .Where(x => !x.State.FinalState)
                .GroupBy(x => x.Room.Id)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToDictionary(x => x.Key, x => x.count);

            var orderByRoomNum = loadParams.Order.FirstOrDefault(x => x.Name == "RoomNum");
            var sortedData = orderByRoomNum != null
                ? orderByRoomNum.Asc
                    ? query.AsEnumerable().OrderBy(x => x.RoomNum, new NumericComparer()).AsQueryable()
                    : query.AsEnumerable().OrderByDescending(x => x.RoomNum, new NumericComparer()).AsQueryable()
                : query.Order(loadParams);

            var result = sortedData.Paging(loadParams)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Area,
                    x.Type,
                    x.OwnershipType,
                    x.RoomNum,
                    x.EntranceNum,
                    x.CadastralNumber,
                    x.Notation,
                    x.Entrance,
                    AccountsNum = accountsPerRoom.Get(x.Id),
                    x.ChamberNum
                })
                .ToList();

            return new ListDataResult(result, totalCount);
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<Room> domain, BaseParams baseParams)
        {
            var room = domain.Get(baseParams.Params.GetAsId());

            var accountsPerRoom = this.BasePersonalAccountDomain.GetAll()
                .Where(x => x.Room.Id == room.Id).Count(x => !x.State.FinalState);

            return new BaseDataResult(new
            {
                room.Id,
                room.Area,
                room.CreatedFromPreviouisVersion,
                room.Description,
                room.EntranceNum,
                room.Floor,
                room.LivingArea,
                room.OwnershipType,
                room.RealityObject,
                room.RoomNum,
                room.RoomsCount,
                room.Type,
                AccountsNum = accountsPerRoom,
                room.IsRoomHasNoNumber,
                room.Notation,
                room.CadastralNumber,
                room.Entrance,
                room.ChamberNum,
                room.IsRoomCommonPropertyInMcd,
                room.IsCommunal,
                room.CommunalArea,
                room.PrevAssignedRegNumber,
                room.RecognizedUnfit,
                room.RecognizedUnfitReason,
                room.RecognizedUnfitDocNumber,
                room.RecognizedUnfitDocDate,
                room.RecognizedUnfitDocFile,
                room.HasSeparateEntrance
            });
        }
    }
}