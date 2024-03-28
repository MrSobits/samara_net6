namespace Bars.Gkh.DomainService.Administration
{
    using Bars.B4;
    using Bars.Gkh.Entities.Administration.FormatDataExport;

    public interface IFormatDataExportEntityInfoService
    {
        /// <summary>
        /// Обновить значение <see cref="FormatDataExportEntity"/>
        /// </summary>
        IDataResult UpdateExportEntitiesInfo(BaseParams baseParams);
    }
}
