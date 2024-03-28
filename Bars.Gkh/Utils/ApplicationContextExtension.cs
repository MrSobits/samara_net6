namespace Bars.Gkh.Utils
{
    using Bars.B4.Application;

    /// <summary>
    /// Расширение для <see cref="IAppContext"/>
    /// </summary>
    public static class ApplicationContextExtension
    {
        /// <summary>
        /// Определяет тип контекста приложения
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ApplicationContextType GetContextType(this IAppContext context)
        {
            
            if (context.GetType().FullName == "Bars.B4.WebAppContext")
            {
                return ApplicationContextType.WebApplication;
            }
            if (context.GetType().FullName == "Bars.B4.Modules.Console.CoreComponents")
            {
                return ApplicationContextType.ConsoleApplication;
            }
            if (context.GetType().FullName == "Bars.B4.Modules.Tasks.Executor.App.TasksAppContext")
            {
                return ApplicationContextType.TaskApplication;
            }
            return ApplicationContextType.Unknown;
        }
    }

    /// <summary>
    /// Тип контекста приложения
    /// </summary>
    public enum ApplicationContextType
    {
        /// <summary>
        /// Неизвестный контекст
        /// </summary>
        Unknown,
        /// <summary>
        /// Контекст веб-приложения
        /// </summary>
        WebApplication,
        /// <summary>
        /// Контекст консольного приложения
        /// </summary>
        ConsoleApplication,
        /// <summary>
        /// Контекст приложения расчетов
        /// </summary>
        TaskApplication
    }
}