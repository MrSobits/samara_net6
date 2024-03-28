namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers.GenericDeserializers
{
    using IR;
    using Mapping;

    public class DbfImportExportSerializer<T> : DefaultSerializer<T> where T : class
    {
        public DbfImportExportSerializer(ImportExportMapperHolder mapperHolder) : base(mapperHolder)
        {
        }

        protected override IIRTranslator Translator
        {
            get { return new DbfIRTranslator(); }
        }
    }
}