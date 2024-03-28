namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Map
{
    using Entity;
    using B4.Modules.Mapping.Mappers;

    public class ViewDocumentClwMap : PersistentObjectMap<ViewDocumentClw>
    {

        public ViewDocumentClwMap() :
                base("Реестр документов ПИР", "VIEW_CLW_DOCUMENT_REGISTER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.RealityObjectId, "Идентификатор жилого дома по договору").Column("RO_ID");
            this.Property(x => x.ClaimWorkId, "Идентификатор ПИР").Column("CLW_ID");
            this.Property(x => x.DocumentType, "Тип документа").Column("DOC_TYPE");
            this.Property(x => x.BaseType, "Тип основания ПИР").Column("BASE_TYPE");
            this.Property(x => x.BaseInfo, "Основание ПИР").Column("BASE_INFO");
            this.Property(x => x.DebtorType, "Тип должника").Column("DEBTOR_TYPE");
            this.Property(x => x.Address, "Адрес").Column("ADDRESS");
            this.Property(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.ReviewDate, "Дата рассмотрения").Column("REVIEW_DATE");
            this.Property(x => x.PretensSum, "Сумма претензии").Column("PRETENS_SUM");
            this.Property(x => x.PretensPenalty, "Пени претензии").Column("PRETENS_PENALTY");
            this.Property(x => x.PretensPlanedDate, "Планируемый срок оплаты в претензии").Column("PRETENS_PLANED_DATE");
            this.Property(x => x.LawsuitNumber, "Номер заявления").Column("LAW_BID_NUMBER");
            this.Property(x => x.LawsuitDate, "Дата заявления").Column("LAW_BID_DATE");
            this.Property(x => x.LawsuitSum, "Погашенная сумма долга").Column("LAW_DEBT_SUM");
            this.Property(x => x.LawsuitPenalty, "Погашенная сумма пени").Column("LAW_PENALTY_DEBT");
        }
    }
}
