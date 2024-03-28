namespace Bars.Gkh.RegOperator.Export.ExportToEbir
{
    public interface IEbirExport
    {
        string Format { get; }

        long GetExportFileId(long periodId);
    }
}