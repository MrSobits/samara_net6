namespace Bars.GkhCr.Regions.Tatarstan
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        /// <inheritdoc />
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("object_outdoor_cr", "Объект программы благоустройства дворов", typeof(ObjectOutdoorCr))
            };
        }
    }
}
