namespace Bars.Gkh.Overhaul.Hmao
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("ovrhl_published_program", "Опубликованная программа", typeof(PublishedProgram)),
                 new StatefulEntityInfo("ovrhl_program_version", "Версия программы ДПКР", typeof(ProgramVersion)),
                new StatefulEntityInfo("ovrhl_decision_notice", "Уведомления решения собственников", typeof(SpecialAccountDecisionNotice)),
                new StatefulEntityInfo("ovrhl_dpkr_documents", "Документы ДКПР", typeof(DpkrDocument))
            };
        }
    }
}