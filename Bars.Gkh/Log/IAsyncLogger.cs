namespace Bars.Gkh.Log
{
    using System.Threading.Tasks;

    using Bars.B4;

    /// <summary>
    /// Асинхронный логгер
    /// </summary>
    public interface IAsyncLogger<in T> where T: class
    {
        /// <summary>
        /// Добавляет сущность в очередь для сохранения
        /// </summary>
        /// <param name="entity">Сохраняемая сущность</param>
        void Add(T entity);

        /// <summary>
        /// Сохранить кэш
        /// </summary>
        void Flush();
    }
}