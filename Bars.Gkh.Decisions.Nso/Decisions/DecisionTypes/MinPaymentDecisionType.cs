namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using B4;
    using Domain.Decisions;
    using Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид Решение о минимальном размере фонда КР
    /// </summary>
    public class MinPaymentDecisionType : AbstractDecisionType
    {
        public MinPaymentDecisionType(string name, string js, bool isDefault = false) : base(name, js, isDefault)
        {
        }

        public override string Code
        {
            get { return "MinPayment"; }
        }

        public override IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams)
        {
            baseDecision.JsonObject = JsonConvert.SerializeObject(new
            {
                baseDecision.StartDate,
                baseDecision.File,
                DecisionCode = baseParams.Params.GetAs<string>("DecisionName"),

                Norm = baseParams.Params.GetAs<string>("Norm"),
                Decision = baseParams.Params.GetAs<string>("Decision")
            });

            return new BaseDataResult();
        }

        public override IDataResult GetValue(GenericDecision baseDecision)
        {
            throw new System.NotImplementedException();
        }
    }
}