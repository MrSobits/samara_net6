namespace Bars.Gkh.Gis.Migrations._2014.Version_2014112000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014112000")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014111901.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GIS_REAL_EST_TYPE_GROUP",
                new Column("NAME", DbType.String, 22, ColumnProperty.NotNull));

            this.Database.AddRefColumn("GIS_REAL_ESTATE_TYPE", new RefColumn("REAL_EST_TYPE_GROUP_ID", "GIS_RL_EST_TYPE_GRP_ID", "GIS_REAL_EST_TYPE_GROUP", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_REAL_EST_TYPE_GROUP");
            this.Database.RemoveColumn("GIS_REAL_ESTATE_TYPE", "REAL_EST_TYPE_GROUP_ID");
        }
    }
}