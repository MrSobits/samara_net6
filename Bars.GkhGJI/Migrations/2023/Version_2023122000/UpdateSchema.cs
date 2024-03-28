namespace Bars.GkhGji.Migrations._2023.Version_2023122200
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023122200")]
    [MigrationDependsOn(typeof(Version_2023121800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_RESOLPROS_DEFINITION",
              new Column("EXECUTION_DATE", DbType.DateTime),
              new Column("DOCUMENT_DATE", DbType.DateTime),
              new Column("DOC_NUMBER", DbType.Int32),
              new Column("DOCUMENT_NUM", DbType.String, 50),
              new Column("DEF_TIME", DbType.DateTime, 25),
              new Column("DATE_PROC", DbType.DateTime),
              new Column("DESCRIPTION", DbType.String, 500),
              new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
              new RefColumn("FILE_ID", "GJI_RESOLPROS_DEFINITION_FILE", "B4_FILE_INFO", "ID"),
              new RefColumn("SIGNER_ID", ColumnProperty.None, "GJI_RESOLPROS_DEFINITION_SIGNER", "GKH_DICT_INSPECTOR", "ID"),
              new RefColumn("ISSUED_DEFINITION_ID", ColumnProperty.None, "GJI_RESOLPROS_DEFINITION_ISSUER", "GKH_DICT_INSPECTOR", "ID"),
              new RefColumn("RESOLPROS_ID", ColumnProperty.NotNull, "GJI_RESOLPROS_DEFINITION_RESOLPROS", "GJI_RESOLPROS", "ID"),
              new Column("EXTERNAL_ID", DbType.String, 36));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_RESOLPROS_DEFINITION");

        }
    }
}