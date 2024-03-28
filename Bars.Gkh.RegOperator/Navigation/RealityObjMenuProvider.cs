namespace Bars.Gkh.RegOperator
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
            root.Add("Агенты доставки", "realityobjectedit/{0}/deliveryagent").AddRequiredPermission("Gkh.RealityObject.Register.DeliveryAgent.View");
            //root.Add("Расчетно-кассовые центры", "realityobjectedit/{0}/cashpaymentcenter").AddRequiredPermission("Gkh.RealityObject.Register.CashPaymentCenter.View");

            root.Add("Сведения о помещениях", "realityobjectedit/{0}/room").AddRequiredPermission("Gkh.RealityObject.Register.HouseInfo.View").WithIcon("icon-neighbourhood");

            root.Add("Счета учета средств").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.View");
            root.Add("Счета учета средств").Add("Счет начислений", "realityobjectedit/{0}/realtychargeaccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.View");
            root.Add("Счета учета средств").Add("Счет оплат", "realityobjectedit/{0}/realtypaymentaccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.View");
            root.Add("Счета учета средств").Add("Счет расчета с поставщиками", "realityobjectedit/{0}/realtysupplieraccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.View");
            root.Add("Счета учета средств").Add("Счет субсидий", "realityobjectedit/{0}/realtysubsidyaccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.SubsidyAccount.View");

            root.Add("Займы", "realityobjectedit/{0}/loan").AddRequiredPermission("Gkh.RealityObject.Register.Loan.View");
        }
    }
}
