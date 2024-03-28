namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Рапоряжение ГЖИ
    /// </summary>
    public class Disposal : DocumentGji
    {
        /// <summary>
        /// Тип распоряжения
        /// </summary>
        public virtual TypeDisposalGji TypeDisposal { get; set; }

        /// <summary>
        /// Дата начала обследования
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания обследования
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Согласование с прокуротурой
        /// </summary>
        public virtual TypeAgreementProsecutor TypeAgreementProsecutor { get; set; }

        /// <summary>
        /// Номер документа с результатом согласования
        /// </summary>
        public virtual string DocumentNumberWithResultAgreement { get; set; }

        /// <summary>
        /// Результат согласования
        /// </summary>
        public virtual TypeAgreementResult TypeAgreementResult { get; set; }

        /// <summary>
        /// Дата документа с результатом согласования
        /// </summary>
        public virtual DateTime? DocumentDateWithResultAgreement { get; set; }

        /// <summary>
        /// Должностное лицо (ДЛ) вынесшее распоряжение
        /// </summary>
        public virtual Inspector IssuedDisposal { get; set; }

        /// <summary>
        /// Ответственный за исполнение
        /// </summary>
        public virtual Inspector ResponsibleExecution { get; set; }

        /// <summary>
        /// Вид проверки
        /// </summary>
        public virtual KindCheckGji KindCheck { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Выезд на объект с
        /// </summary>
        public virtual DateTime? ObjectVisitStart { get; set; }

        /// <summary>
        /// Выезд на объект по
        /// </summary>
        public virtual DateTime? ObjectVisitEnd { get; set; }

        /// <summary>
        /// Выезд инспектора в командировку
        /// </summary>
        public virtual bool OutInspector { get; set; }

        /// <summary>
        /// Время начала визита (Время с)
        /// </summary>
        public virtual DateTime? TimeVisitStart { get; set; }

        /// <summary>
        /// Время окончания визита (Время по)
        /// </summary>
        public virtual DateTime? TimeVisitEnd { get; set; }

        /// <summary>
        /// Номер документа (Уведомление о проверке)
        /// </summary>
        public virtual string NcNum { get; set; }

        /// <summary>
        /// Дата документа (Уведомление о проверке)
        /// </summary>
        public virtual DateTime? NcDate { get; set; }

        /// <summary>
        /// Номер исходящего письма  (Уведомление о проверке)
        /// </summary>
        public virtual string NcNumLatter { get; set; }

        /// <summary>
        /// Дата исходящего пиьма  (Уведомление о проверке)
        /// </summary>
        public virtual DateTime? NcDateLatter { get; set; }

        /// <summary>
        /// Уведомление получено (Уведомление о проверке)
        /// </summary>
        public virtual YesNo NcObtained { get; set; }

        /// <summary>
        /// Уведомление отправлено (Уведомление о проверке)
        /// </summary>
        public virtual YesNo NcSent { get; set; }

        //ToDo ГЖИ следующие поля необходимо выпилить поля после перехода на правила
        #region Мусор
        /// <summary>
        /// Не хранимое поле. идентификатор Акта проверки общего. Для того чтобы несколько раз нельзя было делать Общий акт
        /// </summary>
        public virtual long? ActCheckGeneralId { get; set; }

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
        /// Не хранимое
        /// </summary>
        public virtual bool HasChildrenActCheck { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual KindKNDGJI KindKNDGJI { get; set; }

        /// <summary>
        /// Номер в ЕРП
        /// </summary>
        public virtual string ERPID { get; set; }

        /// <summary>
        /// Номер решения о согласовании
        /// </summary>
        public virtual string ProcAprooveNum { get; set; }

        /// <summary>
        /// Дата решения о согласовании
        /// </summary>
        public virtual DateTime? ProcAprooveDate { get; set; }

        /// <summary>
        /// Файл решения о согласовании
        /// </summary>
        public virtual FileInfo ProcAprooveFile { get; set; }

        /// <summary>
        /// Файл решения о согласовании
        /// </summary>
        public virtual string FioProcAproove { get; set; }

        /// <summary>
        /// Файл решения о согласовании
        /// </summary>
        public virtual string PositionProcAproove { get; set; }
        #endregion
    }
}