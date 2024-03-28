namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Collections.Generic;
    using Bars.B4;

    public interface IDpkrService
    {
        IDataResult CreateProgramCrByDpkr(BaseParams baseParams);

        IEnumerable<RealityObjectDpkrInfo> GetOvrhlYears(BaseParams baseParams);
    }
}