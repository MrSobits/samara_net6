namespace Bars.Gkh.Gis.Migrations._2014.Version_2014111901
{
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014111901")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014111900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "HOUSEREGISTER",
                new Column("ID", DbType.Int64,
                    ColumnProperty.NotNull | ColumnProperty.PrimaryKey | ColumnProperty.Identity),
                new Column("FIASADDRESS", DbType.Int64),
                new Column("REGION", DbType.String, 200),
                new Column("AREA", DbType.String, 200),
                new Column("CITY", DbType.String, 200),
                new Column("STREET", DbType.String, 200),
                new Column("HOUSENUM", DbType.String, 200),
                new Column("BUILDNUM", DbType.String, 200),
                new Column("TOTALSQUARE", DbType.Double),
                new Column("BUILDDATE", DbType.DateTime),
                new Column("HOUSETYPE", DbType.Int64),
                new Column("MINIMUMFLOORS", DbType.Int32),
                new Column("MAXIMUMFLOORS", DbType.Int32),
                new Column("NUMBERLIVING", DbType.Int32),
                new Column("NUMBERINDIVIDUALCOUNTER", DbType.Int32),
                new Column("PRIVATIZATIONDATE", DbType.DateTime),
                new Column("NUMBERLIFTS", DbType.Int32),
                new Column("TYPEROOF", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("WALLMATERIAL", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("PHYSICALWEAR", DbType.Double),
                new Column("NUMBERENTRANCES", DbType.Int32),
                new Column("ROOFINGMATERIAL", DbType.Int64),
                new Column("TYPEPROJECT", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("HEATINGSYSTEM", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("NUMBERACCOUNT", DbType.Int32),
                new Column("MANORGS", DbType.String, 500),
                new Column("SUPPLIER", DbType.String, 200),
                new Column("SUPPLIERINN", DbType.Int64)
            );

            if (!this.Database.TableExists("GIS_SERVICE_DICTIONARY"))
            {
                this.Database.AddEntityTable("SERVICEDICTIONARY",
                    new Column("ID", DbType.Int64,
                        ColumnProperty.NotNull | ColumnProperty.PrimaryKey | ColumnProperty.Identity),
                    new Column("CODE", DbType.Int32),
                    new Column("NAME", DbType.String, 200),
                    new Column("MEASURE", DbType.String, 200)
                );
            }

            this.Database.AddEntityTable("HOUSESERVICEREGISTER",
                new Column("ID", DbType.Int64,
                    ColumnProperty.NotNull | ColumnProperty.PrimaryKey | ColumnProperty.Identity),
                new RefColumn("HOUSEID", ColumnProperty.NotNull, "HOUSE_SERV_REG_HOUSEID_FKEY", "HOUSEREGISTER", "ID"),
                new RefColumn("SERVICEID", ColumnProperty.NotNull, "HOUSE_SERV_REG_SERVID_FKEY", "SERVICEDICTIONARY",
                    "ID"),
                new Column("HOUSEADDRESS", DbType.String, 500),
                new Column("CALCUALTIONDATE", DbType.DateTime),
                new Column("VOLUMEINDIVIDUALCOUNTER", DbType.Double),
                new Column("VOLUMENORMATIVE", DbType.Double),
                new Column("KOEFODN", DbType.String, 500),
                new Column("VOLUMEDISTRIBUTED", DbType.Double),
                new Column("VOLUMENOTDISTRIBUTED", DbType.Double),
                new Column("VOLUMEODNINDIVIDUALCOUNTER", DbType.Double),
                new Column("VOLUMEODNNORMATIVE", DbType.Double),
                new Column("TARIFF", DbType.Double),
                new Column("TARIFFDATE", DbType.DateTime),
                new Column("RSO", DbType.String, 200),
                new Column("RSOINN", DbType.Int64)
            );
        }

        public override void Down()
        {
            this.Database.RemoveTable("HOUSESERVICEREGISTER");
            this.Database.RemoveTable("HOUSEREGISTER");

            if (!this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh" && x.Version == "2016052000"))
            {
                this.Database.RemoveTable("SERVICEDICTIONARY");
            }

        }
    }
}