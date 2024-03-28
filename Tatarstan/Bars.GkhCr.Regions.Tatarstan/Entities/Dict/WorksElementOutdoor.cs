namespace Bars.GkhCr.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    /// <summary>
    /// Сущность для связи элементов двора с видами работ двора
    /// </summary>
    public class WorksElementOutdoor : BaseEntity
    {
        /// <summary>
        /// Работа двора
        /// </summary>
        public virtual WorkRealityObjectOutdoor Work { get; set; }

        /// <summary>
        /// Элемент двора
        /// </summary>
        public virtual ElementOutdoor ElementOutdoor { get; set; }
    }
}