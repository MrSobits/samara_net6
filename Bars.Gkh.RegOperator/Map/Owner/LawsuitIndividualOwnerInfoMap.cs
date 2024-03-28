namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities.Owner;

    public class LawsuitIndividualOwnerInfoMap : JoinedSubClassMap<LawsuitIndividualOwnerInfo>
    {
        public LawsuitIndividualOwnerInfoMap() :  base("Собственник физ. лицо в исковом заявлении", "REGOP_LAWSUIT_IND_OWNER_INFO") {}

        protected override void Map()
        {
            this.Property(x => x.Surname, "Фамилия").Column("SURNAME").NotNull();
            this.Property(x => x.FirstName, "Имя").Column("FIRST_NAME").NotNull();
            this.Property(x => x.SecondName, "Отчество").Column("SECOND_NAME");

            this.Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
            this.Property(x => x.BirthPlace, "Место рождения").Column("BIRTH_PLACE");
            this.Property(x => x.LivePlace, "Место жительства").Column("LIVE_PLACE");
            this.Property(x => x.DocIndCode, "Код документа").Column("DOC_IND_CODE");
            this.Property(x => x.DocIndName, "Название документа").Column("DOC_IND_NAME");
            this.Property(x => x.DocIndSerial, "Серия документа").Column("DOC_IND_SERIAL");
            this.Property(x => x.DocIndNumber, "Номер документа").Column("DOC_IND_NUMBER");
            this.Property(x => x.DocIndDate, "Дата документа").Column("DOC_IND_DATE");
            this.Property(x => x.DocIndIssue, "Организация, выдавшая документ").Column("DOC_IND_ISSUE");
        }
    }
}
