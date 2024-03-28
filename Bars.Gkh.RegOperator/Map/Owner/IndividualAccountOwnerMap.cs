namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>Маппинг для "Абонент - физ.лицо"</summary>
    public class IndividualAccountOwnerMap : JoinedSubClassMap<IndividualAccountOwner>
    {
        public IndividualAccountOwnerMap()
            :
                base("Абонент - физ.лицо", "REGOP_INDIVIDUAL_ACC_OWN")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.FirstName, "Имя").Column("FIRST_NAME").Length(100).NotNull();
            this.Property(x => x.Surname, "Фамилия").Column("SURNAME").Length(100);
            this.Property(x => x.SecondName, "Отчество").Column("SECOND_NAME").Length(100);
            this.Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
            this.Property(x => x.BirthPlace, "Место рождения").Column("BIRTH_PLACE").Length(300);
            this.Property(x => x.IdentityType, "Тип документа").Column("ID_TYPE");
            this.Property(x => x.IdentitySerial, "Серия документа").Column("ID_SERIAL").Length(250);
            this.Property(x => x.IdentityNumber, "Номер документа").Column("ID_NUM").Length(250);
            this.Property(x => x.AddressOutsideSubject, "Адрес за пределами субъекта").Column("ADDRESS_OUT_SUBJECT").Length(500);
            this.Property(x => x.Email, "Электронный адрес").Column("EMAIL").Length(250);
            this.Property(x => x.Gender, "Пол").Column("GENDER");
            this.Property(x => x.DateDocumentIssuance, "Дата выдачи документа").Column("DATE_DOCUMENT_ISSUANCE").Length(250);
            this.Reference(x => x.FiasFactAddress, "Фактический адрес").Column("FIAS_FACT_ADDRESS_ID").Fetch();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").Fetch();
            this.Reference(x => x.RegistrationAddress, "Адрес прописки").Column("REGISTRATION_RO_ID").Fetch();
            this.Reference(x => x.RegistrationRoom, "Адрес прописки + помещение").Column("REGISTRATION_ROOM_ID").Fetch();
            this.Property(x => x.DocumentIssuededOrg, "Кем выдан документ").Column("DOCUMENT_ISSUEDED_ORG").Length(250);
        }
    }
}