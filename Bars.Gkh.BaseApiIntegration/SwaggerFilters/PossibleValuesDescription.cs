namespace Bars.Gkh.BaseApiIntegration.SwaggerFilters
{
    using System;
    using System.Linq;

    using Bars.Gkh.Config.Attributes.DataAnnotations;

    using Swashbuckle.Swagger;

    /// <summary>
    /// Фильтр, чтобы отображались значения для <see cref="PossibleValuesAttribute"/>
    /// </summary>
    public class PossibleValuesDescription: ISchemaFilter
    {
        /// <inheritdoc />
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            var properties = type.GetProperties()
                .Where(x => x.GetCustomAttributes(true)
                    .Any(y => y.GetType() == typeof(PossibleValuesAttribute)))
                .ToList();

            if (!properties.Any())
            {
                return;
            }

            foreach (var property in properties)
            {
                var attributeValues = (property.GetCustomAttributes(true)
                    .Single(x => x is PossibleValuesAttribute) as PossibleValuesAttribute)
                    .Values;
                
                var propertyKey = schema.properties.Keys.Single(x => string.Equals(x, property.Name, StringComparison.CurrentCultureIgnoreCase));
                schema.properties[propertyKey].description += ": " + string.Join(", ", attributeValues);
            }
        }
    }
}