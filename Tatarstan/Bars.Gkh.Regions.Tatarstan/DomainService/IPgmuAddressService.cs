namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    /// <summary>
    /// Интерфейс сервиса адресов ПГМУ
    /// </summary>
    public interface IPgmuAddressService
    {
        /// <summary>
        /// Получение значения адресного объекта на основе фильтра и значений родительских адр. объектов
        /// </summary>
        /// <param name="value">Значение фильтруемого свойства</param>
        /// <param name="parentValue">Словарь: ключ - наименование адр. объекта, значение - значение адр. объекта</param>
        /// <param name="propertyName">Наименование фильтруемого свойства класса PgmuAddress</param>
        /// <returns></returns>
        IEnumerable<object> GetPgmuAddressObjectValue(string propertyName, string value, Dictionary<string, string> parentValue);

        /// <summary>
        /// Получение идентификатора адреса через значения его составляющих 
        /// </summary>
        /// <param name="addressObjectDict">Словарь со значениями составляющего адреса</param>
        /// <returns></returns>
        long GetPgmuAddressId(Dictionary<string, string> addressObjectDict);

        /// <summary>
        /// Скомбинировать адрес ПГМУ в строку
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        string CombinePgmuAddress(PgmuAddress address);
    }
}