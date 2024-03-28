namespace Bars.Gkh.ExecutionAction
{
    using System;

    /// <summary>
    /// Возможность повторного запуска обязательного действия вручную
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RepeatableAttribute : Attribute
    {
    }
}