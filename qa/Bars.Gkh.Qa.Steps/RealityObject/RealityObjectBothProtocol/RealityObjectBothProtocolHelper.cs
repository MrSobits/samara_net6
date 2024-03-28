namespace Bars.Gkh.Qa.Steps
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Enums;

    using TechTalk.SpecFlow;

    internal class RealityObjectBothProtocolHelper : BindingBase
    {
        /// <summary>
        /// текущий протокол/решение
        /// </summary>
        public static BaseParams Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentRealityObjectBothProtocol"))
                {
                    throw new SpecFlowException("Отсутствует текущий протокол/решение");
                }

                var realityObjectBothProtocol = ScenarioContext.Current.Get<BaseParams>("CurrentRealityObjectBothProtocol");

                return realityObjectBothProtocol;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentRealityObjectBothProtocol"))
                {
                    ScenarioContext.Current.Remove("CurrentRealityObjectBothProtocol");
                }

                ScenarioContext.Current.Add("CurrentRealityObjectBothProtocol", value);
            }
        }

        public static List<RealityObjectBothProtocolProxy> GetRealityObjectProtocols(long realityObjectId)
        {
            var protocols = Container.ResolveDomain<RealityObjectDecisionProtocol>().GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId)
                .Select(x => new RealityObjectBothProtocolProxy
                                 {
                                     Id = x.Id,
                                     ProtocolDate = x.ProtocolDate,
                                     ProtocolNumber = x.DocumentNum,
                                     ProtocolType = CoreDecisionType.Owners,
                                     State = x.State
                                 })
               
                .ToList();

            var govProtocols = Container.ResolveDomain<GovDecision>().GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId)
                .Select(x => new RealityObjectBothProtocolProxy
                {
                    Id = x.Id,
                    ProtocolDate = x.ProtocolDate,
                    ProtocolNumber = x.ProtocolNumber,
                    ProtocolType = CoreDecisionType.Government,
                    State = x.State
                })
                .ToList();

            protocols.AddRange(govProtocols);

            return protocols;
        }
    }
}
