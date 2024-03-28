namespace Bars.GkhGji.Regions.Nso
{
    using System.Collections.Generic;
	using Bars.B4.Modules.States;
    using Bars.GkhGji.Regions.Nso.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gji_appcits_executant", "Обращение граждан - Исполнитель", typeof(AppealCitsExecutant)),
				new StatefulEntityInfo("gji_mkd_change_notification", "Уведомление о смене способа управления МКД", typeof (MkdChangeNotification)),
				new StatefulEntityInfo("gji_document_protocol197", "Документы ГЖИ - Протокол по ст.19.7 КоАП РФ", typeof(Protocol197))
            };
        }
    }
}