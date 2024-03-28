namespace Bars.Gkh.Migrations._2018.Version_2018032300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018032300")]
    [MigrationDependsOn(typeof(Version_2018020800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("DUTY_POSTPONEMENT", DbType.Boolean, true));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT", "DUTY_POSTPONEMENT");
        }
    }
}