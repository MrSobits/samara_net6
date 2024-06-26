﻿namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дифицит по МО для года в краткосрочной программе КР
    /// </summary>
    public class ShortProgramDifitsit : BaseImportableEntity
    {
        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Версия ДПКР
        /// </summary>
        public virtual ProgramVersion Version { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Дифицит в этом году (руб)
        /// </summary>
        public virtual decimal Difitsit { get; set; }

        /// <summary>
        /// Доля средств из Бюджета региона (%)
        /// </summary>
        public virtual decimal BudgetRegionShare { get; set; }
    }
}