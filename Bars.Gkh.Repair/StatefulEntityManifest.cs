namespace Bars.Gkh.Repair
{
    using System.Collections.Generic;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Repair.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new StatefulEntityInfo[]
            {
                new StatefulEntityInfo("repair_object", "Объект текущего ремонта", typeof(RepairObject))
            };
        }
    }
}