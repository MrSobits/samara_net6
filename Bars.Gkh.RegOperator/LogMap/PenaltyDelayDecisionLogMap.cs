namespace Bars.Gkh.RegOperator.LogMap
{
    using System;
    using Decisions.Nso.Entities;

    class PenaltyDelayDecisionLogMap : UltimateDecisionLogMap<PenaltyDelayDecision>
    {
        public PenaltyDelayDecisionLogMap()
        {
            Name("Решение о сроке уплаты взносов");

            MapProperty(x => x.Decision, "Decision", "Решение", x =>
            {
                if (x == null || x.Count == 0)
                {
                    return "Nothing";
                }
                else
                {
                    string result = "";
                    foreach (var des in x)
                    {
                        result += "С " + String.Format("{0:dd.MM.yyyy}", des.From)
                                 + " по " + String.Format("{0:dd.MM.yyyy}", des.To)
                                 + " кол-во дней просрочки= " + des.DaysDelay.ToString()
                                 + " допустимая просрочка месяц= " + des.MonthDelay.ToString() + ";";
                    }
                    return result;
                }
            });
        }
    }
}