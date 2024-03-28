namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using B4;
    using Domain.Decisions;
    using Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид Решение о выборе владельца счета
    /// </summary>
    public class AccountOwnerDecisionType : AbstractDecisionType
    {
        public AccountOwnerDecisionType(string name, string js, bool isDefault = false) : base(name, js, isDefault)
        {
        }

        public override string Code
        {
            get { return "AccountOwner"; }
        }

        public override IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams)
        {
            baseDecision.JsonObject = JsonConvert.SerializeObject(new
            {
                baseDecision.StartDate,
                baseDecision.File,
                DecisionCode = baseParams.Params.GetAs<string>("DecisionName"),

                OwnerName = baseParams.Params.GetAs<string>("OwnerName")
            });

            return new BaseDataResult(true);
        }

        public override IDataResult GetValue(GenericDecision baseDecision)
        {
            throw new System.NotImplementedException();
        }
    }
}