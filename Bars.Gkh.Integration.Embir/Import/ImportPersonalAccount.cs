namespace Bars.Gkh.Integration.Embir.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Integration.DataFetcher;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;
    using Gkh.Import.Impl;
    using Newtonsoft.Json.Linq;

    public class ImportPersonalAccount : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key { get { return Id; } }

        public override string CodeImport { get { return "ImportEmbir"; } }

        public override string Name { get { return "Импорт лицевых счетов с ЕМБИР"; } }

        public override string PossibleFileExtensions { get { return string.Empty; } }

        public override string PermissionName { get { return "Import.Embir.View"; } }

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        public IDomainService<IndividualAccountOwner> IndivAccountOwnerDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<Room> RoomDomain { get; set; }

        public IRepository<State> StateDomain { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        private long previousFileId = 0;

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            Exception thrown = null;
            try
            {
                var webClientFetcher = new WebClientFetcher();
                var importHelper = new ImportIntegrationHelper(Container);
                var httpQueryBuilder = importHelper.GetHttpQueryBuilder();

                httpQueryBuilder.AddParameter("type", "PersonalAccountEntity");
                var skip = 0;
                var take = 1000;
                var select = new DynamicDictionary
                {
                    {"StreetGuidId", "FlatNum.RealtyObjectId.FiasAddress.StreetGuidId"},
                    {"House", "FlatNum.RealtyObjectId.FiasAddress.House"},
                    {"Housing", "FlatNum.RealtyObjectId.FiasAddress.Housing"},
                    {"AccountNum", "AccountNum"},
                    {"Family", "Family"},
                    {"Name", "Name"},
                    {"LastName", "LastName"},
                    {"BirthDate", "BirthDate"},
                    {"FlatNum", "FlatNum.FlatNum"},
                    {"TotalSquare", "FlatNum.TotalSquare"}
                };

                httpQueryBuilder.AddDictionary("select", select);
                httpQueryBuilder.AddParameter("take", take);
                httpQueryBuilder.AddParameter("order", "id desc");

                LogImport.Info("Info", "Before loading");
                dynamic[] dynPersAccounts = Enumerable.ToArray(webClientFetcher.GetData(httpQueryBuilder));

                LogImport.Info("Info", "Loaded: " + dynPersAccounts.Length);

                var startState =
                    StateDomain.GetAll().FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.StartState);

                if (startState == null)
                {
                    throw new ValidationException("Необходимо добавить начальный статус для лицевых счетов");
                }

                var persAccountsByNum = PersonalAccountDomain.GetAll()
                    .GroupBy(x => x.PersonalAccountNum)
                    .ToDictionary(x => x.Key, y => y.First());

                var indivAccountOwnersByName = IndivAccountOwnerDomain.GetAll()
                    .Select(x => new
                    {
                        x.FirstName,
                        x.Surname,
                        x.SecondName,
                        x.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => "{0} {1} {2}".FormatUsing(x.Surname.Trim().ToUpper(), x.FirstName.Trim().ToUpper(), x.SecondName.Trim().ToUpper()))
                    .ToDictionary(x => x.Key, x => x.First().Id);

                var indivAccountOwnerCache = IndivAccountOwnerDomain.GetAll().ToDictionary(x => x.Id, x => x);

                var realObjByAddressGuid = RealityObjectDomain.GetAll().Where(x => x.FiasAddress != null).Select(x => new
                {
                    x.FiasAddress.StreetGuidId,
                    x.FiasAddress.House,
                    x.FiasAddress.Housing,
                    x.Id
                })
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}_{2}".FormatUsing(x.StreetGuidId, x.House, x.Housing))
                    .ToDictionary(x => x.Key, x => x.First().Id);

                var roomByRo = RoomDomain.GetAll().AsEnumerable().GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var areaShareByRoom = PersonalAccountDomain.GetAll()
                    .GroupBy(x => x.Room.Id)
                    .ToDictionary(x => x.Key, y => y.SafeSum(x => x.AreaShare));

                var persistenceContext = SessionProvider.CurrentSession.GetSessionImplementation().PersistenceContext;

                var realityObjectByAddress = RealityObjectDomain.GetAll().Where(x => x.FiasAddress != null).Select(x => new
                {
                    x.Id,
                    x.Address
                }).ToDictionary(x => x.Id, x => x.Address);

                LogImport.Info("Info", "Caches initialized");

                var listIndivAccToSave = new List<IndividualAccountOwner>();
                var listRoomToSave = new List<Room>();
                var listPersAccToSave = new List<BasePersonalAccount>();

                while (0 < dynPersAccounts.Length)
                {
                    var index = 0;
                    foreach (var dynPersAccount in dynPersAccounts)
                    {
                        index++;
                        LogImport.Info("Info", "Item: " + index);
                        try
                        {
                            var persAccount = (JObject) (dynPersAccount);

                            var persAccNum = persAccount["AccountNum"].ToStr();
                            var personalAccount = persAccountsByNum.Get(persAccNum) ?? new BasePersonalAccount();
                            personalAccount.State = startState;
                            personalAccount.PersonalAccountNum = persAccNum;
                            var accNumStr = "Номер л/с: {0}".FormatUsing(persAccNum);

                            var indivAcc = new IndividualAccountOwner();
                            var surname = persAccount["Family"].ToStr();
                            var firstName = persAccount["Name"].ToStr();
                            var secondName = persAccount["LastName"].ToStr();
                            var fio = "{0} {1} {2}".FormatUsing(surname.Trim().ToUpper(), firstName.Trim().ToUpper(), secondName.Trim().ToUpper());

                            var isPersAccChange = false;
                            var isAlreadyToSave = false;
                            if (!surname.IsEmpty() || !firstName.IsEmpty() || !secondName.IsEmpty())
                            {
                                indivAcc.Surname = surname;
                                indivAcc.FirstName = firstName;
                                indivAcc.SecondName = secondName;
                                indivAcc.Name = fio;
                                indivAcc.IdentityNumber = string.Empty;
                                indivAcc.IdentitySerial = string.Empty;
                                indivAcc.IdentityType = IdentityType.Passport;

                                if (personalAccount.AccountOwner != null)
                                {
                                    indivAcc = indivAccountOwnerCache[personalAccount.AccountOwner.Id];

                                    if ((indivAcc.Surname.IsEmpty() || indivAcc.Surname == surname) &&
                                        (indivAcc.FirstName.IsEmpty() || indivAcc.FirstName == firstName) &&
                                        (indivAcc.SecondName.IsEmpty() || indivAcc.SecondName == secondName) &&
                                        (indivAcc.Surname.IsEmpty() || indivAcc.FirstName.IsEmpty() || indivAcc.SecondName.IsEmpty()))
                                    {
                                        indivAcc.Surname = surname;
                                        indivAcc.FirstName = firstName;
                                        indivAcc.SecondName = secondName;

                                        listIndivAccToSave.Add(indivAcc);
                                        LogImport.Info("Информация",
                                            "Дополнен ФИО абонента. ФИО: {0} {1} {2}.".FormatUsing(surname, firstName,
                                                secondName));
                                    }
                                }
                                else
                                {
                                    long value;
                                    if (indivAccountOwnersByName.TryGetValue(fio, out value))
                                    {
                                        if (value == 0)
                                        {
                                            indivAcc =
                                                listIndivAccToSave.FirstOrDefault(
                                                    x => "{0} {1} {2}".FormatUsing(x.Surname.Trim().ToUpper(), x.FirstName.Trim().ToUpper(), x.SecondName.Trim().ToUpper()) == fio);

                                            if (indivAcc == null)
                                            {
                                                LogImport.Info("Информация",
                                        "Добавлен новый абонент. ФИО: {0} {1} {2}.".FormatUsing(surname, firstName,
                                            secondName));

                                                indivAcc = new IndividualAccountOwner
                                                {
                                                    Surname = surname,
                                                    FirstName = firstName,
                                                    SecondName = secondName,
                                                    Name = fio,
                                                    IdentityNumber = string.Empty,
                                                    IdentitySerial = string.Empty,
                                                    IdentityType = IdentityType.Passport
                                                };

                                                listIndivAccToSave.Add(indivAcc);
                                                if (!indivAccountOwnersByName.ContainsKey(fio))
                                                {
                                                    indivAccountOwnersByName.Add(fio, 0);
                                                }
                                            }

                                            isAlreadyToSave = true;
                                        }
                                        else
                                        {
                                            indivAcc = indivAccountOwnerCache[value];
                                        }
                                    }
                                }

                                if (indivAcc.Id == 0 && !isAlreadyToSave)
                                {
                                    LogImport.Info("Информация",
                                        "Добавлен новый абонент. ФИО: {0} {1} {2}.".FormatUsing(surname, firstName,
                                            secondName));

                                    listIndivAccToSave.Add(indivAcc);
                                    if (!indivAccountOwnersByName.ContainsKey(fio))
                                    {
                                        indivAccountOwnersByName.Add(fio, 0);
                                    }
                                }

                                if (personalAccount.AccountOwner == null)
                                {
                                    personalAccount.AccountOwner = indivAcc;
                                    isPersAccChange = true;
                                }
                            }
                            else
                            {
                                if (personalAccount.AccountOwner == null)
                                {
                                    LogImport.Error("Ошибка",
                                        "Лицевой счет не имеет собственника. {0} .".FormatUsing(accNumStr));
                                }
                            }

                            if (personalAccount.Room == null)
                            {
                                var streetGuid = persAccount["StreetGuidId"].ToStr();
                                var houseNum = persAccount["House"].ToStr();
                                var housing = persAccount["Housing"].ToStr();

                                var key = "{0}_{1}_{2}".FormatUsing(streetGuid, houseNum, housing);
                                long realObjId;
                                if (realObjByAddressGuid.TryGetValue(key, out realObjId))
                                {
                                    var roomNum = persAccount["FlatNum"].ToStr();

                                    var room = new Room();
                                    room.RoomNum = roomNum;
                                    room.RealityObject = new RealityObject
                                    {
                                        Id = realObjId
                                    };
                                    room.Type = RoomType.Living;
                                    room.OwnershipType = RoomOwnershipType.NotSet;

                                    List<Room> value;
                                    if (roomByRo.TryGetValue(realObjId, out value))
                                    {
                                        var existRoom = value.FirstOrDefault(x => x.RoomNum == roomNum);
                                        if (existRoom != null)
                                        {
                                            room = existRoom;
                                            room = (Room) persistenceContext.Unproxy(room);
                                        }
                                        else
                                        {
                                            value.Add(room);
                                        }
                                    }
                                    else
                                    {
                                        roomByRo.Add(realObjId, new List<Room> {room});
                                    }


                                    if (room.Id == 0)
                                    {
                                        LogImport.Info("Информация",
                                            "Добавлено новое помещение. Адрес: {0}. Номер помещения: {1}".FormatUsing(
                                                realityObjectByAddress[realObjId], roomNum));
                                    }

                                    var area = persAccount["TotalSquare"].ToDecimal();
                                    if (area != 0)
                                    {
                                        if (room.Area == 0)
                                        {
                                            room.Area = area;
                                            listRoomToSave.Add(room);
                                            LogImport.Info("Информация",
                                                "У помещения  жилого дома добавлена запись в поле 'Площадь'.  Адрес: {0}. Номер помещения: {1}. Площадь: {2}"
                                                    .FormatUsing(realityObjectByAddress[realObjId], roomNum, area));
                                        }
                                        else
                                        {
                                            LogImport.Info("Информация",
                                                "У помещения  жилого дома уже есть запись в поле 'Площадь'. Адрес: {0}. Номер помещения: {1}. Значение: {1}. Значение из ЕМБИР: {2}"
                                                    .FormatUsing(realityObjectByAddress[realObjId], roomNum, room.Area, area));
                                        }
                                    }

                                    if (personalAccount.Room == null)
                                    {
                                        isPersAccChange = true;
                                        personalAccount.Room = room;
                                    }

                                    if (room.Id > 0)
                                    {
                                        listRoomToSave.Add(room);
                                    }
                                }
                                else
                                {
                                    LogImport.CountAddedRows++;
                                    LogImport.Error("Ошибка", "Не удалось определить Жилой дом. {0}".FormatUsing(accNumStr));
                                    continue;
                                }
                            }

                            if (personalAccount.Id == 0 || isPersAccChange)
                            {
                                listPersAccToSave.Add(personalAccount);
                            }

                            foreach (
                                var newPersAccs in
                                    listPersAccToSave.Where(x => x.AreaShare == 0 && x.Room.Id > 0).GroupBy(x => x.Room.Id))
                            {
                                var availibleAreaShare = Math.Max(1 - areaShareByRoom.Get(newPersAccs.Key), 0M);
                                var areaShare = (availibleAreaShare/newPersAccs.Count()).RoundDecimal(2);
                                foreach (var basePersonalAccount in newPersAccs)
                                {
                                    basePersonalAccount.AreaShare = areaShare;
                                    LogImport.Info("Информация",
                                        "У лицевого счета определена доля собственности. {0}. Доля собственности: {1}"
                                            .FormatUsing(accNumStr, areaShare));
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            LogImport.CountAddedRows++;
                            LogImport.Error("Ошибка", "Произошла ошибка: " + exception.Message);
                            continue;
                        }

                        LogImport.CountAddedRows++;
                    }

                    LogImport.Info("Info", "Items processed");

                    skip += take;

                    httpQueryBuilder = importHelper.GetHttpQueryBuilder();
                    httpQueryBuilder.AddParameter("type", "PersonalAccountEntity");
                    httpQueryBuilder.AddDictionary("select", select);
                    httpQueryBuilder.AddParameter("skip", skip);
                    httpQueryBuilder.AddParameter("take", take);
                    httpQueryBuilder.AddParameter("order", "id desc");

                    try
                    {
                        var repo = Container.ResolveRepository<IndividualAccountOwner>();
                        using (var tr = Container.Resolve<IDataTransaction>())
                        {
                            foreach (var accountOwner in listIndivAccToSave)
                            {
                                if (accountOwner.Id != 0)
                                {
                                    repo.Update(accountOwner);
                                }
                                else
                                {
                                    repo.Save(accountOwner);
                                }
                            }

                            tr.Commit();
                            LogImport.Info("Info", "IAO saved successsfully");
                        }
                    }
                    catch (Exception exception)
                    {
                        LogImport.Error("Ошибка", "Произошла ошибка сохранения абонентов: " + exception.Message + "\n" + exception.StackTrace);
                    }

                    try
                    {
                        var repo = Container.ResolveRepository<Room>();
                        using (var tr = Container.Resolve<IDataTransaction>())
                        {
                            foreach (var room in listRoomToSave)
                            {
                                if (room.Id != 0)
                                {
                                    repo.Update(room);
                                }
                                else
                                {
                                    repo.Save(room);
                                }
                            }

                            tr.Commit();
                            LogImport.Info("Info", "Rooms saved successsfully");
                        }
                    }
                    catch (Exception exception)
                    {
                        LogImport.Error("Ошибка", "Произошла ошибка сохранения комнат: " + exception.Message + "\n" + exception.StackTrace);
                    }

                    try
                    {
                        SaveLog();
                        LogImport.Info("Info", "Log saved successsfully");
                    }
                    catch (Exception exception)
                    {
                        LogImport.Info("Info", "Log saved not successsfully. " + exception.Message);
                    }

                    try
                    {
                        var itemIndex = 0;
                            foreach (var basePersonalAccount in listPersAccToSave)
                            {
                                if (basePersonalAccount.Id != 0)
                                {
                                    PersonalAccountDomain.Update(basePersonalAccount);
                                }
                                else
                                {
                                    PersonalAccountDomain.Save(basePersonalAccount);
                                }

                                LogImport.Info("Info", "Log saved not successsfully. " + "Account saved successsfully. " + itemIndex);
                                itemIndex++;
                            }
                    }
                    catch (Exception exception)
                    {
                        LogImport.Error("Ошибка", "Произошла ошибка сохранения счетов: " + exception.Message + "\n" + exception.StackTrace);
                    }
                    finally
                    {
                        listIndivAccToSave.Clear();
                        listRoomToSave.Clear();
                        listPersAccToSave.Clear();
                    }

                    try
                    {
                        dynPersAccounts = Enumerable.ToArray(webClientFetcher.GetData(httpQueryBuilder));
                    }
                    catch (Exception)
                    {
                        dynPersAccounts = new dynamic[0];
                    }
                }
            }
            catch (Exception exception)
            {
                LogImport.Error("Ошибка", "Произошла ошибка: " + exception.Message + "\n" + exception.StackTrace);
            }

            try
            {
                SaveLog();
            }
            catch (Exception)
            {
            }

            return new ImportResult();
        }

        private void SaveLogInternal()
        {
            var repo = Container.ResolveRepository<LogImport>();
            repo.Save(new LogImport
            {
                CountChangedRows = LogImport.CountChangedRows,
                CountError = LogImport.CountError,
                CountWarning = LogImport.CountWarning,
                ImportKey = Key,
                UploadDate = DateTime.Now,
            });
        }

        private void SaveLog()
        {
            var repo = Container.ResolveDomain<LogImport>();
            if (previousFileId != 0)
            {
                /*var entity = repo.GetAll().FirstOrDefault(x => x.LogFile.Id == previousFileId);
                if (entity != null)
                {
                    repo.Delete(entity.Id);
                }*/
            }

            LogImport.SetFileName(Name);
            LogImport.ImportKey = Key;

            LogManager = Container.Resolve<ILogImportManager>();

            LogManager.FileNameWithoutExtention = Name;
            try
            {
                LogManager.AddLog(LogImport);
            }
            catch (Exception)
            {
            }

            previousFileId = LogManager.Save();
        }
    }
}