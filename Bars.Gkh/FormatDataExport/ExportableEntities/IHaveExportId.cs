namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// Наличие поля сквозной идентификации ExportId
    /// </summary>
    public interface IHaveExportId : IHaveId
    {
        /// <summary>
        /// Идентификатор для экспорта
        /// </summary>
        long ExportId { get; set; }
    }
}