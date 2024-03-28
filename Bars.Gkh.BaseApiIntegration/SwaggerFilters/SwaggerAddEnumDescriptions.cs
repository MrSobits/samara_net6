namespace Bars.Gkh.BaseApiIntegration.SwaggerFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Description;

    using Bars.B4.Utils;

    using Swashbuckle.Swagger;

    /// <summary>
    /// Фильтр для добавления значений перечислений в сваггер
    /// </summary>
    public class SwaggerAddEnumDescriptions : IDocumentFilter
    {
        /// <inheritdoc />
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            // добавить описания перечислений в модели результатов
            foreach (var schemaDictionaryItem in swaggerDoc.definitions)
            {
                foreach (var propertyDictionaryItem in schemaDictionaryItem.Value.properties)
                {
                    var property = propertyDictionaryItem.Value;
                    var propertyEnums = property.@enum;

                    if (propertyEnums == null && property.type == "array")
                    {
                        propertyEnums = property.items.@enum;
                    }

                    if (propertyEnums != null && propertyEnums.Count > 0)
                    {
                        property.description += ":" + this.DescribeEnum(propertyEnums, false);
                    }
                }
            }

            // добавить описания enum к входным параметрам
            if (swaggerDoc.paths.Count <= 0)
            {
                return;
            }

            foreach (var pathItem in swaggerDoc.paths.Values)
            {
                this.DescribeEnumParameters(pathItem.parameters);

                var possibleParameterisedOperations = new List<Operation> { pathItem.get, pathItem.post, pathItem.put, pathItem.delete };
                possibleParameterisedOperations
                    .Where(x => x != null)
                    .ForEach(x => this.DescribeEnumParameters(x.parameters));
            }
        }

        /// <summary>
        /// Создать описание для перечислений, находящихся во входных параметрах
        /// </summary>
        /// <param name="parameters"></param>
        private void DescribeEnumParameters(IList<Parameter> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var param in parameters)
            {
                var paramEnums = param.@enum;
                if (paramEnums != null && paramEnums.Count > 0)
                {
                    param.description += this.DescribeEnum(paramEnums, true);
                }
            }
        }

        /// <summary>
        /// Сформировать описание для перечисления
        /// </summary>
        private string DescribeEnum(IEnumerable<object> enums, bool isIngoingParameter)
        {
            var enumDescriptions = new List<string>();
            foreach (var enumOption in enums)
            {
                var enumOptionDisplay = (DisplayAttribute)enumOption.GetType()
                    .GetFields()
                    .Single(x => x.Name == enumOption.ToString())
                    .GetCustomAttributes(false)
                    .SingleOrDefault(x => x is DisplayAttribute);

                enumDescriptions.Add($"<li>{Convert.ToInt64(enumOption)} = {enumOptionDisplay?.Value}</li>");
            }

            return $@"<ul>{string.Join(isIngoingParameter ? "" : ", ", enumDescriptions.ToArray())}<ul>";
        }
    }
}