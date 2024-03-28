namespace Bars.Gkh.ExecutionAction.ExecutionActionResolver.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;

    using Castle.Windsor;

    /// <summary>Резолвинг объекта из <see cref="BaseParams" /></summary>
    public class ExecutionActionResolver : IExecutionActionResolver
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public static string Code => typeof(ExecutionActionResolver).GUID.ToString();

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить действие
        /// </summary>
        /// <param name="baseParams">Код действия и параметры</param>
        /// <exception cref="ArgumentNullException">Не указан код действия</exception>
        /// <exception cref="KeyNotFoundException">Действие не найдено</exception>
        public IExecutionAction Resolve(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code), "Не указан код действия");
            }

            try
            {
                return this.Container.Resolve<IExecutionAction>(code);
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Действие '{code}' не найдено", ex);
            }
        }
    }
}