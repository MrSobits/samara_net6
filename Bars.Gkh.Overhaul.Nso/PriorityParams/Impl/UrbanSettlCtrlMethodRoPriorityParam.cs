namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using Entities;
    using Enums;

    public class UrbanSettlCtrlMethodRoPriorityParam : BaseCtrlMethodRoPriorityParam
    {
        public override string Id
        {
            get { return "UrbanSettlCtrlMethodRo"; }
        }

        public override string Name
        {
            get { return "Выбранный способ управления МКД(городские поселения)"; }
        }

        public override object GetValue(IStage3Entity obj)
        {
            if (obj.RealityObject.MoSettlement != null)
            {
                var level = obj.RealityObject.MoSettlement.Level;

                if (level == TypeMunicipality.UrbanSettlement || level == TypeMunicipality.UrbanArea)
                {
                    return base.GetValue(obj);
                }
            }
            else
            {
                var placeName = obj.RealityObject.FiasAddress.PlaceName ?? string.Empty;

                if (placeName.StartsWith("г.") || placeName.StartsWith("пгт."))
                {
                    return base.GetValue(obj);
                }
            }

            return 0;
        }
    }
}