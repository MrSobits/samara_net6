namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System;

    using Bars.Gkh.SystemDataTransfer.Caching;

    /// <summary>
    /// Интерфейс генерации типов
    /// </summary>
    public interface IDtoEntityBuilder
    {
        /// <summary>
        /// Имя сборки
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        /// Наименование типа
        /// </summary>
        string TypeSignature { get; }

        /// <summary>
        /// Создать экземпляр
        /// </summary>
        object CreateInstance();

        /// <summary>
        /// Вернуть сгенерированный тип
        /// </summary>
        Type GetDefinedType();

        /// <summary>
        /// Вернуть хранитель для работы с кэшем
        /// </summary>
        ICacheDtoHolder GetCacheHolder();
    }
}