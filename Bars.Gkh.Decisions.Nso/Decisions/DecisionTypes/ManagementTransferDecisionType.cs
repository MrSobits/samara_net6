namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using B4;

    using Bars.Gkh.Decisions.Nso.Entities;

    using Domain.Decisions;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид Решение о передаче управления
    /// </summary>
    public class ManagementTransferDecisionType : AbstractDecisionType
    {
        public ManagementTransferDecisionType(string name, string js, bool isDefault = false) : base(name, js, isDefault)
        {
        }

        public override string Code
        {
            get { return "ManageTransfer"; }
        }

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