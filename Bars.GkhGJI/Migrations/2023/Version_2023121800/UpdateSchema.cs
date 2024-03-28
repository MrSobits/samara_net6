namespace Bars.GkhGji.Migrations._2023.Version_2023121800
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023121800")]
    [MigrationDependsOn(typeof(Version_2023111500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("PAYDED_HALF", DbType.Boolean, false));
            Database.AddColumn("GJI_RESOLUTION", new Column("WRITTEN_OFF", DbType.Boolean, false));
            Database.AddColumn("GJI_RESOLUTION", new Column("WRITTEN_OFF_DESCRIPTION", DbType.String, 1500));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_RESOLUTION", "WRITTEN_OFF_DESCRIPTION");
            this.Database.RemoveColumn("GJI_RESOLUTION", "WRITTEN_OFF");
            this.Database.RemoveColumn("GJI_RESOLUTION", "PAYDED_HALF");
        }
    }
}