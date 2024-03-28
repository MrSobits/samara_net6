namespace Bars.Gkh.Overhaul.Hmao.Map.Version
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;

    public class ChangeYearOwnerDecisionMap : GkhBaseEntityMap<ChangeYearOwnerDecision>
    {
        public ChangeYearOwnerDecisionMap()
            : base("OVRHL_CHANGE_YEAR_OWNER_DECISION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentBase, "Документ основание").Column("DOCUMENT_BASE");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.Date, "Дата").Column("DATE");
            this.Property(x => x.Remark, "Примечание").Column("REMARK");
            this.Property(x => x.OldYear, "Старый год").Column("OLD_YEAR");
            this.Property(x => x.NewYear, "Новый год").Column("NEW_YEAR");

            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Reference(x => x.VersionRecordStage1, "Ссылка на запись первого этапа").Column("STAGE1_ID").NotNull().Fetch();
        }
    }
}