namespace Bars.GkhGji.Migrations._2023.Version_2023111500
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023111500")]
    [MigrationDependsOn(typeof(Version_2023101600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("FAST_TRACK", DbType.Boolean, false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPEAL_CITIZENS", "FAST_TRACK");
        }
    }
}