namespace Bars.Gkh.Regions.Msk.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Информация ДПКР
    /// </summary>
    public class LiftInfo : BaseEntity
    {
        /// <summary>
        /// Uid
        /// </summary>
        public virtual RealityObjectInfo RealityObjectInfo { get; set; }

        /// <summary>
        /// Подъезд
        /// </summary>
        public virtual string Porch { get; set; }

        /// <summary>
        /// Грузоподъемность
        /// </summary>
        public virtual int Capacity { get; set; }

        /// <summary>
        /// Количество остановок
        /// </summary>
        public virtual int StopCount { get; set; }

        /// <summary>
        /// Год установки
        /// </summary>
        public virtual int InstallationYear { get; set; }

        /// <summary>
        /// Срок эксплуатации
        /// </summary>
        public virtual string LifeTime { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual string Period { get; set; }
    }

}
