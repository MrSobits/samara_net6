namespace Bars.Gkh.Overhaul.Tat.Navigation
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
            root.Add("Сведения о кап.ремонте").Add("Долгосрочная программа кап. ремонта");
            root.Add("Сведения о кап.ремонте").Add("Выполнение кап. ремонта");
            root.Add("Протоколы собственников помещений", "B4.controller.longtermprobject.PropertyOwnerProtocols");
            root.Add("Решения собственников", "B4.controller.longtermprobject.PropertyOwnerDecision");

            root.Add("Счета учета средств");
            root.Add("Счета учета средств").Add("Специальный счет", "B4.controller.longtermprobject.SpecialAccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.Special.View");          
        }
    }
}
