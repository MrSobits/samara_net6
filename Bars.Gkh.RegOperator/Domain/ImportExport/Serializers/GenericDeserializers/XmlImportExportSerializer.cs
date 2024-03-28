namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers.GenericDeserializers
{
    using IR;
    using Mapping;

    public class XmlImportExportSerializer<T> : DefaultSerializer<T> where T : class
    {
        public XmlImportExportSerializer(ImportExportMapperHolder mapperHolder) : base(mapperHolder)
        {
        }

        protected override IIRTranslator Translator
        {
            get { return new XmlIRTranslator(); }
        }
    }
}