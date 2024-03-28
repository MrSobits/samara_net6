namespace Bars.Gkh.RegOperator.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс репозитория начислений по ЛС
    /// </summary>
    public interface IPersonalAccountChargeRepository
    {
        /// <summary>
        /// Получить начисления за период, которые надо зафиксиировать
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="notActive">Исключать неактивные ЛС</param>
        IQueryable<PersonalAccountCharge> GetNeedToBeFixedForPeriod(IPeriod period, bool notActive = true);

        /// <summary>
        /// Получить начисления за период
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="realtyObjects">Дома</param>
        IQueryable<PersonalAccountCharge> GetChargesByPeriodAndRealties(IPeriod period, IEnumerable<RealityObject> realtyObjects);


        /// <summary>
        /// Получить начисления по неактивным лс, которые нужно вычесть
        /// </summary>
        /// <param name="period">Период</param>
        /// <returns>Начисления</returns>
        IQueryable<PersonalAccountCharge> GetNeedToBeUndo(IPeriod period);
    }
}