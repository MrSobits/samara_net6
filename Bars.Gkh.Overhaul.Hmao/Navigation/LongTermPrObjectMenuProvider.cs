namespace Bars.Gkh.Overhaul.Hmao.Navigation
{
      using Bars.B4;

    public class LongTermPrObjectMenuKey
    {
        public static string Key
        {
            get { return "LongTermPrObject"; }
        }

        public static string Description
        {
            get
            {
                return "Меню объекта долгосрочной программы";
            }
        }
    }

    public class LongTermPrObjectMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return LongTermPrObjectMenuKey.Key;
            }
        }

        public string Description
        {
            get
            {
                return LongTermPrObjectMenuKey.Description;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.longtermprobject.Edit").WithIcon("icon-outline");
            root.Add("Сведения о кап.ремонте").Add("Долгосрочная программа кап. ремонта");
            root.Add("Сведения о кап.ремонте").Add("Выполнение кап. ремонта");
            /*root.Add("Протоколы собственников помещений", "B4.controller.longtermprobject.PropertyOwnerProtocols");
            root.Add("Решения собственников", "B4.controller.longtermprobject.PropertyOwnerDecision");*/
        }
    }
}
