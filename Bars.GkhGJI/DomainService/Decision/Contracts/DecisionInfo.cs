namespace Bars.GkhGji.DomainService.Contracts
{
    public class DecisionInfo
    {
        public string InspectorNames { get; protected set; }

        public string InspectorIds { get; protected set; }

        public string BaseName { get; protected set; }

        public string PlanName { get; protected set; }

        public DecisionInfo(string inspectorNames, string inspectorIds, string baseName, string planName)
        {
            InspectorIds = inspectorIds;
            InspectorNames = inspectorNames;
            BaseName = baseName;
            PlanName = planName;
        }
    }
}
