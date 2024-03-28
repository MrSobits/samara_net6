namespace Bars.Gkh.Overhaul.Hmao.Navigation
{
    using B4;

    public class LongTermPrObjectRealMenuKey
    {
        public static string Key
        {
            get { return "LongTermPrObjectReal"; }
        }

        public static string Description
        {
            get
            {
                return "Меню объекта долгосрочной программы";
            }
        }
    }

    public class LongTermPrObjectRealMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return LongTermPrObjectRealMenuKey.Key;
            }
        }

        public string Description
        {
            get
            {
                return LongTermPrObjectRealMenuKey.Description;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.longtermprobject.Edit").WithIcon("icon-outline");
            root.Add("Сведения о кап.ремонте").Add("Долгосрочная программа кап. ремонта");
            root.Add("Сведения о кап.ремонте").Add("Выполнение кап. ремонта");
            /*root.Add("Протоколы собственников помещений", "B4.controller.longtermprobject.PropertyOwnerProtocols");
            root.Add("Решения собственников", "B4.controller.longtermprobject.PropertyOwnerDecision");*/
            root.Add("Учет займов", "B4.controller.longtermprobject.Loan").AddRequiredPermission("Ovrhl.LongTermProgramObject.Loan.View");

            /*var account = root.Add("Счета учета средств").Add("Счет у регионального оператора");
            account.Add("Счет начислений", "B4.controller.longtermprobject.AccrualsAccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.Accruals.View");
            account.Add("Реальный счет", "B4.controller.longtermprobject.RealAccount");*/
        }
    }
}