namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// 
    /// </summary>
    public class CompetitionLotTypeWork : BaseImportableEntity
    {
        /// <summary>
        /// Лот конкурса
        /// </summary>
        public virtual CompetitionLot Lot { get; set; }

        /// <summary>
        /// Вид работы объекта КР
        /// </summary>
        public virtual TypeWorkCr TypeWork { get; set; }
    }
}