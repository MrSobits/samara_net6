namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2014090400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014090400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tatarstan.Migration.Version_2014090200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_TAT_GIS_PAYMENT",
                new RefColumn("PAYFINE_ID", ColumnProperty.NotNull, "GJI_TAT_GIS_PAYMENT_PAY", "GJI_RESOLUTION_PAYFINE", "ID"),
                new Column("DATE_RECIEVE", DbType.DateTime),
                new Column("CUIP", DbType.String, ColumnProperty.NotNull),
                new Column("JOBJ", DbType.String, 10000, ColumnProperty.NotNull));

            Database.AddIndex("GJI_TAT_GIS_PAYMENT_UIP", false, "GJI_TAT_GIS_PAYMENT", "CUIP");
        }

        public override void Down()
        {
            Database.RemoveIndex("GJI_TAT_GIS_PAYMENT_UIP", "GJI_TAT_GIS_PAYMENT");

            Database.RemoveTable("GJI_TAT_GIS_CHARGE");
        }
    }
}