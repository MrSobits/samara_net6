namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Services
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.StateTariffCommittee.Api.Version1.Models.RealityObject;

    /// <summary>
    /// Интерфейс сервиса для работы с <see cref="RealityObject"/>
    /// </summary>
    public interface IRealityObjectService
    {
        /// <summary>
        /// Получить перечисление <see cref="RealityObject"/>
        /// </summary>
        /// <param name="oktmo">ОКТМО</param>
        /// <param name="period">Отчетный период</param>
        IEnumerable<RealityObject> List(long? oktmo, DateTime? period);
        
        /// <summary>
        /// Получить перечисление <see cref="RealityObjectInConsumerContext"/>
        /// </summary>
        /// <param name="oktmo">ОКТМО</param>
        /// <param name="period">Отчетный период</param>
        IEnumerable<RealityObjectInConsumerContext> ListInConsumerContext(long? oktmo, DateTime? period);

        /// <summary>
        /// Получить перечисление <see cref="RealityObjectInConsumerTariffContext"/>
        /// </summary>
        /// <param name="oktmo">ОКТМО</param>
        /// <param name="period">Отчетный период</param>
        /// <param name="pageGuid">Гуид запрашиваемой страницы</param>
        PagedApiServiceResult ListInConsumerTariffContext(long? oktmo, DateTime? period, Guid? pageGuid);
    }
}