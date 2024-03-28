namespace Bars.Gkh.BaseApiIntegration.SwaggerFilters
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Bars.Gkh.BaseApiIntegration.Attributes;

    using Castle.Core.Internal;

    using Swashbuckle.Swagger;

    /// <summary>
    /// Фильтр, чтобы применялся <see cref="SwaggerIgnoreAttribute"/>
    /// </summary>
    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        /// <inheritdoc />
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (schema?.properties == null || type == null)
            {
                return;
            }

            var excludedProperties = type.GetProperties()
                .Where(t => t.GetCustomAttribute<SwaggerIgnoreAttribute>() != null);

            foreach (var excludedProperty in excludedProperties)
            {
                var excludedKey = schema.properties.Keys.Single(x => string.Equals(x, excludedProperty.Name, StringComparison.CurrentCultureIgnoreCase));
                if (!excludedKey.IsNullOrEmpty())
                {
                    schema.properties.Remove(excludedKey);
                }
            }
        }
    }
}