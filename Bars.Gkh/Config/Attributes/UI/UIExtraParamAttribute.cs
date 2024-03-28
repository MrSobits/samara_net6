namespace Bars.Gkh.Config.Attributes.UI
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UIExtraParamAttribute : Attribute
    {
        public string Name { get; }

        public object Value { get; }

        public UIExtraParamAttribute(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}