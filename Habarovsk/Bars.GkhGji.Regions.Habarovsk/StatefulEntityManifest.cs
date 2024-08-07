﻿namespace Bars.GkhGji.Regions.Habarovsk
{
    using Bars.B4.Modules.States;
    using Entities;
    using System.Collections.Generic;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gji_romcategory", "ГЖИ - Расчет коэффициента риска", typeof(ROMCategory)),
                new StatefulEntityInfo("oss_request", "Запрос протокола ОСС", typeof(ProtocolOSPRequest)),
                new StatefulEntityInfo("gkh_manorg_license_reissuance", "Обращение за переоформлением лицензии", typeof(LicenseReissuance)),
                new StatefulEntityInfo("courtpractice", "ГЖИ - Административная практика", typeof(CourtPractice))
            };
        }
    }
}