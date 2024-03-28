namespace GisGkhLibrary.Enums.HouseMgmt
{
    public enum AccountType
    {
        /// <summary>
        /// 
        /// </summary>
        NotDefined = 0,

        /// <summary>
        /// Лицевой счет для оплаты за жилое помещение и коммунальные услуги
        /// </summary>
        UOAccount = 1,

        /// <summary>
        /// Лицевой счет для оплаты за коммунальные услуги.
        /// </summary>
        RSOAccount = 2,

        /// <summary>
        /// Лицевой счет для оплаты капитального ремонта
        /// </summary>
        CRAccount = 3,

        /// <summary>
        /// Лицевой счет РКЦ
        /// </summary>
        RCAccount = 4,

        /// <summary>
        /// Лицевой счет ОГВ/ОМС
        /// </summary>
        OGVorOMSAccount = 5,

        /// <summary>
        /// Лицевой счет ТКО
        /// </summary>
        TKOAccount = 6,

    }
}
