using Bars.B4;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System;
using System.Collections.Generic;
using static Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl.MakeKPKRFromDPKRService;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version
{
    public interface IMakeKPKRFromDPKRService
    {
        string MakeKPKR(ProgramVersion version, short startYear, byte yearCount, bool firstYearPSD, bool firstYearWithoutWork, bool KWithWorks, bool PSDWithWorks, bool PSDNext3, bool EathWorkPSD, bool OneProgramCR);
        string MakeSubKPKR(ProgramVersion version, short startYear, byte yearCount, bool firstYearPSD, bool firstYearWithoutWork, long[] KEIds);
        List<KEWithHouse> GetKE(ProgramVersion version, short startYear, byte yearCount);
        SumView GetCosts(ProgramVersion version, long[] selectedKE);        
        List<SumByYearsView> GetCostsByYear(ProgramVersion version, long[] selectedKE);
        bool CheckCostsByYear(ProgramVersion version, long[] selectedKE);
        IDataResult MoveTypeWork(BaseParams baseParams, Int64 programId, Int64 typeworkToMoveId);
    }
}
