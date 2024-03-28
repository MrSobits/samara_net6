namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using B4.Utils;
    using Castle.Windsor;
    using GenericDeserializers;
    using Mapping;

    public class DefaultImportExportSerializer<T> : IImportExportSerializer<T> where T : class
    {
        protected IWindsorContainer Container { get; private set; }

        private readonly ImportExportMapperHolder _mappingHolder;

        public virtual string Code { get { return "default"; } }

        public DefaultImportExportSerializer(IWindsorContainer container)
        {
            Container = container;
            _mappingHolder = container.Resolve<ImportExportMapperHolder>();
        }

        public virtual ImportResult<T> Deserialize(Stream data, IImportMap format, string fileName = null, DynamicDictionary extraParams = null)
        {
            var serializer = GetSerializer(format);

            return serializer.Deserialize(data, format);
        }

        public virtual Stream Serialize(List<T> data, IImportMap format)
        {
            var serializer = GetSerializer(format);

            return serializer.Serialize(data, format);
        }

        private DefaultSerializer<T> GetSerializer(IImportMap format)
        {
            DefaultSerializer<T> serializer = null;
            switch (format.Format.ToLowerInvariant())
            {
                case "xml":
                    serializer = new XmlImportExportSerializer<T>(_mappingHolder);
                    break;
                case "dbf":
                    serializer = new DbfImportExportSerializer<T>(_mappingHolder);
                    break;
                case "json":
                    serializer = new JsonImportExportSerializer<T>(_mappingHolder);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return serializer;
        }
    }
}