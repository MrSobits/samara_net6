namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using B4.Modules.States;
    using DomainService.PersonalAccount;
    using Entities;
    using Gkh.Entities;

    /// <summary>
    /// Реализация фабрики лицевых счетов
    /// </summary>
    public class PersonalAccountFactory : IPersonalAccountFactory
    {
        private readonly IStateProvider _stateProvider;
        private readonly IAccountNumberService _accountNumberService;
        private readonly IRoomAreaShareSpecification _roomAreaShareSpecification;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="accountNumberService">Сервис генерации номер лицевого счета</param>
        /// <param name="stateProvider">Провайдер статусов</param>
        /// <param name="roomAreaShareSpecification">Спецификация помещения</param>
        public PersonalAccountFactory(
            IAccountNumberService accountNumberService,
            IStateProvider stateProvider,
            IRoomAreaShareSpecification roomAreaShareSpecification)
        {
            _accountNumberService = accountNumberService;
            _stateProvider = stateProvider;
            _roomAreaShareSpecification = roomAreaShareSpecification;
        }

        /// <summary>
        /// Создать новый лицевой счет
        /// </summary>
        /// <param name="owner">Собственник лицевого счета</param>
        /// <param name="room">Помещение</param>
        /// <param name="dateOpen">Дата открытия счета</param>
        /// <param name="areaShare">Доля собственности</param>
        /// <returns>Лицевой счет</returns>
        public virtual BasePersonalAccount CreateNewAccount(PersonalAccountOwner owner, Room room, DateTime dateOpen, decimal areaShare)
        {
            var account = BasePersonalAccount.AccountBuilder
                .GetBuilder()
                .SetInvariantProperties(owner, room, dateOpen, areaShare)
                .SetRoomSpecification(_roomAreaShareSpecification)
                .Build();

            _stateProvider.SetDefaultState(account);
            _accountNumberService.Generate(account);

            return account;
        }
    }
}