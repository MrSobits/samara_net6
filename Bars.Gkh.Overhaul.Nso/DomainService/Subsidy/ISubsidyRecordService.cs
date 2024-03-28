namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.IO;

    using Bars.B4;

    public interface ISubsidyRecordService
    {
        IDataResult GetSubsidy(BaseParams baseParams);

        IDataResult CalcOwnerCollection(BaseParams baseParams);

        IDataResult CalcValues(BaseParams baseParams);

        Stream PrintReport(BaseParams baseParams);
    }
}