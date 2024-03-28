namespace Bars.Gkh.Overhaul.Nso
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("ovrhl_published_program", "Опубликованная программа", typeof(PublishedProgram)),
                new StatefulEntityInfo("ovrhl_decision_notice", "Уведомления решения собственников", typeof(SpecialAccountDecisionNotice)),
                new StatefulEntityInfo("ovrhl_bank_statement", "Банковские выписки счетов", typeof(AccBankStatement))
            };
        }
    }
}