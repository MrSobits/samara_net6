namespace Bars.Gkh.Overhaul.Tat
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("ovrhl_short_prog_object", "Дом краткосрочной программы", typeof(ShortProgramRealityObject)),
                new StatefulEntityInfo("ovrhl_short_prog_defect_list", "Дефектная ведомость краткосрочной программы", typeof(ShortProgramDefectList)),
                new StatefulEntityInfo("ovrhl_program_version", "Версия программы ДПКР", typeof(ProgramVersion)),
                new StatefulEntityInfo("ovrhl_decision_notice", "Уведомления решения собственников", typeof(SpecialAccountDecisionNotice)),
                new StatefulEntityInfo("ovrhl_bank_statement", "Банковские выписки счетов", typeof(AccBankStatement))
            };
        }
    }
}