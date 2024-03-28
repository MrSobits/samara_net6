namespace Bars.Gkh.Entities.EmergencyObj
{
    using System;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Собственники
    /// </summary>
    public class InterlocutorInformation : BaseGkhEntity
    {
        /// <summary>
        /// Номер квартиры
        /// </summary>
        public virtual string ApartmentNumber { get; set; }

        /// <summary>
        /// Площадь квартиры
        /// </summary>
        public virtual decimal? ApartmentArea { get; set; }

        /// <summary>
        /// ФИО собственника
        /// </summary>
        public virtual string FIO { get; set; }
        
        /// <summary>
        /// Тип собственности
        /// </summary>
        public virtual RoomOwnershipType PropertyType { get; set; }

        /// <summary>
        /// Наличие несовершеннолетних или недееспособных собственников
        /// </summary>
        public virtual bool AvailabilityMinorsAndIncapacitatedProprietors { get; set; }

        /// <summary>
        /// Дата направления требования о сносе/реконструкции аварийного МКД - дата, необязательное.
        /// </summary>
        public virtual DateTime? DateDemolitionIssuing { get; set; }

        /// <summary>
        /// Дата получения требования о сносе/реконструкции аварийного МКД - дата, необязательное.
        /// </summary>
        public virtual DateTime? DateDemolitionReceipt { get; set; }

        /// <summary>
        /// Дата направления уведомления об изъятии жилого помещения
        /// </summary>
        public virtual DateTime? DateNotification { get; set; }

        /// <summary>
        /// Дата получения уведомления об изъятии жилого помещения
        /// </summary>
        public virtual DateTime? DateNotificationReceipt { get; set; }

        /// <summary>
        /// Дата заключения соглашения об изъятии аварийного жилого дома
        /// </summary>
        public virtual DateTime? DateAgreement { get; set; }

        /// <summary>
        /// Дата получения отказа от заключения соглашения
        /// </summary>
        public virtual DateTime? DateAgreementRefusal { get; set; }

        /// <summary>
        /// Дата направления искового заявления в суд
        /// </summary>
        public virtual DateTime? DateOfReferralClaimCourt { get; set; }

        /// <summary>
        /// Дата вынесения решения судом 1 инстанции
        /// </summary>
        public virtual DateTime? DateOfDecisionByTheCourt { get; set; }
        
        /// <summary>
        /// Результат рассмотрения искового заявления
        /// </summary>
        public virtual string ConsiderationResultClaim { get; set; }
        
        /// <summary>
        /// Дата направления апелляции
        /// </summary>
        public virtual DateTime? DateAppeal { get; set; }

        /// <summary>
        /// Дата вынесения решения апелляции
        /// </summary>
        public virtual DateTime? DateAppealDecision { get; set; }

        /// <summary>
        /// Результат рассмотрения апелляции
        /// </summary>
        public virtual string AppealResult { get; set; }
    }
}