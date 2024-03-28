namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Лицензия управляющей организации
    /// </summary>
    public class ManOrgLicense : BaseImportableEntity, IStatefulEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Ссылка на заявку 
        /// </summary>
        public virtual ManOrgLicenseRequest Request { get; set; }
        /// <summary>
        /// строковй номер лицензии на случай если захотят в каком то формате сохранять номер
        /// </summary>
        public virtual string LicNumber { get; set; }

        /// <summary>
        /// Номер для авто генерации
        /// </summary>
        public virtual int? LicNum { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? DateIssued { get; set; }

        /// <summary>
        /// Срок действия
        /// </summary>
        public virtual DateTime? DateValidity { get; set; }

        /// <summary>
        /// Дата внесения в реестр лицензии
        /// </summary>
        public virtual DateTime? DateRegister { get; set; }

        /// <summary>
        /// Номер приказа о предоставлении лиценизии
        ///  </summary>
        public virtual string DisposalNumber { get; set; }

        /// <summary>
        /// Дата приказа о предоставлении лиценизии
        ///  </summary>
        public virtual DateTime? DateDisposal { get; set; }

        /// <summary>
        /// Основание прекращения деятельности
        ///  </summary>
        public virtual TypeManOrgTerminationLicense TypeTermination { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Дата прекращения
        /// </summary>
        public virtual DateTime? DateTermination { get; set; }

        /// <summary>
        /// Организация, принявшая решение
        /// </summary>
        public virtual string OrganizationMadeDecisionTermination { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentTermination { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocumentNumberTermination { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? DocumentDateTermination { get; set; }

        /// <summary>
        /// Файл документа
        /// </summary>
        public virtual FileInfo TerminationFile { get; set; }

        /// <summary>
        /// Лицензирующий орган
        /// </summary>
        public virtual HousingInspection.HousingInspection HousingInspection { get; set; }

        /// <summary>
        /// Документ удостоверяющий личность 
        /// </summary>
        public virtual TypeIdentityDocument? TypeIdentityDocument { get; set; }

        /// <summary>
        /// Серия документа удостоверяющего личность
        /// </summary>
        public virtual string IdSerial { get; set; }

        /// <summary>
        /// Номер документа удостоверяющего личность
        /// </summary>
        public virtual string IdNumber { get; set; }

        /// <summary>
        /// Кем выдан документ удостоверяющий личность 
        /// </summary>
        public virtual string IdIssuedBy { get; set; }

        /// <summary>
        /// Дата выдачи документа удостоверяющег оличность
        /// </summary>
        public virtual DateTime? IdIssuedDate { get; set; }

        /// <summary>
        /// Номер ЕРУЛ
        /// </summary>
        public virtual string ERULNumber { get; set; }

        /// <summary>
        /// Дата ЕРУЛ
        /// </summary>
        public virtual DateTime? ERULDate { get; set; }
    }
}
