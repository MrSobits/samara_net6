namespace Bars.Gkh.Config.Impl.Internal
{
    using System;
    using System.Reflection;

    using Bars.B4.Utils.Annotations;

    public class PropertyMetadataArgs
    {
        public PropertyMetadataArgs(string parent, string extends, string key, Type type)
        {
            ArgumentChecker.NotNull(type, "type");
            Type = type;
            Parent = parent;
            Extends = extends;
            Key = key;
        }

        public PropertyMetadataArgs(string parent, string key, PropertyInfo property)
            : this(parent, null, key, property.PropertyType)
        {
            ArgumentChecker.NotNull(property, "property");
            Property = property;
        }

        public string Key { get; private set; }

        public string Parent { get; private set; }

        public PropertyInfo Property { get; private set; }

        public Type Type { get; private set; }

        public string Extends { get; private set; }
    }
}