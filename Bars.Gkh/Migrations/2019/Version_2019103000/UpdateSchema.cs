namespace Bars.Gkh.Migrations._2019.Version_2019103000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019103000")]
    [MigrationDependsOn(typeof(Version_2019072300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("Share", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "Share");
        }
    }
}