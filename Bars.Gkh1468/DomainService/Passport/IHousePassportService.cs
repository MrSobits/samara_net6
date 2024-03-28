namespace Bars.Gkh1468.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Enums;

    /// <summary>
    /// Сервис паспортов дома
    /// </summary>
    public interface IHousePassportService
    {
        /// <summary>
        /// Получение паспорта жилого дома на период
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        /// <returns>Паспорт ОКИ</returns>
        IDataResult GetPassport(RealityObject realityObject, int year, int month);

        /// <summary>
        /// Получение паспорта жилого дома на текущий период
        /// </summary>
        IDataResult GetCurrentPassport(RealityObject realityObject);

        /// <summary>
        /// Получение отношения количества паспортов с заполненностью 100% 
        /// на общее число паспортов
        /// </summary>
        /// <param name="houseType">Тип жилого дома</param>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <returns>Процент</returns>
        Dictionary<long, decimal> GetPassportsPercentage(TypeRealObj houseType, int year, int month);

        Dictionary<long, decimal> GetPassportsPercentageByHouse(int year, int month);
    }
}