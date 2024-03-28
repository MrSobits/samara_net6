namespace Bars.Esia.OAuth20.App.Entities
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Enums;
    using Bars.Esia.OAuth20.App.Extensions;

    using Newtonsoft.Json.Linq;

    public class BaseEsiaOrganizationInfo
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Тип организации
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string Ogrn { get; set; }

        public BaseEsiaOrganizationInfo()
        {
        }

        public BaseEsiaOrganizationInfo(JObject organizationInfo)
        {
            this.Id = organizationInfo.GetPropertyValue("oid");
            this.ShortName = organizationInfo.GetPropertyValue("shortName");
            this.FullName = organizationInfo.GetPropertyValue("fullName");
            this.Ogrn = organizationInfo.GetPropertyValue("ogrn");

            var orgTypeDict = new Dictionary<string, OrganizationType>
            {
                { "AGENCY", OrganizationType.Agency },
                { "LEGAL", OrganizationType.Legal },
                { "BUSINESS", OrganizationType.Business }
            };

            this.OrganizationType = orgTypeDict.Get(organizationInfo.GetPropertyValue("type"));
        }
    }
}