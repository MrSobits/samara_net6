namespace Bars.Esia.OAuth20.App.Providers
{
    using System.Collections.Generic;

    using Bars.Esia.OAuth20.App.Entities;

    /// <summary>
    /// Интерфейс поставщика параметров приложения
    /// </summary>
    public interface IAuthAppOptionProvider
    {
        /// <summary>
        /// Получить общие параметры приложения
        /// </summary>
        AuthAppOptions GetAuthAppOptions();

        /// <summary>
        /// Получить параметры ЕСИА
        /// </summary>
        EsiaOptions GetEsiaOptions();

        /// <summary>
        /// Получить отсутствуюшие обязательные параметры
        /// </summary>
        IEnumerable<string> GetNotExistRequiredOptions();
    }
}