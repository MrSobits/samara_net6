namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023110800
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023110800")]
    [MigrationDependsOn(typeof(Version_2023080700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_APPCIT_ANSWER_LTEXT", new Column("DESCRIPTION2", DbType.Binary));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_APPCIT_ANSWER_LTEXT", "DESCRIPTION2");
        }
    }
}