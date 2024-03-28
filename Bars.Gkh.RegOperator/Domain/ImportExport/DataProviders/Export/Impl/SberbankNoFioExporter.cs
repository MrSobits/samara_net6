namespace Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export.Impl
{
    using ImportMaps;
    using Mapping;

    public class SberbankNoFioExporter : SberbankExportProvider
    {
        private IImportMap _map = new PersonalAccountInfoSberNoFioMap();

        public override IImportMap Mapper
        {
            get { return _map; }
        }
    }
}