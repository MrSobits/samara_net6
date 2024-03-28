namespace Bars.Gkh.RegOperator.DomainService.RealityObjectSpecialOrRegOperatorAccount
{
    using Decisions.Nso.Entities;
    using Gkh.Entities;

    /// <summary>
    /// Вспомогательный интерфейс для работы со счетами рег оператора и спец счетами
    /// </summary>
    public interface IRealityObjectSpecialOrRegOperatorAccountService
    {
        /// <summary>
        /// Обработка изменения протокола решения
        /// </summary>
        /// <param name="ownerDecision">Решение о владельце счета</param>
        /// <param name="accountTypeDecision">Решение о формировании фонда КР</param>
        /// <param name="ro">Объект недвижимости</param>
        void HandleSpecialAccountByProtocolChange(AccountOwnerDecision ownerDecision,
            CrFundFormationDecision accountTypeDecision,
            RealityObject ro);

        /// <summary>
        /// Сделать счет неактивным
        /// </summary>
        /// <param name="ro">МКД</param>
        void SetNonActiveByRealityObject(RealityObject ro);

        /// <summary>
        /// Изменить статус Л/С относящихся к дому протокола на "активный". Если конечно это возможно ;-)
        /// </summary>
        /// <param name="protocol">Протокол решений собственников</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется</param>
        void SetPersonalAccountStatesActiveIfAble(RealityObjectDecisionProtocol protocol, bool deletingCurrentProtocol = false);

        /// <summary>
        /// Изменить статус Л/С относящихся к дому протокола на "неактивный". Если конечно это нужно ;-)
        /// </summary>
        /// <param name="protocol">Протокол решений собственников</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется</param>
        void SetPersonalAccountStatesNonActiveIfNeeded(RealityObjectDecisionProtocol protocol, bool deletingCurrentProtocol = false);

        /// <summary>
        /// Добавить оплату на открытие счета. Если конечно это нужно ;-)
        /// </summary>
        /// <param name="protocol">Протокол решений собственников</param>
        void AddPaymentForOpeningAccount(RealityObjectDecisionProtocol protocol);
    }
}