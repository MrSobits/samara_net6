namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Utils;

    /// <summary>
    /// Акт проверки
    /// </summary>
    public class ActCheckProxy : DocumentGjiProxy
    {
        [Display("Время составления документа")]
        public string Time { get; set; }

        [Display("Площадь")]
        public decimal Area { get; set; }

        [Display("Квартира")]
        public string Apartment { get; set; }

        [Display("Передано в прокуратуру")]
        public bool IsSubmittedToProsecutor { get; set; }

        [Display("Дата передачи")]
        public DateTime SubmitDate { get; set; }

        [Display("Нарушения выявлены")]
        public string IsViolationFound { get; set; }

        [Display("Заголовок описания")]
        public string DescriptionTitle { get; set; }

        [Display("Описание")]
        public string Description { get; set; }

        [Display("Невыявленные нарушения")]
        public string NotFoundViolations { get; set; }
        
        [Display("Тип лица")]
        public string ContragentType { get; set; }

        [Display("Продолжительность проверки")]
        public string InspectionDuration { get; set; }

        [Display("Инспекторы")]
        public List<Inspector> Inspectors { get; set; }

        [Display("Лица, присутствующие при проверке")]
        public List<Witness> Witnesses { get; set; }

        [Display("Лица, ознакомленные с копией распоряжения/приказа о проведении проверки ")]
        public List<FamiliarizedPerson> FamiliarizedPersons { get; set; }

        [Display("Дата и время проведения проверки")]
        public List<InspectionDateTime> InspectionDateTimes { get; set; }

        [Display("Дома")]
        public List<Realty> Realties { get; set; }

        [Display("Нарушения")]
        public List<Violation> Violations { get; set; }

        [Display("Ход проведения проверки")]
        public InspectionProgress InspectionProgress { get; set; }

        [Display("Предоставленные документы")]
        public List<ProvidedDocument> ProvidedDocuments { get; set; }

        [Display("Инспектируемые части")]
        public List<InspectingPart> InspectingParts { get; set; }

        [Display("Определения")]
        public List<Definition> Definitions { get; set; }

        [Display("Приложения")]
        public List<Attachment> Attachments { get; set; }


    }
}
