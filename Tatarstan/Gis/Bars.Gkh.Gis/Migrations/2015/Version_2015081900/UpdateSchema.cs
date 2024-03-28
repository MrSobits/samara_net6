namespace Bars.Gkh.Gis.Migrations._2015.Version_2015081900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015081900")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015081700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveColumn("GIS_INTEGR_DICT", "CNT_REF_RECS");
            this.Database.RemoveColumn("GIS_INTEGR_DICT", "CNT_NOT_REF_RECS");
        }

        public override void Down()
        {
            this.Database.AddColumn("GIS_INTEGR_DICT", new Column("CNT_NOT_REF_RECS", DbType.Int32));
            this.Database.AddColumn("GIS_INTEGR_DICT", new Column("CNT_REF_RECS", DbType.Int32));
        }
    }
}