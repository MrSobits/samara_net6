using Bars.B4;

namespace Bars.Gkh.RegOperator.DomainService.RealityObjectProtocol
{
    using System.Collections.Generic;

    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Интерфейс для сохранения или изменения решений?
    /// </summary>

    public interface IRealityObjectBothProtocolService
    {
        /// <summary>
        /// Сохранить или обновить протокол
        /// </summary>
        IDataResult SaveOrUpdateDecisions(BaseParams baseParams);

        /// <summary>
        /// Загрузить протокол
        /// </summary>
        IDataResult Get(BaseParams baseParams);

        /// <summary>
        /// Удалить протокол
        /// </summary>
        IDataResult Delete(BaseParams baseParams);

        /// <summary>
        /// Получить доступные типы протоколов
        /// </summary>
        IDictionary<CoreDecisionType, bool> GetAvaliableTypes(BaseParams baseParams);
    }
}
