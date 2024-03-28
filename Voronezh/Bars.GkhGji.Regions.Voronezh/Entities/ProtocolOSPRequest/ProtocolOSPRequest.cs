namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Enums;

    /// <summary>
    /// Протокол заявки ОСП
    /// </summary>
    public class ProtocolOSPRequest : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string FIO { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Помещение
        /// </summary>
        public virtual string Room { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// ФИАС ГУИД Дома
        /// </summary>
        public virtual string RoFiasGuid { get; set; }

        /// <summary>
        /// ФИАС ГУИД Пользователя ЕСИА
        /// </summary>
        public virtual string UserEsiaGuid { get; set; }

        /// <summary>
        /// Дата заявки
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// ИД заявки на сайте ГЖИ
        /// </summary>
        public virtual string GjiId { get; set; }

        /// <summary>
        /// Одобрено
        /// </summary>
        public virtual FuckingOSSState Approved { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Дата заявки
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата заявки
        /// </summary>
        public virtual DateTime? DateTo { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastralNumber { get; set; }

        /// <summary>
        /// Номер доверенности
        /// </summary>
        public virtual string AttorneyNumber { get; set; }

        /// <summary>
        /// Дата доверенности
        /// </summary>
        public virtual DateTime? AttorneyDate { get; set; }

        /// <summary>
        /// ФИО доверителя
        /// </summary>
        public virtual string AttorneyFio { get; set; }

        /// <summary>
        ///Файл доверенности
        /// </summary>
        public virtual FileInfo AttorneyFile { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        ///Файл протокола
        /// </summary>
        public virtual FileInfo ProtocolFile { get; set; }

        /// <summary>
        /// Причина отказа
        /// </summary>
        public virtual string ResolutionContent { get; set; }

        /// <summary>
        /// Номер заявки
        /// </summary>
        public virtual string RequestNumber { get; set; }

        /// <summary>
        /// Серия и номер документа собственности
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Дата документа собственности
        /// </summary>
        public virtual DateTime? DocDate { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// Тип заявителя
        /// </summary>
        public virtual OSSApplicantType ApplicantType { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Дата требуемого протокола
        /// </summary>
        public virtual DateTime? ProtocolDate { get; set; }

        /// <summary>
        /// Номер требуемого протокола
        /// </summary>
        public virtual string ProtocolNum { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// тип протокола (новый)
        /// </summary>
        public virtual OwnerProtocolTypeDecision OwnerProtocolType { get; set; }

    }
}