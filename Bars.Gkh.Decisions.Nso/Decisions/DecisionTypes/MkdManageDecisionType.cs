namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using System;
    using B4;
    using Entities;
    using Domain.Decisions;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид решения о выборе способа управления МКД
    /// </summary>
    public class MkdManageDecisionType : AbstractDecisionType
    {
        public MkdManageDecisionType(string name, string js, bool isDefault = false)
            : base(name, js, isDefault) { }

        public override sealed string Name { get; set; }

        public override string Code { get { return "MkdManageDecisionType"; } }

        public override sealed string Js { get; set; }

        public override sealed bool IsDefault { get; set; }

        public override IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams)
        {
            baseDecision.JsonObject = JsonConvert.SerializeObject(new
            {
                baseDecision.StartDate,
                baseDecision.File,
                DecisionCode = baseParams.Params.GetAs<string>("DecisionName"),
                
                EndDate = baseParams.Params.GetAs<DateTime>("EndDate"),
                ManOrg = baseParams.Params.GetAs<string>("ManOrg"),
                AuthorizedName = baseParams.Params.GetAs<string>("AuthorizedName")
            });
            
            return new BaseDataResult();
        }

        public override IDataResult GetValue(GenericDecision baseDecision)
        {
            throw new NotImplementedException();
        }
    }
}