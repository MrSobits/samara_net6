namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Запись краткосрочной программы в которой бует подробно разбиение из каких Бюджетов будет финансирвоатся выполнение работ по всему ООИ
    /// </summary>
    public class ShortProgramRecord : BaseImportableEntity
    {

        /// <summary>
        /// Ссылка на Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Ссылка на Корректировку ДПКР (это считается связь с долгосрочной программой)
        /// </summary>
        public virtual VersionRecordStage2 Stage2 { get; set; }

        // год в краткосрочке 
        public virtual int Year { get; set; }

        /// <summary>
        /// Из Средств собственников на кап. ремонт
        /// </summary>
        public virtual decimal OwnerSumForCr { get; set; }
        
        /// <summary>
        /// Из Бюджета ФСР
        /// </summary>
        public virtual decimal BudgetFcr { get; set; }

        /// <summary>
        /// Из Бюджета других источников
        /// </summary>
        public virtual decimal BudgetOtherSource { get; set; }

        /// <summary>
        /// Из Бюджета региона
        /// </summary>
        public virtual decimal BudgetRegion { get; set; }

        /// <summary>
        /// Из Бюджета МО
        /// </summary>
        public virtual decimal BudgetMunicipality { get; set; }

        /// <summary>
        /// Дифицит для конкретного Этого ООИ, 
        /// который считается по формуле = DpkrCorrection.Sum - OwnerSumForCR - BudgetFcr - BudgetOtherSource
        /// </summary>
        public virtual decimal Difitsit { get; set; }
    }
}