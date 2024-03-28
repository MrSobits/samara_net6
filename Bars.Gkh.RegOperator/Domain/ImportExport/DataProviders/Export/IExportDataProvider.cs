namespace Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export
{
    using B4;
    using Mapping;

    /// <summary>
    /// Интерфейс для получения данных для экспорта
    /// </summary>
    public interface IExportDataProvider
    {
        IImportMap Mapper { get; }

        IDataResult<ExportOutput> GetData(BaseParams @params);
    }
}