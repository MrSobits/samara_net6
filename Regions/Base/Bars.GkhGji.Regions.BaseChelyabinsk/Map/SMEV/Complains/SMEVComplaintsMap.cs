namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVComplaintsMap : BaseEntityMap<SMEVComplaints>
    {
        
        public SMEVComplaintsMap() : 
                base("", "SMEV_CH_COMPLAINTS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").Fetch();
            Property(x => x.ComplaintId, "Ид жалобы").Column("COMPLAINT_ID");
            Property(x => x.Number, "Номер проверки").Column("CMP_NUMBER");
            Property(x => x.ComplaintState, "Номер проверки").Column("TOR_STATE");
            Property(x => x.CommentInfo, "Пояснительный текст к жалобе").Column("COMMENT_INFO");
            Property(x => x.EsiaOid, "Идентификатор заявителя в ЕСИА").Column("ESIA_OID");
            Property(x => x.RequesterRole, "Тип заявителя запроса").Column("REQUESTER_ROLE");
            Reference(x => x.RequesterContragent, "Контрагент заявитель").Column("CONTRAGENT_ID");
            Property(x => x.RequesterFIO, "ФИО заявитель").Column("REQUESTER_FIO");
            Property(x => x.IdentityDocumentType, "Тип документа").Column("DOC_FL_TYPE");
            Property(x => x.DocSeries, "Серия").Column("DOC_FL_SERIES");
            Property(x => x.DocNumber, "Номер").Column("DOC_FL_NUMBER");
            Property(x => x.INNFiz, "Серия").Column("INN_FL");
            Property(x => x.SNILS, "Номер").Column("SNILS");
            Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
            Property(x => x.BirthAddress, "Место рождения").Column("BIRTH_PLACE");
            Property(x => x.Gender, "Пол").Column("GENDER");
            Property(x => x.Nationality, "Национальность").Column("NATION");
            Property(x => x.RegAddess, "Место регистрации").Column("REG_ADDRESS");
            Property(x => x.Email, "e-mail").Column("EMAIL");
            Property(x => x.MobilePhone, "Мобилка").Column("MOBILE");
            Property(x => x.Ogrnip, "ОГРНИП").Column("ORGNIP");
            Property(x => x.LegalFullName, "Полное наименование").Column("LEGAL_NAME");
            Property(x => x.Ogrn, "ОГРН").Column("OGRN");
            Property(x => x.Inn, "ИНН").Column("INN");
            Property(x => x.LegalAddress, "Юрилический адрес").Column("LEGAL_ADDRESS");
            Property(x => x.WorkingPosition, "Должность").Column("POSITION");
            Property(x => x.OrderId, "Идентификатор на ЕПГУ").Column("ORDERID");
            Property(x => x.RevokeFlag, "Флаг отзыва жалобы").Column("IS_REVOKE");
            Property(x => x.ComplaintDate, "Дата отправки жалобы с ЕПГУ").Column("COMPL_DATE");
            Property(x => x.Okato, "ОКАТО").Column("OKATO");
            Property(x => x.AppealNumber, "Номер проверки").Column("APPEAL_NUMBER");
            Property(x => x.TypeAppealDecision, "Вид обжалуемого решения").Column("TYPE_APPEAL_DECISION");
            Property(x => x.LifeEvent, "Жизненная ситуация").Column("EVENT");
            Property(x => x.RequestDate, "Дата запроса").Column("REQUEST_DATE").NotNull();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.DecisionReason, "Результат").Column("DEC_REASON");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Property(x => x.Request_ID, "Серия").Column("REQUESTID");
            Property(x => x.Entry_ID, "Номер").Column("ENTRY_ID");
            Reference(x => x.SMEVComplaintsDecision, "State").Column("DEC_ID");
            Reference(x => x.State, "State").Column("STATE_ID");
            Reference(x => x.FileInfo, "File").Column("FILE_INFO_ID");
        }
    }
}
