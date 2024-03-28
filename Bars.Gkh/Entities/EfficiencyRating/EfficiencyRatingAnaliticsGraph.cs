namespace Bars.Gkh.Entities.EfficiencyRating
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.PlotBuilding.Model;
    using Bars.Gkh.DomainService.EfficiencyRating;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums.EfficiencyRating;

    /// <summary>
    /// Сущность-конфиг для построения графика аналитики Рейтинга Эффективности УО
    /// </summary>
    public class EfficiencyRatingAnaliticsGraph : BaseEntity
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public EfficiencyRatingAnaliticsGraph()
        {
            this.Periods = new HashSet<EfficiencyRatingPeriod>();
            this.Municipalities = new HashSet<Municipality>();
            this.ManagingOrganizations = new HashSet<ManagingOrganization>();
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Уровень детализации
        /// </summary>
        public virtual AnaliticsLevel AnaliticsLevel { get; set; }

        /// <summary>
        /// Категория графика
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Отображать график в разрезе
        /// </summary>
        public virtual ViewParam ViewParam { get; set; }

        /// <summary>
        /// Фактор
        /// </summary>
        public virtual string FactorCode { get; set; }

        /// <summary>
        /// Тип графика
        /// </summary>
        public virtual DiagramType DiagramType { get; set; }

        /// <summary>
        /// Периоды, за которые строится график
        /// </summary>
        public virtual ISet<EfficiencyRatingPeriod> Periods { get; set; }

        /// <summary>
        /// Муниципалитеты
        /// </summary>
        public virtual ISet<Municipality> Municipalities { get; set; }

        /// <summary>
        /// Управляющие организации
        /// </summary>
        public virtual ISet<ManagingOrganization> ManagingOrganizations { get; set; }

        /// <summary>
        /// Сохраненный график
        /// </summary>
        public virtual IPlotData Data { get; set; }
    }
}