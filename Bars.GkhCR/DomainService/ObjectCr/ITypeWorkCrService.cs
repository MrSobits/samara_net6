namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Collections;
    using Bars.B4;

    public interface ITypeWorkCrService
    {
        IDataResult ListRealityObjectWorksByPeriod(BaseParams baseParams);

        IDataResult CalcPercentOfCompletion(BaseParams baseParams);

        IDataResult MoveTypeWork(BaseParams baseParams, Int64 programId, Int64 typeworkToMoveId);

        IList ListByProgramCr(BaseParams baseParams, bool isPaging, ref int totalCount);

        IDataResult CreateTypeWork(BaseParams baseParams);
    }
}
