namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers.GenericDeserializers
{
    using IR;
    using Mapping;

    public class JsonImportExportSerializer<T> : DefaultSerializer<T> where T : class
    {
        public JsonImportExportSerializer(ImportExportMapperHolder mapperHolder) : base(mapperHolder)
        {
        }

        protected override IIRTranslator Translator
        {
            get { return new JsonIRTranslator(); }
        }
    }
}