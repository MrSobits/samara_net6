namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120402
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014120402")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_TENANT_SUBSIDY_REGISTER", new[]
            {
                new Column("PSS", DbType.String),
                new Column("CALCULATIONMONTH", DbType.DateTime),
                new Column("MANAGEMENTORGANIZATIONACCOUNT", DbType.Int64),
                new Column("SURNAME", DbType.String),
                new Column("NAME", DbType.String),
                new Column("PATRONYMIC", DbType.String),
                new Column("DATEOFBIRTH", DbType.DateTime),
                new Column("ARTICLECODE", DbType.Int64),
                new RefColumn("SERVICE", "GIS_TNT_SBS_SRV_ID", "GIS_SERVICE_DICTIONARY", "ID"),
                new Column("BANKNAME", DbType.String),
                new Column("BEGINDATE", DbType.DateTime),
                new Column("INCOMINGSALDO", DbType.Double),
                new Column("ACCRUEDSUM", DbType.Double),
                new Column("RECALCULATEDSUM", DbType.Double),
                new Column("ADVANCEDPAYMENT", DbType.Double),
                new Column("PAYMENTSUM", DbType.Double),
                new Column("SMOSUM", DbType.Double),
                new Column("SMORECALCULATEDSUM", DbType.Double),
                new Column("CHANGESSUM", DbType.Double),
                new Column("ENDDATE", DbType.DateTime),
                new Column("ORGANIZATIONUNIT", DbType.String),
                new RefColumn("LOADEDFILE", "GIS_TNT_SBS_LOADFILE_ID", "GIS_LOADED_FILE_REGISTER", "ID"),
                new Column("ADDRESS", DbType.String),
                new Column("PERSONALACCOUNTID", DbType.Int64)
            });
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_TENANT_SUBSIDY_REGISTER");
        }
    }
}