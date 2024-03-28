namespace Bars.Gkh.RegOperator.Distribution
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainModelServices;

    /// <summary>
    /// Интерфейс распределения с автоматическим определением цели распределения
    /// </summary>
    public interface IDistributionAutomatically
    {
        /// <summary>
        /// Вернуть объекты для автораспределения
        /// </summary>
        /// <param name="distributables">Распределяемые объекты</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список</returns>
        IDataResult ListAutoDistributionObjs(IEnumerable<IDistributable> distributables, BaseParams baseParams);

        /// <summary>
        /// Вернуть объект для автораспределения по текущему распределению
        /// </summary>
        /// <param name="distributable">Распределяемая выписка</param>
        /// <returns>Список</returns>
        IDistributionArgs GetDistributionArgs(IDistributable distributable);
    }
}