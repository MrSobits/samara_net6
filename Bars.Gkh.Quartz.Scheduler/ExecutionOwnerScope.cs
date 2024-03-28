namespace Bars.Gkh.Quartz.Scheduler
{
    using System;

    using Bars.B4;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Utils;

    using global::Quartz;

    /// <summary>
    /// Класс для вызова методов в скопе инициатора выполнения запланированной задачи
    /// </summary>
    public static class ExecutionOwnerScope
    {
        /// <summary>
        /// Вызвать метод в скопе инициатора выполнения запланированной задачи
        /// </summary>
        /// <param name="executionContext">Контекст выполнения задачи</param>
        /// <param name="action">Экземпляр делегата, инкапсулирующего выполняемый метод</param>
        public static void CallInNewScope(IJobExecutionContext executionContext, Action action)
        {
            var executionOwner = ExecutionOwnerScope.GetExecutionOwner(executionContext);

            ExecutionOwnerScope.CallInNewScope(executionOwner, action);
        }

        /// <summary>
        /// Вызвать метод в скопе инициатора выполнения запланированной задачи
        /// </summary>
        /// <param name="executionOwner">Инициатор выполнения задачи</param>
        /// <param name="action">Экземпляр делегата, инкапсулирующего выполняемый метод</param>
        public static void CallInNewScope(IExecutionOwner executionOwner, Action action)
        {
            UserExecutionOwner userExecutionOwner = null;

            if (executionOwner != null && executionOwner.Type == ExecutionOwnerType.User)
            {
                userExecutionOwner = executionOwner as UserExecutionOwner;
            }

            if (userExecutionOwner != null)
            {
                var userIdentity = new UserIdentity(userExecutionOwner.UserId, userExecutionOwner.Name, Guid.NewGuid().ToString("N"), DynamicDictionary.Create(), "Form");

                ExplicitSessionScope.CallInNewScope(userIdentity, action);
            }
            else
            {
                ExplicitSessionScope.CallInNewScope(action);
            }
        }

        private static IExecutionOwner GetExecutionOwner(IJobExecutionContext executionContext)
        {
            IExecutionOwner result = null;

            object propertyValue;

            if (executionContext.MergedJobDataMap.TryGetValue("ExecutionOwner", out propertyValue))
            {
                result = propertyValue as IExecutionOwner;
            }

            return result;
        }
    }
}
