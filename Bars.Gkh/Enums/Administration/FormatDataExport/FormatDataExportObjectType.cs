namespace Bars.Gkh.Enums.Administration.FormatDataExport
{
    using Bars.B4.Utils;

    public enum FormatDataExportObjectType
    {
        /// <summary>
        /// Долгосрочная программа капитального ремонта
        /// </summary>
        [Display("Долгосрочная программа капитального ремонта")]
        CrProgram = 1,

        /// <summary>
        /// Работы долгосрочной программы капитального ремонта
        /// </summary>
        [Display("Работы долгосрочной программы капитального ремонта")]
        CrProgramWorks = 2
    }
}
