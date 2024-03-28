namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Вычислительный процесс, запущенный пользователем
    /// </summary>
    public class ComputingProcess : BaseEntity
    {
        /// <summary>
        /// Статус процесса
        /// </summary>
        public virtual ComputingProcessStatus Status { get; set; }

        /// <summary>
        /// Тип процесса
        /// </summary>
        public virtual ComputingProcessType Type { get; set; }

        /// <summary>
        /// Пользователь, запустивший процесс
        /// </summary>
        public virtual User Issuer { get; set; }

        /// <summary>
        /// Название процесса
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Идентификатор Task, который выполняет процесс
        /// </summary>
        public virtual long TaskId { get; set; }

        public override DateTime ObjectCreateDate { get; set; }
    }
}
