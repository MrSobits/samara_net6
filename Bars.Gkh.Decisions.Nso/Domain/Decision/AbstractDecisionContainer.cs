namespace Bars.Gkh.Decisions.Nso.Domain.Decisions
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;

    public abstract class AbstractDecisionContainer : IDecisionContainer
    {
        public abstract IDictionary<IDecision, ICollection<IDecision>> DecisionTransfers { get; }

        public ICollection<IDecision> AllDecisions
        {
            get
            {
                return _decisionCache
                ?? (_decisionCache =
                    DecisionTransfers.Return(
                        x =>
                            x.Keys
                                .ToList()
                                .Union(x.Values.SelectMany(d => d.ToList()))
                                .Distinct()
                                .ToList()));
            }
        }

        public ICollection<IDecisionType> AllTypes
        {
            get
            {
                return _decisionTypesCache
                       ?? (_decisionTypesCache =
                           DecisionTransfers.Return(
                               x =>
                                   x.Keys
                                       .ToList()
                                       .Union(x.Values.SelectMany(d => d.ToList()))
                                       .Distinct()
                                       .ToList().Select(d => d.DecisionType).Distinct().ToList()));
            }
        }

        public virtual IList<string> GetSiblings(string code)
        {
            return new List<string>();
        }

        private List<IDecision> _decisionCache;
        private List<IDecisionType> _decisionTypesCache;
    }
}