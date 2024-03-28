namespace Bars.GkhGji.Regions.Tomsk
{
    using System.Collections.Generic;
    using B4.Modules.States;
    using Entities;
	using Bars.GkhGji.Regions.Tomsk.Entities.AppealCits;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gji_document_actvisual", "Документы ГЖИ - Акт визуального осмотра", typeof (ActVisual)),
                new StatefulEntityInfo("gji_document_admincase", "Документы ГЖИ - Административное дело", typeof(AdministrativeCase)),
                new StatefulEntityInfo("gji_requirement", "Документы ГЖИ - Требование", typeof (Requirement)),
				new StatefulEntityInfo("gji_appcits_executant", "Обращение граждан - Исполнитель", typeof(AppealCitsExecutant))
            };
        }
    }
}