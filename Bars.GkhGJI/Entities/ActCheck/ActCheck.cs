namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Акт проверки
    /// </summary>
    public class ActCheck : DocumentGji
    {
        /// <summary>
        /// Проверяемая площадь
        /// </summary>
        public virtual decimal? Area { get; set; }

        /// <summary>
        /// Передано в прокуратуру
        /// </summary>
        public virtual YesNoNotSet ToProsecutor { get; set; }

        /// <summary>
        /// Дата передачи
        /// </summary>
        public virtual DateTime? DateToProsecutor { get; set; }

        /// <summary>
        /// Тип акта
        /// </summary>
        public virtual TypeActCheckGji TypeActCheck { get; set; }

        /// <summary>
        /// Тип акта2
        /// </summary>
        public virtual TypeCheck TypeCheckAct { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public virtual string Flat { get; set; }
        
        /// <summary>
        /// Должностное лицо, подписавшее акт проверки
        /// </summary>
        public virtual Inspector Signer { get; set; }

        /// <summary>
        /// акт направлен в прокуратуру
        /// </summary>
        public virtual bool ActToPres { get; set; }

        /// <summary>
        ///Невозможно провести проверку
        /// </summary>
        public virtual bool Unavaliable { get; set; }

        /// <summary>
        /// Причина невозможности проведения проверки
        /// </summary>
        public virtual string UnavaliableComment { get; set; }

        /// <summary>
        /// Требуется направление в Роспотребнадзор
        /// </summary>
        public virtual YesNo? ReferralResolutionToRospotrebnadzor { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        public virtual string DocumentPlace { get; set; }

        /// <summary>
        /// Место составления (выбор из ФИАС)
        /// </summary>
        public virtual FiasAddress DocumentPlaceFias { get; set; }

        /// <summary>
        /// Время составления акта
        /// </summary>
        public virtual DateTime? DocumentTime { get; set; }

        /// <summary>
        /// Статус ознакомления с результатами проверки
        /// </summary>
        public virtual AcquaintState? AcquaintState { get; set; }

        /// <summary>
        /// ФИО должностного лица, отказавшегося от ознакомления с актом проверки
        /// </summary>
        public virtual string RefusedToAcquaintPerson { get; set; }

        /// <summary>
        /// ФИО должностного лица, ознакомившегося с актом проверки
        /// </summary>
        public virtual string AcquaintedPerson { get; set; }
        
        /// <summary>
        /// Должность лица, ознакомившегося с актом проверки
        /// </summary>
        public virtual string AcquaintedPersonTitle { get; set; }

        /// <summary>
        /// Дата ознакомления
        /// </summary>
        public virtual DateTime? AcquaintedDate { get; set; }

        // ToDo ГЖИ выпилить следующие поля после перехода на правила

        /// <summary>
        /// Список жилых домов акта (Используется при создании объекта)
        /// </summary>
        public virtual List<long> RealityObjectsList { get; set; }

        /// <summary>
        /// Список родительских документов (Используется при создании объекта)
        /// </summary>
        public virtual List<long> ParentDocumentsList { get; set; }

        /// <summary>
        /// Не хранимое поле Постановление прокуратуры (Если документ передан в прокуратуру)
        /// </summary>
        public virtual DocumentGji ResolutionProsecutor { get; set; }

        /// <summary>
        /// Не хранимое поле проверяемый дом Акта проверки. Если Акт на 1 дом то на клиенте вместо грида необходимо поставить Панель
        /// </summary>
        public virtual ActCheckRealityObject ActCheckGjiRealityObject { get; set; }

        /// <summary>
        /// Инспектор подписавший акт проверки
        /// </summary>
        public virtual Inspector SignatoryInspector { get; set; }

        /// <summary>
        /// Гуид ЕРКНМ для места составления
        /// </summary>
        public virtual string PlaceErknmGuid { get; set; }  
    }
}
