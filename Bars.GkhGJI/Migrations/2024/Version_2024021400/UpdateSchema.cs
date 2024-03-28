namespace Bars.GkhGji.Migrations._2024.Version_2024021400
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024021400")]
    [MigrationDependsOn(typeof(Version_2024020500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("REDIRECTED", DbType.Boolean, false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPEAL_CITIZENS", "REDIRECTED");
        }
    }
}