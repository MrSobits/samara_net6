namespace Bars.Gkh.ExecutionAction
{
    using System;

    /// <summary>
    /// Действие не доступно для выбора пользователем
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HiddenActionAttribute : Attribute
    {
    }
}