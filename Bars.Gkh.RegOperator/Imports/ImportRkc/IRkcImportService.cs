using System.Collections.Generic;
using Bars.Gkh.Import;
using Bars.Gkh.RegOperator.Services.DataContracts;

namespace Bars.Gkh.RegOperator.Imports.ImportRkc
{
    /// <summary>
    /// Интерфейс для импорта лицевых счетов из PKЦ
    /// </summary>
    public interface IRkcImportService
    {
        ILogImport Import(ImportRkcRecord record);

        List<SyncRkcAccountResult> GetAccountResult();
    }
}