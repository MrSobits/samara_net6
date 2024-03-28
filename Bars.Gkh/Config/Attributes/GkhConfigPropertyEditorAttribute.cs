namespace Bars.Gkh.Config.Attributes
{
    using System;

    /// <summary>
    ///     Атрибут, указывающий на использование кастомного редактора
    ///     для параметра конфигурации.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class GkhConfigPropertyEditorAttribute : Attribute
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="path">Полный путь до редактора (например: Full.path.to.Editor)</param>
        /// <param name="xtype">XType редактора (например: mycutieeditor)</param>
        public GkhConfigPropertyEditorAttribute(string path, string xtype)
        {
            this.Path = path;
            this.Xtype = xtype;
        }

        /// <summary>
        ///     Полный путь до редактора
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     XType редактора
        /// </summary>
        public string Xtype { get; set; }
    }
}