namespace Bars.Gkh.RegOperator.DomainService.CashPaymentCenter
{
    using System;
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для привязки объекта к РКЦ.
    /// <remarks>Для подмены способа добавления в зависимости от единых настроек приложения (РКЦ-Дом или РКЦ-ЛС)</remarks>
    /// </summary>
    public interface ICashPaymentCenterObjectsService
    {
        /// <summary>
        /// Привязать объект к РКЦ
        /// </summary>
        /// <param name="cachPaymentCenterId">Идентификатор РКЦ</param>
        /// <param name="ids">Идентификаторы объектов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        /// <returns>Результат работы</returns>
        IDataResult AddObjects(long cachPaymentCenterId, long[] ids, DateTime dateStart, DateTime? dateEnd);

        /// <summary>
        /// Открепить объект от РКЦ
        /// </summary>        
        /// <param name="ids">Идентификаторы объектов</param>
        /// <returns>Результат работы</returns>
        IDataResult DeleteObjects(long[] ids);

        /// <summary>
        /// Врезать привязку объекта к РКЦ
        /// </summary>
        /// <remarks>
        /// В период действия текущего договора «врезается» период действия нового договора. Например:
        /// Действующий договор: c 2010 по 2020 год. Врезается договор другого РКЦ с периодом: c 2015 по 2016 год. 
        /// В результате будет 3 записи:
        /// 1) Старый РКЦ с 2010 по 2014.
        /// 2) Новый РКЦ с 2015 по 2016.
        /// 3) Старый РКЦ с 2017 по 2020.
        /// </remarks>
        /// <param name="cachPaymentCenterId">Идентификатор РКЦ</param>
        /// <param name="ids">Идентификаторы объектов</param>
        /// <param name="dateStart">Дата начала действия договора</param>
        /// <param name="dateEnd">Дата окончания действия договора</param>
        /// <returns>Результат работы</returns>
        IDataResult InsertObjects(long cachPaymentCenterId, long[] ids, DateTime dateStart, DateTime? dateEnd);

        /// <summary>
        /// Установить расчётно-кассовый центр, из которого вызвана функция,
        /// всем Л/С без расчётно-кассового центра
        /// </summary>
        /// <param name="cashPaymentCenterId">Идентификатор РКЦ</param>
        /// <returns>Результат работы</returns>
        IDataResult SetCashPaymentCenters(long cashPaymentCenterId);
    }
}
