namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016030500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016030500")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_CONSTRUCTION_OBJECT", new Column("DESCRIPTION", DbType.String, 500, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_CONSTRUCTION_OBJECT", "DESCRIPTION");
        }
    }
}