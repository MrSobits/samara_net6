namespace Bars.Gkh.RegOperator.NSO.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Extenstions;

    using NHibernate;

    public class CreateFakePersonalAccountsAction : BaseExecutionAction
    {
        private State _state;
        const string _format = "{0}{1}";
        private int _idx = 0;

        
        public IRepository<Wallet> WalletRepo { get; set; }

        public static string Id = "CreateFakePersonalAccountsAction";

        #region Implementation of IExecutionAction

        /// <summary>
        /// Код для регистрации
        /// </summary>
        public string Code { get { return Id; } }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description { get { return "Генерация фейковый ЛС с привязкой к дому"; } }

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name { get { return "Генерация фейковый ЛС с привязкой к дому"; } }

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action { get { return CreateAccounts; } }

        #endregion

        private BaseDataResult CreateAccounts()
        {
            var roomDomain = Container.ResolveRepository<Room>();
            var ownerDomain = Container.ResolveRepository<IndividualAccountOwner>();
            var paDomain = Container.ResolveRepository<BasePersonalAccount>();
            var sessionProv = Container.Resolve<ISessionProvider>();

            using (Container.Using(roomDomain, ownerDomain, paDomain, sessionProv))
            {
                var session = sessionProv.GetCurrentSession();
                var oldF = session.FlushMode;
                session.FlushMode = FlushMode.Never;

                List<RealityObject> ros = GetRosWithoutAccs();

                try
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        foreach (var ro in ros)
                        {
                            Room room = GetOrCreateRoom(roomDomain, ro);
                            PersonalAccountOwner owner = GetOrCreateOwner(ownerDomain);
                            BasePersonalAccount account = GetOrCreateAccount(paDomain, owner, room);
                        }

                        Flush();

                        try
                        {
                            session.FlushMode = oldF;
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseDataResult(false, ex.Message);
                }

                return new BaseDataResult();
            }
        }

        private Room GetOrCreateRoom(IRepository<Room> roomDomain, RealityObject ro)
        {
            var room =
                roomDomain.GetAll()
                    .FirstOrDefault(x => (x.RoomNum == "00007" || x.RoomNum == "111") && x.RealityObject.Id == ro.Id)
                ?? new Room();

            decimal area;
            if (ro.AreaLivingOwned > 0)
            {
                area = ro.AreaLivingOwned.GetValueOrDefault();
            }
            else if (ro.AreaLiving > 0)
            {
                area = ro.AreaLiving.GetValueOrDefault();
            }
            else
            {
                area = ro.AreaMkd.GetValueOrDefault();
            }

            room.RoomNum = area > 0 ? "00007" : "111";
            room.LivingArea = room.Area = area;
            room.RealityObject = ro;

            roomDomain.SaveOrUpdate(room);

            return room;
        }

        private PersonalAccountOwner GetOrCreateOwner(IRepository<IndividualAccountOwner> ownerDomain)
        {
            var owner = new IndividualAccountOwner()
            {
                Surname = "Фиктивный",
                FirstName = "Счет",
                SecondName = "Дома",
                IdentityNumber = string.Empty,
                IdentitySerial = string.Empty
            };

            ownerDomain.SaveOrUpdate(owner);

            return owner;
        }

        private BasePersonalAccount GetOrCreateAccount(IRepository<BasePersonalAccount> paDomain, PersonalAccountOwner owner, Room room)
        {
            var acc = paDomain.GetAll()
                .FirstOrDefault(x => x.Room.Id == room.Id && x.AccountOwner.Id == owner.Id)
                      ?? new BasePersonalAccount();

            acc.Room = room;
            acc.AccountOwner = owner;
            acc.OpenDate = new DateTime(2014, 8, 1);
            acc.AreaShare = 1;
            acc.State = GetOpenState();
            acc.PersAccNumExternalSystems = room.RealityObject.ExternalId;
            acc.PersonalAccountNum = CreateAccountNum(room.RealityObject);

            CreateWallets(acc);

            paDomain.SaveOrUpdate(acc);

            return acc;
        }

        private void CreateWallets(BasePersonalAccount acc)
        {
            foreach (var wallet in acc.GetWallets())
            {
                if (wallet.Id == 0)
                {
                    WalletRepo.Save(wallet);
                }
            }
        }

        private string CreateAccountNum(RealityObject realityObject)
        {
            var idPlusIdx = "{0}{1}".FormatUsing(realityObject.Id, _idx++);
            var add = 12 - idPlusIdx.Length;
            var addAtEnd = string.Empty;

            if (add > 0)
            {
                addAtEnd = string.Join("", Enumerable.Range(0, add).Select(x => "0"));
            }

            return _format.FormatUsing(idPlusIdx, addAtEnd);
        }

        private State GetOpenState()
        {
            if (_state == null)
            {
                _state = GetOrCreateState();
            }

            return _state;
        }

        private State GetOrCreateState()
        {
            var stateDomain = Container.ResolveRepository<State>();
            var stateProv = Container.Resolve<IStateProvider>();
            using (Container.Using(stateDomain, stateProv))
            {
                var info = stateProv.GetStatefulEntityInfo(typeof (BasePersonalAccount));
                var state = stateDomain.GetAll()
                    .FirstOrDefault(x => x.Name == "Открыт" && x.TypeId == info.TypeId)
                    ?? new State()
                    {
                        Code = "Open",
                        Description = "Открыт",
                        Name = "Открыт",
                        StartState = true,
                        OrderValue = 1,
                        TypeId = info.TypeId
                    };

                return state;
            }
        }

        private void Flush()
        {
        }

        private List<RealityObject> GetRosWithoutAccs()
        {
            var roDomain = Container.ResolveDomain<RealityObject>();
            var accDomain = Container.ResolveDomain<BasePersonalAccount>();

            using (Container.Using(roDomain, accDomain))
            {
                var ros = roDomain.GetAll()
                    .Where(
                        x =>
                            !accDomain.GetAll()
                                .Where(acc => acc.Room.RoomNum == "00007" || acc.Room.RoomNum == "007")
                                .Any(acc => acc.Room.RealityObject.Id == x.Id)
                    )
                    .Select(x => new RealityObject()
                    {
                        Id = x.Id,
                        AreaMkd = x.AreaMkd,
                        AreaLivingOwned = x.AreaLivingOwned,
                        AreaLiving = x.AreaLiving,
                        ExternalId = x.ExternalId
                    })
                    .ToList();

                return ros;
            }
        }
    }
}