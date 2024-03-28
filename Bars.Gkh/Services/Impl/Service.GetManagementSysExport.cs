namespace Bars.Gkh.Services.Impl
{
    using System.IO;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;

    partial class Service
    {
        public Stream GetManagementSysExport(string periodStart)
        {
            var export = Container.Resolve<IDataExportService>("ManagementSysExport");

            var param = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    new KeyValuePair<string, object>("periodStart", periodStart)
                }
            };

            if (export != null)
            {
                return export.ExportData(param).FileStream;
            }

            return null;
        }
    }
}
