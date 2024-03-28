namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017110100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017110100")]
    [MigrationDependsOn(typeof(Version_2017103100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable(
            "GJI_CH_INSPECTION_OMSU",
            new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
            new Column("PLAN_ID", DbType.Int64, 22),
            new Column("DATE_START", DbType.DateTime),
            new Column("COUNT_DAYS", DbType.Int32),
            new Column("COUNT_HOURS", DbType.Int32),
            new Column("REASON", DbType.String, 500),
            new Column("OMSU_PERSON", DbType.String, 500),
            new Column("ANOTHER_REASONS", DbType.String, 500),
            new Column("TYPE_BASE_JURAL", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("TYPE_FACT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("URI_REGISTRATION_NUMBER", DbType.Int32),
            new Column("URI_REGISTRATION_DATE", DbType.DateTime),
            new Column("TYPE_FORM", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_GJI_INSPECT_OMSU_PLAN", false, "GJI_CH_INSPECTION_OMSU", "PLAN_ID");
            Database.AddForeignKey("FK_GJI_CH_INSPECT_OMSU_INS", "GJI_CH_INSPECTION_OMSU", "ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_CH_INSPECT_OMSU_PLAN", "GJI_CH_INSPECTION_OMSU", "PLAN_ID", "GJI_DICT_PLANJURPERSON", "ID");

            Database.AddEntityTable(
                    "GJI_CH_BASEOMSU_CONTRAGENT",
                    new RefColumn("BASEOMSU_ID", ColumnProperty.NotNull, "GJI_OMSUCONTR_JUR", "GJI_CH_INSPECTION_OMSU", "ID"),
                    new RefColumn("CONTRAGENT_ID", "GJI_CH_OMSUCONT_CONTR", "GKH_CONTRAGENT", "ID"));
            

        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_CH_BASEOMSU_CONTRAGENT");
            this.Database.RemoveTable("GJI_CH_INSPECTION_OMSU");
        }
    }
}