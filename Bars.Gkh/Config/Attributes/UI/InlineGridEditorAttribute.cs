namespace Bars.Gkh.Config.Attributes.UI
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class InlineGridEditorAttribute : GkhConfigPropertyEditorAttribute
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public InlineGridEditorAttribute()
            : base("B4.ux.config.InlineGridEditor", "inlinegrideditor")
        {
        }
    }
}