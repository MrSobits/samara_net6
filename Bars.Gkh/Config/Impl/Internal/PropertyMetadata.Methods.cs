namespace Bars.Gkh.Config.Impl.Internal
{
    using System;
    using System.ComponentModel;

    using Bars.B4.Utils;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Utils;

    public partial class PropertyMetadata
    {
        private static object GetValue(Type type)
        {
            var parameterlessConstructor = type.GetConstructor(new Type[0]);
            return parameterlessConstructor != null ? Activator.CreateInstance(type) : type.GetDefaultValue();
        }

        private void Init(PropertyMetadataArgs args)
        {
            if (string.IsNullOrEmpty(args.Extends))
            {
                this.Parent = args.Parent;
            }

            var sectionAttribute = this.AttributeProvider.GetAttribute<GkhConfigSectionAttribute>(true);
            var propertyAttribute = this.AttributeProvider.GetAttribute<GkhConfigPropertyAttribute>(true);

            if (!typeof(IGkhConfigSection).IsAssignableFrom(this.Type))
            {
                this.DefaultValue = this.AttributeProvider.GetAttribute<DefaultValueAttribute>(true).Return(x => x.Value)
                    ?? PropertyMetadata.GetValue(this.Type);
            }

            this.DisplayName = propertyAttribute.Return(x => x.DisplayName)
                ?? sectionAttribute.Return(x => x.DisplayName)
                ?? this.AttributeProvider.GetDisplayName() 
                ?? this.PropertyInfo.Return(x => x.Name);

            this.Hidden = propertyAttribute.Return(x => x.Hidden, sectionAttribute.Return(x => x.Hidden)) || !string.IsNullOrEmpty(args.Extends);
            this.UIHidden = propertyAttribute.Return(x => x.UIHidden, sectionAttribute.Return(x => x.UIHidden));
        }
    }
}