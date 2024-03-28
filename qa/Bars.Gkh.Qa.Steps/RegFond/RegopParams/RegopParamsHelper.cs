namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class RegopParamsHelper : BindingBase
    {
        public static string GetInternalName(string paramExternalName)
        {
            switch (paramExternalName)
            {
                case "Счет регионального оператора":
                    {
                        return "RegopCalcAccount";
                    }

                case "Счёт регионального оператора":
                    {
                        return "RegopCalcAccount";
                    }

                case "Специальный счет регионального оператора":
                    {
                        return "RegopSpecialCalcAccount";
                    }

                case "Специальный счет":
                    {
                        return "SpecialCalcAccount";
                    }

                case "Не выбран":
                    {
                        return "Unknown";
                    }

                default:
                    {
                        throw new SpecFlowException(
                            string.Format("Нет параметра с наименованием {0}", paramExternalName));
                   }
            }
        }
    }
}
