namespace Bars.Gkh.Gis.Migrations._2015.Version_2015020400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015020400")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014122200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_SERVICE_RECALCULATION_REGISTER",
                new RefColumn("SERVICE_ID", "GIS_SER_REC_HOU_SER_REG_SER_ID", "GIS_HOUSE_SERVICE_REGISTER", "ID"),
                new Column("RECALCULATION_MONTH", DbType.DateTime),
                new Column("RECALCULATION_SUM", DbType.Decimal),
                new Column("RECALCULATION_NDS", DbType.Decimal),
                new Column("RECALCULATION_VOLUME", DbType.Decimal)
            );

            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", "TYPE_AREA", DbType.Int32);
            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", "TYPE_CONTRACT", DbType.Int32);
            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", "SERVICE_SUM", DbType.Decimal);
            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", "SERVICE_NDS", DbType.Decimal);
            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", "SERVICE_RECALCULATION", DbType.Decimal);
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_SERVICE_RECALCULATION_REGISTER");
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "TYPE_AREA");
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "TYPE_CONTRACT");
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "SERVICE_SUM");
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "SERVICE_NDS");
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "SERVICE_RECALCULATION");
        }
    }
}