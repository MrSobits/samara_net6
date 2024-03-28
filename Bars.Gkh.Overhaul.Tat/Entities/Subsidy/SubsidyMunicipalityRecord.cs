namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    ///  Запись субсидии для муниципального образования
    /// </summary>
    public class SubsidyMunicipalityRecord : BaseEntity
    {
        /// <summary>
        /// Субсидия МО
        /// </summary>
        public virtual SubsidyMunicipality SubsidyMunicipality { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int SubsidyYear { get; set; }

        /// <summary>
        /// Бюджет ГСК ФСР
        /// </summary>
        public virtual decimal BudgetFcr { get; set; }

        /// <summary>
        /// Бюджет региона
        /// </summary>
        public virtual decimal BudgetRegion { get; set; }

        /// <summary>
        /// Бюджет МО
        /// </summary>
        public virtual decimal BudgetMunicipality { get; set; }

        /// <summary>
        /// Cредства собственников
        /// </summary>
        public virtual decimal OwnerSource { get; set; }

        /// <summary>
        /// Итоговый бюдет на КР в этойм году
        /// </summary>
        public virtual decimal BudgetCr { get; set; }

        /// <summary>
        /// не хранимые поля которые берутся всегда из версии - Необходимая сумма на КР в этой году
        /// </summary>
        public virtual decimal NeedFinance { get; set; }

        /// <summary>
        /// не хранимые поля которые берутся всегда из верси - Остаток либо положительный либо отрицательный
        /// </summary>
        public virtual decimal Deficit { get; set; }
    }
}