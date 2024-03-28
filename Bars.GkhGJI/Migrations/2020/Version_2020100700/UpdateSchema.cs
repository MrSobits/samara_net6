namespace Bars.GkhGji.Migrations._2020.Version_2020100700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020100700")]
    [MigrationDependsOn(typeof(Version_2020091700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "RO_ID");

            Database.AddEntityTable(
              "GJI_MKD_LIC_STATEMENT_RO",
              new RefColumn("REQUEST_ID", ColumnProperty.NotNull, "GJI_MKD_LIC_STATEMENT_RO_STMNT", "GJI_MKD_LIC_STATEMENT", "ID"),
              new RefColumn("RO_ID", ColumnProperty.NotNull, "GJI_MKD_LIC_STATEMENT_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_MKD_LIC_STATEMENT_FILE");

        }
    }
}