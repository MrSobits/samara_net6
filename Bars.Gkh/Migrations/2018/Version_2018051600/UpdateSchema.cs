namespace Bars.Gkh.Migrations._2018.Version_2018051600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018051600")]
    [MigrationDependsOn(typeof(Version_2018050100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GKH_RO_ASSBERBANKCLIENT",
            new Column("CLIENT_CODE", DbType.String, 5, ColumnProperty.NotNull | ColumnProperty.Unique));

            Database.AddColumn("GKH_REALITY_OBJECT", new RefColumn("ASSBERBANKCLIENT_ID", ColumnProperty.Null, "GKH_REALITY_OBJECT_GKH_RO_ASSBERBANKCLIENT_ASSBERBANKCLIENT_ID_ID", "GKH_RO_ASSBERBANKCLIENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_RO_ASSBERBANKCLIENT");

            Database.RemoveColumn("GKH_REALITY_OBJECT", "ASSBERBANKCLIENT_ID");
        }
    }
}