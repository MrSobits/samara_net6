namespace Bars.Gkh.FormatDataExport.ProxySelectors
{
    using System;

    /// <summary>
    /// Ссылка на прокси-сущность, для поддержки ссылочной целостности экспорта
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ProxyIdAttribute : Attribute
    {
        /// <summary>
        /// Тип прокси сущности
        /// </summary>
        public Type ProxyType { get; }

        public ProxyIdAttribute(Type proxyType)
        {
            this.ProxyType = proxyType;
        }
    }
}