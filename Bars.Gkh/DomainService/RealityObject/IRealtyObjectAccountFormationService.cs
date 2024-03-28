namespace Bars.Gkh.DomainService
{
    /// <summary>
    /// Интерфейс сервиса для актуализации способа формирования фонда дома.
    /// <remarks>Устанавливает значение свойства AccountFormationVariant</remarks>
    /// </summary>
    public interface IRealtyObjectAccountFormationService
    {
        /// <summary>
        /// Актуализировать способ формирования фонда в сущности дома
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        void ActualizeAccountFormationType(long realityObjectId);
    }
}
