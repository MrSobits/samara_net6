namespace Bars.GkhGji.Regions.Zabaykalye
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities.AppealCits;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gji_appcits_executant", "Обращение граждан - Исполнитель", typeof(AppealCitsExecutant)),
            };
        }
    }
}