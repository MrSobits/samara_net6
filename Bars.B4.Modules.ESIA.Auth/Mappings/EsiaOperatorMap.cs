namespace Bars.B4.Modules.ESIA.Auth.Mappings
{
    using Bars.B4.Modules.ESIA.Auth.Entities;
    using Bars.B4.Modules.Mapping.Mappers;

    /// <summary>
    /// Маппинг для "Оператор с привязанной учеткой ЕСИА"
    /// </summary>
    public class EsiaOperatorMap : BaseEntityMap<EsiaOperator>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public EsiaOperatorMap() : 
                base("Оператор с привязанной учеткой ЕСИА", "ESIA_OPERATOR")
        {
        }

        /// <summary>
        /// Mapping
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Operator, "Оператор в ЖКХ").Column("OPERATOR_ID").Fetch().NotNull();
            this.Property(x => x.UserId, "UserId").Column("USERID").Length(50);
            this.Property(x => x.UserName, "UserName").Column("USERNAME").Length(50);
            this.Property(x => x.Gender, "Gender").Column("GENDER").Length(10);
            this.Property(x => x.LastName, "LastName").Column("LASTNAME").Length(50);
            this.Property(x => x.FirstName, "FirstName").Column("FIRSTNAME").Length(50);
            this.Property(x => x.MiddleName, "MiddleName").Column("MIDDLENAME").Length(50);
            this.Property(x => x.PersonSnils, "PersonSnils").Column("PERSON_SNILS").Length(50);
            this.Property(x => x.PersonEmail, "PersonEmail").Column("PERSON_EMAIL").Length(50);
            this.Property(x => x.BirthDate, "BirthDate").Column("BIRTHDATE").Length(50);
            this.Property(x => x.OrgPosition, "OrgPosition").Column("ORG_POSITION").Length(300);
            this.Property(x => x.OrgName, "OrgName").Column("ORG_NAME").Length(300);
            this.Property(x => x.OrgShortName, "OrgShortName").Column("ORG_SHORTNAME").Length(300);
            this.Property(x => x.OrgType, "OrgType").Column("ORG_TYPE").Length(50);
            this.Property(x => x.OrgOgrn, "OrgOgrn").Column("ORG_OGRN").Length(50);
            this.Property(x => x.OrgInn, "OrgInn").Column("ORG_INN").Length(50);
            this.Property(x => x.OrgKpp, "OrgKpp").Column("ORG_KPP").Length(50);
            this.Property(x => x.OrgAddresses, "OrgAddresses").Column("ORG_ADDRESSES").Length(500);
            this.Property(x => x.OrgLegalForm, "OrgLegalForm").Column("ORG_LEGALFORM").Length(200);
            this.Property(x => x.IsActive, "IsActive").Column("IS_ACTIVE");
        }
    }
}