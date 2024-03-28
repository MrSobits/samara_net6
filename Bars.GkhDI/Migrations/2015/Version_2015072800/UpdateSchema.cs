namespace Bars.GkhDi.Migrations.Version_2015072800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Regions.Tatarstan.Migrations.Version_2014031200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_COMMUNAL_SERVICE", new Column("CONS_NORM_LIV_HOUSE", DbType.String, 300));
            Database.AddColumn("DI_COMMUNAL_SERVICE", new RefColumn("UNIT_MEASURE_LIV_HOUSE_ID", "CNORMS_LHOUSE_UM", "GKH_DICT_UNITMEASURE", "ID"));
            Database.AddColumn("DI_COMMUNAL_SERVICE", new Column("ADDITIONAL_INFO_LIV_HOUSE", DbType.String, 300));
            Database.AddColumn("DI_COMMUNAL_SERVICE", new Column("CONS_NORM_HOUSING", DbType.String, 300));
            Database.AddColumn("DI_COMMUNAL_SERVICE", new RefColumn("UNIT_MEASURE_HOUSING_ID", "CNORMS_HOUSING_UM", "GKH_DICT_UNITMEASURE", "ID"));
            Database.AddColumn("DI_COMMUNAL_SERVICE", new Column("ADDITIONAL_INFO_HOUSING", DbType.String, 300));

            Database.AddEntityTable(
                "DI_CONSUMPTION_NORMS_NPA",
                new RefColumn("BASE_SERVICE_ID", "DI_CONS_NORMS_NPA_BS", "DI_BASE_SERVICE", "ID"),
                new Column("CONS_NORM_NPA_DATE", DbType.Date),
                new Column("CONS_NORM_NPA_NUMBER", DbType.String, 300),
                new Column("CONS_NORM_NPA_ACCEPTOR", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_COMMUNAL_SERVICE", "CONS_NORM_LIV_HOUSE");
            Database.RemoveColumn("DI_COMMUNAL_SERVICE", "UNIT_MEASURE_LIV_HOUSE_ID");
            Database.RemoveColumn("DI_COMMUNAL_SERVICE", "ADDITIONAL_INFO_LIV_HOUSE");
            Database.RemoveColumn("DI_COMMUNAL_SERVICE", "CONS_NORM_HOUSING");
            Database.RemoveColumn("DI_COMMUNAL_SERVICE", "UNIT_MEASURE_HOUSING_ID");
            Database.RemoveColumn("DI_COMMUNAL_SERVICE", "ADDITIONAL_INFO_HOUSING");

            Database.RemoveTable("DI_CONSUMPTION_NORMS_NPA");
        }
    }
}