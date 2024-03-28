namespace Bars.Gkh.Migrations._2017.Version_2017070601
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017070601")]
    [MigrationDependsOn(typeof(Version_2017062600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CLW_LAWSUIT", new Column("CB_DATE_DIRECTION_SSP", DbType.Date));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT", "CB_DATE_DIRECTION_SSP");
        }
    }
}