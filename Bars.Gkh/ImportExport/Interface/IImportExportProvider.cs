namespace Bars.Gkh.ImportExport
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Интерфейс универсального экспорта/импорта данных системы
    /// </summary>
    public interface IImportExportProvider
    {
        void Import(Stream stream);

        MemoryStream Export(IEnumerable<string> tableNames);

        Dictionary<string, string> GetEntityNames();
    }
}