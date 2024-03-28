namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Кэш для получения актуальных тарифа и площади 
    /// </summary>
    public interface ITariffAreaCache : IDisposable
    {
        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="mainInfo">Основная информация</param>
        /// <param name="period">Период</param>
        void Init(PersonalAccountRecord[] mainInfo, IPeriod period);

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="mainInfo">Основная информация</param>
        /// <param name="periods">Периоды</param>
        void Init(PersonalAccountRecord mainInfo, IPeriod[] periods);

        /// <summary>
        /// Получить запись с актуальными тарифом и площадью
        /// </summary>
        /// <param name="account">Лс</param>
        /// <param name="period">Период</param>
        /// <returns>Запись с актуальными тарифом и площадью</returns>
        TariffAreaRecord GetTariffArea(BasePersonalAccount account, IPeriod period);       
    }
}