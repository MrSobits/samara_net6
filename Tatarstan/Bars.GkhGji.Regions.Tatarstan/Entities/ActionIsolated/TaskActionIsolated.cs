namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Задание КНМ без взаимодействия с контролируемыми лицами 
    /// </summary>
    public class TaskActionIsolated : DocumentGji
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// План мероприятий
        /// </summary>
        public virtual PlanActionGji PlanAction { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }

        /// <summary>
        /// Обращение гражданина
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }


        /// <summary>
        /// ДЛ, вынесшее задание
        /// </summary>
        public virtual Inspector IssuedTask { get; set; }

        /// <summary>
        /// Ответственный за исполнение
        /// </summary>
        public virtual Inspector ResponsibleExecution { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string PersonName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Вид мероприятия
        /// </summary>
        public virtual KindAction KindAction { get; set; }

        /// <summary>
        /// Основание КНМ
        /// </summary>
        public virtual TypeBaseAction TypeBase { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        public virtual TypeDocObject TypeObject { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual TypeJurPerson TypeJurPerson { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Время начала мероприятия
        /// </summary>
        public virtual DateTime? TimeStart { get; set; }

        /// <summary>
        /// Документ основание Наименование
        /// </summary>
        public virtual string BaseDocumentName { get; set; }

        /// <summary>
        /// Документ основание Номер
        /// </summary>
        public virtual string BaseDocumentNumber { get; set; }

        /// <summary>
        /// Документ основание Дата
        /// </summary>
        public virtual DateTime? BaseDocumentDate { get; set; }

        /// <summary>
        /// Документ основание Файл
        /// </summary>
        public virtual FileInfo BaseDocumentFile { get; set; }
    }
}
