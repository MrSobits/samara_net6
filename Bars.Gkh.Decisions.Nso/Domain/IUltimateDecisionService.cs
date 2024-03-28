namespace Bars.Gkh.Decisions.Nso.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Gkh.Entities;

    public interface IUltimateDecisionService
    {
        T GetActualDecision<T>(RealityObject realityObject) where T : UltimateDecision;
        T GetActualDecision<T>(long realityObjId) where T : UltimateDecision;
        //ICollection<T> GetActualDecisions<T>(ICollection<long> realityObjIds) where T : UltimateDecision;
        ICollection<T> GetActualDecisions<T>(IQueryable<RealityObject> realityObjects) where T : UltimateDecision;
    }
}
