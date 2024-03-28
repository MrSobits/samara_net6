namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.B4.Modules.FileStorage;

    public class SMEVStayingPlace : BaseEntity
    {
        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Страна выдавшая документ
        /// </summary>
        public virtual string DocCountry { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string CitizenLastname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string CitizenFirstname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string CitizenGivenname { get; set; }


        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime CitizenBirthday { get; set; }


        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string CitizenSnils { get; set; }


        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual SMEVStayingPlaceDocType DocType { get; set; }


        /// <summary>
        /// Серия документа
        /// </summary>
        public virtual string DocSerie { get; set; }


        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocNumber { get; set; }


        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        public virtual DateTime DocIssueDate { get; set; }


        /// <summary>
        /// Регион запроса
        /// </summary>
        public virtual string RegionCode { get; set; }

        /// <summary>
        /// Регион регистрации
        /// </summary>
        public virtual string LPlaceRegion { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public virtual string LPlaceDistrict { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public virtual string LPlaceCity { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public virtual string LPlaceStreet { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual string LPlaceHouse { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public virtual string LPlaceBuilding { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public virtual string LPlaceFlat { get; set; }

        /// <summary>
        /// Действительность регистрации
        /// </summary>
        public virtual string RegStatus { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// TaskId
        /// </summary>
        public virtual string TaskId { get; set; }
    }
}
