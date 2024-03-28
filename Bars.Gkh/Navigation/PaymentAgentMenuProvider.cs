namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class PaymentAgentMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "PaymentAgent";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки платежного агента";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.paymentagent.Edit").AddRequiredPermission("Gkh.Orgs.PaymentAgent.View").WithIcon("icon-shield-rainbow");
        }
    }
}
