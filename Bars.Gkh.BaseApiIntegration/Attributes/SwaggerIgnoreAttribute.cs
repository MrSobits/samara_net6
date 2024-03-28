namespace Bars.Gkh.BaseApiIntegration.Attributes
{
    using System;

    /// <summary>
    /// Атрибут для игнорирования свойства при загрузки документации в сваггере
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerIgnoreAttribute : Attribute
    {
    }
}