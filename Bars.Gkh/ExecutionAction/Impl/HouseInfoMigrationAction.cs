namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class HouseInfoMigrationAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public override string Name => "Создание квартир в старом реестре на основе нового реестра (к ошибке 39324)";

        public override string Description => "Создание квартир в старом реестре на основе нового реестра. "
            + "Это действие необходимо выполнять когда в регионе не подключен модуль Регоператора и "
            + "пользователи утверждают, что пропали квартиры.";

        public override Func<IDataResult> Action => this.HouseInfoMigration;

        public BaseDataResult HouseInfoMigration()
        {
            var roomService = this.Container.Resolve<IDomainService<Room>>();
            var houseInfoService = this.Container.Resolve<IDomainService<RealityObjectHouseInfo>>();

            var houseInfoDict = houseInfoService.GetAll()
                .Select(x => new {x.RealityObject.Id, x.NumObject})
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Where(y => !string.IsNullOrWhiteSpace(y.NumObject))
                        .Select(y => y.NumObject)
                        .GroupBy(y => y)
                        .ToDictionary(y => y.Key));

            var roomsToTransferByRo = roomService.GetAll()
                .Select(x => new {x.RealityObject.Id, x.RoomNum, x.Area, x.Type})
                .AsEnumerable()
                .Where(y => !string.IsNullOrWhiteSpace(y.RoomNum))
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(
                        y => new RealityObjectHouseInfo
                        {
                            RealityObject = new RealityObject {Id = x.Key},
                            NumObject = y.RoomNum.Trim(),
                            TotalArea = y.Area,
                            Name = y.Type == RoomType.Living ? "Квартира" : null
                        })
                        .ToList());

            var housesInfoToCreate = new List<RealityObjectHouseInfo>();

            foreach (var roomsToTransfer in roomsToTransferByRo)
            {
                if (houseInfoDict.ContainsKey(roomsToTransfer.Key))
                {
                    var robjectRooms = houseInfoDict[roomsToTransfer.Key];

                    housesInfoToCreate.AddRange(roomsToTransfer.Value.Where(room => !robjectRooms.ContainsKey(room.NumObject)));
                }
                else
                {
                    housesInfoToCreate.AddRange(roomsToTransfer.Value);
                }
            }

            if (!housesInfoToCreate.Any())
            {
                return new BaseDataResult();
            }

            this.SessionProvider.CloseCurrentSession();

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        housesInfoToCreate.ForEach(x => session.Insert(x));

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