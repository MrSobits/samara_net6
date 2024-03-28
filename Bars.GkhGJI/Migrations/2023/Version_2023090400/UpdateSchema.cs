namespace Bars.GkhGji.Migrations._2023.Version_2023090400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023090400")]
    [MigrationDependsOn(typeof(_2023.Version_2023080200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_EMAIL", new Column("APPEAL_SOURCE", DbType.Int16, ColumnProperty.NotNull, 0));
            Database.AddColumn("GKH_ENTITY_CHANGE_LOG", new Column("PARRENT_ENTITY_ID", DbType.Int64, ColumnProperty.None));

        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_ENTITY_CHANGE_LOG", "PARRENT_ENTITY_ID");
            Database.RemoveColumn("GJI_EMAIL", "APPEAL_SOURCE");
        }
    }
}