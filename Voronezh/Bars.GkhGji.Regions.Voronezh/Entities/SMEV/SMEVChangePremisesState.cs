namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Enums;
    using System;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVChangePremisesState : BaseEntity
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
        /// Тип запроса
        /// </summary>
        public virtual ChangePremisesType ChangePremisesType { get; set; }

        /// <summary>
        /// RealityObject
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Room
        /// </summary>
        public virtual Room Room { get; set; }

        /// <summary>
        /// Кадастровый
        /// </summary>
        public virtual string CadastralNumber { get; set; }

        //ответ

        /// <summary>
        /// Тип заявителя
        /// </summary>
        public virtual OwnerType DeclarantType { get; set; }

        /// <summary>
        /// Имя заявителя
        /// </summary>
        public virtual string DeclarantName { get; set; }

        /// <summary>
        /// Адрес заявителя
        /// </summary>
        public virtual string DeclarantAddress { get; set; }

        /// <summary>
        /// Полное наименование органа местного самоуправления, осуществляющего перевод помещения
        /// </summary>
        public virtual string Department { get; set; }

        /// <summary>
        /// Площадь помещения
        /// </summary>
        public virtual string Area { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public virtual string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual string House { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public virtual string Block { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public virtual string Apartment { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public virtual RoomType RoomType { get; set; }

        /// <summary>
        /// Цель использования помещения
        /// </summary>
        public virtual string Appointment { get; set; }

        /// <summary>
        /// Номер акта
        /// </summary>
        public virtual string ActNumber { get; set; }

        /// <summary>
        /// Акт
        /// </summary>
        public virtual string ActName { get; set; }

        /// <summary>
        /// Дата акта
        /// </summary>
        public virtual DateTime? ActDate { get; set; }

        /// <summary>
        /// Старый тип помещения
        /// </summary>
        public virtual RoomType OldPremisesType { get; set; }

        /// <summary>
        /// Новый тип помещения
        /// </summary>
        public virtual RoomType NewPremisesType { get; set; }

        /// <summary>
        /// Условия перевода
        /// </summary>
        public virtual string ConditionTransfer { get; set; }

        /// <summary>
        /// Наименование ответственного лица
        /// </summary>
        public virtual string ResponsibleName { get; set; }

        /// <summary>
        /// Должность ответственного лица
        /// </summary>
        public virtual string ResponsiblePost { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        public virtual DateTime? ResponsibleDate { get; set; }

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

        public virtual FileInfo AnswerFile { get; set; }
    }
}
