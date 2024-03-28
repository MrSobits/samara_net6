namespace Bars.GkhGji.Regions.Archangelsk
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.GkhGji.Regions.Archangelsk.Entities;

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