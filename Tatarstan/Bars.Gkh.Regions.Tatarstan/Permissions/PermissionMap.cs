namespace Bars.Gkh.Regions.Tatarstan.Permissions
{
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    using Gkh.Entities.EmergencyObj;

    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Namespace("Administration.Notify", "Уведомление пользователей");
            this.CRUDandViewPermissions("Administration.Notify");

            this.Namespace("B4.Audit.EmailMessage", "Отправка писем");
            this.Permission("B4.Audit.EmailMessage.View", "Просмотр");

            this.Namespace("Gkh.Orgs.Contragent.Register.Provider", "Сведения по поставщикам");
            this.Permission("Gkh.Orgs.Contragent.Register.Provider.ProviderCode_Edit", "Код поставщика - изменение");
            this.Permission("Gkh.Orgs.Contragent.Register.Provider.ProviderCode_View", "Код поставщика - просмотр");
            this.Permission("Gkh.Orgs.Contragent.Register.Provider.ProviderCode_Generate", "Кнопка \"Получить код\" - просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.SendDuUstav_View", "Отправить договор/устав в ГИС ЖКХ");

            this.Namespace("Gkh.RealityObject.RoleTypeHousePermission", "Настройка добавления домов");
            this.Permission("Gkh.RealityObject.RoleTypeHousePermission.View", "Просмотр");
            this.Permission("Gkh.RealityObject.RoleTypeHousePermission.Edit", "Изменение");

            this.Permission("Gkh.RealityObject.Field.View.SendTechPassport_View", "Отправить ТехПаспорт в ГИС ЖКХ");
            this.Permission("Gkh.RealityObject.Field.View.ObjectConstruction_View", "Объект строительства");
            this.Permission("Gkh.RealityObject.Field.Edit.ObjectConstruction_Edit", "Объект строительства");
            this.Permission("Gkh.RealityObject.Field.View.BuiltOnResettlementProgram_View", "Построен по программе переселения");
            this.Permission("Gkh.RealityObject.Field.Edit.BuiltOnResettlementProgram_Edit", "Построен по программе переселения");
            this.Permission("Gkh.RealityObject.Field.View.TechPassportFile_View", "Электронный паспорт дома");

            this.Permission("Gkh.RealityObject.Field.View.CentralHeatingStation_View", "Наименование ЦТП");
            this.Permission("Gkh.RealityObject.Field.View.AddressCtp_View", "Адрес ЦТП");
            this.Permission("Gkh.RealityObject.Field.View.NumberInCtp_View", "Порядковый номер объекта в ЦТП");
            this.Permission("Gkh.RealityObject.Field.View.PriorityCtp_View", "Приоритет вывода из эксплуатации ЦТП");
            this.Permission("Gkh.RealityObject.Field.Edit.CentralHeatingStation_Edit", "Наименование ЦТП");
            this.Permission("Gkh.RealityObject.Field.Edit.AddressCtp_Edit", "Адрес ЦТП");
            this.Permission("Gkh.RealityObject.Field.Edit.NumberInCtp_Edit", "Порядковый номер объекта в ЦТП");
            this.Permission("Gkh.RealityObject.Field.Edit.PriorityCtp_Edit", "Приоритет вывода из эксплуатации ЦТП");

            this.Namespace("Gkh.RealityObject.Register.Intercom", "Сведения о домофонах");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Intercom");

            this.Namespace("Gkh.Orgs.GasEquipmentOrg", "ВДГО");
            this.CRUDandViewPermissions("Gkh.Orgs.GasEquipmentOrg");
            
            #region Аварийный дом

            #region Аварийный дом - Сведения о собственниках
            this.Namespace("Gkh.EmergencyObject.InterlocutorInformation", "Сведения о собственниках");

            #region Аварийный дом - Сведения о собственниках
            this.CRUDandViewPermissions("Gkh.EmergencyObject.InterlocutorInformation");

            #endregion Аварийный дом - Собственник

            this.Namespace<InterlocutorInformation>("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor", "Поля");

            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.ApartmentArea_Edit", "Площадь квартиры");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.PropertyType_Edit", "Тип собственности");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.AvailabilityMinorsAndIncapacitatedProprietors_Edit", "Наличие несовершеннолетних или недееспособных собственников");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateDemolitionIssuing_Edit", "Дата направления требования о сносе/реконструкции аварийного МКД - дата, необязательное.");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateDemolitionReceipt_Edit", "Дата получения требования о сносе/реконструкции аварийного МКД - дата, необязательное.");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateNotification_Edit", "Дата направления уведомления об изъятии жилого помещения");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateNotificationReceipt_Edit", "Дата получения уведомления об изъятии жилого помещения");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateAgreement_Edit", "Дата заключения соглашения об изъятии аварийного жилого дома");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateAgreementRefusal_Edit", "Дата получения отказа от заключения соглашения");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateOfReferralClaimCourt_Edit", "Дата направления искового заявления в суд");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateOfDecisionByTheCourt_Edit", "Дата вынесения решения судом 1 инстанции");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.ConsiderationResultClaim_Edit", "Результат рассмотрения искового заявления");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateAppeal_Edit", "Дата направления апелляции");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateAppealDecision_Edit", "Дата вынесения решения апелляции");
            this.Permission("Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.AppealResult_Edit", "Результат рассмотрения апелляции");

            #endregion Аварийный дом - Собственник

            #endregion Аварийный дом

            this.Namespace("Gkh.EmergencyObject.Register.Documents", "Документы");
            this.Permission("Gkh.EmergencyObject.Register.Documents.View", "Просмотр");

            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject", "Объекты строительства");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.View", "Просмотр");

            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register", "Реестры");
            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport", "Паспорт объекта");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.View", "Просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Edit", "Изменение");
            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields", "Поля");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.FiasAddress_Edit", "Строительный адрес - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.FiasAddress_View", "Строительный адрес - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ResettlementProgram_Edit", "Программа переселения - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ResettlementProgram_View", "Программа переселения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.Description_Edit", "Примечание - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.Description_View", "Примечание - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.SumSmr_Edit", "Сумма на СМР - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.SumSmr_View", "Сумма на СМР - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.SumDevolopmentPsd_Edit", "Сумма на разработку экспертизы ПСД - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.SumDevolopmentPsd_View", "Сумма на разработку экспертизы ПСД - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateEndBuilder_Edit", "Дата завершения работ подрядчиком - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateEndBuilder_View", "Дата завершения работ подрядчиком - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateStartWork_Edit", "Дата начала работ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateStartWork_View", "Дата начала работ - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateStopWork_Edit", "Дата остановки работ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateStopWork_View", "Дата остановки работ - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateResumeWork_Edit", "Дата возобновления работ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateResumeWork_View", "Дата возобновления работ - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ReasonStopWork_Edit", "Причина остановки работ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ReasonStopWork_View", "Причина остановки работ - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateCommissioning_Edit", "Дата сдачи в эксплуатацию - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateCommissioning_View", "Дата сдачи в эксплуатацию - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.LimitOnHouse_Edit", "Лимит на дом - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.LimitOnHouse_View", "Лимит на дом - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.TotalArea_Edit", "Общая площадь дома - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.TotalArea_View", "Общая площадь дома - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberApartments_Edit", "Количество квартир - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberApartments_View", "Количество квартир - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ResettleProgNumberApartments_Edit", "в т.ч. по программе переселения - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ResettleProgNumberApartments_View", "в т.ч. по программе переселения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberFloors_Edit", "Количество этажей - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberFloors_View", "Количество этажей - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberEntrances_Edit", "Количество подъездов - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberEntrances_View", "Количество подъездов - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberLifts_Edit", "Количество лифтов - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberLifts_View", "Количество лифтов - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.WallMaterial_Edit", "Материал стен - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.WallMaterial_View", "Материал стен - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.RoofingMaterial_Edit", "Материал кровли - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.RoofingMaterial_View", "Материал кровли - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.TypeRoof_Edit", "Тип кровли - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.TypeRoof_View", "Тип кровли - просмотр");

            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents", "Документация");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.View", "Просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Create", "Создание записей");
            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields", "Поля");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Name_Edit", "Документ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Name_View", "Документ - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Number_Edit", "Номер - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Number_View", "Номер - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Date_Edit", "Дата - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Date_View", "Дата - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.File_Edit", "Файл - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.File_View", "Файл - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Contragent_Edit", "Участник процесса - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Contragent_View", "Участник процесса - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Description_Edit", "Описание - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Description_View", "Описание - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Type_Edit", "Тип документа - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Type_View", "Тип документа - просмотр");
            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes", "Типы документов");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.MatchingSheet", "Лист согласования технических условий");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.Psd", "ПСД на строительство и инженерные сети");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.PsdExpertise", "Экспертиза ПСД");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.CommissioningAct", "Акт ввода дома в эксплуатацию");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.Other", "Иные документы");

            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos", "Фото-архив");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.View", "Просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Create", "Создание записей");
            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields", "Поля");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Date_Edit", "Дата изображения - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Date_View", "Дата изображения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Name_Edit", "Наименование - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Name_View", "Наименование - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Group_Edit", "Группа - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Group_View", "Группа - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.File_Edit", "Файл - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.File_View", "Файл - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Description_Edit", "Описание - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Description_View", "Описание - просмотр");

            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks", "Виды работ");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.View", "Просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Create", "Создание записей");
            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields", "Поля");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Work_Edit", "Вид работы - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Work_View", "Вид работы - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.TypeWork_View", "Тип работы - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.UnitMeasureName_View", "Единица измерения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.YearBuilding_Edit", "Год строительства - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.YearBuilding_View", "Год строительства - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.HasPsd_Edit", "Наличие ПСД - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.HasPsd_View", "Наличие ПСД - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.HasExpertise_Edit", "Наличие экспертизы - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.HasExpertise_View", "Наличие экспертизы - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Volume_Edit", "Объем - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Volume_View", "Объем - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Sum_Edit", "Сумма - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Sum_View", "Сумма - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Deadline_Edit", "Контрольный срок - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Deadline_View", "Контрольный срок - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Description_Edit", "Примечание - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Description_View", "Примечание - просмотр");

            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants", "Участники строительства");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.View", "Просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Create", "Создание записей");
            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields", "Поля");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerType_Edit", "Тип заказчика - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerType_View", "Тип заказчика - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ContragentInn_View", "ИНН - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ContragentContactName_View", "ФИО руководителя - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ContragentContactPhone_View", "Контактный телефон - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.Description_Edit", "Дополнительная информация - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.Description_View", "Дополнительная информация - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ParticipantType_Edit", "Участник строительства - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ParticipantType_View", "Участник строительства - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.Contragent_Edit", "Наименование участника - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.Contragent_View", "Наименование участника - просмотр");
            this.Namespace<ConstructionObject>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes", "Типы заказчиков");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.Gzhf", "ГЖФ");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.Ispolkom", "Исполком");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.Minstroj", "Минстрой");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.Gisu", "ГИСУ");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.TechnicalSupervisionOrganization", "Организация технического надзора");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.PsdDevelopers", "Разработчики ПСД");

            this.Namespace<ConstructionObjectContract>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts", "Договоры");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.View", "Просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Create", "Создание записей");
            this.Namespace<ConstructionObjectContract>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields", "Поля");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Number_Edit", "Номер - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Number_View", "Номер - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Date_Edit", "Дата - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Date_View", "Дата - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.File_Edit", "Файл - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.File_View", "Файл - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Sum_Edit", "Сумма договора - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Sum_View", "Сумма договора - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateStart_Edit", "Дата начала действия договора - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateStart_View", "Дата начала действия договора - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateEnd_Edit", "Дата окончания действия договора - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateEnd_View", "Дата окончания действия договора - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateStartWork_Edit", "Дата начала работ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateStartWork_View", "Дата начала работ - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateEndWork_Edit", "Дата окончания работ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateEndWork_View", "Дата окончания работ - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Name_Edit", "Договор - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Name_View", "Договор - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Contragent_Edit", "Подрядная организация - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Contragent_View", "Подрядная организация - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Type_Edit", "Тип договора - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Type_View", "Тип договора - просмотр");
            this.Namespace<ConstructionObjectContract>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types", "Типы договоров");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types.Smr", "Договор на СМР");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types.Psd", "Договор на ПСД");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types.Supervision", "Технадзор");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types.Additional", "Доп. соглашение");

            this.Namespace<ConstructObjMonitoringSmr>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr", "Мониторинг СМР");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.View", "Просмотр");
            this.Namespace<ConstructObjMonitoringSmr>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule", "График выполнения работ");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.View", "Просмотр");
            this.Namespace<ConstructObjMonitoringSmr>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns", "Колонки");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.WorkName_View", "Вид работы - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.UnitMeasureName_View", "Единица измерения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.DateStartWork_Edit", "Начало работ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.DateStartWork_View", "Начало работ - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.DateEndWork_Edit", "Окончание работ - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.DateEndWork_View", "Окончание работ - просмотр");

            this.Namespace<ConstructObjMonitoringSmr>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress", "Ход выполнения работ");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.View", "Просмотр");
            this.Namespace<ConstructObjMonitoringSmr>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns", "Колонки");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.WorkName_View", "Вид работы - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.UnitMeasureName_View", "Единица измерения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.Volume_View", "Плановый объем - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.Sum_View", "Плановая сумма - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.VolumeOfCompletion_View", "Объем выполнения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.PercentOfCompletion_View", "Процент выполнения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.CostSum_View", "Сумма расходов - просмотр");
            this.Namespace<ConstructObjMonitoringSmr>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields", "Поля");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.VolumeOfCompletion_Edit", "Объем выполнения - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.VolumeOfCompletion_View", "Объем выполнения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.PercentOfCompletion_Edit", "Процент выполнения - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.PercentOfCompletion_View", "Процент выполнения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.CostSum_Edit", "Сумма расходов - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.CostSum_View", "Сумма расходов - просмотр");

            this.Namespace<ConstructObjMonitoringSmr>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers", "Численность рабочих");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.View", "Просмотр");
            this.Namespace<ConstructObjMonitoringSmr>("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns", "Колонки");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.WorkName_View", "Вид работы - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.UnitMeasureName_View", "Единица измерения - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.Volume_View", "Плановый объем - просмотр");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.CountWorker_Edit", "Количество работников - изменение");
            this.Permission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.CountWorker_View", "Количество работников - просмотр");

            this.Namespace("Gkh.Dictionaries.PeriodNormConsumption", "Отчетные периоды нормативов потребления");
            this.CRUDandViewPermissions("Gkh.Dictionaries.PeriodNormConsumption");
            this.Namespace<NormConsumption>("Gkh.NormConsumption", "Нормативы потребления");
            this.Permission("Gkh.NormConsumption.View", "Просмотр");

            this.Namespace("Gkh.Dictionaries.PlanPaymentsPercentage", "Процент планируемых оплат");
            this.CRUDandViewPermissions("Gkh.Dictionaries.PlanPaymentsPercentage");

            #region Рейтинг эффективности УО
            this.Namespace("Gkh.Orgs.EfficiencyRating", "Рейтинг эффективности УО");
            this.Namespace("Gkh.Orgs.EfficiencyRating.ManagingOrganization", "Рейтинг эффективности управляющих организаций");
            this.Permission("Gkh.Orgs.EfficiencyRating.ManagingOrganization.View", "Просмотр");
            this.Permission("Gkh.Orgs.EfficiencyRating.ManagingOrganization.Edit", "Изменение записей");
            this.Permission("Gkh.Orgs.EfficiencyRating.ManagingOrganization.CalcValues", "Рассчитать показатели");
            this.Permission("Gkh.Orgs.EfficiencyRating.ManagingOrganization.MassCalcValues", "Массовый расчет показателей");

            this.Namespace("Gkh.Orgs.EfficiencyRating.Analitics", "Аналитические показатели");
            this.Permission("Gkh.Orgs.EfficiencyRating.Analitics.View", "Просмотр раздела");

            this.Namespace("Gkh.Orgs.EfficiencyRating.Analitics.Graphics", "Графики");
            this.Permission("Gkh.Orgs.EfficiencyRating.Analitics.Graphics.View", "Просмотр");
            this.Permission("Gkh.Orgs.EfficiencyRating.Analitics.Graphics.Edit", "Изменение записей");
            this.Permission("Gkh.Orgs.EfficiencyRating.Analitics.Graphics.Delete", "Удаление записей");

            this.Namespace("Gkh.Orgs.EfficiencyRating.Analitics.Constructor", "Конструктор");
            this.Permission("Gkh.Orgs.EfficiencyRating.Analitics.Constructor.View", "Просмотр");
            this.Permission("Gkh.Orgs.EfficiencyRating.Analitics.Constructor.Create", "Создание записей");

            this.Namespace("Gkh.Orgs.EfficiencyRating.Constructor", "Редактор рейтинга эффективности");
            this.CRUDandViewPermissions("Gkh.Orgs.EfficiencyRating.Constructor");
            this.Permission("Gkh.Orgs.EfficiencyRating.Constructor.Code", "Изменение кода");

            this.Namespace("Gkh.Dictionaries.EfficiencyRating", "Рейтинг эффективности");
            this.Namespace("Gkh.Dictionaries.EfficiencyRating.Period", "Периоды рейтинга эффективности");
            this.CRUDandViewPermissions("Gkh.Dictionaries.EfficiencyRating.Period");
            #endregion

            #region Расщепление платежей
            this.Namespace("Gkh.ChargesSplitting", "Расщепление платежей");

            this.Permission("Gkh.ChargesSplitting.Period_View", "Отчетные периоды - Просмотр");
            this.Permission("Gkh.ChargesSplitting.View", "Расщепление платежей - Просмотр");

            this.Namespace("Gkh.ChargesSplitting.ContractPeriodSumm", "Договоры ресурсоснабжения (УО)");
            this.Permission("Gkh.ChargesSplitting.ContractPeriodSumm.ImportExport_View", "Импорт/Экспорт - Просмотр");

            this.Namespace("Gkh.ChargesSplitting.ContractPeriodSumm.Register", "Реестр");
            this.Permission("Gkh.ChargesSplitting.ContractPeriodSumm.Register.Uo_View", "Управляющая организация - Просмотр");
            this.Permission("Gkh.ChargesSplitting.ContractPeriodSumm.Register.Rso_View", "Ресурсоснабжающая организация - Просмотр");

            this.Namespace("Gkh.ChargesSplitting.ContractPeriodSumm.Detail", "Карточка договора");
            this.Permission("Gkh.ChargesSplitting.ContractPeriodSumm.Detail.Print_View", "Печать - Просмотр");
            this.Permission("Gkh.ChargesSplitting.ContractPeriodSumm.Detail.Uo_View", "Управляющая организация - Просмотр");
            this.Permission("Gkh.ChargesSplitting.ContractPeriodSumm.Detail.Rso_View", "Ресурсоснабжающая организация - Просмотр");
            this.Permission("Gkh.ChargesSplitting.ContractPeriodSumm.Detail.Uo_Edit", "Управляющая организация - Редактирование");
            this.Permission("Gkh.ChargesSplitting.ContractPeriodSumm.Detail.Rso_Edit", "Ресурсоснабжающая организация - Редактирование");

            this.Namespace("Gkh.ChargesSplitting.BudgetOrg", "Договоры ресурсоснабжения (Бюджет)");
            this.Permission("Gkh.ChargesSplitting.BudgetOrg.Edit", "Изменение");
            this.Permission("Gkh.ChargesSplitting.BudgetOrg.ImportExport_View", "Импорт/Экспорт - Просмотр");

            this.Namespace("Gkh.ChargesSplitting.FuelEnergyResource", "Договоры ТЭР");
            this.Permission("Gkh.ChargesSplitting.FuelEnergyResource.Edit", "Изменение");
            this.Permission("Gkh.ChargesSplitting.FuelEnergyResource.Import_View", "Импорт - Просмотр");
            #endregion

            this.Namespace<RealityObjectOutdoor>("Gkh.RealityObjectOutdoor.Register", "Реестры");
            this.Namespace("Gkh.RealityObjectOutdoor.Register.Image", "Фото-архив");
            this.CRUDandViewPermissions("Gkh.RealityObjectOutdoor.Register.Image");

            this.Namespace("Gkh.RealityObjectOutdoor.Register.Image.Field", "Поля");
            this.Namespace("Gkh.RealityObjectOutdoor.Register.Image.Field.View", "Просмотр");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.View.DateImage_View", "Дата изображения");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.View.Name_View", "Наименование");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.View.ImagesGroup_View", "Группа");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.View.Period_View", "Период");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.View.WorkCr_View", "Вид работы");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.View.File_View", "Файл");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.View.Description_View", "Описание");

            this.Namespace("Gkh.RealityObjectOutdoor.Register.Image.Field.Edit", "Редактирование");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.DateImage_Edit", "Дата изображения");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.Name_Edit", "Наименование");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.ImagesGroup_Edit", "Группа");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.Period_Edit", "Период");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.WorkCr_Edit", "Вид работы");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.File_Edit", "Файл");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.Description_Edit", "Описание");

            this.Namespace("Gkh.RealityObjectOutdoor.Register.Element", "Элементы двора до благоустройства");
            this.CRUDandViewPermissions("Gkh.RealityObjectOutdoor.Register.Element");

            this.Namespace("Gkh.RealityObjectOutdoor.Register.Element.Field", "Поля");
            this.Namespace("Gkh.RealityObjectOutdoor.Register.Element.Field.View", "Просмотр");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Element.Field.View.Name_View", "Наименование элемента");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Element.Field.View.Measure_View", "Ед. измерения");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Element.Field.View.Volume_View", "Объем");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Element.Field.View.Condition_View", "Состояние");

            this.Namespace("Gkh.RealityObjectOutdoor.Register.Element.Field.Edit", "Редактирование");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Element.Field.Edit.Volume_Edit", "Объем");
            this.Permission("Gkh.RealityObjectOutdoor.Register.Element.Field.Edit.Condition_Edit", "Состояние");

            this.Namespace("GkhCr.ObjectCr.AdditionalParametersViewCreate", "Дополнительные параметры - Просмотр и создание");
            this.Permission("GkhCr.ObjectCr.AdditionalParametersViewCreate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.AdditionalParametersViewCreate.Create", "Создание");

            this.Namespace("GkhCr.ObjectCr.AdditionalParameters", "Дополнительные параметры - Поля");
            this.Namespace("GkhCr.ObjectCr.AdditionalParameters.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.RequestKtsDate_View", "Дата поступления запроса в КТС");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.TechConditionKtsDate_View", "Дата выдачи технических условий");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.TechConditionKtsRecipient_View", "Технические условия выданы (кому)");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.RequestVodokanalDate_View", "Дата поступления запроса в МУП Водоканал");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.TechConditionVodokanalDate_View", "Дата выдачи технических условий");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.TechConditionVodokanalRecipient_View", "Технические условия выданы (кому)");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.EntryForApprovalDate_View", "Дата поступления проекта на согласование");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.ApprovalKtsDate_View", "Дата согласования проекта в КТС");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.ApprovalVodokanalDate_View", "Дата согласования проекта в МУП Водоканал");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.InstallationPercentage_View", "Процент монтажа проекта");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.ClientAccepted_View", "Статус приемки объекта Заказчиком");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.ClientAcceptedChangeDate_View", "Дата изменения статуса приемки объекта заказчиком");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.InspectorAccepted_View", "Статус приемки объекта инспектором Ростехнадзора");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.View.InspectorAcceptedChangeDate_View", "Дата изменения статуса приемки объекта инспектором");

            this.Namespace("GkhCr.ObjectCr.AdditionalParameters.Edit", "Редактирование");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.RequestKtsDate_Edit", "Дата поступления запроса в КТС");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.TechConditionKtsDate_Edit", "Дата выдачи технических условий");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.TechConditionKtsRecipient_Edit", "Технические условия выданы (кому)");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.RequestVodokanalDate_Edit", "Дата поступления запроса в МУП Водоканал");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.TechConditionVodokanalDate_Edit", "Дата выдачи технических условий");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.TechConditionVodokanalRecipient_Edit", "Технические условия выданы (кому)");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.EntryForApprovalDate_Edit", "Дата поступления проекта на согласование");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.ApprovalKtsDate_Edit", "Дата согласования проекта в КТС");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.ApprovalVodokanalDate_Edit", "Дата согласования проекта в МУП Водоканал");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.InstallationPercentage_Edit", "Процент монтажа проекта");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.ClientAccepted_Edit", "Статус приемки объекта Заказчиком");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.ClientAcceptedChangeDate_Edit", "Дата изменения статуса приемки объекта заказчиком");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.InspectorAccepted_Edit", "Статус приемки объекта инспектором Ростехнадзора");
            this.Permission("GkhCr.ObjectCr.AdditionalParameters.Edit.InspectorAcceptedChangeDate_Edit", "Дата изменения статуса приемки объекта инспектором");
            this.Permission("GkhCr.ObjectCr.AdditionalParametersViewCreate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.AdditionalParametersViewCreate.Create", "Создание");

            this.Namespace("GkhCr.SpecialObjectCr.AdditionalParametersViewCreate", "Дополнительные параметры - Просмотр и создание");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParametersViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParametersViewCreate.Create", "Создание");

            this.Namespace("GkhCr.SpecialObjectCr.AdditionalParameters", "Дополнительные параметры - Поля");
            this.Namespace("GkhCr.SpecialObjectCr.AdditionalParameters.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.RequestKtsDate_View", "Дата поступления запроса в КТС");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.TechConditionKtsDate_View", "Дата выдачи технических условий");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.TechConditionKtsRecipient_View", "Технические условия выданы (кому)");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.RequestVodokanalDate_View", "Дата поступления запроса в МУП Водоканал");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.TechConditionVodokanalDate_View", "Дата выдачи технических условий");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.TechConditionVodokanalRecipient_View", "Технические условия выданы (кому)");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.EntryForApprovalDate_View", "Дата поступления проекта на согласование");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.ApprovalKtsDate_View", "Дата согласования проекта в КТС");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.ApprovalVodokanalDate_View", "Дата согласования проекта в МУП Водоканал");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.InstallationPercentage_View", "Процент монтажа проекта");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.ClientAccepted_View", "Статус приемки объекта Заказчиком");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.ClientAcceptedChangeDate_View", "Дата изменения статуса приемки объекта заказчиком");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.InspectorAccepted_View", "Статус приемки объекта инспектором Ростехнадзора");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.View.InspectorAcceptedChangeDate_View", "Дата изменения статуса приемки объекта инспектором");

            this.Namespace("GkhCr.SpecialObjectCr.AdditionalParameters.Edit", "Редактирование");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.RequestKtsDate_Edit", "Дата поступления запроса в КТС");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.TechConditionKtsDate_Edit", "Дата выдачи технических условий");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.TechConditionKtsRecipient_Edit", "Технические условия выданы (кому)");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.RequestVodokanalDate_Edit", "Дата поступления запроса в МУП Водоканал");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.TechConditionVodokanalDate_Edit", "Дата выдачи технических условий");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.TechConditionVodokanalRecipient_Edit", "Технические условия выданы (кому)");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.EntryForApprovalDate_Edit", "Дата поступления проекта на согласование");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.ApprovalKtsDate_Edit", "Дата согласования проекта в КТС");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.ApprovalVodokanalDate_Edit", "Дата согласования проекта в МУП Водоканал");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.InstallationPercentage_Edit", "Процент монтажа проекта");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.ClientAccepted_Edit", "Статус приемки объекта Заказчиком");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.ClientAcceptedChangeDate_Edit", "Дата изменения статуса приемки объекта заказчиком");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.InspectorAccepted_Edit", "Статус приемки объекта инспектором Ростехнадзора");
            this.Permission("GkhCr.SpecialObjectCr.AdditionalParameters.Edit.InspectorAcceptedChangeDate_Edit", "Дата изменения статуса приемки объекта инспектором");

            //Интеграция с ЕГСО ОВ
            this.Namespace("Administration.OutsideSystemIntegrations.EgsoIntegration", "Интеграция с ЕГСО ОВ");
            this.Permission("Administration.OutsideSystemIntegrations.EgsoIntegration.View", "Просмотр");

            #region ФССП
            
            this.Namespace("Clw.Fssp", "ФССП");

            this.Namespace("Clw.Fssp.CourtOrderGku", "Реестр судебных распоряжений по ЖКУ");
            this.Permission("Clw.Fssp.CourtOrderGku.View", "Просмотр");
            
            this.Namespace("Clw.Fssp.CourtOrderGku.Litigation", "Реестр делопроизводств");
            this.Permission("Clw.Fssp.CourtOrderGku.Litigation.Edit", "Редактирование");

            this.Namespace("Clw.Fssp.CourtOrderGku.UploadDownloadInfo", "Реестр загрузки/выгрузки информации");
            this.Permission("Clw.Fssp.CourtOrderGku.UploadDownloadInfo.Add", "Добавление");
            this.Permission("Clw.Fssp.CourtOrderGku.UploadDownloadInfo.View", "Отображать загрузки всех пользователей");
            
            this.Namespace("Clw.Fssp.CourtOrderGku.AddressMatchingRegistry", "Реестр сопоставления адресов");
            this.Permission("Clw.Fssp.CourtOrderGku.AddressMatchingRegistry.Match", "Сопоставление адресов");
            this.Permission("Clw.Fssp.CourtOrderGku.AddressMatchingRegistry.Mismatch", "Удаление связей");
            this.Permission("Clw.Fssp.CourtOrderGku.AddressMatchingRegistry.ShowAll", "Отображать адреса всех пользователей");

            this.Namespace("Clw.Fssp.PaymentGku", "Оплаты по ЖКУ");
            this.Permission("Clw.Fssp.PaymentGku.View", "Просмотр");
            
            #endregion

            #region Виджеты
            this.Permission("Widget.Debt", "Панель запросов о задолженности");
            this.Permission("Widget.SendingDataResult", "Панель результатов отправки данных с ГИС ЖКХ");
            this.Permission("Widget.SoprAppealsStatistic", "Статистика по реестру обращений ГЖИ (связанные с СОПР)");
            this.Permission("Widget.SoprAppealDetailsStatistic", "Обращения СОПР");
            #endregion
            
            this.Permission("Gkh.Orgs.Contragent.Field.IncludeInSopr_View", "Включен в СОПР - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.IncludeInSopr_Edit", "Включен в СОПР - Изменение");

            this.Namespace("Administration.TariffDataIntegrationLog", "Логи интеграции данных по тарифам");
            this.Permission("Administration.TariffDataIntegrationLog.View", "Просмотр");
        }
    }
}