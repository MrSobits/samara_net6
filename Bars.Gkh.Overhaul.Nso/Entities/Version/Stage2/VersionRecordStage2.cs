namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    /// <summary>
    /// Версионирование второго этапа
    /// </summary>
    public class VersionRecordStage2 : BaseEntity
    {
        /// <summary>
        /// Версия 3го этапа
        /// </summary>
        public virtual VersionRecord Stage3Version { get; set; }

        /// <summary>
        /// Вес конструктивного элемента
        /// </summary>
        public virtual int CommonEstateObjectWeight { get; set; }

        /// <summary>
        /// Сумма по 2му этапу
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Объект общего имущества
        /// </summary>
        public virtual CommonEstateObject CommonEstateObject { get; set; }
    }
}