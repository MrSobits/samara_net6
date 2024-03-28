namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using B4;
    using Domain.Decisions;
    using Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид Решение об установлении сроков проведения работ по КР
    /// </summary>
    public class AccumulationTransferDecisionType : AbstractDecisionType
    {
        public AccumulationTransferDecisionType(string name, string js, bool isDefault = false) : base(name, js, isDefault)
        {
        }

        public override string Code
        {
            get { return "AccumulationTransfer"; }
        }

        public override IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams)
        {
           baseDecision.JsonObject = JsonConvert.SerializeObject(new
            {
                baseDecision.StartDate,
                baseDecision.File,
                DecisionCode = baseParams.Params.GetAs<string>("DecisionName"),

                Sum = baseParams.Params.GetAs<decimal>("Sum")
            });

           return new BaseDataResult();
        }

        public override IDataResult GetValue(GenericDecision baseDecision)
        {
            throw new System.NotImplementedException();
        }
    }
}