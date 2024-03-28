namespace Bars.Gkh.Regions.Perm.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Сервис для возможности формирования "Основание проверки соискателей лицензии" для Перьми
    /// </summary>
    public class ManorgLicenceApplicantsProvider : IManorgLicenceApplicantsProvider
    {
        private readonly LicenseRequestType[] hasComplianceWithRequirements =
        {
            LicenseRequestType.GrantingLicense,
            LicenseRequestType.RenewalLicense,
            LicenseRequestType.TerminationActivities
        };

        public IDomainService<ManOrgLicenseRequest> ManOrgLicenseRequestDomain { get; set; }

        public IDomainService<BaseLicenseApplicants> BaseLicenseApplicantsDomain { get; set; }

        /// <inheritdoc />
        public IEnumerable<ManorgLicenceApplicantsRule> GetRules(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("requestId");
            var request = this.ManOrgLicenseRequestDomain.Get(id);

            if (request.IsNull())
            {
                return Enumerable.Empty<ManorgLicenceApplicantsRule>();
            }

            var result = new Dictionary<string, ManorgLicenceApplicantsRule>();

            if (request.Type.HasValue)
            {
                this.AddComplianceWithRequirements(result, request);
                this.AddMatchInformation(result, request);
            } 

            return result.Values;
        }

        private void AddComplianceWithRequirements(IDictionary<string, ManorgLicenceApplicantsRule> dict, ManOrgLicenseRequest request)
        {
            if (!this.hasComplianceWithRequirements.Contains(request.Type.Value))
            {
                return;
            }

            if (dict.ContainsKey("ComplianceWithRequirements"))
            {
                return;
            }

            if (this.BaseLicenseApplicantsDomain.GetAll()
                .Any(x => x.InspectionType == InspectionGjiType.ComplianceWithRequirements && x.ManOrgLicenseRequest == request))
            {
                return;
            }

            dict.Add("ComplianceWithRequirements", new ManorgLicenceApplicantsRule
            {
                Text = "Соблюдение обязательных требований",
                ExtraParams = new Dictionary<string, object>
                {
                    { "InspectionType", InspectionGjiType.ComplianceWithRequirements }
                }
            });
        }

        private void AddMatchInformation(IDictionary<string, ManorgLicenceApplicantsRule> dict, ManOrgLicenseRequest request)
        {
            if (request.Type != LicenseRequestType.GrantingLicense)
            {
                return;
            }

            if (dict.ContainsKey("MatchInformation"))
            {
                return;
            }

            if (this.BaseLicenseApplicantsDomain.GetAll()
                .Any(x => x.InspectionType == InspectionGjiType.MatchInformation && x.ManOrgLicenseRequest == request))
            {
                return;
            }

            dict.Add("MatchInformation", new ManorgLicenceApplicantsRule
            {
                Text = "Соответствие сведений",
                ExtraParams = new Dictionary<string, object>
                {
                    { "InspectionType", InspectionGjiType.MatchInformation }
                }
            });
        }
    }
}