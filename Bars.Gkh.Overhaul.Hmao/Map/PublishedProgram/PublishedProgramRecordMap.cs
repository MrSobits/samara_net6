namespace Bars.Gkh.Overhaul.Hmao.Map.PublishedProgram
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>Маппинг для "Запись Опубликованной программы"</summary>
    public class PublishedProgramRecordMap : BaseImportableEntityMap<PublishedProgramRecord>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PublishedProgramRecordMap()
            :
                base("Запись Опубликованной программы", "OVRHL_PUBLISH_PRG_REC")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PublishedProgram, "Ссылка на версию программы").Column("PUBLISH_PRG_ID").NotNull().Fetch();
            this.Reference(x => x.Stage2, "Ссылка на версию").Column("STAGE2_ID");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            this.Property(x => x.Sum, "Порядковый номер").Column("SUM").NotNull();
            this.Property(x => x.IndexNumber, "Стоимость").Column("INDEX_NUMBER").NotNull();
            this.Property(x => x.Locality, "Населенный пункт").Column("LOCALITY").Length(250);
            this.Property(x => x.Street, "Улица").Column("STREET").Length(250);
            this.Property(x => x.House, "Дом").Column("HOUSE").Length(250);
            this.Property(x => x.Housing, "Корпус").Column("HOUSING").Length(250);
            this.Property(x => x.Address, "Адрес").Column("ADDRESS").Length(250);
            this.Property(x => x.CommissioningYear, "Год ввода в эксплуатацию").Column("YEAR_COMMISSIONING").NotNull();
            this.Property(x => x.CommonEstateobject, "Объект общего имущества").Column("COMMON_ESTATE_OBJECT").Length(250);
            this.Property(x => x.Wear, "Износ").Column("WEAR").NotNull();
            this.Property(x => x.LastOverhaulYear, "Дата последнего капитального ремонта").Column("YEAR_LAST_OVERHAUL").NotNull();
            this.Property(x => x.PublishedYear, "Плановый год проведения капитального ремонта").Column("YEAR_PUBLISHED").NotNull();
        }
    }
}