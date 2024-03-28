namespace Bars.GisIntegration.Base.Entities.External.Housing.Notif
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    /// <summary>
    /// Уведомление
    /// </summary>
    public class Notif:BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }

        /// <summary>
        /// Тип новости
        /// </summary>
        public virtual  NotifType NotifType { get; set; }

        /// <summary>
        /// Тема новости
        /// </summary>
        public virtual  string NotifTopic { get; set; }

        /// <summary>
        /// Сообщение 
        /// </summary>
        public virtual  string NotifContent { get; set; }

        /// <summary>
        /// В период "С"
        /// </summary>
        public virtual  DateTime NotifFrom { get; set; }

        /// <summary>
        /// В период "По"
        /// </summary>
        public virtual  DateTime NotifTo { get; set; }

        /// <summary>
        /// Важность новости
        /// </summary>
        public virtual  bool IsImportant { get; set; }

        /// <summary>
        /// Все дома в адресатах
        /// </summary>
        public virtual  bool IsAll { get; set; }

        /// <summary>
        /// Не ограничено
        /// </summary>
        public virtual  bool IsUnlim { get; set; }

        /// <summary>
        /// Отправлено
        /// </summary>
        public virtual  bool IsSend { get; set; }

        /// <summary>
        /// Классификатор видов работ (услуг)
        /// </summary>
        public virtual  WorkType WorkType { get; set; }

        /// <summary>
        /// Дата и время начала работ
        /// </summary>
        public virtual  DateTime WorkFrom { get; set; }

        /// <summary>
        /// Дата и время окончания работ
        /// </summary>
        public virtual  DateTime WorkTo { get; set; }

        /// <summary>
        /// Статус удаления
        /// </summary>
        public virtual  bool IsDel { get; set; }

        /// <summary>
        /// Внешний идентификатор поставщика данных
        /// </summary>
        public virtual  string OuterId { get; set; }

        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
    }
}
