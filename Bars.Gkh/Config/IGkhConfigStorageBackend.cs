namespace Bars.Gkh.Config
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Config.Impl.Internal;

    /// <summary>
    /// Бэкенд хранения настроек
    /// </summary>
    public interface IGkhConfigStorageBackend
    {
        /// <summary>
        /// Получение сохраненных настроек
        /// </summary>
        /// <returns></returns>
        IDictionary<string, ValueHolder> GetConfig();

        /// <summary>
        /// Сохраненние измененных настроек
        /// </summary>
        /// <param name="values">Измененные настройки</param>
        void UpdateConfig(IDictionary<string, ValueHolder> values);
    }
}