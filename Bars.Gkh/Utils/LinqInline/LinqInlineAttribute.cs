namespace Bars.Gkh.Utils.LinqInline
{
    using System;

    /// <summary>
    /// Заменить вызов метода на дерево выражений <see cref="LinqInliner.Inlining{T}"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LinqInlineAttribute : Attribute
    {
        public string ExpressionName { get; set; }

        public LinqInlineAttribute()
        {
        }

        public LinqInlineAttribute(string name)
        {
            this.ExpressionName = name;
        }
    }
}