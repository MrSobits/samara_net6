namespace Bars.GkhGji.Migrations._2021.Version_2021072100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021072100")]
    [MigrationDependsOn(typeof(Version_2021052800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("SERIAL_NUMBER", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "SERIAL_NUMBER");
        }
    }
}