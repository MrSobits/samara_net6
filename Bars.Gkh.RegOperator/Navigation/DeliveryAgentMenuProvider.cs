namespace Bars.Gkh.RegOperator.Navigation
{
    using Bars.B4;

    public class DeliveryAgentMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "DeliveryAgent";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки агента доставки";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "deliveryagentedit/{0}/edit");
            root.Add("Муниципальные образования", "deliveryagentedit/{0}/municipality").AddRequiredPermission("Gkh.Orgs.DeliveryAgent.Municipality.View");
            root.Add("Жилые дома", "deliveryagentedit/{0}/realobj").AddRequiredPermission("Gkh.Orgs.DeliveryAgent.RealityObject.View");
        }
    }
}