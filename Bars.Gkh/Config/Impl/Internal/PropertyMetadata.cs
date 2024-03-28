namespace Bars.Gkh.Config.Impl.Internal
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     Внутреннее описание полей конфигурации
    /// </summary>
    public partial class PropertyMetadata
    {
        public PropertyMetadata(PropertyMetadataArgs args)
        {
            AttributeProvider = new AttributeProvider();

            if (args.Property != null)
            {
                AttributeProvider.Append(args.Property);
            }

            AttributeProvider.Append(args.Type);

            Type = args.Type;
            PropertyInfo = args.Property;

            Init(args);
        }

        /// <summary>
        ///     Связанные атрибуты
        /// </summary>
        public AttributeProvider AttributeProvider { get; private set; }

        /// <summary>
        ///     Значение по умолчанию
        /// </summary>
        public object DefaultValue { get; private set; }

        /// <summary>
        ///     Отображаемое имя
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        ///     Признак сокрытия из представления в веб-интерфейсе
        /// </summary>
        public bool Hidden { get; private set; }

        /// <summary>
        /// Признак сокрытия из представления на UI
        /// </summary>
        public bool UIHidden { get; private set; }

        /// <summary>
        ///     Порядок
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     Ключ родительского элемента
        /// </summary>
        public string Parent { get; private set; }

        /// <summary>
        ///     Само поле
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        ///     Тип поля
        /// </summary>
        public Type Type { get; private set; }
    }
}