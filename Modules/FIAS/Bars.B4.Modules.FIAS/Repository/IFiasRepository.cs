namespace Bars.B4.Modules.FIAS
{
    using Bars.B4.Modules.FIAS.Enums;
    using DataAccess;
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс для получения записей ФИАС
    /// </summary>
    public interface IFiasRepository : IRepository<Fias>
    {
        /// <summary>
        /// Метод получения населенных пунктов
        /// </summary>
        /// <param name="filter">Строка фильтрации</param>
        /// <param name="parentGuid">AOGuid родительской записи</param>
        /// <returns>IDinamicAddress</returns>
        List<IDinamicAddress> GetPlacesDinamicAddress(string filter, string parentGuid = null);

        /// <summary>
        /// Метод получения улиц
        /// </summary>
        /// <param name="filter">Строка фильтрации</param>
        /// <param name="parentguid">AOGuid родительской записи</param>
        /// <returns>IDinamicAddress</returns>
        List<IDinamicAddress> GetStreetsDinamicAddress(string filter, string parentguid);

        /// <summary>
        /// Метод получения номера дома, корпуса, строения
        /// </summary>
        /// <param name="filter">Строка фильтрации</param>
        /// <param name="parentguid">AOGuid родительской записи</param>
        /// <param name="estimatetype">Тип владения</param>
        /// <returns>IDinamicAddress</returns>
        List<IDinamicAddress> GetHousesDynamicAddress(string filter, string parentguid, FiasEstimateStatusEnum estimatetype = FiasEstimateStatusEnum.NotDefined);

        /// <summary>
        /// Метод получения адреса
        /// </summary>
        /// <param name="filter">AOGuid запрашиваемой записи</param>
        /// <returns>IDinamicAddress</returns>
        IDinamicAddress GetDinamicAddress(string filter);
    }
}
