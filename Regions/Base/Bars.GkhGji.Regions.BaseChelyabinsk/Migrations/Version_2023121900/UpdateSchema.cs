namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2023121900
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using System.Data;

    [Migration("2023121900")]
    [MigrationDependsOn(typeof(Version_2023112900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_PROTOCOL197_DEFINITION",             
              new Column("EXECUTION_DATE", DbType.DateTime),
              new Column("DOCUMENT_DATE", DbType.DateTime),
              new Column("DOC_NUMBER", DbType.Int32),
              new Column("DOCUMENT_NUM", DbType.String, 50),
              new Column("DEF_TIME", DbType.DateTime, 25),
              new Column("DATE_PROC", DbType.DateTime),
              new Column("DESCRIPTION", DbType.String, 500),
              new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
              new RefColumn("FILE_ID", "GJI_PROTOCOL197_DEFINITION_FILE", "B4_FILE_INFO", "ID"),
              new RefColumn("SIGNER_ID", ColumnProperty.None, "GJI_PROTOCOL197_DEFINITION_SIGNER", "GKH_DICT_INSPECTOR", "ID"),
              new RefColumn("ISSUED_DEFINITION_ID", ColumnProperty.None, "GJI_PROTOCOL197_DEFINITION_ISSUER", "GKH_DICT_INSPECTOR", "ID"),
              new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GJI_PROTOCOL197_DEFINITION_P197", "GJI_PROTOCOL197", "ID"),
              new Column("EXTERNAL_ID", DbType.String, 36));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PROTOCOL197_DEFINITION");
           
        }

    }
}