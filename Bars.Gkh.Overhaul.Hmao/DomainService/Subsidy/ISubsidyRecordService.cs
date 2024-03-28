namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System.IO;

    using Bars.B4;

    public interface ISubsidyRecordService
    {
        IDataResult GetSubsidy(BaseParams baseParams);

        IDataResult CalcOwnerCollection(BaseParams baseParams);
        IDataResult UpdateSaldoBallance(BaseParams baseParams);
        IDataResult CalcValues(BaseParams baseParams);

        Stream PrintReport(BaseParams baseParams);

        IDataResult GetSubsidyByMu(long muId, int periodStart, int periodEnd);
    }
}