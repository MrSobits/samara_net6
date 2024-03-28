namespace Bars.Gkh
{
    using System.Collections.Generic;

    using B4.Modules.States;

    using Entities;
    using Entities.Suggestion;
    using Modules.ClaimWork.Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gkh_emergency_object", "Аварийный объект", typeof(EmergencyObject)),
                new StatefulEntityInfo("gkh_real_obj", "Жилой дом", typeof(RealityObject)),
                new StatefulEntityInfo("gkh_citizen_suggestion", "Обращение граждан (устаревший реестр)", typeof(CitizenSuggestion)),
                new StatefulEntityInfo("gkh_manorg_license_request", "Заявка на получение лицензии", typeof(ManOrgLicenseRequest)),
                new StatefulEntityInfo("gkh_manorg_license", "Лицензия УО", typeof(ManOrgLicense)),
                new StatefulEntityInfo("gkh_person_request_exam", "Заявка на допуск к экзамену", typeof(PersonRequestToExam)),
                new StatefulEntityInfo("clw_claim_work", "Претензионная работа", typeof(BaseClaimWork)),
                new StatefulEntityInfo("gkh_person", "Должностное лицо", typeof(Person)),
                new StatefulEntityInfo("gkh_person_qc", "Квалификационный аттестат", typeof(PersonQualificationCertificate))
            };
        }
    }
}