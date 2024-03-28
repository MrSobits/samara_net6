namespace Bars.Gkh.RegOperator.Imports.BenefitsCategory
{
    using System;
    using Entities;
    using Entities.Dict;

    public sealed class RecordVersion2
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
        /// FAMIL + IMJA + OTCH
        /// </summary>
        public string OwnerFullName { get; set; }

        /// <summary>
        /// LSA
        /// </summary>
        public string AccountNum { get; set; }

        /// <summary>
        /// KOD_POST
        /// </summary>
        public string CodePost { get; set; }

        /// <summary>
        /// KOD_NNASP
        /// </summary>
        public string CodeNnasp { get; set; }

        /// <summary>
        /// KOD_NYLIC
        /// </summary>
        public string CodeNylic { get; set; }

        /// <summary>
        /// NDOM
        /// </summary>
        public string Ndom { get; set; }

        /// <summary>
        /// NKORP
        /// </summary>
        public string Nkorp { get; set; }

        /// <summary>
        /// NKW
        /// </summary>
        public string Nkw { get; set; }

        /// <summary>
        /// NKOMN
        /// </summary>
        public string Nkomn { get; set; }

        /// <summary>
        /// KOD_POST + KOD_NNASP + KOD_NYLIC + NDOM + NKORP + NKW + NKOMN
        /// </summary>
        public string AddressCode { get; set; }

        /// <summary>
        /// NKOD
        /// </summary>
        public string Ncode { get; set; }

        /// <summary>
        /// DATPRS1
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// DATPRPO1
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// PersAccPrivilegedCategoryId
        /// </summary>
        public long PersAccPrivilegedCategoryId { get; set; }

        /// <summary>
        /// PrivilegedCategory
        /// </summary>
        public PrivilegedCategory PrivilegedCategory { get; set; }

        /// <summary>
        /// PersAcc
        /// </summary>
        public BasePersonalAccount PersAcc { get; set; }
    }
}