namespace Bars.Gkh.Config.Attributes.UI
{
    using System;

    /// <summary>
    /// Указывает на необходимость вынести поле секции конфигурации
    /// в дерево элементов конфигурации
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NavigationAttribute : Attribute
    {
    }
}