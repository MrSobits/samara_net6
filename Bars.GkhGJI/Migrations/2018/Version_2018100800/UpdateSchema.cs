namespace Bars.GkhGji.Migrations._2018.Version_2018100800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018100800")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018092600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("STATEMENT_SUBJECTS", DbType.String));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPEAL_CITIZENS", "STATEMENT_SUBJECTS");
        }

    }
}