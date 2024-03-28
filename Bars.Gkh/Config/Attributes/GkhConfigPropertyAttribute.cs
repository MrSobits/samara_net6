namespace Bars.Gkh.Config.Attributes
{
    using System;

    /// <summary>
    /// Атрибут, помечающий поле в описании конфигурации
    /// Все поля, им не отмеченные, не будут рассматриваться при составлении
    /// карты конфигурации
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class GkhConfigPropertyAttribute : Attribute
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Признак сокрытия из отображения в веб-интерфейсе
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Скрыть панель инструментов
        /// </summary>
        public bool HideToolbar { get; set; }

        /// <summary>
        /// Признак сокрытия из отображения в конфигурации на UI
        /// </summary>
        public bool UIHidden { get; set; }
    }
}