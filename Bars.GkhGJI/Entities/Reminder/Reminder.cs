namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Enums;

    /// <summary>
    /// Напоминание по действиям ГЖИ
    /// </summary>
    public class Reminder : BaseEntity
    {
        /// <summary>
        /// Актуально ли данное напоминание
        /// </summary>
        public virtual bool Actuality { get; set; }

        /// <summary>
        /// Тип напоминания
        /// </summary>
        public virtual TypeReminder TypeReminder { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public virtual CategoryReminder CategoryReminder { get; set; }

        /// <summary>
        /// Номер напоминаиния
        /// </summary>
        public virtual string Num { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }

        /// <summary>
        /// Проверка ГЖИ
        /// </summary>
        public virtual InspectionGji InspectionGji { get; set; }

        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji DocumentGji { get; set; }

        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Поручитель
        /// </summary>
        public virtual Inspector Guarantor { get; set; }

        /// <summary>
        /// Проверяющий(инспектор)
        /// </summary>
        public virtual Inspector CheckingInspector { get; set; }
    }
}
