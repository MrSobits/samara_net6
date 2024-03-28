namespace Bars.Gkh.RegOperator.Imports.BenefitsCategory
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    public sealed class Record
    {
        /// <summary>
        /// Запись корректна.
        /// </summary>
        public bool IsValidRecord { get; set; }

        /// <summary>
        /// Номер записи
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// ИД дома
        /// </summary>
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
        public string FlatNum { get; set; }

        /// <summary>
        /// KOD_LG
        /// </summary>
        public string PrivilegedCategoryCode { get; set; }

        /// <summary>
        /// FAMIL + IMJA + OTCH
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// DATE_LS
        /// </summary>
        public string DateLs { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        public ChargePeriod Period { get; set; }
    }
}
