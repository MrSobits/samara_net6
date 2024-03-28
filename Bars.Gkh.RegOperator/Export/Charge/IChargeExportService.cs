namespace Bars.Gkh.RegOperator.Export
{
    using B4;

    /// <summary>
    /// Экспортер начислений
    /// </summary>
    public interface IChargeExportService
    {
        IDataResult Export(BaseParams baseParams);
    }
}