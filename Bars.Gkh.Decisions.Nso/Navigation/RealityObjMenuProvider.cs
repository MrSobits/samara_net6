namespace Bars.Gkh.Decisions.Nso
{
    using Bars.B4;
    using Bars.Gkh.Navigation;

    public class RealityObjMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return RealityObjMenuKey.Key;
            }
        }

        public string Description
        {
            get
            {
                return RealityObjMenuKey.Description;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Протоколы решений", "realityobjectedit/{0}/decisionprotocol")
                .AddRequiredPermission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.View");
            // GKH-4052
            //root.Add("Протокол решений органа гос. власти", "realityobjectedit/{0}/govdecisionprotocol")
            //    .AddRequiredPermission("Gkh.RealityObject.Register.GovProtocolDecisionViewCreate.View");
            root.Add("Действующие решения", "realityobjectedit/{0}/decisionhistory")
                .AddRequiredPermission("Gkh.RealityObject.Register.DecisionHistory.View");
        }
    }
}