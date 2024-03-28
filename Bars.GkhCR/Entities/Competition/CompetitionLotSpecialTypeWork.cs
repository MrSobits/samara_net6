namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// 
    /// </summary>
    // todo не уверена, что эта сущность нужна
    public class CompetitionLotSpecialTypeWork : BaseImportableEntity
    {
        /// <summary>
        /// Лот конкурса
        /// </summary>
        public virtual CompetitionLot Lot { get; set; }

        /// <summary>
        /// Вид работы объекта КР
        /// </summary>
        public virtual SpecialTypeWorkCr TypeWork { get; set; }
    }
}