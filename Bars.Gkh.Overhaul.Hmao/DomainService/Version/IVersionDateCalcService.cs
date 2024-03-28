namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;

    /// <summary>
    /// Интерфейс для поулчения расчетных Дат 
    /// </summary>
    public interface IVersionDateCalcService
    {
        /// <summary>
        /// Получить дату расчета ДПКР
        /// </summary>
        IDataResult GetDateCalcDpkr(BaseParams baseParams);

        /// <summary>
        /// Получить дату расчета показателей собираемости
        /// </summary>
        IDataResult GetDateCalcOwnerCollection(BaseParams baseParams);

        /// <summary>
        /// Получить дату расчета корректировки
        /// </summary>
        IDataResult GetDateCalcCorrection(BaseParams baseParams);

        /// <summary>
        /// Получить дату опубликования
        /// </summary>
        IDataResult GetDateCalcPublished(BaseParams baseParams);

        /// <summary>
        /// Метод получения даты опубликования по массиву МО
        /// </summary>
        DateTime GetDateCalcPublished(List<long> muIds);
    }
}