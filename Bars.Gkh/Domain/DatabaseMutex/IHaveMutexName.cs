namespace Bars.Gkh.Domain.DatabaseMutex
{
    /// <summary>Интерфейс добавляющий метод генерации уникального ключа для блокировки</summary>
    public interface IHaveMutexName
    {
        /// <summary>
        /// Получить ключ для блокировки
        /// </summary>
        /// <returns>
        /// Ключ для блокировки
        /// </returns>
        string GetMutexName();
    }
}