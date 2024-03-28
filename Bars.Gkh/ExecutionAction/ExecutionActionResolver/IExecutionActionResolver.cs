namespace Bars.Gkh.ExecutionAction.ExecutionActionResolver
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;

    /// <summary>
    /// Резолвер действий
    /// </summary>
    public interface IExecutionActionResolver
    {
        /// <summary>Получить действие по коду из <see cref="BaseParams"/></summary>
        /// <param name="baseParams">Код действия и параметры</param>
        /// <exception cref="ArgumentNullException">Не указан код действия</exception>
        /// <exception cref="KeyNotFoundException">Действие не найдено</exception>
        IExecutionAction Resolve(BaseParams baseParams);
    }
}