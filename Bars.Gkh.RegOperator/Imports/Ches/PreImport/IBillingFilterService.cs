namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис фильтрации импорта
    /// </summary>
    public interface IBillingFilterService
    {
        /// <summary>
        /// Запрещен импорт любых данных
        /// </summary>
        bool IsNotAllowAllRows { get; }

        /// <summary>
        /// Описание настроек импорта
        /// </summary>
        string ConfigDescription { get; }

        /// <summary>
        /// Проверить ЛС
        /// </summary>
        /// <param name="accountNumber">Номер лицевого счета</param>
        /// <param name="errorMessage">Сообщение об ошибке, если ЛС не прошел проверку</param>
        bool CheckByAccountNumber(string accountNumber,  out string errorMessage);

        /// <summary>
        /// Проверить дом
        /// </summary>
        /// <param name="realityObject">Дом</param>
        /// <param name="errorMessage">Сообщение об ошибке, если дом не прошел проверку</param>
        bool CheckByRealityObject(RealityObject realityObject, out string errorMessage);

        /// <summary>
        /// Проверить параметры фильтрации
        /// </summary>
        void ValidateConfig();

        /// <summary>
        /// Инициализировать сервис фильтрации
        /// </summary>
        /// <param name="date"></param>
        void Initialize(DateTime date);
    }
}