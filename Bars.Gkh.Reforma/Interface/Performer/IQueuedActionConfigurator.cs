namespace Bars.Gkh.Reforma.Interface.Performer
{
    /// <summary>
    /// Настройщик запланированного действия
    /// </summary>
    /// <typeparam name="TParam">Тип параметра</typeparam>
    /// <typeparam name="TResult">Тип результата</typeparam>
    public interface IQueuedActionConfigurator<in TParam, TResult> : IQueuedActionConfigurator
    {
        /// <summary>
        /// Установка колбэка
        /// </summary>
        /// <param name="callback">Колбэк</param>
        /// <param name="supressExceptions">Подавлять исключения, возникающие в ходе исполнения</param>
        /// <returns>Настройщик запланированного действия</returns>
        IQueuedActionConfigurator<TParam, TResult> WithCallback(PerformerCallback<TResult> callback, bool supressExceptions = false);

        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <returns>Настройщик запланированного действия</returns>
        IQueuedActionConfigurator<TParam, TResult> WithParameters(TParam parameters);

        /// <summary>
        /// Установка сериализованных параметров действия
        /// </summary>
        /// <param name="parameters">Сериализованные параметры</param>
        /// <returns>Настройщик запланированного действия</returns>
        IQueuedActionConfigurator<TParam, TResult> WithJsonEncodedParameters(string parameters);
    }

    /// <summary>
    /// Настройщик запланированного действия
    /// </summary>
    public interface IQueuedActionConfigurator
    {
        /// <summary>
        /// Установка колбэка
        /// </summary>
        /// <param name="callback">Колбэк</param>
        /// <param name="supressExceptions">Подавлять исключения, возникающие в ходе исполнения</param>
        /// <returns>Настройщик запланированного действия</returns>
        IQueuedActionConfigurator WithCallback(PerformerCallback callback, bool supressExceptions = false);

        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <returns>Настройщик запланированного действия</returns>
        IQueuedActionConfigurator WithParameters(object parameters);

        /// <summary>
        /// Установка сериализованных параметров действия
        /// </summary>
        /// <param name="parameters">Сериализованные параметры</param>
        /// <returns>Настройщик запланированного действия</returns>
        IQueuedActionConfigurator WithJsonEncodedParameters(string parameters);
    }
}