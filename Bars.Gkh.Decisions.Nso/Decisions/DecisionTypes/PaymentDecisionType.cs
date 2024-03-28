namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using B4;
    using Entities;
    using Domain.Decisions;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид Решение о размере взноса на КР
    /// </summary>
    public class PaymentDecisionType : AbstractDecisionType
    {
        public PaymentDecisionType(string name, string js, bool isDefault = false) : base(name, js, isDefault)
        {
        }

        public override string Code
        {
            get { return "Payment"; }
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