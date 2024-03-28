namespace Bars.Gkh.RegOperator.Imports.Room
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Enums;

    public sealed class Record
    {
        public bool isValidRecord { get; set; }

        public bool CreateOwner { get; set; }

        public int RowNumber { get; set; }

        public long RealtyObjectId { get; set; }

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
        /// FLAT_PLACE_NUM
        /// </summary>
        public string Apartment { get; set; }

        /// <summary>
        /// TOTAL_AREA
        /// </summary>
        public decimal Area { get; set; }

        /// <summary>
        /// LIVE_AREA
        /// </summary>
        public decimal LivingArea { get; set; }

        /// <summary>
        /// FLAT_PLACE_TYPE (Тип помещения (выбирается одно из значений: Жилое, Нежилое))
        /// </summary>
        public RoomType RoomType { get; set; }

        /// <summary>
        /// PROPERTY_TYPE (Тип собственности (Возможные значения: Частная, Муниципальная, Государственная, Коммерческая))
        /// </summary>
        public RoomOwnershipType OwnershipType { get; set; }

        /// <summary>
        /// BILL_TYPE (Тип счета (Возможные значения: Счет физ.лица, Счет бр.лица))
        /// </summary>
        public PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// LS_NUM
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// RKC_LS_NUM (Номер ЛС в РКЦ)
        /// </summary>
        public string PersAccNumExternalSystems { get; set; }

        /// <summary>
        /// LS_DATE
        /// </summary>
        public DateTime AccountCreateDate { get; set; }

        /// <summary>
        /// SHARE
        /// </summary>
        public decimal AreaShare { get; set; }

        /// <summary>
        /// SURNAME
        /// </summary>
        public string OwnerPhysSurname { get; set; }

        /// <summary>
        /// NAME
        /// </summary>
        public string OwnerPhysFirstName { get; set; }

        /// <summary>
        /// LASTNAME
        /// </summary>
        public string OwnerPhysSecondName { get; set; }

        /// <summary>
        /// INN
        /// </summary>
        public string OwnerJurInn { get; set; }

        /// <summary>
        /// KPP
        /// </summary>
        public string OwnerJurKpp { get; set; }

        /// <summary>
        /// RENTER_NAME
        /// </summary>
        public string OwnerJurName { get; set; }

        /// <summary>
        /// KLADR
        /// </summary>
        public string Kladr { get; set; }

        /// <summary>
        /// CADASTRAL_NUM
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// RKC_NUM
        /// </summary>
        public string RkcIdentifier { get; set; }

        /// <summary>
        /// RKC_START_DATE
        /// </summary>
        public DateTime? RkcDateStart { get; set; }

        /// <summary>
        /// RKC_END_DATE
        /// </summary>
        public DateTime? RkcDateEnd { get; set; }
    }
}