namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System;

    public class SMEVSocialHire : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// RO
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// room
        /// </summary>
        public virtual Room Room { get; set; }

        //ответ
        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string ContractNumber { get; set; }

        /// <summary>
        /// Тип договора
        /// </summary>
        public virtual SocialHireContractType ContractType { get; set; }

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Назначение объекта
        /// </summary>
        public virtual string Purpose { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual string TotalArea { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public virtual string LiveArea { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? Birthday { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string Birthplace { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        public virtual string Citizenship { get; set; }

        /// <summary>
        /// Вид документа
        /// </summary>
        public virtual SocialHireDocType DocumentType { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        public virtual string DocumentSeries { get; set; }

        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Владелец договора
        /// </summary>
        public virtual YesNo IsContractOwner { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public virtual string AnswerDistrict { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        public virtual string AnswerCity { get; set; }


        /// <summary>
        /// Улица
        /// </summary>
        public virtual string AnswerStreet { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual string AnswerHouse { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public virtual string AnswerFlat { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        public virtual string AnswerRegion { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
