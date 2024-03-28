namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using System;

    public class SMEVComplaints : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Статус жалобы
        /// </summary>
        public virtual string ComplaintState { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Ид жалобы
        /// </summary>
        public virtual string ComplaintId { get; set; }

        /// <summary>
        /// Номер проверки
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Пояснительный текст к жалобе
        /// </summary>
        public virtual string CommentInfo { get; set; }

        //Данные о заявителе, подающем жалобу applicantData

        /// <summary>
        /// Идентификатор заявителя в ЕСИА
        /// </summary>
        public virtual string EsiaOid { get; set; }

        /// <summary>
        /// Тип заявителя запроса
        /// </summary>
        public virtual RequesterRole RequesterRole { get; set; }

        /// <summary>
        /// Контрагент заявитель
        /// </summary>
        public virtual Contragent RequesterContragent { get; set; }

        /// <summary>
        /// ФИО заявитель
        /// </summary>
        public virtual string RequesterFIO { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual IdentityDocumentType IdentityDocumentType { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        public virtual string DocSeries { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// ИНН Заявителя
        /// </summary>
        public virtual string INNFiz { get; set; }

        /// <summary>
        /// ИНН Заявителя
        /// </summary>
        public virtual string SNILS { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthAddress { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public virtual Gender Gender { get; set; }

        /// <summary>
        ///Национальность
        /// </summary>
        public virtual string Nationality { get; set; }

        /// <summary>
        /// Место регистрации
        /// </summary>
        public virtual string RegAddess { get; set; }

        /// <summary>
        /// e-mail
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Мобилка
        /// </summary>
        public virtual string MobilePhone { get; set; }

        //Данные о заявителе, подающем жалобу applicantBusinessmanType

        /// <summary>
        /// ОГРНИП
        /// </summary>
        public virtual string Ogrnip { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public virtual string LegalFullName { get; set; }

        /// <summary>
        ///ОГРН
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Юрилический адрес
        /// </summary>
        public virtual string LegalAddress { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string WorkingPosition { get; set; }

        //Данные о заявителе, подающем жалобу epguDataType
        /// <summary>
        /// Идентификатор на ЕПГУ
        /// </summary>
        public virtual string OrderId { get; set; }

        /// <summary>
        /// Флаг отзыва жалобы
        /// </summary>
        public virtual bool RevokeFlag { get; set; }

        /// <summary>
        /// Дата отправки жалобы с ЕПГУ
        /// </summary>
        public virtual string ComplaintDate { get; set; }

        /// <summary>
        /// ОКАТО региона, в котором находится орган, на который сформирована жалоба
        /// </summary>
        public virtual string Okato { get; set; }

        /// <summary>
        /// Номер проверки
        /// </summary>
        public virtual string AppealNumber { get; set; }

        /// <summary>
        /// Вид обжалуемого решения
        /// </summary>
        public virtual string TypeAppealDecision { get; set; }

        /// <summary>
        /// Жизненная ситуация
        /// </summary>
        public virtual string LifeEvent { get; set; }

        /// <summary>
        /// Дата поступления
        /// </summary>
        public virtual DateTime RequestDate { get; set; }

        /// <summary>
        /// Причина текущего решения SMEVComplaintsDecision
        /// </summary>
        public virtual SMEVComplaintsDecision SMEVComplaintsDecision { get; set; }

        /// <summary>
        /// Причина текущего решения
        /// </summary>
        public virtual string DecisionReason { get; set; }

        /// <summary>
        /// Обоснование принятого решения
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string Request_ID { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string Entry_ID { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}
