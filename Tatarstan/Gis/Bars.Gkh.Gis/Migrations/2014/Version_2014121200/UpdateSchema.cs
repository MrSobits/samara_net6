namespace Bars.Gkh.Gis.Migrations._2014.Version_2014121200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2014121200")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014121100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RenameColumn("GIS_HOUSE_SERVICE_REGISTER", "CALCUALTIONDATE", "CALCULATION_DATE");
            this.Database.ChangeColumn("GIS_HOUSE_REGISTER", new Column("TOTALSQUARE", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_REGISTER", new Column("PHYSICALWEAR", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("CHARGE", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("PAYMENT", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMEINDIVIDUALCOUNTER", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMENORMATIVE", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("COEFODN", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMEDISTRIBUTED", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMENOTDISTRIBUTED", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMEODNINDIVIDUALCOUNTER", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMEODNNORMATIVE", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("TARIFF", DbType.Decimal));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("TOTALVOLUME", DbType.Decimal));
        }

        public override void Down()
        {
            this.Database.RenameColumn("GIS_HOUSE_SERVICE_REGISTER", "CALCULATION_DATE", "CALCUALTIONDATE");
            this.Database.ChangeColumn("GIS_HOUSE_REGISTER", new Column("TOTALSQUARE", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_REGISTER", new Column("PHYSICALWEAR", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("CHARGE", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("PAYMENT", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMEINDIVIDUALCOUNTER", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMENORMATIVE", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("COEFODN", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMEDISTRIBUTED", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMENOTDISTRIBUTED", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMEODNINDIVIDUALCOUNTER", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("VOLUMEODNNORMATIVE", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("TARIFF", DbType.Double));
            this.Database.ChangeColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("TOTALVOLUME", DbType.Double));
        }
    }
}