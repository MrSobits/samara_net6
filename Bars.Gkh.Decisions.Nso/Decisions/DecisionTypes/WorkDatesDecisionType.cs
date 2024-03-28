namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using B4;
    using Entities;
    using Domain.Decisions;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид Решение о минимальном размере фонда КР
    /// </summary>
    public class WorkDatesDecisionType : AbstractDecisionType
    {
        public WorkDatesDecisionType(string name, string js, bool isDefault = false) : base(name, js, isDefault)
        {
        }

        public override string Code
        {
            get { return "WorkDates"; }
        }

        public override IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams)
        {
            baseDecision.JsonObject = JsonConvert.SerializeObject(new
            {
                baseDecision.StartDate,
                baseDecision.File,
                DecisionCode = baseParams.Params.GetAs<string>("DecisionName"),

                Year = baseParams.Params.GetAs<string>("Year"),
                Work = baseParams.Params.GetAs<string>("Work")
            });

            return new BaseDataResult();
        }

        public override IDataResult GetValue(GenericDecision baseDecision)
        {
            throw new System.NotImplementedException();
        }
    }
}