namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers.GenericDeserializers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Domain.ImportExport.IR;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;

    public abstract class DefaultSerializer<T> where T : class
    {
        private readonly ImportExportMapperHolder _mapperHolder;
        private readonly Dictionary<MemberInfo, ProviderMapper> _mappers;

        /// <summary>
        /// Признак того, что словарь маппинга <see cref="_mappers"/> был заполнен.
        /// </summary>
        private bool mapperWasFilled = false;

        private readonly Dictionary<MemberInfo, bool> _shouldSkip;

        protected abstract IIRTranslator Translator { get; }

        protected DefaultSerializer(ImportExportMapperHolder mapperHolder)
        {
            _mapperHolder = mapperHolder;
            _mappers = new Dictionary<MemberInfo, ProviderMapper>();
            _shouldSkip = new Dictionary<MemberInfo, bool>();
        }

        public virtual ImportResult<T> Deserialize(Stream data, IImportMap format)
        {
            _shouldSkip.Clear();

            var model = Translator.Parse(data);
			var props = typeof(T).GetProperties();
			FillMappers(format, props);

			return new ImportResult<T> { Rows = GetRows(model, props) };
        }

		private IEnumerable<ImportRow<T>> GetRows(IEnumerable<IRModel> model, PropertyInfo[] props)
        {
            var cannotFillType = true;

            foreach (var row in model)
            {
                var entry = Activator.CreateInstance<T>();

                foreach (var property in props)
                {
                    if (ShouldSkip(property))
                    {
                        continue;
                    }

                    var map = _mappers.Get(property);

                    if (map != null)
                    {
                        var path = map.Lookuper.Lookup();

                        if (row.PropertyBag.All(x => !String.Equals(x.Name, path, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            SkipInFuture(property);
                            continue;
                        }

                        var value = row.GetAnyPropertyBagValue(map.Lookuper.Lookup());

                        var parsedValue = map.Parser.Return(x => x.Parse(value));

                        if (parsedValue == null && value == null)
                        {
                            continue;
                        }

                        cannotFillType = false;

                        property.SetValue(entry,
                            ConvertHelper.ConvertTo(parsedValue ?? value, property.PropertyType),
                            new object[0]);
                    }
                }

                if (cannotFillType)
                {
                    yield break;
                }

                yield return new ImportRow<T> { Value = entry };
            }
        }

        public virtual Stream Serialize(List<T> data, IImportMap format)
        {
            _shouldSkip.Clear();

            var type = typeof (T);
            var props = type.GetProperties();

            FillMappers(format, props);

            var metaData = new List<IRModelProperty>();
            FillPropertyBag(props, ref metaData, null);

            var models = new List<IRModel>();
            foreach (var x in data)
            {
                var model = new IRModel();
                var modelProperties = new List<IRModelProperty>();
                var cannotTranslate = FillPropertyBag(props, ref modelProperties, x);

                model.PropertyBag = modelProperties;

                if (cannotTranslate)
                    break;

                models.Add(model);
            }

            var allModel = new IRModel { ModelName = "root", Children = models };

            return Translator.FromModel(allModel, metaData);
        }

        private bool FillPropertyBag(PropertyInfo[] props, ref List<IRModelProperty> bag, T entry)
        {
            var cannotTranslate = true;
            foreach (var propertyInfo in props)
            {
                if (ShouldSkip(propertyInfo))
                {
                    continue;
                }

                if (!_mappers.ContainsKey(propertyInfo))
                {
                    SkipInFuture(propertyInfo);
                    continue;
                }

                cannotTranslate = false;

                var mapper = _mappers[propertyInfo];

                bag.Add(new IRModelProperty
                {
                    Name = mapper.Lookuper.Lookup(),
                    Value = entry.IsNotNull() ? propertyInfo.GetValue(entry, new object[0]) : null,
                    Type = propertyInfo.PropertyType,
                    DLength = mapper.DLength,
                    NLength = mapper.NLength
                });
            }

            return cannotTranslate;
        }

        private void FillMappers(IImportMap format, PropertyInfo[] props)
        {
            // Достаточно заполнить словарь маппинга один раз.
            if (this.mapperWasFilled)
            {
                return;
            }

            foreach (var propertyInfo in props)
            {
                var mapper = this._mapperHolder.GetMapper<T>(propertyInfo, format);
                if (mapper != null && !this._mappers.ContainsKey(propertyInfo))
                {
                    this._mappers.Add(propertyInfo, mapper);
                }
            }

            this.mapperWasFilled = true;
        }

        private bool ShouldSkip(PropertyInfo property)
        {
            return _shouldSkip.Get(property);
        }

        private void SkipInFuture(PropertyInfo property)
        {
            if (!_shouldSkip.ContainsKey(property))
            {
                _shouldSkip.Add(property, true);
            }
        }
    }
}