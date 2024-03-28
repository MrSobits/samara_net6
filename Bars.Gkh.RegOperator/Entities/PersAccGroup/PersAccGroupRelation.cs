namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Сущность связи лицевого счета с группой лицевых счетов
    /// </summary>
    public class PersAccGroupRelation : BaseEntity
    {
        /// <summary>
        /// Группа лицевых счетов, в которой состоит текущий ЛС
        /// </summary>
        public virtual PersAccGroup Group { get; set; }

        /// <summary>
        /// Лицевой счет, состоящий в группе
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }
    }
}
