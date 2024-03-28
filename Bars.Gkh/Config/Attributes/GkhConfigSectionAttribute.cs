namespace Bars.Gkh.Config.Attributes
{
    using System;

    /// <summary>
    /// Атрибут, обозначающий корневую секцию конфигурации.
    /// Не требуется для подчиненных секций
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GkhConfigSectionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GkhConfigSectionAttribute"/> class. 
        /// </summary>
        /// <param name="alias">
        /// Имя, под которым секция будет представлена в файле конфигурации
        /// </param>
        public GkhConfigSectionAttribute(string alias)
        {
            this.Alias = alias;
        }

        /// <summary>
        /// Имя, под которым секция будет представлена в файле конфигурации
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Признак сокрытия из отображения в веб-интерфейсе
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Признак сокрытия из отображения в конфигурации на UI
        /// </summary>
        public bool UIHidden { get; set; }

        /// <summary>
        /// Узел, который будет считаться родительским при отображении этого узла.
        /// Может быть передан как тип секции (typeof(TSection)), так и её текстовый alias
        /// </summary>
        public object UIParent { get; set; }

        /// <summary>
        /// Секция конфигурации, поля которой будут дополнены полями текущей секции.
        /// Может быть передан как тип секции (typeof(TSection)), так и её текстовый alias
        /// </summary>
        public object UIExtends { get; set; }
    }
}