namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.GkhGji.Enums;

    /// <summary>
    /// Основание плановой проверки юр. лиц ГЖИ
    /// </summary>
    public class BaseJurPerson : InspectionGji
    {
        /// <summary>
        /// План проверки юр. лиц
        /// </summary>
        public virtual PlanJurPersonGji Plan { get; set; }

        /// <summary>
        /// Дата начала проверки
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Срок проверки (Количество дней)
        /// </summary>
        public virtual int? CountDays { get; set; }

        /// <summary>
        /// Срок проверки (Количество часов)
        /// </summary>
        public virtual int? CountHours{ get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Иные основания проверки
        /// </summary>
        public virtual string AnotherReasons { get; set; }

        /// <summary>
        /// Тип основания проверки ЮЛ
        /// </summary>
        public virtual TypeBaseJurPerson TypeBaseJuralPerson { get; set; }

        /// <summary>
        /// Факт проверки ЮЛ
        /// </summary>
        public virtual TypeFactInspection TypeFact { get; set; }

        /// <summary>
        /// Форма проверки ЮЛ
        /// </summary>
        public virtual TypeFormInspection TypeForm { get; set; }

        /// <summary>
        /// Нехранимое поле, идентификаторы инспекторов проверки юр.лиц
        /// </summary>
        public virtual string JurPersonInspectors { get; set; }

        /// <summary>
        /// Нехранимое поле, идентификаторы отделов проверки юр.лиц
        /// </summary>
        public virtual string JurPersonZonalInspections { get; set; }

        /// <summary>
        /// Учетный номер проверки в едином реестре проверок
        /// </summary>
        public virtual int? UriRegistrationNumber { get; set; }

        /// <summary>
        /// Дата присвоения учетного номера
        /// </summary>
        public virtual DateTime? UriRegistrationDate { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }
        
        /// Порядковый номер в плане
        /// </summary>
        public virtual int PlanNumber { get; set; }
    }
}