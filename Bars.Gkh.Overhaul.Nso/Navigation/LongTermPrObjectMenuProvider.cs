namespace Bars.Gkh.Overhaul.Nso.Navigation
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
            root.Add("Сведения о кап.ремонте")
                .Add("Региональная программа кап. ремонта", "B4.controller.longtermprobject.LongTermProgram")
                .AddRequiredPermission("Ovrhl.LongTermProgramObject.InfoCr.LongTermProgram");
            root.Add("Сведения о кап.ремонте")
                .Add("Выполнение кап. ремонта", "B4.controller.longtermprobject.ExecutionLongTermProgram")
                .AddRequiredPermission("Ovrhl.LongTermProgramObject.InfoCr.ExecutionLongTermProgram");

            //root.Add("Протоколы собственников помещений", "B4.controller.longtermprobject.PropertyOwnerProtocols");
            //root.Add("Решения собственников", "B4.controller.longtermprobject.PropertyOwnerDecision");
            root.Add("Фонд капитального ремонта", "B4.controller.longtermprobject.OverhaulFund");
        }
    }
}
