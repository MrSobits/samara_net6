namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils.Annotations;

    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Extenstions;

    using Castle.Windsor;
    using DomainService.PersonalAccount;
    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using NHibernate.Linq;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Реализация сервиса создания лицевых счетов
    /// </summary>
    public class PersonalAccountCreateService : IPersonalAccountCreateService
    {
        /// <summary>
        /// Сервис массовой работы с DTO KC
        /// </summary>
        public IMassPersonalAccountDtoService AccountDtoService { get; set; }
        public IDomainService<State> StateDomainService { get; set; }
        public IStateProvider StateProvider { get; set; }
        public IPersonalAccountOperationService PersonalAccountOperationService { get; set; }
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        /// <summary>
        /// Фабрика лицевых счетов
        /// </summary>
        public IPersonalAccountFactory PersonalAccountFactory { get; set; }
        /// <summary>
        /// DomainService собственников
        /// </summary>
        public IDomainService<PersonalAccountOwner> AccountOwnerDomain { get; set; }
        /// <summary>
        /// DomainService помещений
        /// </summary>
        public IDomainService<BasePersonalAccount> AccountDomain { get; set; }
        /// <summary>
        /// DomainService помещений
        /// </summary>
        public IDomainService<Room> RoomDomain { get; set; }
        /// <summary>
        /// Домен-сервис кошельков
        /// </summary>
        public IDomainService<Wallet> WalletDomain { get; set; }
        /// <summary>
        /// Интерфейс для определения способа формирования фонда
        /// </summary>
        public ITypeOfFormingCrProvider TypeOfFormingCrProvider { get; set; }
        
        /// <summary>
        /// DomainService История принадлежности лс абоненту
        /// </summary>
        public IDomainService<AccountOwnershipHistory> OwnershipHistoryDomain { get; set; }

        /// <summary>
        /// Создать лицевой счет
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        public IDataResult CreateNewAccount(BaseParams baseParams)
        {
            var ownerId = baseParams.Params.GetAsId("AccountOwner");
            var openDate = baseParams.Params.GetAs<DateTime>("OpenDate");
            var roomProxies = baseParams.Params.GetAs<RoomProxy[]>("Rooms");

            var owner = this.AccountOwnerDomain
                .GetAll()
                .Where(x => x.Id == ownerId)
                .FetchMany(x => x.Accounts)
                .AsEnumerable()
                .First();

            var roomIds = roomProxies.Select(x => x.Id).ToList();

            var roomEntities = this.RoomDomain.GetAll()
                .Where(x => roomIds.Contains(x.Id))
                .ToDictionary(x => x.Id);

            var records = new List<BasePersonalAccount>();

            var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

            this.AccountDtoService.InitCache();

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var roomProxy in roomProxies.Where(x => roomEntities.ContainsKey(x.Id)))
                    {
                        var account = this.CreateNewAccount(owner, roomEntities[roomProxy.Id], openDate, roomProxy.AreaShare);
                        this.AccountDtoService.AddPersonalAccount(account);

                        foreach (var wallet in account.GetWallets())
                        {
                            this.WalletDomain.SaveOrUpdate(wallet);
                        }

                        var typeOfFormingCr = this.TypeOfFormingCrProvider.GetTypeOfFormingCr(roomEntities[roomProxy.Id].RealityObject);

                        if (typeOfFormingCr == CrFundFormationType.Unknown || typeOfFormingCr == CrFundFormationType.SpecialAccount)
                        {
                            account.State = this.StateDomainService.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == entityInfo.TypeId);
                        }

                        records.Add(account);

                        this.AccountDomain.Save(account);

                        if (account.State.Code == "4")
                        {
                            this.PersonalAccountOperationService.LogAccountDeactivate(
                                account,
                                DateTime.Now,
                                DateTime.Now,
                                "Создан лицевой счет");
                        }

                        this.OwnershipHistoryDomain.Save(new AccountOwnershipHistory(account, owner, openDate));
                    }

                    tr.Commit();

                    this.AccountDtoService.ApplyChanges();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult(records);
        }

        /// <summary>
        /// Создать лицевой счет
        /// </summary>
        /// <param name="owner">Собственник</param>
        /// <param name="room">Помещение</param>
        /// <param name="dateOpen">Дата открытия</param>
        /// <param name="areaShare">Доля собственности</param>
        /// <returns>Лицевой счет</returns>
        public BasePersonalAccount CreateNewAccount(PersonalAccountOwner owner, Room room, DateTime dateOpen, decimal areaShare)
        {
            ArgumentChecker.NotNull(owner, nameof(owner));

            return owner.CreateAccount(this.PersonalAccountFactory, room, dateOpen, areaShare);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class RoomProxy
        {
            public long Id { get; set; }
            public decimal AreaShare { get; set; }
        }
    }
}
