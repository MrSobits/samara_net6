namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020091700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2020091700")]
    [MigrationDependsOn(typeof(Version_2020060400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("gji_control_list", new Column("name", DbType.String, 512));
            Database.AddColumn("gji_control_list", new Column("approval_details", DbType.String, 2048));
        }

        public override void Down()
        {
            Database.RemoveColumn("gji_control_list", "name");
            Database.RemoveColumn("gji_control_list", "approval_details");
        }
    }
}
