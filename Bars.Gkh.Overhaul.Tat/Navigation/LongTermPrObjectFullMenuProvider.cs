﻿namespace Bars.Gkh.Overhaul.Tat.Navigation
{
    using Bars.B4;

    public class LongTermPrObjectFullMenuKey
    {
        public static string Key
        {
            get { return "LongTermPrObjectFull"; }
        }

        public static string Description
        {
            get
            {
                return "Меню объекта долгосрочной программы";
            }
        }
    }

    public class LongTermPrObjectFullMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return LongTermPrObjectFullMenuKey.Key;
            }
        }

        public string Description
        {
            get
            {
                return LongTermPrObjectFullMenuKey.Description;
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

            //var account = root.Add("Счета учета средств");
            //account.Add("Специальный счет", "B4.controller.longtermprobject.SpecialAccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.Special.View");
            //account.Add("Счет у регионального оператора").Add("Счет начислений", "B4.controller.longtermprobject.AccrualsAccount").AddRequiredPermission("Gkh.RealityObject.Register.Accounts.Accurals.View");
            //account.Add("Счет у регионального оператора").Add("Реальный счет", "B4.controller.longtermprobject.RealAccount").AddRequiredPermission("Ovrhl.LongTermProgramObject.RealAccount.View");
        }
    }
}