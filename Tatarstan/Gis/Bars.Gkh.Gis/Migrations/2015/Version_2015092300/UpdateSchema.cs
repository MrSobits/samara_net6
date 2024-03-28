namespace Bars.Gkh.Gis.Migrations._2015.Version_2015092300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015092300")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015092200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("BIL_NORMATIVE_STORAGE", new Column("NORMATIVE_VALUE", DbType.String, 200));
        }

        public override void Down()
        {
            this.Database.ChangeColumn("BIL_NORMATIVE_STORAGE", new Column("NORMATIVE_VALUE", DbType.Decimal));
        }
    }
}