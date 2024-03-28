namespace Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService.ChangeRoomAddress.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public class ChangeRoomAddressService : IChangeRoomAddressService
    {
        private readonly IGkhUserManager userManager;
        private readonly IWindsorContainer container;

        public ChangeRoomAddressService(IGkhUserManager userManager, IWindsorContainer container)
        {
            this.userManager = userManager;
            this.container = container;
        }

        /// <inheritdoc />
        public IDataResult SaveRoomAddress(BaseParams baseParams)
        {
            var basePersonalAccountId = baseParams.Params.GetAsId();
            var roomId = baseParams.Params.GetAs<long>("ApartmentNum");
            var user = userManager.GetActiveUser();

            var basePersonalAccountRepository = this.container.ResolveRepository<BasePersonalAccount>();
            var roomRepository = this.container.ResolveRepository<Room>();
            var personalAccountOperationLogDomain = this.container.ResolveDomain<EntityLogLight>();

            using (this.container.Using(basePersonalAccountRepository, roomRepository, personalAccountOperationLogDomain))
            {
                var basePersonalAccount = basePersonalAccountRepository.Get(basePersonalAccountId);
                var room = roomRepository.Get(roomId);
                var indexShare = basePersonalAccountRepository.GetAll()
                    .Where(w => w.Room.Id == roomId)
                    .Where(w => !w.State.FinalState)
                    .SafeSum(c => c.AreaShare);

                if (indexShare + basePersonalAccount.AreaShare <= decimal.One)
                {
                    var entityLogLight = new EntityLogLight
                    {
                        ParameterName = "room",
                        PropertyDescription = basePersonalAccount.Room.RoomNum,
                        PropertyValue = room.RoomNum,
                        DateActualChange = DateTime.Now,
                        User = user.Login,
                        DateApplied = DateTime.Now,
                        ClassName = "BasePersonalAccount",
                        PropertyName = "Room",
                        EntityId = basePersonalAccountId
                    };
                    basePersonalAccount.Room = room;

                    this.container.InTransaction(() =>
                    {
                        basePersonalAccountRepository.Update(basePersonalAccount);
                        personalAccountOperationLogDomain.Save(entityLogLight);
                    });

                    return new BaseDataResult(true, "Данные о квартире изменены");
                }

                return BaseDataResult.Error($"По помещению № {room.RoomNum} имеются активные ЛС с долей собственности 1");
            }
        }
    }
}