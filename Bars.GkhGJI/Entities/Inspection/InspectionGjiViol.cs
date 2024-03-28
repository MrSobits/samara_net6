namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Нарушение проверки
    /// Эта таблица хранит в себе нарушение проверки, без привязки к конкретному документу
    /// </summary>
    public class InspectionGjiViol : BaseGkhEntity
    {
        /// <summary>
        /// Проверка ГЖИ
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolationGji Violation { get; set; }

        /// <summary>
        /// Мероприятие по устранению данног онарушения 
        /// потребовалось для того чтобы в предписании назначать мероприятия а в приказе или акте видеть данное мероприятие 
        /// </summary>
        public virtual string Action { get; set; }

        /// <summary>
        /// Плановая дата устранения
        /// </summary>
        public virtual DateTime? DatePlanRemoval { get; set; }

        /// <summary>
        /// фактическая дата устранения
        /// </summary>
        public virtual DateTime? DateFactRemoval { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// сумма работ по устранению нарушений
        /// </summary>
        public virtual decimal? SumAmountWorkRemoval { get; set; }

        /// <summary>
        /// Дата отмены
        /// </summary>
        public virtual DateTime? DateCancel { get; set; }

        /// <summary>
        ///ГУИД ГИС ЕРП
        /// </summary>
        public virtual string ERPGuid { get; set; }

        /// <summary>
        /// Не хранимое поле. Наименование Нарушения
        /// </summary>
        public virtual string ViolationGji { get; set; }
    }
}