namespace Bars.Gkh.Config.Impl.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Bars.B4.Utils;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    ///     Индексатор класса.
    ///     Строит карту полей класса
    /// </summary>
    internal static class ClassIndexer
    {
        public static IEnumerable<PropertyMetadataArgs> IndexRootSection(Type type)
        {
            if (typeof(IGkhConfigSection).IsAssignableFrom(type))
            {
                var attribute = type.GetAttribute<GkhConfigSectionAttribute>(true);

                string extends = null;
                if (attribute.UIExtends != null)
                {
                    var uiExtendsType = attribute.UIExtends as Type;
                    if (uiExtendsType != null)
                    {
                        extends = uiExtendsType.GetAttribute<GkhConfigSectionAttribute>(true).Alias;
                    }
                    else
                    {
                        extends = attribute.UIExtends as string;
                    }
                }

                string parent = null;
                if (string.IsNullOrEmpty(extends))
                {
                    if (attribute.UIParent != null)
                    {
                        var uiParentType = attribute.UIParent as Type;
                        if (uiParentType != null)
                        {
                            parent = uiParentType.GetAttribute<GkhConfigSectionAttribute>(true).Alias;
                        }
                        else
                        {
                            parent = attribute.UIParent as string;
                        }
                    }
                }

                yield return new PropertyMetadataArgs(parent, extends, attribute.Alias, type);

                foreach (var sub in IndexSubsection(type, extends, attribute.Alias))
                {
                    yield return sub;
                }
            }
            else
            {
                throw new Exception("Ожидался тип, реализующий IGkhConfigSection");
            }
        }

        public static IEnumerable<PropertyMetadataArgs> IndexSubsection(Type type, string extends, string parent)
        {
            foreach (var property in
                type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;
                var hasAttribute = property.HasAttribute<GkhConfigPropertyAttribute>(true);
                var ignore = property.HasAttribute<IgnoreAttribute>(true);
                if (!hasAttribute || ignore)
                {
                    continue;
                }

                var isSection = typeof(IGkhConfigSection).IsAssignableFrom(propertyType);

                yield return new PropertyMetadataArgs(extends ?? parent, parent + '.' + property.Name, property);

                if (!isSection)
                {
                    continue;
                }

                foreach (
                    var sub in
                        IndexSubsection(
                            propertyType,
                            (extends ?? parent) + '.' + property.Name,
                            parent + '.' + property.Name))
                {
                    yield return sub;
                }
            }
        }
    }
}