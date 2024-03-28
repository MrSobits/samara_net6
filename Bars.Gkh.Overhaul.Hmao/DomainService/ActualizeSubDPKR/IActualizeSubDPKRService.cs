using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Collections.Generic;
using static Bars.Gkh.Overhaul.Hmao.Services.ActualizeSubDPKR.ActualizeSubDPKRService;

namespace Bars.Gkh.Overhaul.Hmao.Services.ActualizeSubDPKR
{
    public interface IActualizeSubrecordService
    {
        List<HouseWithReasonView> GetAddEntriesList(ProgramVersion version);

        List<HouseWithReasonView> GetDeleteEntriesList(ProgramVersion version);        

        void RemoveHouseForAdd(long houseId);

        void RemoveHouseForDelete(long houseId);

        void Actualize(ProgramVersion version, int endYear);

        void ClearCache();
        void RemoveSelected(ProgramVersion version, long[] selectedAddId, long[] selectedDeleteId);
    }
}
