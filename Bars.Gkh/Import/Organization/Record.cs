namespace Bars.Gkh.Import.Organization
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public enum OrgType
    {
        ManagingOrganization = 10,

        ResourceProvider = 20,

        CommunalServiceProvider = 30,

        HousingServiceProvider = 40,

        DirectManagement = 50
    }

    public sealed class MunicipalityProxy
    {
        public long Id { get; set; }

        public string FiasId { get; set; }
    }
    
    public sealed class ContragentProxy
    {
        public string Name { get; set; }

        public string Kpp { get; set; }

        public string Inn { get; set; }
    }

    public sealed class Record
    {
        public IOrganizationImportHelper ImportHelper { get; set; }

        public Contragent Contragent { get; set; }

        public object Organization { get; set; }

        public string ContragentMixedKey { get; set; }

        public string ContragentOrganizationMixedKey { get; set; }

        public bool isValidRecord { get; set; }

        public int RowNumber { get; set; }

        public long RealtyObjectId { get; set; }

        public long OrganizationId { get; set; }

        public long ContragentMunicipalityId { get; set; }

        public string ContragentMunicipalityFiasId { get; set; }

        /// <summary>
        /// ID_DOMA
        /// </summary>
        public long ImportRealtyObjectId { get; set; }
        
        /// <summary>
        /// MU
        /// </summary>
        public string MunicipalityName { get; set; }

        /// <summary>
        /// CITY + TYPE_CITY
        /// </summary>
        public string LocalityName { get; set; }

        /// <summary>
        /// STREET + TYPE_STREET
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// HOUSE_NUM
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// LITER
        /// </summary>
        public string Letter { get; set; }
        
        /// <summary>
        /// KORPUS
        /// </summary>
        public string Housing { get; set; }

        /// <summary>
        /// BUILDING
        /// </summary>
        public string Building { get; set; }

        /// <summary>
        /// KLADR
        /// </summary>
        public string StreetKladrCode { get; set; }

        /// <summary>
        /// ID_COM
        /// </summary>
        public int ImportOrganizationId { get; set; }

        /// <summary>
        /// TYPE_COM
        /// </summary>
        public OrgType OrganizationType { get; set; }

        /// <summary>
        /// NAME_COM
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// INN
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// KPP
        /// </summary>
        public string Kpp { get; set; }

        /// <summary>
        /// OGRN
        /// </summary>
        public string Ogrn { get; set; }

        /// <summary>
        /// DATE_REG
        /// </summary>
        public DateTime DateRegistration { get; set; }

        /// <summary>
        /// TYPE_LAW_FORM
        /// </summary>
        public OrganizationForm OrganizationForm { get; set; }
        
        /// <summary>
        /// MU_COM
        /// </summary>
        public string OrgMunicipalityName { get; set; }

        /// <summary>
        /// CITY_COM + TYPE_CITY_COM
        /// </summary>
        public string OrgLocalityName { get; set; }

        /// <summary>
        /// STREET_COM + TYPE_STREET_COM
        /// </summary>
        public string OrgStreetName { get; set; }

        /// <summary>
        /// HOUSE_NUM_COM
        /// </summary>
        public string OrgHouse { get; set; }

        /// <summary>
        /// LITER_COM
        /// </summary>
        public string OrgLetter { get; set; }

        /// <summary>
        /// KORPUS_COM
        /// </summary>
        public string OrgHousing { get; set; }

        /// <summary>
        /// BUILDING_COM
        /// </summary>
        public string OrgBuilding { get; set; }

        /// <summary>
        /// KLADR_COM
        /// </summary>
        public string OrgStreetKladrCode { get; set; }
        
        /// <summary>
        /// DATE_START_CON
        /// </summary>
        public DateTime ContractStartDate { get; set; }

        /// <summary>
        /// TYPE_CON
        /// </summary>
        public TypeManagementManOrg TypeManagement { get; set; }
        
        /// <summary>
        /// NUM_DOG
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// DATE_DOG
        /// </summary>
        public DateTime? DocumentDate { get; set; }
    }
}