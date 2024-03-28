namespace Bars.Gkh.RegOperator.Export.Reports
{
    using Bars.B4;

    /// <summary>
    /// Экспортер информации по лицевым счетам
    /// </summary>
    public interface IPersonalAccountInfoExportService
    {
        IDataResult Export(BaseParams baseParams);
    }
}
