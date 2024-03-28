namespace Bars.Gkh.RegOperator.DomainService
{
    /// <summary>
    /// Интерфейс проставления статусов ЛС по состоянию дома
    /// </summary>
    public interface IRealityObjectPersonalAccountStateProvider
    {
        /// <summary>
        /// Проставить актуальные статусы ЛС по состоянию дома и единым настройкам
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        void SetPersonalAccountStateIfNeed(Gkh.Entities.RealityObject realityObject);
    }
}