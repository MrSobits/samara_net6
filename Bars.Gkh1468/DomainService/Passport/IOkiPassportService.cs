namespace Bars.Gkh1468.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public interface IOkiPassportService
    {
        /// <summary>
        /// Получение паспорта по муниципальному образованию за период
        /// </summary>
        /// <param name="municipality">МО</param>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <returns>Паспорт ОКИ</returns>
        IDataResult GetPassport(Municipality municipality, int year, int month);

        /// <summary>
        /// Получение паспорта МО за текущий период
        /// </summary>
        /// <param name="municipality">МО</param>
        /// <returns>паспорт ОКИ</returns>
        IDataResult GetCurrentPassport(Municipality municipality);

        Dictionary<long, decimal> GetPassportsPercentage(int year, int month);
    }
}