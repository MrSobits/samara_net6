namespace Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr
{
    using Bars.B4.DataAccess;
    using Bars.GkhCr.Enums;

    public class TypeWorkRealityObjectOutdoorHistory : BaseEntity
    {
        /// <summary>
        /// Вид работы объекта.
        /// </summary>
        public virtual TypeWorkRealityObjectOutdoor TypeWorkRealityObjectOutdoor { get; set; }

        /// <summary>
        /// Тип действия для истории вида работ объекта.
        /// </summary>
        public virtual TypeWorkCrHistoryAction TypeAction { get; set; }

        /// <summary>
        /// Объем выполнения.
        /// </summary>
        public virtual decimal? Volume { get; set; }

        /// <summary>
        /// Сумма расходов.
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        ///  Имя пользователя.
        /// </summary>
        public virtual string UserName { get; set; }
    }
}
