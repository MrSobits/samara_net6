namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021501
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //----- Требование
            Database.AddEntityTable(
                "GJI_REQUIREMENT",
                new Column("DOCUMENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUMBER", DbType.String, 50),
                new Column("CLAUSE", DbType.String, 500),
                new Column("DESTINATION", DbType.String, 500),
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("TYPE_REQUIREMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_REQUIREMENT_DOC", false, "GJI_REQUIREMENT", "DOCUMENT_ID");
            Database.AddIndex("IND_REQUIREMENT_ST", false, "GJI_REQUIREMENT", "STATE_ID");
            Database.AddIndex("IND_REQUIREMENT_FILE", false, "GJI_REQUIREMENT", "FILE_ID");
            Database.AddForeignKey("FK_REQUIREMENT_DOC", "GJI_REQUIREMENT", "DOCUMENT_ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_REQUIREMENT_ST", "GJI_REQUIREMENT", "STATE_ID", "B4_STATE", "ID");
            Database.AddForeignKey("FK_REQUIREMENT_FILE", "GJI_REQUIREMENT", "FILE_ID", "B4_FILE_INFO", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_REQUIREMENT");
        }
    }
}
