namespace Bars.Gkh.DomainService.TechPassport
{
    using System.Collections.Generic;

    using Bars.Gkh.Services.DataContracts.HousesInfo;

    /// <summary>
    /// Интерфейс взаимодействия с тех паспортом для сервисов
    /// </summary>
    public interface ITehPassportValueService
    {
        /// <summary>
        /// Метод возвращает информацию по лифтам
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        IDictionary<long, List<LiftInfo>> GetLiftsByHouseIds(long[] houseIds);

        /// <summary>
        /// Метод возвращает информацию по приборам учёта
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        IDictionary<long, List<MeteringDeviceInfo>> GetMeteringDevicesByHouseIds(long[] houseIds);

        /// <summary>
        /// Метод возвращает информацию по мусоропроводу
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        IDictionary<long, GarbageInfo> GetGarbageInfoByHouseIds(long[] houseIds);

        /// <summary>
        /// Метод возвращает информацию по облицовачным материалам
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        IDictionary<long, string> GetFacadesByHouseIds(long[] houseIds);

        /// <summary>
        /// Метод возвращает информацию по сведениям об инженерных системах
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        IDictionary<long, EngineeringSystemProxy> GetEngineeringSystemByHouseIds(long[] houseIds);

        /// <summary>
        /// Метод возвращает информацию по количеству вентиляционных каналов
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        IDictionary<long, VentilationInfo> GetNumVentilationDuctByHouseIds(long[] houseIds);
    }

    /// <summary>
    /// Прокси класс для инженерных систем
    /// </summary>
    public class EngineeringSystemProxy
    {
        /// <summary>
        /// Сведения об инженерных системах: Теплоснабжение
        /// </summary>
        public string HeatingType { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Горячего водоснабжение
        /// </summary>
        public string HotWaterType { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Холодного водоснабжение
        /// </summary>
        public string ColdWaterType { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Электроснабжение
        /// </summary>
        public string ElectricalType { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Водоотведение
        /// </summary>
        public string SewerageType { get; set; }
    }
}