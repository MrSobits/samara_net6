namespace Bars.GkhGji.Migrations.Version_2014042800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014042200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DICT_HEAT_SEAS_RESOL",
                new Column("ACCEPT_DATE", DbType.DateTime),
                new RefColumn("DOC_ID", ColumnProperty.NotNull, "HEAT_RESOL_DOC", "B4_FILE_INFO", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "HEAT_RESOL_PERIOD", "GJI_DICT_HEATSEASONPERIOD", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "HEAT_RESOL_MUNIC", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_HEAT_SEAS_RESOL");
        }
    }
}