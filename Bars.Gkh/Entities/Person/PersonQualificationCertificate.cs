namespace Bars.Gkh.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Квалификационный аттестат
    /// </summary>
    public class PersonQualificationCertificate : BaseImportableEntity, IStatefulEntity
    {
        public virtual State State { get; set; }

        /// <summary>
        /// Физ лицо
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// Номер КА
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Номер бланка
        /// </summary>
        public virtual string BlankNumber { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? IssuedDate { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Дата получения
        /// </summary>
        public virtual DateTime? RecieveDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Выдан дубликат
        /// </summary>
        public virtual bool HasDuplicate { get; set; }

        /// <summary>
        /// Номер дубликата
        /// </summary>
        [Obsolete("Use QualificationDocument")]
        public virtual string DuplicateNumber { get; set; }

        /// <summary>
        /// Дата выдачи дубликата
        /// </summary>
        [Obsolete("Use QualificationDocument")]
        public virtual DateTime? DuplicateIssuedDate { get; set; }

        /// <summary>
        /// Фаил дубликата
        /// </summary>
        [Obsolete("Use QualificationDocument")]
        public virtual FileInfo DuplicateFile { get; set; }

        /// <summary>
        /// Аттестат аннулирован
        /// </summary>
        public virtual bool HasCancelled { get; set; }

        /// <summary>
        /// Основание отмены квалификационоого аттестата
        /// </summary>
        public virtual TypeCancelationQualCertificate TypeCancelation { get; set; }

        /// <summary>
        /// Дата анулирования
        /// </summary>
        public virtual DateTime? CancelationDate { get; set; }

        /// <summary>
        /// Номер протокола аннулирования
        /// </summary>
        public virtual string CancelNumber { get; set; }

        /// <summary>
        /// Дата протокола аннулирования
        /// </summary>
        public virtual DateTime? CancelProtocolDate { get; set; }

        /// <summary>
        /// Протокол аннулирования
        /// </summary>
        public virtual FileInfo CancelFile { get; set; }

        /// <summary>
        /// Аннулирование отменено
        /// </summary>
        public virtual bool HasRenewed { get; set; }

        /// <summary>
        /// Наименование суда
        /// </summary>
        public virtual string CourtName { get; set; }

        /// <summary>
        /// Номер судебного акта
        /// </summary>
        public virtual string CourtActNumber { get; set; }

        /// <summary>
        /// Дата судебного акта
        /// </summary>
        public virtual DateTime? CourtActDate { get; set; }

        /// <summary>
        /// Судебный акт
        /// </summary>
        public virtual FileInfo ActFile { get; set; }

        /// <summary>
        /// Заявка на доступ к экзамену 
        /// </summary>
        public virtual PersonRequestToExam RequestToExam { get; set; }

        /// <summary>
        /// Файл заявления о выдаче квалификационного аттестата
        /// </summary>
        public virtual FileInfo FileIssueApplication { get; set; }

        /// <summary>
        /// Дата подачи заявления о выдаче квалификационного аттестата
        /// </summary>
        public virtual DateTime? ApplicationDate { get; set; }

        /// <summary>
        /// Уведомление лицензионной комиссии о результатах экзамена
        /// </summary>
        public virtual FileInfo FileNotificationOfExamResults { get; set; }

        /// <summary>
        /// Квалификационный аттестат получен в другом регионе
        /// </summary>
        public virtual bool? IsFromAnotherRegion { get; set; }

        /// <summary>
        /// Код региона где был выдан аттестат
        /// </summary>
        public virtual string RegionCode { get; set; }

        /// <summary>
        ///Аттестат выдан
        /// </summary>
        public virtual string IssuedBy { get; set; }

    }
}
