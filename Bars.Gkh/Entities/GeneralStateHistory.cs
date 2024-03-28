namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// История изменения обощенных состояний для сущностей (не являющихся ссылками на b4_state)
    /// </summary>
    public class GeneralStateHistory : BaseEntity
    {
        /// <summary>
        /// Дата изменения
        /// </summary>
        public virtual DateTime ChangeDate { get; set; }//!

        /// <summary>
        /// Id сущности
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Тип сущности (полное наименование класса)
        /// </summary>
        public virtual string EntityType { get; set; }

        /// <summary>
        /// Код описателя
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Исходное состояние
        /// </summary>
        public virtual string StartState { get; set; }

        /// <summary>
        /// Финальное состояние
        /// </summary>
        public virtual string FinalState { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public virtual string UserLogin { get; set; }

        protected GeneralStateHistory() { }

        public GeneralStateHistory(
            long entityId,
            string entityType,
            string code,
            string oldValue,
            string newValue,
            User user)
        {
            this.ChangeDate = DateTime.UtcNow;
            this.EntityId = entityId;
            this.EntityType = entityType;
            this.Code = code;
            this.FinalState = newValue;
            this.StartState = oldValue;
            this.UserLogin = user.Login;
            this.UserName = user.Name;
        }
    }
}