namespace Bars.Gkh.Overhaul.Nso.Navigation
{
    using Bars.B4;

    public class LongTermPrObjectSpecialMenuKey
    {
        public static string Key
        {
            get { return "LongTermPrObjectSpecial"; }
        }

        public static string Description
        {
            get
            {
                return "Меню объекта долгосрочной программы";
            }
        }
    }

    public class LongTermPrObjectSpecialMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return LongTermPrObjectSpecialMenuKey.Key;
            }
        }

        public string Description
        {
            get
            {
                return LongTermPrObjectSpecialMenuKey.Description;
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

            //root.Add("Счета учета средств");
            //root.Add("Счета учета средств").Add("Специальный счет", "B4.controller.longtermprobject.SpecialAccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.Special.View");
            root.Add("Фонд капитального ремонта", "B4.controller.longtermprobject.OverhaulFund");
        }
    }
}
