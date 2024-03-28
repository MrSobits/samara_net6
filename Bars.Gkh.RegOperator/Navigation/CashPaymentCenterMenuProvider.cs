namespace Bars.Gkh.RegOperator.Navigation
{
    using Bars.B4;

    public class CashPaymentCenterMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "CashPaymentCenter";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки расчетно-кассвого центра";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "cashpaymentcenteredit/{0}/edit");
            root.Add("Муниципальные образования", "cashpaymentcenteredit/{0}/municipality")
                .AddRequiredPermission("Gkh.Orgs.CashPaymentCenter.Municipality.View");
            root.Add("Объекты", "cashpaymentcenteredit/{0}/realobj")
                .AddRequiredPermission("Gkh.Orgs.CashPaymentCenter.RealityObject.View");
            root.Add("Обслуживаемые УК", "cashpaymentcenteredit/{0}/manorg")
                .AddRequiredPermission("Gkh.Orgs.CashPaymentCenter.ManOrg.View");
        }
    }
}