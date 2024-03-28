namespace Bars.Gkh.Overhaul.Nso.Navigation
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
            root.Add("Сведения о кап.ремонте")
                .Add("Региональная программа кап. ремонта", "B4.controller.longtermprobject.LongTermProgram")
                .AddRequiredPermission("Ovrhl.LongTermProgramObject.InfoCr.LongTermProgram");
            root.Add("Сведения о кап.ремонте")
                .Add("Выполнение кап. ремонта", "B4.controller.longtermprobject.ExecutionLongTermProgram")
                .AddRequiredPermission("Ovrhl.LongTermProgramObject.InfoCr.ExecutionLongTermProgram");

            //root.Add("Протоколы собственников помещений", "B4.controller.longtermprobject.PropertyOwnerProtocols");
            //root.Add("Решения собственников", "B4.controller.longtermprobject.PropertyOwnerDecision");
            root.Add("Учет займов", "B4.controller.longtermprobject.Loan").AddRequiredPermission("Ovrhl.LongTermProgramObject.Loan.View");

            /*var account = root.Add("Счета учета средств").Add("Счет у регионального оператора");
            account.Add("Счет начислений", "B4.controller.longtermprobject.AccrualsAccount")
                .AddRequiredPermission("Gkh.RealityObject.Register.Accounts.Accruals.View");
            account.Add("Счета оплат", "B4.controller.longtermprobject.PaymentAccount");*/

            root.Add("Показатели сбора взносов на КР", "B4.controller.longtermprobject.ContributionCollection");
            root.Add("Фонд капитального ремонта", "B4.controller.longtermprobject.OverhaulFund");
        }
    }
}