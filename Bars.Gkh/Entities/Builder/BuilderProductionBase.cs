namespace Bars.Gkh.Entities
{
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Производственная база
    /// </summary>
    public class BuilderProductionBase : BaseGkhEntity
    {
        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// правоустанавливающий документ
        /// </summary>
        public virtual FileInfo DocumentRight { get; set; }

        /// <summary>
        /// Вид оснащения
        /// </summary>
        public virtual KindEquipment KindEquipment { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Notation { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual int? Volume { get; set; }
    }
}
