namespace Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    public class TypeWorkRealityObjectOutdoor : BaseEntity
    {
        /// <summary>
        /// Объект программы благоустройства дворов.
        /// </summary>
        public virtual Entities.ObjectOutdoorCr.ObjectOutdoorCr ObjectOutdoorCr { get; set; }

        /// <summary>
        /// Вид работы.
        /// </summary>
        public virtual WorkRealityObjectOutdoor WorkRealityObjectOutdoor { get; set; }
        
        /// <summary>
        /// Объем (плановый).
        /// </summary>
        public virtual decimal? Volume { get; set; }

        /// <summary>
        /// Сумма (плановая).
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Примечание.
        /// </summary>
        public virtual string Description { get; set; }
        
        /// <summary>
        /// Признак является ли запись активной.
        /// </summary>
        public virtual bool IsActive { get; set; }
    }
}
