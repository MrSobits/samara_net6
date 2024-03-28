namespace Bars.Gkh.Decisions.Nso.Domain
{
    using System;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Service для получения/установки активного протокола
    /// </summary>
    public interface IRealityObjectDecisionProtocolService
    {
        /// <summary>
        /// Получение протокола, действующего в данный момент
        /// </summary>
        /// <returns>Протокол, действующий в данный момент</returns>
        RealityObjectDecisionProtocol GetActiveProtocol(RealityObject realityObject);

        /// <summary>
        /// Получение протокола, действующего на дату, переданную в качестве параметра
        /// </summary>
        /// <param name="realityObject"></param>
        /// <param name="date">Дата действия протокола</param>
        /// <returns>Протокол</returns>
        RealityObjectDecisionProtocol GetActiveProtocolForDate(RealityObject realityObject, DateTime date);

        /// <summary>
        /// Делаем активным протокол последний по дате и с финальным статусом,
        /// если таковой отсутствует - делаем лс неактивным
        /// </summary>
        /// <param name="entity">Удаляемый протокол (решений собственников и органов госвласти)</param>
        void SetNextActualProtocol<T>(T entity) where T : IDecisionProtocol;
    }
}