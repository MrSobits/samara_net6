namespace Bars.Gkh.RegOperator.DomainService
{
    using System.IO;

    using Bars.B4;

    public interface IUnacceptedChargesExportService
    {
        Stream UnacceptedChargesExport(BaseParams baseParams);
    }
}