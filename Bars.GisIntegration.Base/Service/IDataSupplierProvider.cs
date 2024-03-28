namespace Bars.GisIntegration.Base.Service
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Провайдер поставщика данных
    /// </summary>
    public interface IDataSupplierProvider
    {
        /// <summary>
        /// Получить текущего поставщика данных
        /// </summary>
        /// <returns>Поставщик данных</returns>
        RisContragent GetCurrentDataSupplier();

        /// <summary>
        /// Проверить контрагента на принадлежность текущему оператору
        /// </summary>
        /// <param name="contragent">Контрагент</param>
        /// <returns></returns>
        bool IsDelegacy(RisContragent contragent);

        /// <summary>
        /// Получить идентификаторы контрагентов, привязанных к текущему оператору
        /// </summary>
        /// <returns></returns>
        List<long> GetContragentIds();
    }
}