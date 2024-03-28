namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.B4.Modules.FileStorage;

    public class SMEVSNILS : BaseEntity
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
        /// дата рождения
        /// </summary>
        public virtual DateTime BirthDate { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string SNILS { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string PatronymicName { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public virtual SMEVGender SMEVGender { get; set; }

        /// <summary>
        /// Тип места рождения
        /// </summary>
        public virtual SnilsPlaceType SnilsPlaceType { get; set; }

        /// <summary>
        /// населенный пункт
        /// </summary>
        public virtual string Settlement { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public virtual string District { get; set; }

        /// <summary>
        /// Область
        /// </summary>
        public virtual string Region { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        public virtual string Country { get; set; }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public virtual string Series { get; set; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? IssueDate { get; set; }

        /// <summary>
        /// Выдан
        /// </summary>
        public virtual string Issuer { get; set; }


        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }


    }
}
