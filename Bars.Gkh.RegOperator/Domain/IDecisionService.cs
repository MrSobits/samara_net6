namespace Bars.Gkh.RegOperator.Domain
{
    using System.IO;

    using Bars.B4;

    public interface IDecisionService
    {
        Stream DownloadContract(BaseParams baseParams);
    }
}