namespace Bars.Gkh.ClaimWork
{
    using System.Collections.Generic;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.B4.Modules.States;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("clw_document_lawsuit", "Документ ПИР - Исковое работа", typeof(Lawsuit)),
                new StatefulEntityInfo("clw_document_actviol", "Документ ПИР - Акт выявления нарушений", typeof(ActViolIdentificationClw)),
                new StatefulEntityInfo("clw_document_notification", "Документ ПИР - Уведомление", typeof(NotificationClw)),
                new StatefulEntityInfo("clw_document_pretension", "Документ ПИР - Претензия", typeof(PretensionClw))
            };
        }
    }
}