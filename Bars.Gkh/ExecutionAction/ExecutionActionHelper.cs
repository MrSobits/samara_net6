namespace Bars.Gkh.ExecutionAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Помощник при работе с действиями
    /// </summary>
    public static class ExecutionActionHelper
    {
        private static IWindsorContainer Container => ApplicationContext.Current.Container;

        /// <summary>
        /// Метод выполняет проверку, что указанное действие успешно выполнено
        /// </summary>
        /// <typeparam name="TAction">Тип действия</typeparam>
        /// <param name="code">Код, если не совпадает с типом</param>
        /// <exception cref="ValidationException">Исключение будет выброшено, если действие выполнено с ошибками</exception>
        public static void EnsureSuccess<TAction>(string code = null) where TAction : IExecutionAction
        {
            if (ExecutionActionHelper.IsFailure<TAction>(code))
            {
                throw new ValidationException($"Не выполнено обязательное действие \"{ExecutionActionHelper.GetName<TAction>(code)}\"");
            }
        }

        /// <summary>
        /// Метод выполняет проверку, что указанное действие не выполнено или выполнено с ошибками
        /// </summary>
        /// <typeparam name="TAction">Тип действия</typeparam>
        /// <param name="code">Код, если не совпадает с типом</param>
        /// <exception cref="ValidationException">Исключение будет выброшено, если действие успешно выполнено</exception>
        public static void EnsureFailure<TAction>(string code = null) where TAction : IExecutionAction
        {
            if (ExecutionActionHelper.IsSuccess<TAction>(code))
            {
                throw new ValidationException($"Действие \"{ExecutionActionHelper.GetName<TAction>(code)}\" уже выполнено");
            }
        }

        /// <summary>
        /// Метод выполняет проверку, что указанное действие успешно выполнено
        /// </summary>
        /// <typeparam name="TAction">Тип действия</typeparam>
        /// <param name="code">Код, если не совпадает с типом</param>
        public static bool IsSuccess<TAction>(string code = null)
            where TAction : IExecutionAction
        {
            var result = false;
            code = code ?? typeof(TAction).Name;

            ExecutionActionHelper.Container.UsingForResolved<IRepository<ExecutionActionResult>>((ioc, repository) =>
            {
                result = repository.GetAll()
                    .Where(x => x.Task.ActionCode == code)
                    .Any(x => x.Status == ExecutionActionStatus.Success);
            });

            return result;
        }

        /// <summary>
        /// Метод выполняет проверку, что указанное действие не выполнено или выполнено с ошибками
        /// </summary>
        /// <typeparam name="TAction">Тип действия</typeparam>
        /// <param name="code">Код, если не совпадает с типом</param>
        public static bool IsFailure<TAction>(string code = null)
            where TAction : IExecutionAction
        {
            return !ExecutionActionHelper.IsSuccess<TAction>(code);
        }

        private static string GetName<TAction>(string code = null)
        {
            var actions = ExecutionActionHelper.Container.ResolveAll<IExecutionAction>();
            using (ExecutionActionHelper.Container.Using((IExecutionAction[])actions))
            {
                code = code ?? typeof(TAction).Name;
                return actions.FirstOrDefault(x => x.Code == code)?.Name;
            }
        }
    }
}