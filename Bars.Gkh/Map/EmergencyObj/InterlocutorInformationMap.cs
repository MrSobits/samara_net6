namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities.EmergencyObj;


    /// <summary>Маппинг для "Сведения о собственниках"</summary>
    public class InterlocutorInformationMap : BaseImportableEntityMap<InterlocutorInformation>
    {
        
        public InterlocutorInformationMap() : 
                base("Сведения о собственниках", "GKH_INTERLOCUTOR_INFORMATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ApartmentNumber, "Номер квартиры").Column("APARTMENT_NUMBER");
            Property(x => x.ApartmentArea, "Площадь квартиры").Column("APARTMENT_AREA");
            Property(x => x.FIO, "ФИО собственника").Column("FIO");
            Property(x => x.PropertyType, " Тип собственности").Column("PROPERTY_TYPE");
            Property(x => x.AvailabilityMinorsAndIncapacitatedProprietors, "Наличие несовершеннолетних или недееспособных собственников").Column("AVAILABILITY_MINORD_AND_INCAPACITATED_PROPRIEORS");
            Property(x => x.DateDemolitionIssuing, "Дата направления требования о сносе/реконструкции аварийного МКД - дата, необязательное.").Column("DATE_DEMOLITION_ISSUING");
            Property(x => x.DateDemolitionReceipt, "Дата получения требования о сносе/реконструкции аварийного МКД - дата, необязательное.").Column("DATE_DEMOLITION_RECEIPT");
            Property(x => x.DateNotification, "Дата направления уведомления об изъятии жилого помещения").Column("DATE_NOTIFICATION");
            Property(x => x.DateNotificationReceipt, "Дата получения уведомления об изъятии жилого помещения").Column("DATE_NOTIFICATION_RECEIPT");
            Property(x => x.DateAgreement, "Дата заключения соглашения об изъятии аварийного жилого дома").Column("DATE_AGREEMENT");
            Property(x => x.DateAgreementRefusal, "Дата получения отказа от заключения соглашения").Column("DATE_AGREEMENT_REFUSAL");
            Property(x => x.DateOfReferralClaimCourt, "Дата направления искового заявления в суд").Column("DATE_OF_REFERRAL_CLAIM_COURT");
            Property(x => x.DateOfDecisionByTheCourt, "Дата вынесения решения судом 1 инстанции").Column("DATE_OF_DECISION_BY_COURT");
            Property(x => x.ConsiderationResultClaim, "Результат рассмотрения искового заявления").Column("CONSIDERATION_RESULT_CLAIM");
            Property(x => x.DateAppeal, "Дата направления апелляции").Column("DATE_APPEAL");
            Property(x => x.DateAppealDecision, "Дата вынесения решения апелляции").Column("DATE_APPEAL_DECISION");
            Property(x => x.AppealResult, "Результат рассмотрения апелляции").Column("APPEAL_RESULT");
            
        }
    }
}
