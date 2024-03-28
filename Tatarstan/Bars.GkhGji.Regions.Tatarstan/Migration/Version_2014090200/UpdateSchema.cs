namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2014090200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014090200")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_TAT_GIS_CHARGE",
                new RefColumn("RESOL_ID", ColumnProperty.NotNull, "GJI_TAT_GIS_CHARGE_RES", "GJI_RESOLUTION", "ID"),
                new Column("DATE_SEND", DbType.DateTime),
                new Column("IS_SENT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("JOBJ", DbType.String, 10000, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TAT_GIS_CHARGE");
        }
    }
}