namespace Bars.Gkh.Services.Override
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Services.DataContracts.HouseSearch;

    public interface IServiceOverride
    {
        /// <summary>
        /// Получить дома
        /// </summary>
        /// <param name="streetId">ИД улицы</param>
        /// <param name="filter"></param>
        /// <returns>Ответ со списком домов</returns>
        HousesResponse GetHouses(string streetId, string filter);
        
        /// <summary>
        /// Получить дома в МО
        /// </summary>
        /// <param name="munId">ИД МО</param>
        /// <returns>Ответ со списком домов</returns>
        HousesResponse GetHousesByMu(string munId);

        /// <summary>
        /// Получить объекты фиас
        /// </summary>
        /// <param name="aolevel">Набор уровней объекта ФИАС</param>
        /// <param name="parentGuid">Уникальный ИД родителя получаемых объектов</param>
        /// <returns>Набор объектов ФИАС</returns>
        IQueryable<Fias> GetObjects(IEnumerable<FiasLevelEnum> aolevel, string parentGuid = "");

        /// <summary>
        /// Получить объекты фиас
        /// </summary>
        /// <param name="aolevel">Уровень объекта ФИАС</param>
        /// <param name="parentGuid">Уникальный ИД родителя получаемых объектов</param>
        /// <returns>Набор объектов ФИАС</returns>
        IQueryable<Fias> GetObjects(FiasLevelEnum aolevel, string parentGuid = "");
    }
}