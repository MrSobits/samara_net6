namespace Bars.Gkh.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService.Dict.RealEstateType;

    /// <summary>
    /// Экспорт для предварительного просмотре при изменении типов домов
    /// </summary>
    public class UpdateRoTypesPreviewExport : BaseDataExportService
    {
        /// <summary>
        /// Данные для экспорта
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRealEstateTypeService>();

            var result = service.UpdateRobjectTypesPreview(baseParams).Data;

            return (IList)result;
        }
    }
}
