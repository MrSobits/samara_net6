namespace Bars.Gkh.Reforma.Interface.DataCollectors
{
    using Bars.Gkh.Reforma.Entities.Dict;

    /// <summary>
    /// Интерфейс работы с хранимыми файлами
    /// </summary>
    /// <typeparam name="T">Тип файла</typeparam>
    public interface ICollectedFile<T>
    {
        /// <summary>
        /// Обработать файл
        /// </summary>
        /// <param name="entity">Объект</param>
        /// <param name="syncProvider">Провайдер синхронизации с Реформой ЖКХ</param>
        /// <returns>Успешность операции</returns>
        bool Process(T entity, ISyncProvider syncProvider);
    }
}