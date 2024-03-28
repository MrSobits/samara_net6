namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using Bars.B4.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Базовый класс для сборщиков информации
    /// </summary>
    public abstract class DataCollectorBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        protected DataCollectorBase(IWindsorContainer container)
        {
            this.Container = container;
        }

        #endregion

        #region Properties

        /// <summary>
        /// IoC контейнер
        /// </summary>
        protected IWindsorContainer Container { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Проверка заполненности обязательных полей
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="data">Инстанс</param>
        /// <returns>Список имен невалидных полей</returns>
        protected string[] CheckRequiredFields<T>(T data)
        {
            var type = typeof(T);
            var properties =
                type.GetProperties()
                    .Select(x => new { Property = x, Attribute = x.GetAttribute<XmlElementAttribute>(false) })
                    .Where(x => x.Attribute != null && !x.Attribute.IsNullable)
                    .Select(x => x.Property);

            return
                properties.Select(property => new { property, value = property.GetValue(data, null) })
                    .Where(@t => this.IsDefaultValue(@t.property.PropertyType, @t.value))
                    .Select(@t => @t.property.Name)
                    .ToArray();
        }

        /// <summary>
        /// Создает клон объекта путем его сериализации и десериализации.
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект</param>
        /// <returns>Клон объекта</returns>
        protected T Clone<T>(T obj)
        {
            //var serializer = new XmlSerializer(typeof(T));
            //using (var ms = new MemoryStream())
            //{
            //    serializer.Serialize(ms, obj);
            //    ms.Seek(0, SeekOrigin.Begin);
            //    return (T)serializer.Deserialize(ms);
            //}

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// Проверяет, что поле равно его дефолтному значению.
        /// Иными словами - не заполнено.
        /// </summary>
        /// <param name="type">Тип поля</param>
        /// <param name="value">Значение</param>
        /// <returns>Не заполнено?</returns>
        private bool IsDefaultValue(Type type, object value)
        {
            if (type == typeof(string))
            {
                return value.ToStr().IsEmpty();
            }

            return !type.IsValueType && value == null;
        }

        #endregion
    }
}