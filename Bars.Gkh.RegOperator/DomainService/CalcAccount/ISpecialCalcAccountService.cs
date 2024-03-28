namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Decisions.Nso.Entities;
    using Entities;
    using Gkh.Entities;

    public interface ISpecialCalcAccountService
    {
        IDataResult ListRegister(BaseParams baseParams);

        IDataResult EditPaymentCrSpecAccNotRegop(BaseParams baseParams);

        IDataResult GetPaymentCrSpecAccNotRegop(BaseParams baseParams);

        IDataResult ListPaymentCrSpecAccNotRegop(BaseParams baseParams);

        SpecialCalcAccount GetRobjectActiveSpecialAccount(RealityObject ro);

        /// <summary>
        /// Получить специальный расчетный счет по дому.
        /// </summary>
        /// <param name="ro">
        /// Жилой дом.
        /// </param>
        /// <returns>
        /// Специальный расчетный счет <see cref="SpecialCalcAccount"/>.
        /// </returns>
        SpecialCalcAccount GetSpecialAccount(RealityObject ro);

        /// <summary>
        /// Обработка изменения протокола решения
        /// </summary>
        /// <param name="ownerDecision">Решение о владельце счета</param>
        /// <param name="accountTypeDecision">Решение о формировании фонда КР</param>
        /// <param name="ro">Объект недвижимости</param>
        void HandleSpecialAccountByProtocolChange(AccountOwnerDecision ownerDecision,
            CrFundFormationDecision accountTypeDecision,
            GovDecision govDecision,
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
        /// <param name="isRealityObjectChange">Не добавляем в excluded protocol при изменении пар-ров дома</param>
        void SetPersonalAccountStatesNonActiveIfNeeded(RealityObjectDecisionProtocol protocol, 
            bool deletingCurrentProtocol = false,
            bool isRealityObjectChange = false);

        void AddPaymentForOpeningAccount(RealityObjectDecisionProtocol protocol);

        /// <summary>
        /// Производит актуализацию спецсчетов. 
        /// Вызывается при изменении "Дата протокола" или "Дата вступления в силу", 
        ///     т.к в этом случае спецсчета дома на котором висит протокол нуждаются в проверке областей действия
        /// </summary>
        /// <param name="realityObjectId">Идентифкатор дома</param>
        void ValidateActualSpecAccount(long realityObjectId);
    }
}