namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System.Collections.Generic;
    using B4;

    using Bars.Gkh.Enums.Decisions;

    using Entities;
    using Gkh.Entities;

    using Bars.Gkh.Decisions.Nso.Entities.Decisions;

    public interface IGovDecisionAccountService
    {
        /// <summary>
        /// Установить правильный статус Л/С относящихся к дому протокола. Если конечно это возможно ;-)
        /// </summary>
        /// <param name="protocol">Протокол решений собственников</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется</param>
        void SetPersonalAccountStateIfNeeded(GovDecision protocol, bool deletingCurrentProtocol = false);

        /// <summary>
        /// Изменить статус Л/С относящихся к дому протокола на "неактивный". Если конечно это нужно ;-)
        /// </summary>
        /// <param name="protocol">Протокол решений собственников</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется</param>
        /// <param name="isRealityObjectChange">Не добавляем в excluded protocol при изменении пар-ров дома</param>
        void SetPersonalAccountStatesNonActiveIfNeeded(GovDecision protocol,
            bool deletingCurrentProtocol = false,
            bool isRealityObjectChange = false);
    }
}
