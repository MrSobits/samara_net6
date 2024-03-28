namespace Bars.GkhDi
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.GkhDi.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("di_disinfo", "Раскрытие информации", typeof(DisclosureInfo))
            };
        }
    }
}