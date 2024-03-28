namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Voronezh.Entities;

    /// <summary>
    /// Маапинг сущности <see cref="LawSuitDebtWorkSSPDocument"/>
    /// </summary>
    public class LawSuitDebtWorkSSPDocumentMap : BaseEntityMap<LawSuitDebtWorkSSPDocument>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public LawSuitDebtWorkSSPDocumentMap() : base("Документ искового зявления", "CLW_LAWSUIT_DEBT_WORK_SSP_DOC") { }

        /// <summary>
        /// Маппинги
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Date, "Дата документа").Column("DATE").NotNull();
            this.Property(x => x.TypeLawsuitDocument, "Тип документа").Column("TYPE_DOC").NotNull();
            this.Property(x => x.Note, "Примечание").Column("NOTE").Length(255);
            this.Property(x => x.Number, "Номер документа").Column("NUMBER").NotNull();
            this.Property(x => x.NumberString, "Номер документа").Column("NUMBER_DOC");
            this.Reference(x => x.Rosp, "Наименование РОСП").Column("ROSP_ID").Fetch();
            this.Property(x => x.CollectDebtFrom, "ИП в отношении").Column("COLLECT_DEBT_FROM");
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch().NotNull();
            this.Reference(x => x.LawSuitDebtWorkSSP, "Исковая работа").Column("LAWSUIT_DEBTWORKSSP_ID").Fetch().NotNull();
        }
    }
}