namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class RoomMigrationAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public override string Description => "Перенос сведений о помещениях и квартирах в новый реестр";

        public override string Name => "Перенос сведений о помещениях и квартирах в новый реестр";

        public override Func<IDataResult> Action => this.TransferSorgRoContracts;

        public BaseDataResult TransferSorgRoContracts()
        {
            var roomService = this.Container.Resolve<IDomainService<Room>>();
            var apartService = this.Container.Resolve<IDomainService<RealityObjectApartInfo>>();
            var houseService = this.Container.Resolve<IDomainService<RealityObjectHouseInfo>>();

            var roomDict = roomService.GetAll()
                .Select(x => new {x.RealityObject.Id, x.RoomNum})
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Where(y => !string.IsNullOrWhiteSpace(y.RoomNum))
                        .Select(y => y.RoomNum)
                        .GroupBy(y => y)
                        .ToDictionary(y => y.Key));

            var roomsToTransferByRo = apartService.GetAll().Select(
                apart => new
                {
                    roId = apart.RealityObject.Id,
                    RoomNum = apart.NumApartment,
                    Area = apart.AreaTotal.HasValue ? apart.AreaTotal.Value : 0m,
                    LivingArea = apart.AreaLiving.HasValue ? apart.AreaLiving.Value : 0m,
                    OwnershipType =
                        apart.Privatized == YesNoNotSet.Yes
                            ? RoomOwnershipType.Private
                            : RoomOwnershipType.NotSet,
                    Type = RoomType.Living
                })
                .AsEnumerable()
                .Union(
                    houseService.GetAll().Select(
                        houseInfo => new
                        {
                            roId = houseInfo.RealityObject.Id,
                            RoomNum = houseInfo.NumObject,
                            Area = houseInfo.TotalArea.HasValue ? houseInfo.TotalArea.Value : 0m,
                            LivingArea = 0m,
                            OwnershipType = RoomOwnershipType.Commerse,
                            Type = RoomType.NonLiving
                        }))
                .Where(x => !string.IsNullOrWhiteSpace(x.RoomNum))
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(
                        y => new Room(new RealityObject {Id = x.Key})
                        {
                            RoomNum = (y.RoomNum.Trim().Length > 10 ? y.RoomNum.Trim().Substring(0, 10) : y.RoomNum.Trim()).ToLower(),
                            Area = y.Area,
                            LivingArea = y.LivingArea,
                            OwnershipType = y.OwnershipType,
                            Type = y.Type,
                            CreatedFromPreviouisVersion = true,
                            Description = "",
                            EntranceNum = 0,
                            RoomsCount = 0,
                            Floor = 0
                        })
                        .GroupBy(y => y.RoomNum)
                        .Select(y => y.First())
                        .ToList());

            var roomsToCreate = new List<Room>();

            foreach (var roomsToTransfer in roomsToTransferByRo)
            {
                if (roomDict.ContainsKey(roomsToTransfer.Key))
                {
                    var robjectRooms = roomDict[roomsToTransfer.Key];

                    roomsToCreate.AddRange(roomsToTransfer.Value.Where(room => !robjectRooms.ContainsKey(room.RoomNum)));
                }
                else
                {
                    roomsToCreate.AddRange(roomsToTransfer.Value);
                }
            }

            this.SessionProvider.CloseCurrentSession();

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        roomsToCreate.ForEach(x => session.Insert(x));

                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        return new BaseDataResult(false, e.Message);
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}