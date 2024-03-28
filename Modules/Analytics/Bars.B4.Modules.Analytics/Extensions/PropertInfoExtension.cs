namespace Bars.B4.Modules.Analytics.Extensions
{
    using System.Reflection;
    using System.Xml.Serialization;
    using Bars.B4.Utils;

    /// <summary>
    /// Расширения для <see cref="PropertyInfo"/>
    /// </summary>
    public static class PropertInfoExtension
    {
        /// <summary>
        /// Получение значения атрибута <see cref="XmlElementAttribute"/> для <see cref="PropertyInfo"/>,
        /// переданного в качестве аргумента.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns>Возвращает <see cref="XmlElementAttribute.ElementName"/> если он задан,
        /// <see cref="MemberInfo.Name"/> в противном случае.</returns>
        public static string GetXmlElementAttrValue(this PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(XmlElementAttribute), false);

            var elementName = attributes.Length > 0 ? ((XmlElementAttribute)attributes[0]).ElementName : string.Empty;

            return string.IsNullOrWhiteSpace(elementName) ? propertyInfo.Name : elementName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {

            var attributes = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false);

            var elementName = attributes.Length > 0 ? ((DisplayAttribute)attributes[0]).Value : string.Empty;

            return string.IsNullOrWhiteSpace(elementName) ? propertyInfo.Name : elementName;
        }
    }
}
