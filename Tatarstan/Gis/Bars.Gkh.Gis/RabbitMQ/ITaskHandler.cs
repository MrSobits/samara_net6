namespace Bars.Gkh.Gis.RabbitMQ
{
    using Castle.Windsor;

    /// <summary>
    /// Интерфейс обработчика заданий
    /// </summary>
    public interface ITaskHandler<T> where T : BaseTask
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        IWindsorContainer Container { get; set; }

        /// <summary>
        /// Выполнить
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        void Run(T task);
    }
}
