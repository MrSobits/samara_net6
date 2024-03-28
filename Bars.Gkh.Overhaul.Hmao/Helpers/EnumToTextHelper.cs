using Bars.Gkh.Enums;
using Bars.Gkh.Overhaul.Hmao.Enum;

namespace Bars.Gkh.Overhaul.Hmao.Helpers
{
    public static class EnumToTextHelper
    {
        public static string TypeHouseToString(TypeHouse typeHouse)
        {
            switch (typeHouse)
            {
                case TypeHouse.BlockedBuilding:
                    return "Блокированной застройки";
                case TypeHouse.Individual:
                    return "Индивидуальный";
                case TypeHouse.ManyApartments:
                    return "Многоквартирный";
                case TypeHouse.NotSet:
                    return "Не задано";
                case TypeHouse.SocialBehavior:
                    return "Общежитие";
                default:
                    return typeHouse.ToString();
            }
        }

        public static string ConditionHouseToString(ConditionHouse conditionHouse)
        {
            switch (conditionHouse)
            {
                case ConditionHouse.Dilapidated:
                    return "Ветхий";
                case ConditionHouse.Emergency:
                    return "Аварийный";
                case ConditionHouse.NotSelected:
                    return "Не выбрано";
                case ConditionHouse.Razed:
                    return "Снесен";
                case ConditionHouse.Serviceable:
                    return "Исправный";
                default:
                    return conditionHouse.ToString();
            }
        }

        public static string ConditionToString(Condition condition)
        {
            switch (condition)
            {
                case Condition.Equal:
                    return "равно";
                case Condition.Greater:
                    return "больше";
                case Condition.Lower:
                    return "меньше";
                default:
                    return condition.ToString();
            }
        }

        public static string BoolToString(bool value)
        {
            switch (value)
            {
                case true:
                    return "да";
                case false:
                    return "нет";
                default:
                    return value.ToString();
            }
        }
        public static string EconFeasibilityToStrinng(EconFeasibilityResult result)
        {
            if (result == EconFeasibilityResult.Economical) { return "Целесообразно"; } else { return "Не целесообразно"; }            
        }
    }
}
