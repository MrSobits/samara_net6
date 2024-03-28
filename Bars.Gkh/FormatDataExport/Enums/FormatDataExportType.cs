namespace Bars.Gkh.FormatDataExport.Enums
{
    using System;

    using Bars.B4.Utils;

    /// <summary>
    /// Блок сведений
    /// </summary>
    [Flags]
    public enum FormatDataExportType : byte
    {
        /// <summary>
        /// Сведения по договорам управления
        /// </summary>
        [Display("Сведения по договорам управления")]
        Du = 0x01,

        /// <summary>
        /// Сведения по уставам
        /// </summary>
        [Display("Сведения по уставам")]
        Ustav = 0x02,

        /// <summary>
        /// Сведения по региональному оператору, начислениях и оплатах
        /// </summary>
        [Display("Сведения по региональному оператору, начислениях и оплатах")]
        Regop = 0x04,

        /// <summary>
        /// Сведения по капитальному ремонту
        /// </summary>
        [Display("Сведения по капитальному ремонту")]
        Pkr = 0x08,

        /// <summary>
        /// Сведения по инспектированию жилищного фонда
        /// </summary>
        [Display("Сведения по инспектированию жилищного фонда")]
        Gji = 0x10,

        /// <summary>
        /// Сведения по техпаспорту дома
        /// </summary>
        [Display("Сведения по техпаспорту дома")]
        Tp = 0x20,

        /// <summary>
        /// Все
        /// </summary>
        [Display("Все")]
        All = 0xFF
    }
}