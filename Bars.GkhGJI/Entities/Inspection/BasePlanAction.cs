namespace Bars.GkhGji.Entities
{
    using System;

    /// <summary>
    /// Основание проверки план мероприятий 
    /// </summary>
    public class BasePlanAction : InspectionGji
    {

        /// <summary>
        /// План мероприятий
        /// </summary>
        public virtual PlanActionGji Plan { get; set; }

        /// <summary>
        /// Место нахождения (или Адрес Физ лица)
        /// Данное поле заполняется втом случае если выбрали тип субъект = ФизЛицо 
        /// </summary>
        public virtual string PersonAddress { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Рабочих дней
        /// </summary>
        public virtual int? CountDays { get; set; }

        /// <summary>
        /// Требование
        /// </summary>
        public virtual string Requirement { get; set; }

    }
}