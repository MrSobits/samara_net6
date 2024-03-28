namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    ///  Субсидия для муниципального образования
    /// </summary>
    public class SubsidyMunicipality : BaseEntity
    {
        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Начальный тариф
        /// </summary>
        public virtual decimal StartTarif { get; set; }

        /// <summary>
        /// Коэффициент роста тарифа
        /// </summary>
        public virtual decimal CoefGrowthTarif { get; set; }

        /// <summary>
        /// Средний коэффициен инфляции в год
        /// </summary>
        public virtual decimal CoefAvgInflationPerYear { get; set; }

        /// <summary>
        /// Учитывать инфляцию
        /// </summary>
        public virtual bool ConsiderInflation { get; set; }

        /// <summary>
        /// Суммарный рисковый коэффициент
        /// </summary>
        public virtual decimal CoefSumRisk { get; set; }

        /// <summary>
        /// Срок возврата займа (лет)
        /// </summary>
        public virtual decimal DateReturnLoan { get; set; }

        /// <summary>
        /// Расчет показателей выполнен
        /// </summary>
        public virtual bool CalculationCompleted { get; set; }

        /// <summary>
        /// Признак была ли применена корректировка ДПКР
        /// </summary>
        public virtual bool DpkrCorrected { get; set; }
    }
}