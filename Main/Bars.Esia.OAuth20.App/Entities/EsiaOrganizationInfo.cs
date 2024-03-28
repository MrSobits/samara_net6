namespace Bars.Esia.OAuth20.App.Entities
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Enums;
    using Bars.Esia.OAuth20.App.Extensions;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Информация об организации в ЕСИА
    /// </summary>
    public class EsiaOrganizationInfo : BaseEsiaOrganizationInfo
    {
        /// <summary>
        /// ИНН
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public string Kpp { get; set; }

        /// <summary>
        /// Код организационно-правовой формы
        /// по общероссийскому классификатору
        /// </summary>
        public string Leg { get; set; }

        /// <summary>
        /// Территориальная принадлежность ОГВ
        /// </summary>
        /// <remarks>
        /// Только для государственных организаций,
        /// код по справочнику «Субъекты Российской федерации» (ССРФ),
        /// для Российской Федерации используется код 00
        /// </remarks>
        public string AgencyTerRange { get; set; }

        /// <summary>
        /// Тип ОГВ
        /// </summary>
        /// <remarks>
        /// Только для государственных организаций
        /// </remarks>
        public AgencyType AgencyType { get; set; }

        /// <summary>
        /// Количество сотрудников организации
        /// </summary>
        public int StaffCount { get; set; }

        /// <summary>
        /// Количество филиалов организации (только для ЮЛ и ОГВ)
        /// </summary>
        public int BranchesCount { get; set; }

        public EsiaOrganizationInfo()
        {
        }

        public EsiaOrganizationInfo(JObject organizationInfo)
            : base(organizationInfo)
        {
            if (organizationInfo == null)
                return;

            this.Inn = organizationInfo.GetPropertyValue("inn");
            this.Kpp = organizationInfo.GetPropertyValue("kpp");
            this.Leg = organizationInfo.GetPropertyValue("leg");
            this.AgencyTerRange = organizationInfo.GetPropertyValue("agencyTerRange");
            this.StaffCount = organizationInfo.GetPropertyValue("staffCount").ToInt();
            this.BranchesCount = organizationInfo.GetPropertyValue("branchesCount").ToInt();

            var agencyTypeDict = new Dictionary<string, AgencyType>
            {
                { "10.FED", AgencyType.Fed },
                { "11.REG", AgencyType.Reg },
                { "12.LCL", AgencyType.Lcl },
                { "13.PVD", AgencyType.Pvd },
                { "20.GOV", AgencyType.Gov },
                { "21.MCL", AgencyType.Mcl },
                { "30.FND", AgencyType.Fnd },
                { "31.PFN", AgencyType.Pfn },
                { "40.MFC", AgencyType.Mfc },
                { "41.LEG", AgencyType.Leg }
            };

            this.AgencyType = agencyTypeDict.Get(organizationInfo.GetPropertyValue("agencyType"));
        }
    }
}