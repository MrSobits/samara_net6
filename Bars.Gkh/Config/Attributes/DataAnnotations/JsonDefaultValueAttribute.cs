namespace Bars.Gkh.Config.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel;

    using Bars.Gkh.Config.Impl.Internal.Serialization;

    /// <summary>
    /// Надстройка над <see cref="DefaultValueAttribute"/>.
    /// В случае, если стандартным конвертером не удалось привести значение к требуемому типу,
    /// попытается десериализовать его JSON-конвертером.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class JsonDefaultValueAttribute : DefaultValueAttribute
    {
        /// <summary>
        /// Пытается привести значение к требуемому типу стандартными средствами - <see cref="TypeConverter"/>.
        /// В случае неудачи попытается десериализовать его JSON-конвертером.
        /// </summary>
        /// <param name="type">A <see cref="T:System.Type"/> that represents the type to convert the value to. </param><param name="value">A <see cref="T:System.String"/> that can be converted to the type using the <see cref="T:System.ComponentModel.TypeConverter"/> for the type and the U.S. English culture. </param>
        public JsonDefaultValueAttribute(Type type, string value)
            : base(type, value)
        {
            if (this.Value == null)
            {
                try
                {
                    this.SetValue(ConfigSerializer.Deserialize(value, type));
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}