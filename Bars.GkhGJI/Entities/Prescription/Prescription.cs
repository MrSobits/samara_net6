namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Предписание
    /// </summary>
    public class Prescription : DocumentGji
    {
        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Номер решения о проблении
        /// </summary>
        public virtual string RenewalApplicationNumber { get; set; }

        /// <summary>
        /// дата решения о продлении
        /// </summary>
        public virtual DateTime? RenewalApplicationDate { get; set; }

        //ToDo ГЖИ после перехода на правила выпилить ниже следующие поля

        /// <summary>
        /// Список нарушений (Используется при создании объекта Предписания)
        /// </summary>
        public virtual List<long> ViolationsList { get; set; }

        /// <summary>
        /// Список родительских документов (Используется при создании объекта)
        /// </summary>
        public virtual List<long> ParentDocumentsList { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual long InspectionId { get; set; }

        /// <summary>
        /// Сведения об ознакомлении (для НСО)
        /// </summary>
        public virtual PrescriptionFamiliar IsFamiliar { get; set; }

        /// <summary>
        /// Причина закрыития
        /// </summary>
        public virtual PrescriptionCloseReason? CloseReason { get; set; }

        /// <summary>
        /// Статус предписания
        /// </summary>
        public virtual PrescriptionState PrescriptionState { get; set; }

        /// <summary>
        /// ТИп исполнения предписания
        /// </summary>
        public virtual TypePrescriptionExecution TypePrescriptionExecution { get; set; }

        /// <summary>
        ///Отменено ГЖИ
        /// </summary>
        public virtual bool CancelledGJI { get; set; }

        /// <summary>
        /// Закрыто
        /// </summary>
        public virtual YesNoNotSet Closed { get; set; }

        /// <summary>
        /// Примечание при закрытии
        /// </summary>
        public virtual string CloseNote { get; set; }

        public virtual void Close(PrescriptionCloseReason reason, string closeNote)
        {
            Closed = YesNoNotSet.Yes;
            CloseNote = closeNote;
            CloseReason = reason;
        }
    }
}