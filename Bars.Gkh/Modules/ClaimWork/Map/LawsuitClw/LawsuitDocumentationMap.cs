namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Маапинг сущности <see cref="LawsuitDocument"/>
    /// </summary>
    public class LawsuitDocumentMap : BaseEntityMap<LawsuitDocument>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public LawsuitDocumentMap() : base("Документ искового зявления", "CLW_LAWSUIT_DOCUMENTATION") { }

        /// <summary>
        /// Маппинги
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Date, "Дата документа").Column("DATE").NotNull();
            this.Property(x => x.TypeLawsuitDocument, "Тип документа").Column("TYPE_DOC").NotNull();
            this.Property(x => x.Note, "Примечание").Column("NOTE").Length(255);
            this.Property(x => x.Number, "Номер документа").Column("NUMBER").NotNull();
            this.Reference(x => x.Rosp, "Наименование РОСП").Column("ROSP_ID").Fetch();
            this.Property(x => x.CollectDebtFrom, "ИП в отношении").Column("COLLECT_DEBT_FROM");

            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch().NotNull();
            this.Reference(x => x.Lawsuit, "Исковое зявление").Column("LAWSUIT_ID").Fetch().NotNull();
        }
    }
}