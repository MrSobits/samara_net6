namespace Bars.GkhGji.Regions.Tatarstan.Permissions
{
    using Bars.Gkh.DomainService;

    /// <summary>
    /// Обязательность полей
    /// </summary>
    public class GkhGjiFieldRequirementMap : FieldRequirementMap
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public GkhGjiFieldRequirementMap()
        {
            this.Requirement("GkhGji.Inspection.BaseJurPerson.Field.InspectionBaseType", "Основание проверки");

            this.Namespace("GkhGji.DocumentReestrGji.Disposal", "Распоряжение");
            this.Namespace("GkhGji.DocumentReestrGji.Disposal.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitStart", "Время с");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitEnd", "Время по");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcDate", "Дата");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcNum", "Номер документа");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcDateLatter", "Дата исходящего письма");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcNumLatter", "Номер исходящего письма");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcSent", "Уведомление передано");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcObtained", "Уведомление получено");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.CountDays", "Срок проверки (количество дней)");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.CountHours", "Срок проверки (количество часов)");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.Prosecutor", "Наименование прокуратуры");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.PlannedActions", "Запланированные действия");

            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.DocumentPlace", "Место составления");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.DocumentTime", "Время составления акта");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintState", "Статус ознакомления с результатами проверки");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.RefusedToAcquaintPerson", "ФИО должностного лица, отказавшегося от ознакомления с актом проверки");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintedPerson", "ФИО должностного лица, ознакомившегося с актом проверки");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintedDate", "Дата ознакомления");

            this.Namespace("GkhGji.DocumentReestrGji.Protocol", "Протокол");
            this.Namespace("GkhGji.DocumentReestrGji.Protocol.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.FormatPlace", "Место и время составления протокола");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.FormatDate", "Дата вручения (регистрации) уведомления");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.NotifNumber", "Номер регистрации");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.DateOfProceedings", "Дата и время расмотрения дела");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.ProceedingCopyNum", "Количество экземпляров");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.ProceedingsPlace", "Место рассмотрения дела");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.Remarks", "Замечания со стороны нарушителя");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.Surname", "Фамилия");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.Name", "Имя");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.Patronymic", "Отчество");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.BirthDate", "Дата рождения");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.BirthPlace", "Место рождения");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.FactAddress", "Фактический адрес проживания");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.SerialAndNumber", "Серия и номер паспорта");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.IssueDate", "Дата выдачи");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.IssuingAuthority", "Кем выдан");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.Company", "Место работы, должность");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.Snils", "СНИЛС");

            this.Namespace("GkhGji.DocumentReestrGji.Resolution", "Постановление");
            this.Namespace("GkhGji.DocumentReestrGji.Resolution.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.PatternDict", "Шаблон ГИС ГМП");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.Municipality", "Местонахождение");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.PenaltyAmount", "Сумма штрафа");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.SurName", "Фамилия");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.Name", "Имя");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.Patronymic", "Отчество");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.BirthDate", "Дата рождения");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.BirthPlace", "Место рождения");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.Address", "Фактический адрес проживания");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.SerialAndNumber", "Серия и номер паспорта");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.IssueDate", "Дата выдачи");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.IssuingAuthority", "Кем выдан");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.Company", "Место работы, должность");

            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.Snils", "СНИЛС");
            this.Requirement("GkhGji.DocumentReestrGji.Resolution.Field.SanctionsDuration", "Срок накладываемых санкций");

            this.Namespace("GkhGji.DocumentReestrGji.ProtoclMvd", "Протокол МВД");
            this.Namespace("GkhGji.DocumentReestrGji.ProtoclMvd.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.SurName", "Фамилия");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.Name", "Имя");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.Patronymic", "Отчество");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.BirthDate", "Дата рождения");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.BirthPlace", "Место рождения");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.Address", "Фактический адрес проживания");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.SerialAndNumber", "Серия и номер паспорта");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.IssueDate", "Дата выдачи");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.IssuingAuthority", "Кем выдан");
            this.Requirement("GkhGji.DocumentReestrGji.ProtoclMvd.Field.Company", "Место работы, должность");

            this.Namespace("GkhGji.DocumentReestrGji.Decision", "Решение");
            this.Namespace("GkhGji.DocumentReestrGji.Decision.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.Decision.Field.SubmissionDate", "Дата направления требования о предоставлении документов");
            this.Requirement("GkhGji.DocumentReestrGji.Decision.Field.ReceiptDate", "Дата получения документов во исполнение требования");
            this.Namespace("GkhGji.DocumentReestrGji.Decision.Field.Annex", "Приложения");
            this.Requirement("GkhGji.DocumentReestrGji.Decision.Field.Annex.DocumentType", "Тип документа");

            this.Namespace("GkhGji.Inspection.BaseStatement", "Обращение граждан");
            this.Namespace("GkhGji.Inspection.BaseStatement.MainInfo", "Основная информация");
            this.Namespace("GkhGji.Inspection.BaseStatement.MainInfo.Field", "Поля");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.RegistrationNumber", "Учетный номер проверки в едином реестре проверок");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.RegistrationNumberDate", "Дата присвоения учетного номера");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.CheckDayCount", "Срок проверки (количество дней)");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.CheckDate", "Дата проверки");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.InnOfficial", "ИНН должностного лица");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.InnIndividual", "ИНН физ. лица");

            this.Namespace("GkhGji.Inspection.BaseProsClaim", "Требование прокуратуры");
            this.Namespace("GkhGji.Inspection.BaseProsClaim.MainInfo", "Основная информация");
            this.Namespace("GkhGji.Inspection.BaseProsClaim.MainInfo.Field", "Поля");
            this.Requirement("GkhGji.Inspection.BaseProsClaim.MainInfo.Field.CheckDate", "Дата проверки");
            this.Requirement("GkhGji.Inspection.BaseProsClaim.MainInfo.Field.InnOfficial", "ИНН должностного лица");
            this.Requirement("GkhGji.Inspection.BaseProsClaim.MainInfo.Field.InnIndividual", "ИНН физ. лица");

            this.Namespace("GkhGji.Inspection.WarningInspection", "Предостережение");
            this.Namespace("GkhGji.Inspection.WarningInspection.Field", "Поля");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.Date", "Дата");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.InspectionNumber", "Номер");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.TypeJurPerson", "Тип юридического лица");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.PersonInspection", "Объект проверки");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.Contragent", "Юридическое лицо");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.PhysicalPerson", "Физическое лицо");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.RegistrationNumber", "Учетный номер проверки в едином реестре");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.RegistrationNumberDate", "Дата присвоения учетного номера");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.CheckDayCount", "Срок проверки (количество дней)");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.CheckDate", "Дата проверки");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.Inspectors", "Инспекторы");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.InspectionBasis", "Основание");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.SourceFormType", "Форма поступления");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.ControlType", "Вид контроля");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.InnOfficial", "ИНН должностного лица");
            this.Requirement("GkhGji.Inspection.WarningInspection.Field.InnIndividual", "ИНН физ. лица");

            this.Namespace("GkhGji.Inspection.WarningInspection.Document", "Документ");
            this.Namespace("GkhGji.Inspection.WarningInspection.Document.Field", "Поля");
            this.Requirement("GkhGji.Inspection.WarningInspection.Document.Field.DocumentName", "Наименование документа");
            this.Requirement("GkhGji.Inspection.WarningInspection.Document.Field.DocumentNumber", "Номер документа");
            this.Requirement("GkhGji.Inspection.WarningInspection.Document.Field.DocumentDate", "Дата документа");
            this.Requirement("GkhGji.Inspection.WarningInspection.Document.Field.File", "Файл");

            this.Namespace("GkhGji.Inspection.WarningInspection.Basis", "Основание");
            this.Namespace("GkhGji.Inspection.WarningInspection.Basis.Field", "Поля");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.Field.BaseWarning", "Основание предостережения");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.Field.TakingDate", "Срок принятия мер о соблюдении требований");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.Field.File", "Документ основания");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.Field.ResultText", "Результат предостережения");

            this.Namespace("GkhGji.Inspection.WarningInspection.Basis.Officials", "Должностные лица");
            this.Namespace("GkhGji.Inspection.WarningInspection.Basis.Officials.Field", "Поля");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.Officials.Field.Autor", "ДЛ, вынесшее предостережение");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.Officials.Field.Executant", "Ответственный за исполнение");

            this.Namespace("GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution", "Уведомление о направлении предостережения");
            this.Namespace("GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field", "Поля");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutDate", "Дата");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutNum", "Номер документа");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutDateLatter", "Дата исходящего письма");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutNumLatter", "Номер исходящего письма");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOfCaution.Field.NcOutSent", "Уведомление передано");

            this.Namespace("GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination", "Уведомление об устранении нарушений");
            this.Namespace("GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field", "Поля");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInDate", "Дата");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInNum", "Номер документа");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInDateLatter", "Дата исходящего письма");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInNumLatter", "Номер исходящего письма");
            this.Requirement("GkhGji.Inspection.WarningInspection.Basis.NoticeOnElimination.Field.NcInRecived", "Уведомление получено");

            this.Namespace("GkhGji.AppealCits.Field", "Поля");
            this.Requirement("GkhGji.AppealCits.Field.IsPrelimentaryCheck", "Проведена предварительная проверка");

            #region Протокол ГЖИ РТ
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji", "Протоколы ГЖИ РТ");
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields", "Поля");
            this.ProtocolRequisitesRequire();
            this.AnnexRequire();
            #endregion

            #region Постановление суда
            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji", "Постановление суда");
            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields", "Поля");
            this.ResolutionRequisitesRequire();
            #endregion

            #region Задание КНМ без взаимодействия с контролируемыми лицами
            this.Namespace("GkhGji.DocumentReestrGji.TaskActionIsolated", "Задание КНМ без взаимодействия с контролируемыми лицами");

            this.Namespace("GkhGji.DocumentReestrGji.TaskActionIsolated.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.TaskActionIsolated.Field.PlannedAction", "Запланированные действия");
            #endregion

            #region Мотивированное представление по обращению гражданина
            this.Namespace("GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits", "Мотивированное представление по обращению гражданина");

            this.Namespace("GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.DocumentDate", "Дата");
            this.Requirement("GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.DocumentNumber", "Номер документа");
            this.Requirement("GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.PresentationType", "Вид мотивированного представления");
            this.Requirement("GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.Inspectors", "Инспектор");
            this.Requirement("GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.Official", "Должностное лицо");
            this.Requirement("GkhGji.DocumentReestrGji.MotivatedPresentationAppealCits.Field.ResultType", "Результат");
            #endregion
            
            #region Модуль "Обращения граждан"
            this.Namespace("CitizenAppealModule", "Модуль Обращения граждан");
            this.Namespace("CitizenAppealModule.RapidResponseSystem", "Система оперативного реагирования");
            this.Namespace("CitizenAppealModule.RapidResponseSystem.Field", "Поля");
            this.Requirement("CitizenAppealModule.RapidResponseSystem.Field.ResponseDate", "Дата ответа");
            this.Requirement("CitizenAppealModule.RapidResponseSystem.Field.Theme", "Тема");
            this.Requirement("CitizenAppealModule.RapidResponseSystem.Field.Response", "Ответ");
            this.Requirement("CitizenAppealModule.RapidResponseSystem.Field.CarriedOutWork", "Проведенные работы");
            this.Requirement("CitizenAppealModule.RapidResponseSystem.Field.ResponseFile", "Файл");
            #endregion
        }

        private void ProtocolRequisitesRequire()
        {
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites", "Реквизиты");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DocumentNumber", "Номер протокола");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DocumentDate", "Дата протокола");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Municipality", "Муниципальное образование");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DateOffense", "Дата правонарушения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.ZonalInspection", "Орган ГЖИ, оформивший протокол");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.TimeOffense", "Время правонарушения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.CheckInspectors", "Инспекторы");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DateSupply", "Дата поступления в ГЖИ");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Pattern", "Шаблон ГИС ГМП");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.AnnulReason", "Причина аннулирования");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.UpdateReason", "Причина изменения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Sanction", "Вид санкции");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.PenaltyAmount", "Сумма штрафа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DateTransferSsp", "Дата передачи в ССП");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.TerminationDocumentNum", "Номер документа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.SurName", "Фамилия");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.CitizenshipType", "Гражданство");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Name", "Имя");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Citizenship", "Код страны");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Patronymic", "Отчество");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.IdentityDocumentType", "Тип документа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.SerialAndNumberDocument", "Серия и номер документа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.BirthDate", "Дата рождения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.IssueDate", "Дата выдачи");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.BirthPlace", "Место рождения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.IssuingAuthority", "Кем выдан");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Address", "Фактический адрес проживания");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Company", "Место работы, должность, адрес");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.MaritalStatus", "Семейное положение");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.RegistrationAddress", "Адрес регистрации");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DependentCount", "Количество иждивенцев");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Salary", "Размер зарплаты (пенсии, стипендии) в руб.");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DelegateFio", "ФИО");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.ProcurationNumber", "Доверенность номер");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.ProcurationDate", "Доверенность дата");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DelegateCompany", "Место работы, должность");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.ProtocolExplanation", "Объяснение и замечания по содержанию протокола");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.AccusedExplanation", "Объяснение лица, в отношении которого возбуждено дело");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.TribunalName", "Суд");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.OffenseAddress", "Адрес");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Citizenship", "Код страны");
        }

        private void ResolutionRequisitesRequire()
        {
            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites", "Реквизиты");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DocumentNumber", "Номер протокола");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DocumentDate", "Дата протокола");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DocumentNum", "Номер");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.LiteralNum", "Буквенный подномер");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DocumentSubNum", "Подномер");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DeliveryDate", "Дата вручения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.TypeInitiativeOrg", "Кем вынесено");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.SectorNumber", "Номер участка");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.FineMunicipality", "МО получателя штрафа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Inspector", "Должностное лицо");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Municipality", "Местонахождение");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Sanction", "Вид санкции");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.PenaltyAmount", "Сумма штрафа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DateTransferSsp", "Дата передачи в ССП");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.TerminationDocumentNum", "Номер документа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.SurName", "Фамилия");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.CitizenshipType", "Гражданство");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Name", "Имя");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Citizenship", "Код страны");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Patronymic", "Отчество");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.IdentityDocumentType", "Тип документа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.SerialAndNumberDocument", "Серия и номер документа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.BirthDate", "Дата рождения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.IssueDate", "Дата выдачи");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.BirthPlace", "Место рождения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.IssuingAuthority", "Кем выдан");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Address", "Фактический адрес проживания");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Company", "Место работы, должность, адрес");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.MaritalStatus", "Семейное положение");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.RegistrationAddress", "Адрес регистрации");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DependentCount", "Количество иждивенцев");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Salary", "Размер зарплаты (пенсии, стипендии) в руб.");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DelegateFio", "ФИО");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.ProcurationNumber", "Доверенность номер");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.ProcurationDate", "Доверенность дата");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DelegateCompany", "Место работы, должность");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Citizenship", "Код страны");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.ImprovingFact", "Смягчающие вину обстоятельства");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DisimprovingFact", "Отягчающие вину обстоятельства");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.RulinFio", "ФИО");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.RulingDate", "Дата");
            this.Requirement("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.RulingNumber", "Номер");
        }

        private void AnnexRequire()
        {
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes", "Приложения");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes.Name", "Наименование");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes.DocumentDate", "Дата документа");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes.File", "Файл");
            this.Requirement("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes.Description", "Описание");
        }
    }
}
