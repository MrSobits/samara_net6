namespace Bars.Gkh.Map.Contragent
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    /// <summary>
    /// Маппинг для "Контактная информация по контрагенту"
    /// </summary>
    public class ContragentContactMap : BaseImportableEntityMap<ContragentContact>
    {
        
        /// <summary>
        /// .ctor
        /// </summary>
        public ContragentContactMap() : 
                base("Контактная информация по контрагенту", "GKH_CONTRAGENT_CONTACT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Annotation, "Примечание").Column("ANNOTATION").Length(500);
            Property(x => x.Gender, "Пол").Column("GENDER").DefaultValue("0");
            Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
            Property(x => x.DateEndWork, "Дата окончания работы").Column("DATE_END_WORK");
            Property(x => x.DateStartWork, "Дата начала работы").Column("DATE_START_WORK");
            Property(x => x.Email, "Email").Column("EMAIL").Length(50);
            Property(x => x.Name, "Имя").Column("NAME").Length(100).NotNull();
            Property(x => x.Surname, "Фамилия").Column("SURNAME").Length(100).NotNull();
            Property(x => x.Patronymic, "Отчество").Column("PATRONYMIC").Length(100).NotNull();
            Property(x => x.FullName, "ФИО").Column("FULL_NAME").Length(300).NotNull();
            Property(x => x.Phone, "Телефон").Column("PHONE").Length(50);
            Property(x => x.OrderDate, "Дата приказа").Column("ORDER_DATE");
            Property(x => x.OrderName, "Наименование приказа").Column("ORDER_NAME").Length(100);
            Property(x => x.OrderNum, "Номер приказа").Column("ORDER_NUM").Length(50);
            Property(x => x.NameGenitive, "Имя, родительный падеж").Column("NAME_GENETIVE").Length(100);
            Property(x => x.SurnameGenitive, "Фамилия, родительский падеж").Column("SURNAME_GENETIVE").Length(100);
            Property(x => x.PatronymicGenitive, "Отчество, родительский падеж").Column("PATRONYMIC_GENETIVE").Length(100);
            Property(x => x.NameDative, "Имя, Дательный падеж").Column("NAME_DATIVE").Length(100);
            Property(x => x.SurnameDative, "Фамилия, Дательный падеж").Column("SURNAME_DATIVE").Length(100);
            Property(x => x.PatronymicDative, "Отчество, Дательный падеж").Column("PATRONYMIC_DATIVE").Length(100);
            Property(x => x.NameAccusative, "Имя, Винительный падеж").Column("NAME_ACCUSATIVE").Length(100);
            Property(x => x.SurnameAccusative, "Фамилия, Винительный падеж").Column("SURNAME_ACCUSATIVE").Length(100);
            Property(x => x.PatronymicAccusative, "Отчество, Винительный падеж").Column("PATRONYMIC_ACCUSATIVE").Length(100);
            Property(x => x.NameAblative, "Имя, Творительный падеж").Column("NAME_ABLATIVE").Length(100);
            Property(x => x.SurnameAblative, "Фамилия, Творительный падеж").Column("SURNAME_ABLATIVE").Length(100);
            Property(x => x.PatronymicAblative, "Отчество, Творительный падеж").Column("PATRONYMIC_ABLATIVE").Length(100);
            Property(x => x.NamePrepositional, "Имя, Предложный падеж").Column("NAME_PREPOSITIONAL").Length(100);
            Property(x => x.SurnamePrepositional, "Фамилия, Предложный падеж").Column("SURNAME_PREPOSITIONAL").Length(100);
            Property(x => x.PatronymicPrepositional, "Отчество, Предложный падеж").Column("PATRONYMIC_PREPOSITIONAL").Length(100);
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Reference(x => x.Position, "Должность").Column("POSITION_ID").Fetch();
            Property(x => x.Snils, "СНИЛС").Column("SNILS").Length(50);
            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            Property(x => x.FLDocIssuedDate, "FLDocIssuedDate").Column("FLDOC_ISSUED_DATE");
            Property(x => x.FLDocSeries, "FLDocSeries").Column("FLDOC_SERIES");
            Property(x => x.FLDocNumber, "FLDocNumber").Column("FLDOC_NUMBER");
            Property(x => x.FLDocIssuedBy, "FLDocIssuedBy").Column("ISSUEDBY");
        }
    }
}