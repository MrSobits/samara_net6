namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019100701
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Migration = Bars.B4.Modules.Ecm7.Framework.Migration;

    [Migration("2019100701")]
    [MigrationDependsOn(typeof(Version_2019100700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_TAT_DISPOSAL", new RefColumn("INSPECTION_BASE_TYPE_ID", "GJI_TAT_DISP_INSPECTION_BASE_ID", "GJI_DICT_INSPECTION_BASE_TYPE", "ID"));
            this.Database.AddColumn("GJI_TAT_DISPOSAL", new Column("NOTIFICATION_TYPE", DbType.Int32));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_TAT_DISPOSAL", "INSPECTION_BASE_TYPE_ID");
            this.Database.RemoveColumn("GJI_TAT_DISPOSAL", "NOTIFICATION_TYPE");
        }
    }
}