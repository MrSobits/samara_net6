namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    public class RoomAndAccountOwnerMigrationAction : BaseExecutionAction
    {
        private readonly IWindsorContainer _container;

        private readonly ILogImport _logImport;
        private readonly ILogImportManager _logImportManager;

        private readonly IDomainService<Room> _roomService;
        private readonly IRepository<BasePersonalAccount> _accountService;
        private readonly IDomainService<IndividualAccountOwner> _ownerService;
        private readonly IDomainService<RealityObjectApartInfo> _apartService;
        private readonly IDomainService<RealityObjectHouseInfo> _houseService;
        private readonly IChargePeriodService _periodService;
        private readonly IChargePeriodRepository _chargePeriodRepo;
        private readonly IDomainService<PersonalAccountPeriodSummary> _summaryDomain;

        private readonly IAccountNumberService _accountNumberService;

        private readonly State _accActiveState;

        private readonly FileData _file = null;

        public RoomAndAccountOwnerMigrationAction(
            IWindsorContainer container,
            ILogImport logImport,
            IDomainService<Room> roomService,
            IDomainService<RealityObjectApartInfo> apartService,
            IDomainService<State> stateDomainService,
            IAccountNumberService accountNumberService,
            IRepository<BasePersonalAccount> accountService,
            IDomainService<IndividualAccountOwner> ownerService,
            IDomainService<RealityObjectHouseInfo> houseService,
            ILogImportManager logImportManager,
            IChargePeriodService periodService,
            IDomainService<PersonalAccountPeriodSummary> summaryDomain,
            IChargePeriodRepository chargePeriodRepo)
        {
            this._container = container;
            this._logImport = logImport;
            this._roomService = roomService;
            this._accountService = accountService;
            this._ownerService = ownerService;
            this._houseService = houseService;
            this._logImportManager = logImportManager;
            this._periodService = periodService;
            this._summaryDomain = summaryDomain;
            this._chargePeriodRepo = chargePeriodRepo;
            this._apartService = apartService;
            this._accountNumberService = accountNumberService;

            this._accActiveState = stateDomainService.FirstOrDefault(
                x => x.TypeId == "gkh_regop_personal_account" && x.StartState);
            this._file = new FileData(this.Code, "csv", new byte[] { });
            logImport.SetFileName(this._file.FileName);
            logImport.ImportKey = this.Code;
        }

        public override string Description => "Перенос сведений о помещениях, квартирах и абонентах в новый реестр";

        public override string Name => "Перенос сведений о помещениях, квартирах и абонентах в новый реестр";

        public override Func<IDataResult> Action => this.MigrateRoomsAndAccounts;

        private BaseDataResult MigrateRoomsAndAccounts()
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var existingRoomsByRo = this._roomService.GetAll()
                    .Select(
                        x => new RoomInfo
                        {
                            RoId = x.RealityObject.Id,
                            RoomNum = x.RoomNum,
                            RoomId = x.Id,
                            HasAccount = this._accountService.GetAll()
                                .Any(y => y.Room.RoomNum == x.RoomNum && y.Room.RealityObject.Id == x.RealityObject.Id),
                            Room = x
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Where(y => !string.IsNullOrWhiteSpace(y.RoomNum))
                            .Select(y => y)
                            .GroupBy(y => y.RoomNum)
                            .ToDictionary(y => y.Key));

                // MigrateHouseInfos(existingRoomsByRo);

                this.MigrateApartInfos(existingRoomsByRo);
            }
            finally
            {
                sw.Stop();
                var ms = sw.ElapsedMilliseconds;
                this._logImport.Info("Stopwatch", "Время выполнения операции: " + ms);

                this._logImportManager.FileNameWithoutExtention = this.Name;
                this._logImportManager.Add(this._file, this._logImport);
                this._logImportManager.Save();
            }

            return new BaseDataResult();
        }

        private void MigrateApartInfos(
            Dictionary<long, Dictionary<string, IGrouping<string, RoomInfo>>> existingRoomsByRo)
        {
            var apartInfosByRo = this._apartService.GetAll()
                .Select(
                    apart => new
                    {
                        roId = apart.RealityObject.Id,
                        moId = apart.RealityObject.Municipality.Id,
                        address = apart.RealityObject.Address,
                        apart.RealityObject,
                        RoomNum = apart.NumApartment,
                        Area = apart.AreaTotal.HasValue ? apart.AreaTotal.Value : 0m,
                        LivingArea = apart.AreaLiving.HasValue ? apart.AreaLiving.Value : 0m,
                        OwnershipType =
                            apart.Privatized == YesNoNotSet.Yes
                                ? RoomOwnershipType.Private
                                : RoomOwnershipType.NotSet,
                        Type = RoomType.Living,
                        apart.FioOwner
                    })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.RoomNum))
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(
                        y => new ApartInfo
                        {
                            RealityObject = y.RealityObject,
                            RoomNum =
                                (y.RoomNum.Trim().Length > 10 ? y.RoomNum.Trim().Substring(0, 10) : y.RoomNum.Trim())
                                    .ToLower(),
                            Area = y.Area,
                            LivingArea = y.LivingArea,
                            OwnershipType = y.OwnershipType,
                            Type = y.Type,
                            FioOwner = y.FioOwner
                        })
                        .GroupBy(y => y.RoomNum)
                        .Select(y => y.First())
                        .ToList());

            var apartInfosToCreate = new List<ApartInfo>();
            var ownersToCreate = new List<IndividualAccountOwner>();
            var accountsToCreate = new List<BasePersonalAccount>();

            foreach (var apartInfo in apartInfosByRo)
            {
                if (existingRoomsByRo.ContainsKey(apartInfo.Key))
                {
                    var existingRooms = existingRoomsByRo[apartInfo.Key];
                    var existingRoomsInfos = apartInfo.Value
                        .Where(
                            info =>
                                existingRooms.ContainsKey(info.RoomNum) &&
                                    !string.IsNullOrWhiteSpace(info.FioOwner));

                    foreach (var existingRoomsInfo in existingRoomsInfos)
                    {
                        var roomInfo = existingRooms[existingRoomsInfo.RoomNum];

                        var room = roomInfo.Select(x => x.Room).First();
                        if (!roomInfo.Select(x => x.HasAccount).First())
                        {
                            try
                            {
                                var owner = this.CreateOwner(existingRoomsInfo.FioOwner);
                                ownersToCreate.Add(owner);
                                accountsToCreate.Add(this.CreateAccount(owner, room));
                            }
                            catch (NameValidationException)
                            {
                                this._logImport.Error(
                                    "Не удалость создать абонента",
                                    string.Format(
                                        "Не удалось извлечь имя абонента. Адрес:{0}, Номер помещения: {1}, ФИО: {2}",
                                        room.RealityObject.Address,
                                        existingRoomsInfo.RoomNum,
                                        existingRoomsInfo.FioOwner));
                            }
                            catch (NotSupportedException)
                            {
                                this._logImport.Error(
                                    "Не удалость создать абонента",
                                    string.Format(
                                        "У дома отсутствует привязка к ФИАС. Адрес:{0}, Номер помещения: {1}, ФИО: {2}",
                                        room.RealityObject.Address,
                                        existingRoomsInfo.RoomNum,
                                        existingRoomsInfo.FioOwner));
                            }
                            catch (Exception)
                            {
                                this._logImport.Error(
                                    "Не удалость создать абонента",
                                    string.Format(
                                        "Непредвиденная ошибка. Адрес:{0}, Номер помещения: {1}, ФИО: {2}",
                                        room.RealityObject.Address,
                                        existingRoomsInfo.RoomNum,
                                        existingRoomsInfo.FioOwner));
                            }
                        }
                        else
                        {
                            this._logImport.Info(
                                "Лицевой счет существует",
                                string.Format(
                                    "Адрес:{0}, Номер помещения: {1}",
                                    room.RealityObject.Address,
                                    existingRoomsInfo.RoomNum));
                        }
                    }

                    var absentRooms = apartInfo.Value.Where(room => !existingRooms.ContainsKey(room.RoomNum));
                    apartInfosToCreate.AddRange(absentRooms);
                }
                else
                {
                    apartInfosToCreate.AddRange(apartInfo.Value);
                }
            }

            using (var tr = this._container.Resolve<IDataTransaction>())
            {
                try
                {
                    var roomsToSave = new List<Room>();

                    foreach (var apart in apartInfosToCreate)
                    {
                        var room = new Room
                        {
                            RealityObject = apart.RealityObject,
                            RoomNum = apart.RoomNum,
                            Area = apart.Area,
                            LivingArea = apart.LivingArea,
                            OwnershipType = apart.OwnershipType,
                            Type = apart.Type,
                            CreatedFromPreviouisVersion = true,
                            Description = "",
                            EntranceNum = 0,
                            RoomsCount = 0,
                            Floor = 0,
                        };
                        roomsToSave.Add(room);

                        try
                        {
                            var owner = this.CreateOwner(apart.FioOwner);
                            ownersToCreate.Add(owner);
                            accountsToCreate.Add(this.CreateAccount(owner, room));
                        }
                        catch (NameValidationException)
                        {
                            this._logImport.Error(
                                "Не удалость создать абонента",
                                string.Format(
                                    "Не удалось извлечь имя абонента. Адрес:{0}, Номер помещения: {1}, ФИО: {2}",
                                    room.RealityObject.Address,
                                    apart.RoomNum,
                                    apart.FioOwner));
                        }
                        catch (NotSupportedException)
                        {
                            this._logImport.Error(
                                "Не удалость создать абонента",
                                string.Format(
                                    "У дома отсутствует привязка к ФИАС. Адрес:{0}, Номер помещения: {1}, ФИО: {2}",
                                    room.RealityObject.Address,
                                    apart.RoomNum,
                                    apart.FioOwner));
                        }
                        catch (Exception)
                        {
                            this._logImport.Error(
                                "Не удалость создать абонента",
                                string.Format(
                                    "Непредвиденная ошибка. Адрес:{0}, Номер помещения: {1}, ФИО: {2}",
                                    room.RealityObject.Address,
                                    apart.RoomNum,
                                    apart.FioOwner));
                        }
                    }

                    this._accountNumberService.Generate(accountsToCreate);

                    var currentPeriod = this._chargePeriodRepo.GetCurrentPeriod();

                    if (accountsToCreate.Any())
                    {
                        if (currentPeriod == null)
                        {
                            this._periodService.CreateFirstPeriod();
                            currentPeriod = this._chargePeriodRepo.GetCurrentPeriod();
                        }
                    }

                    foreach (var room in roomsToSave)
                    {
                        this._roomService.Save(room);
                    }

                    foreach (var owner in ownersToCreate)
                    {
                        this._ownerService.Save(owner);
                    }

                    foreach (var account in accountsToCreate)
                    {
                        if (string.IsNullOrWhiteSpace(account.PersonalAccountNum))
                        {
                            this._logImport.Error(
                                "Не удалость сгенерировать номер счета для абонента",
                                string.Format(
                                    "Адрес:{0}, Номер помещения: {1}, ФИО: {2}. Возможная причина: необходимо заполнить коды из Справочника формирования ЛС",
                                    account.Room.RealityObject.Address,
                                    account.Room.RoomNum,
                                    account.AccountOwner.Name));
                        }
                        else
                        {
                            this._accountService.Save(account);
                            this._summaryDomain.Save(
                                new PersonalAccountPeriodSummary
                                {
                                    Period = currentPeriod,
                                    PersonalAccount = account
                                });
                        }
                    }

                    this._logImport.Info("Trace", "accounts");
                    tr.Commit();
                }
                catch (Exception e)
                {
                    this._logImport.Error("Критическая ошибка", e.Message);
                    tr.Rollback();
                    throw;
                }
            }
        }

        private BasePersonalAccount CreateAccount(IndividualAccountOwner owner, Room room)
        {
            var account = new BasePersonalAccount
            {
                Room = room,
                AccountOwner = owner,
                AreaShare = 1,
                OpenDate = new DateTime(2014, 10, 1),
                State = this._accActiveState
            };
            return account;
        }

        private void MigrateHouseInfos(Dictionary<long, Dictionary<string, IGrouping<string, RoomInfo>>> existingRooms)
        {
            var houseInfoByRo = this._houseService.GetAll()
                .Select(
                    houseInfo => new
                    {
                        roId = houseInfo.RealityObject.Id,
                        moId = houseInfo.RealityObject.Municipality.Id,
                        RoomNum = houseInfo.NumObject,
                        Area = houseInfo.TotalArea.HasValue ? houseInfo.TotalArea.Value : 0m,
                        LivingArea = 0m,
                        OwnershipType = RoomOwnershipType.Commerse,
                        Type = RoomType.NonLiving
                    })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.RoomNum))
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(
                        y => new Room
                        {
                            RealityObject =
                                new RealityObject {Id = x.Key, Municipality = new Municipality {Id = y.moId}},
                            RoomNum =
                                (y.RoomNum.Trim().Length > 10 ? y.RoomNum.Trim().Substring(0, 10) : y.RoomNum.Trim())
                                    .ToLower(),
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

            foreach (var houseInfo in houseInfoByRo)
            {
                if (existingRooms.ContainsKey(houseInfo.Key))
                {
                    var robjectRooms = existingRooms[houseInfo.Key];
                    roomsToCreate.AddRange(houseInfo.Value.Where(room => !robjectRooms.ContainsKey(room.RoomNum)));
                }
                else
                {
                    roomsToCreate.AddRange(houseInfo.Value);
                }
            }

            using (var tr = this._container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var room in roomsToCreate)
                    {
                        this._roomService.Save(room);
                    }
                    tr.Commit();
                }
                catch (Exception e)
                {
                    this._logImport.Error("Критическая ошибка", e.Message);
                    tr.Rollback();
                }
            }
        }

        private IndividualAccountOwner CreateOwner(string fullName)
        {
            var person = new FullNameParser().Parse(fullName);

            var owner = new IndividualAccountOwner
            {
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                Surname = person.LastName,
                IdentityNumber = string.Empty,
                IdentitySerial = string.Empty
            };

            return owner;
        }

        internal class RoomInfo
        {
            public long RoId { get; set; }

            public string RoomNum { get; set; }

            public long RoomId { get; set; }

            public bool HasAccount { get; set; }

            public Room Room { get; set; }
        }

        internal class ApartInfo
        {
            public RealityObject RealityObject { get; set; }

            public string RoomNum { get; set; }

            public decimal Area { get; set; }

            public decimal LivingArea { get; set; }

            public RoomOwnershipType OwnershipType { get; set; }

            public RoomType Type { get; set; }

            public string FioOwner { get; set; }
        }
    }
}