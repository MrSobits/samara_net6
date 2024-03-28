namespace Bars.Gkh.Config.Attributes.UI
{
    using System;

    /// <summary>
    /// Указывает на необходимость ограничить по доступу к данному полю
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class PermissionableAttribute : Attribute
    {
    }
}