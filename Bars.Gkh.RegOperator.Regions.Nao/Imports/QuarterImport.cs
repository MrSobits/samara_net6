using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Enums.Import;
using Bars.Gkh.Import;
using Bars.Gkh.Import.Impl;
using System.Reflection;
using System.Threading;

namespace Bars.Gkh.RegOperator.Regions.Nao.Imports
{
    /// <summary>
    /// Импорт помещений по просьбе НАО в их формате
    /// </summary>
    public class QuarterImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => "QuarterImport";

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт помещений, без создания ЛС";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "xls,xlsx";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.QuarterImport";

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = "OK";
            return true;
        }

        /// <summary>
        /// Импорт
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор прогресса</param>
        /// <param name="ct">Уведомление об отмене</param>
        /// <returns>Результат</returns>
        protected override ImportResult Import(BaseParams baseParams, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            return new ImportResult(StatusImport.CompletedWithError, "Не реализовано");
        }
    }
}
