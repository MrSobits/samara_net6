namespace Bars.GkhGji.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;

    /// <summary>
    /// Экспорт постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorDataExport : BaseDataExportService
    {
        /// <summary>
        /// Id объекта в контейнере
        /// </summary>
        public static string Id => nameof(ResolutionRospotrebnadzorDataExport);

        /// <summary>
        /// Метод получения Данных для Экспорта
        /// </summary>
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            return this.Container.Resolve<IResolutionRospotrebnadzorService>().GetViewList(baseParams)
                .Order(loadParam)
                .ToList();
        }
    }
}