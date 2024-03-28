namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Плановые показатели собираемости (по умолчанию)
    /// </summary>
    public class DefaultPlanCollectionInfo : BaseImportableEntity
    {
        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// (% Забивается в ручную) Процент собираемости (То есть есть вероятность того что несоберут нужную сумма)
        /// и поэтому заводят процент который покажет Нормальную сумму средств собственников
        /// </summary>
        public virtual decimal PlanOwnerPercent { get; set; }

        /// <summary>
        /// (% Забивается в ручную) Не снижаемый размер регионального фонда
        /// </summary>
        public virtual decimal NotReduceSizePercent { get; set; }     
    }
}