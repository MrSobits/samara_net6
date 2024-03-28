namespace Bars.Gkh.Overhaul
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("ovrhl_ro_struct_el", "Конструктивные характеристики жилого дома", typeof(RealityObjectStructuralElement))
            };
        }
    }
}