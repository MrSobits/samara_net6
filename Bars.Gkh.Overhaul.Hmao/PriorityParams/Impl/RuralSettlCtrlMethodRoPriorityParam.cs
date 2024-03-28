namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using Entities;
    using Enums;

    public class RuralSettlCtrlMethodRoPriorityParam : BaseCtrlMethodRoPriorityParam
    {
        public override string Id
        {
            get { return "RuralSettlCtrlMethodRo"; }
        }

        public override string Name
        {
            get { return "Выбранный способ управления МКД(сельские поселения)"; }
        }

        public override object GetValue(IStage3Entity obj)
        {
            if (obj.RealityObject.MoSettlement != null)
            {
                var level = obj.RealityObject.MoSettlement.Level;

                if (level == TypeMunicipality.Settlement)
                {
                    return base.GetValue(obj);
                }
            }
            else
            {
                var placeName = obj.RealityObject.FiasAddress.PlaceName ?? string.Empty;

                if (!placeName.StartsWith("г.") && !placeName.StartsWith("пгт."))
                {
                    return base.GetValue(obj);
                }
            }

            return 0;
        }
    }
}