namespace Bars.GkhGji.Entities
{
    using System;
    using B4.Modules.States;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Ответ по обращению
    /// </summary>
    public class AppealCitsAnswer : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Проверка
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Executor { get; set; }

        /// <summary>
        /// Подписант
        /// </summary>
        public virtual Inspector Signer { get; set; }

        /// <summary>
        /// Адресат
        /// </summary>
        public virtual RevenueSourceGji Addressee { get; set; }

        /// <summary>
        /// Содержание ответа
        /// </summary>
        public virtual AnswerContentGji AnswerContent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileDoc { get; set; }

        /// <summary>
        /// Подписанный файл
        /// </summary>
        public virtual FileInfo SignedFile { get; set; }

        /// <summary>
        /// Признак того что ответ отправлен куда нужно 
        /// </summary>
        public virtual bool IsMoved { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Год ответа (необходим для формирования номера документа)
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Дата исполнения (направления ответа)
        /// </summary>
        public virtual DateTime? ExecDate { get; set; }

        /// <summary>
        /// Дата продления срока исполнения
        /// </summary>
        public virtual DateTime? ExtendDate { get; set; }

        /// <summary>
        /// Признак об успешной/не успешной загрузки в ЕАИС ОГ
        /// </summary>
        public virtual bool? IsUploaded { get; set; }

        /// <summary>
        /// Дополнительная информация об не успешной загрузки в ЕАИС ОГ
        /// </summary>
        public virtual string AdditionalInfo { get; set; }

        /// <summary>
        /// Результат рассмотрения
        /// </summary>
        public virtual ConcederationResult ConcederationResult { get; set; }

        /// <summary>
        ///  Вид проверки факта
        /// </summary>
        public virtual FactCheckingType FactCheckingType { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Attachment GUID
        /// </summary>
        public virtual string GisGkhAttachmentGuid { get; set; }

        /// <summary>
        /// Hash
        /// </summary>
        public virtual string Hash { get; set; }

        /// <summary>
        /// Контрагент для перенаправления обращения
        /// </summary>
        public virtual Contragent RedirectContragent { get; set; }

        /// <summary>
        /// Контрагент для перенаправления обращения
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        ///порядковый номер ответа для АС ДОУ(Воронеж)
        /// </summary>
        public virtual string SerialNumber { get; set; }

        /// <summary>
        ///Тип ответа
        /// </summary>
        public virtual TypeAppealAnswer TypeAppealAnswer { get; set; }

        /// <summary>
        ///Тип ответа
        /// </summary>
        public virtual TypeAppealFinalAnswer TypeAppealFinalAnswer { get; set; }

        /// <summary>
        /// Зарегестрирован
        /// </summary>
        public virtual bool Registred { get; set; }

        /// <summary>
        /// Отправлен по email
        /// </summary>
        public virtual bool Sended { get; set; }

        /// <summary>
        /// Отправлен в СЭД
        /// </summary>
        public virtual bool SendedToEdm { get; set; }
        
        /// <summary>
        /// Ответ отправлен
        /// </summary>
        public virtual YesNoNotSet IsSent { get; set; }
    }
}