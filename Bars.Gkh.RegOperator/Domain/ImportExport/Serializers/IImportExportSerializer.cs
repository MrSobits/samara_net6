namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using System.Collections.Generic;
    using System.IO;
    using B4.Utils;
    using Mapping;

    public interface IImportExportSerializer<T> where T : class
    {
        string Code { get; }
        ImportResult<T> Deserialize(Stream data, IImportMap format, string fileName = null, DynamicDictionary extraParams = null);

        Stream Serialize(List<T> data, IImportMap format);
    }
}