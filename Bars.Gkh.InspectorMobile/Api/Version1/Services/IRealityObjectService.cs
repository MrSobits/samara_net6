namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.RealityObject;

    /// <summary>
    /// Интерфейс сервиса для работы с <see cref="RealityObject"/>
    /// </summary>
    public interface IRealityObjectService
    {
        /// <summary>
        /// Получить <see cref="RealityObject"/>
        /// </summary>
        /// <param name="houseId">Идентификатор дома</param>
        /// <returns></returns>
        Task<BaseRealityObject> GetAsync(long houseId);

        /// <summary>
        /// Получить перечисление <see cref="RealityObject"/>
        /// </summary>
        /// <param name="fullList">Признак получения полного списка домов</param>
        /// <param name="houseIds">Массив идентификаторов домов</param>
        /// <returns></returns>
        Task<IEnumerable<BaseRealityObject>> ListAsync(bool fullList, long[] houseIds);
    }
}