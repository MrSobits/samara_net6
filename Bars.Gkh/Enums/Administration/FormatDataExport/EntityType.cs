namespace Bars.Gkh.Enums.Administration.FormatDataExport
{
    using Bars.B4.Utils;

    public enum EntityType
    {
        /// <summary>
        /// Программа капитального ремонта
        /// </summary>
        [Display("Программа капитального ремонта")]
        CrProgram = 122,

        /// <summary>
        /// Программы и краткосрочные планы капитального ремонта: Работы программы
        /// </summary>
        [Display("Программы и краткосрочные планы капитального ремонта: Работы программы")]
        CrProgramHousePlanWork = 126,

        /// <summary>
        /// Программы и краткосрочные планы капитального ремонта: Документы
        /// </summary>
        [Display("Программы и краткосрочные планы капитального ремонта: Документы")]
        СrProgramDoc = 135
    }
}
