namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>Маппинг для Сторона договора "Физическое лицо"</summary>
    public class IndividualOwnerContractMap : JoinedSubClassMap<IndividualOwnerContract>
    {
        
        public IndividualOwnerContractMap() : 
                base("Сторона договора \"Физическое лицо\"", "GKH_RSOCONTRACT_INDIV_OWNER")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TypeContactPerson, "Лицо, являющееся стороной договора").Column("TYPE_PERSON").NotNull();
            this.Property(x => x.FirstName, "Имя").Length(300).Column("FIRST_NAME").NotNull();
            this.Property(x => x.LastName, "Фамилия").Length(300).Column("LAST_NAME").NotNull();
            this.Property(x => x.MiddleName, "Отчество").Length(300).Column("MIDDLE_NAME").NotNull();
            this.Property(x => x.Gender, "Пол").Column("GENDER").NotNull();
            this.Property(x => x.OwnerDocumentType, "Тип документа").Column("DOCUMENT_TYPE").NotNull();
            this.Property(x => x.IssueDate, "Дата выдачи").Column("ISSUE_DATE");
            this.Property(x => x.DocumentSeries, "Серия документа").Length(100).Column("DOCUMENT_SERIES").NotNull();
            this.Property(x => x.DocumentNumber, "Номер документа").Length(100).Column("DOCUMENT_NUMBER").NotNull();
            this.Property(x => x.BirthPlace, "Место рождения").Column("BIRTH_PLACE").Length(500);
            this.Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE").NotNull();
        }
    }
}
