namespace Bars.GisIntegration.Base.Package
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс менеджера пакетов
    /// </summary>
    /// <typeparam name="TPackageInfo">Тип описания пакета</typeparam>
    /// <typeparam name="TPackageId">Тип идентификатора пакета</typeparam>
    public interface IPackageManager<TPackageInfo, in TPackageId> : IPackageManager
        where TPackageInfo : IPackageInfo
    {
        /// <summary>
        /// Создать пакет
        /// </summary>
        /// <param name="packageName">Имя пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Описание пакета</returns>
        new TPackageInfo CreatePackage(string packageName, object notSignedData, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary = null);

        /// <summary>
        /// Сохранить неподписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        void SaveNotSignedData(TPackageInfo packageInfo, object notSignedData, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary = null);

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        void SaveSignedData(TPackageId packageId, string signedData);

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        void SaveSignedData(TPackageInfo packageInfo, string signedData);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        string GetData(TPackageId packageId, bool signed, bool formatted);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        string GetData(TPackageInfo packageInfo, bool signed, bool formatted);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <typeparam name="TDataType">Тип данных пакета</typeparam>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <returns>Данные пакета</returns>
        TDataType GetData<TDataType>(TPackageId packageId, bool signed);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <typeparam name="TDataType">Описание пакета</typeparam>
        /// <param name="packageInfo">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <returns>Данные пакета</returns>
        TDataType GetData<TDataType>(TPackageInfo packageInfo, bool signed);

        /// <summary>
        /// Получить словать транспортных идентификаторов
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <returns>Словарь транспортных идентификаторов</returns>
        Dictionary<Type, Dictionary<string, long>> GetTransportGuidDictionary(TPackageInfo packageInfo);
    }

    public interface IPackageManager
    {
        /// <summary>
        /// Создать пакет
        /// </summary>
        /// <param name="packageName">Имя пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Описание пакета</returns>
        IPackageInfo CreatePackage(string packageName, object notSignedData, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary = null);

        /// <summary>
        /// Сохранить неподписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        void SaveNotSignedData(IPackageInfo packageInfo, object notSignedData, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary = null);

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        void SaveSignedData(object packageId, string signedData);

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        void SaveSignedData(IPackageInfo packageInfo, string signedData);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        string GetData(object packageId, bool signed, bool formatted);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        string GetData(IPackageInfo packageInfo, bool signed, bool formatted);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <typeparam name="TDataType">Тип данных пакета</typeparam>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <returns>Данные пакета</returns>
        TDataType GetData<TDataType>(object packageId, bool signed);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <typeparam name="TDataType">Описание пакета</typeparam>
        /// <param name="packageInfo">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <returns>Данные пакета</returns>
        TDataType GetData<TDataType>(IPackageInfo packageInfo, bool signed);

        /// <summary>
        /// Получить словать транспортных идентификаторов
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <returns>Словарь транспортных идентификаторов</returns>
        Dictionary<Type, Dictionary<string, long>> GetTransportGuidDictionary(IPackageInfo packageInfo);
    }
}
