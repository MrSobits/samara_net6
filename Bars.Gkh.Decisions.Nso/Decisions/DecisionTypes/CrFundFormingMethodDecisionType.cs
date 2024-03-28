namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using B4;
    using Domain.Decisions;
    using Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид решения о способе формирования фонда
    /// </summary>
    public class CrFundFormingMethodDecisionType : AbstractDecisionType
    {
        public CrFundFormingMethodDecisionType(string name, string js, bool isDefault = false)
            : base(name, js, isDefault) { }

        public override string Name { get; set; }

        public override string Code
        {
            get
            {
                return "CrFundFormingMethod";
            }
        }

        public override string Js { get; set; }

        public override bool IsDefault { get; set; }

        public override IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams)
        {
            baseDecision.JsonObject = JsonConvert.SerializeObject(new
            {
                baseDecision.StartDate,
                baseDecision.File,
                DecisionCode = baseParams.Params.GetAs<string>("DecisionName")
            });

            return new BaseDataResult(true);
        }

        public override IDataResult GetValue(GenericDecision baseDecision)
        {
            throw new System.NotImplementedException();
        }
    }
}