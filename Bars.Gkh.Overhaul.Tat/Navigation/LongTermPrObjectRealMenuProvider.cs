namespace Bars.Gkh.Overhaul.Tat.Navigation
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
            root.Add("Сведения о кап.ремонте").Add("Долгосрочная программа кап. ремонта").AddRequiredPermission("Ovrhl.LongTermProgramObject.InfoCr.LongTermProgram.View");
            root.Add("Сведения о кап.ремонте").Add("Выполнение кап. ремонта").AddRequiredPermission("Ovrhl.LongTermProgramObject.InfoCr.ExecutionLongTermProgram.View");
            root.Add("Протоколы собственников помещений", "B4.controller.longtermprobject.PropertyOwnerProtocols");
            root.Add("Решения собственников", "B4.controller.longtermprobject.PropertyOwnerDecision");
            root.Add("Учет займов").AddRequiredPermission("Ovrhl.LongTermProgramObject.AccountLoan.View");

            //root.Add("Счета учета средств").Add("Счет у регионального оператора");
            //root.Add("Счета учета средств").Add("Счет у регионального оператора").Add("Счет начислений", "B4.controller.longtermprobject.AccrualsAccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.Accurals.View");
            //root.Add("Счета учета средств").Add("Счет у регионального оператора").Add("Реальный счет", "B4.controller.longtermprobject.RealAccount").AddRequiredPermission("Ovrhl.LongTermProgramObject.RealAccount.View");
        }
    }
}