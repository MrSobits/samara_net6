namespace Bars.Gkh.DomainService.RegionalFormingOfCr
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Интерфейс для определения способа формирования фонда
    /// </summary>
    public interface ITypeOfFormingCrProvider
    {
        /// <summary>
        /// Определить способ формирования фонда дома
        /// </summary>
        /// <param name="realityObj">Дом</param>
        /// <returns>Тип формирования фонда</returns>
        CrFundFormationType GetTypeOfFormingCr(RealityObject realityObj);

        /// <summary>
        /// Определить способ формирования фонда дома массово
        /// </summary>
        /// <param name="realityObjs">Дома</param>
        /// <returns>Типы формирования фонда по домам</returns>
        Dictionary<long, CrFundFormationType> GetTypeOfFormingCr(IQueryable<RealityObject> realityObjs);

        /// <summary>
        /// Метод возвращает способы формирования фонда, которые участвуют в расчётах
        /// </summary>
        /// <returns>Список способов формирования</returns>
        IList<CrFundFormationType> GetCrFundFormationTypesFromSettings();
    }
}
