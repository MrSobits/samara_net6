namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Collections.Generic;
    using B4;
    using Entities;
    using Gkh.Entities;

    public interface ICalcAccountOverdraftService
    {
        IEnumerable<CalcAccountOverdraft> GetRobjectOverdrafts(RealityObject ro);

        CalcAccountOverdraft GetLastOverdraft(RealityObject robject);

        CalcAccountOverdraft GetLastOverdraft(CalcAccount robject);

        decimal GetRobjectOverdraft(BaseParams baseParams);

        void UpdateAccountOverdraft(CalcAccount account, decimal availableSum);

        void UpdateAccountOverdraft(CalcAccountOverdraft overdraft, decimal availableSum);
    }
}
