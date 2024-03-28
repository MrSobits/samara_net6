namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    /// Версионирование второго этапа
    /// </summary>
    public class VersionRecordStage2 : BaseImportableEntity
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