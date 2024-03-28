namespace Bars.GkhGji.Migrations._2016.Version_2016091300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
    [Migration("2016091300")]
    [MigrationDependsOn(typeof(Version_2016072700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("COMMENT", DbType.String));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPEAL_CITIZENS", "COMMENT");
        }
    }
}