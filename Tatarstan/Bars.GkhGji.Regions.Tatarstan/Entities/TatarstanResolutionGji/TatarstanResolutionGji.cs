namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolutionGji
{
    using System;

    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Постановление ГЖИ РТ
    /// </summary>
    public class TatarstanResolutionGji : Resolution
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string TerminationDocumentNum { get; set; }

        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual TypeDocObject TypeExecutant { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string SurName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Фактический адрес проживания
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Семейное положение
        /// </summary>
        public virtual string MaritalStatus { get; set; }

        /// <summary>
        /// Количество иждивенцев
        /// </summary>
        public virtual int? DependentCount { get; set; }

        /// <summary>
        /// Тип гражданства
        /// </summary>
        public virtual CitizenshipType? CitizenshipType { get; set; }

        /// <summary>
        /// Код страны
        /// </summary>
        public virtual Citizenship Citizenship { get; set; }

        /// <summary>
        /// Тип документа, удостоверяющего личность
        /// </summary>
        public virtual IdentityDocumentType IdentityDocumentType { get; set; }

        /// <summary>
        /// Серия и номер паспорта
        /// </summary>
        public virtual string SerialAndNumberDocument { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? IssueDate { get; set; }

        /// <summary>
        /// Кем выдан
        /// </summary>
        public virtual string IssuingAuthority { get; set; }

        /// <summary>
        /// Место работы, должность, адрес
        /// </summary>
        public virtual string Company { get; set; }

        /// <summary>
        /// Адрес регистрации
        /// </summary>
        public virtual string RegistrationAddress { get; set; }

        /// <summary>
        /// Размер зарплаты (пенсии, стипендии) в руб.
        /// </summary>
        public virtual decimal? Salary { get; set; }

        /// <summary>
        /// Привлекался ли ранее к административной ответственности по ч. 1 ст. 20.6 КоАП РФ
        /// </summary>
        public virtual bool ResponsibilityPunishment { get; set; }
        
        /// <summary>
        /// ФИО законного представителя
        /// </summary>
        public virtual string DelegateFio { get; set; }

        /// <summary>
        /// Место работы, должность законного представителя
        /// </summary>
        public virtual string DelegateCompany { get; set; }

        /// <summary>
        /// Доверенность номер
        /// </summary>
        public virtual string ProcurationNumber { get; set; }

        /// <summary>
        /// Доверенность дата
        /// </summary>
        public virtual DateTime? ProcurationDate { get; set; }

        /// <summary>
        /// Ранее к административной ответственности по ч. 1 ст. 20.6 КоАП РФ привлекались
        /// </summary>
        public virtual bool DelegateResponsibilityPunishment { get; set; }

        /// <summary>
        /// Отягчающие вину обстоятельства
        /// </summary>
        public virtual string DisimprovingFact { get; set; }

        /// <summary>
        /// Смягчающие вину обстоятельства
        /// </summary>
        public virtual string ImprovingFact { get; set; }
    }
}
