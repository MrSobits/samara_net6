namespace Bars.Gkh.Decisions.Nso.Decisions.DecisionTypes
{
    using System;
    using B4;
    using B4.Utils;
    using Domain.Decisions;
    using Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид Решение о выборе кредитной организации
    /// </summary>
    public class CreditOrgChoosingDecisionType : AbstractDecisionType
    {
        public CreditOrgChoosingDecisionType(string name, string js, bool isDefault = false) : base(name, js, isDefault)
        {
        }

        public override string Code
        {
            get { return "CreditOrgChoosing"; }
        }

        public override IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams)
        {
            var creditOrgName = baseParams.Params.Get("CreditOrgName", string.Empty);
            var creditOrgAddress = baseParams.Params["CreditOrgAddress"];
            var creditOrgInn = baseParams.Params["CreditOrgInn"];
            var creditOrgKpp = baseParams.Params["CreditOrgKpp"];
            var creditOrgOgrn = baseParams.Params["CreditOrgOgrn"];
            var creditOrgOkato = baseParams.Params["CreditOrgOkato"];
            var creditOrgBik = baseParams.Params["CreditOrgBik"];
            var creditOrgOkpo = baseParams.Params["CreditOrgOkpo"];
            var creditOrgAcc = baseParams.Params["CreditOrgAcc"];
            var creditOrgCorAcc = baseParams.Params["CreditOrgCorAcc"];

            baseDecision.JsonObject = JsonConvert.SerializeObject(new
            {
                baseDecision.StartDate,
                baseDecision.File,
                DecisionCode = baseParams.Params.GetAs<string>("DecisionName"),

                AccountNum = baseParams.Params["AccountNum"],
                AccountCreateDate = baseParams.Params.GetAs<DateTime>("AccountCreateDate"),
                CreditOrgName = creditOrgName,
                CreditOrgAddress = creditOrgAddress,
                CreditOrgInn = creditOrgInn,
                CreditOrgKpp = creditOrgKpp,
                CreditOrgOgrn = creditOrgOgrn,
                CreditOrgOkato = creditOrgOkato,
                CreditOrgBik = creditOrgBik,
                CreditOrgOkpo = creditOrgOkpo,
                CreditOrgAcc = creditOrgAcc,
                CreditOrgCorAcc = creditOrgCorAcc
            });

            return new BaseDataResult();
        }

        public override IDataResult GetValue(GenericDecision baseDecision)
        {
            throw new System.NotImplementedException();
        }
    }
}