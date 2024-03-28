namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;

    public class DocRequestActionMap : JoinedSubClassMap<DocRequestAction>
    {
        public DocRequestActionMap()
            : base("Действие акта проверки с типом \"Истребование документов\"", "GJI_ACTCHECK_DOC_REQUEST_ACTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ContrPersType, "Тип контролируемого лица").Column("CONTR_PERS_TYPE");
            this.Reference(x => x.ContrPersContragent, "Контрагент контролируемого лица").Column("CONTR_PERS_CONTRAGENT_ID");
            this.Property(x => x.DocProvidingPeriod, "Срок предоставления документов (сутки)").Column("DOC_PROVIDING_PERIOD").NotNull();
            this.Reference(x => x.DocProvidingAddress, "Адрес предоставления документов").Column("DOC_PROVIDING_ADDRESS_ID");
            this.Property(x => x.ContrPersEmailAddress, "Адрес эл. почты контролируемого лица").Column("CONTR_PERS_EMAIL_ADDRESS");
            this.Property(x => x.PostalOfficeNumber, "Номер почтового отделения").Column("POSTAL_OFFICE_NUMBER");
            this.Property(x => x.EmailAddress, "Адрес эл. почты").Column("EMAIL_ADDRESS");
            this.Property(x => x.CopyDeterminationDate, "Дата направления копии определения").Column("COPY_DETERMINATION_DATE");
            this.Property(x => x.LetterNumber, "Номер письма").Column("LETTER_NUMBER");
        }
    }
}