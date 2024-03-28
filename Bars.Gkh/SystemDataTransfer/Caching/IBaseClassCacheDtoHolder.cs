namespace Bars.Gkh.SystemDataTransfer.Caching
{
    /// <summary>
    /// Обертка для работы с наследниками через базовый класс
    /// </summary>
    public interface IBaseClassCacheDtoHolder
    {
        /// <summary>
        /// Добавить зависимость от наследника
        /// </summary>
        /// <param name="holder">Хранитель кэша</param>
        void AddInheritHolder(ICacheDtoHolder holder);
    }
}