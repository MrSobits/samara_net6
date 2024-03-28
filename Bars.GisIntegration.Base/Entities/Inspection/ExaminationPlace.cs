namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Место проведения проверки
    /// </summary>
    public class ExaminationPlace : BaseRisEntity
    {
        /// <summary>
        /// Родительская проверка
        /// </summary>
        public virtual Examination Examination { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int OrderNumber { get; set; }

        /// <summary>
        /// Гуид дома в ФИАС
        /// </summary>
        public virtual string FiasHouseGuid { get; set; }
    }
}
