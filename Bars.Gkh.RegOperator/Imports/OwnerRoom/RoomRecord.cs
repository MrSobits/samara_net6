namespace Bars.Gkh.RegOperator.Imports.OwnerRoom
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Запись
    /// </summary>
    public sealed class RoomRecord
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="rowNumber"></param>
        public RoomRecord(int rowNumber)
        {
            this.RowNumber = rowNumber;
        }

        #region Лицевой счет

        /// <summary>
        /// LS
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// SHARE
        /// </summary>
        public decimal? AreaShare { get; set; }

        /// <summary>
        /// SHARE_DATE
        /// </summary>
        public DateTime? AreaShareDate { get; set; }

        /// <summary>
        /// RKC_LS_NUM (Номер ЛС в РКЦ)
        /// </summary>
        public string PersAccNumExternalSystems { get; set; }

        /// <summary>
        /// RKC_LS_NUM_DATE
        /// </summary>
        public DateTime? PersAccNumExternalSystemsDate { get; set; }

        /// <summary>MU
        /// LS_DATE
        /// </summary>
        public DateTime? AccountCreateDate { get; set; }

        /// <summary>MU
        /// LS_DATE_DATE
        /// </summary>
        public DateTime? AccountCreateDateStart { get; set; }

        #endregion

        #region Помещение

        /// <summary>
        /// TOTAL_AREA
        /// </summary>
        public decimal? Area { get; set; }

        /// <summary>
        /// TOTAL_AREA_DATE
        /// </summary>
        public DateTime? AreaDate { get; set; }

        /// <summary>
        /// LIVE_AREA
        /// </summary>
        public decimal? LivingArea { get; set; }

        /// <summary>
        /// FLAT_PLACE_TYPE (Тип помещения (выбирается одно из значений: Жилое, Нежилое))
        /// </summary>
        public RoomType? RoomType { get; set; }

        /// <summary>
        /// PROPERTY_TYPE (Тип собственности (Возможные значения: Частная, Муниципальная, Государственная, Коммерческая))
        /// </summary>
        public RoomOwnershipType? OwnershipType { get; set; }

        /// <summary>
        /// PROPERTY_TYPE_DATE
        /// </summary>
        public DateTime? OwnershipTypeDate { get; set; }

        #endregion

        #region Абонент

        /// <summary>
        /// BILL_TYPE (Тип счета (Возможные значения: Счет физ.лица, Счет юр.лица))
        /// </summary>
        public PersonalAccountOwnerType OwnerType { get; set; }

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
        /// BIRTH_DATE
        /// </summary>
        public DateTime? BirthDate { get; set; }

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

        #endregion

        /// <summary>
        /// Номер строки
        /// </summary>
        public int RowNumber { get; internal set; }

        /// <summary>
        /// Признак корректности строки
        /// </summary>
        public bool IsValidRecord { get; internal set; }
    }
}