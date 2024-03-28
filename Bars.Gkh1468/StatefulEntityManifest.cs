namespace Bars.Gkh1468
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Entities.Passport;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
                       {
                           //new StatefulEntityInfo("okipassport", "Реестр паспортов ОКИ", typeof(OkiPassport)),
                           new StatefulEntityInfo("okiproviderpassport", "Мои паспорта ОКИ", typeof(OkiProviderPassport)),
                           //new StatefulEntityInfo("housepassport", "Реестр паспортов дома", typeof(HousePassport)),
                           new StatefulEntityInfo("houseproviderpassport", "Мои паспорта домов", typeof(HouseProviderPassport))
                       };
        }
    }
}