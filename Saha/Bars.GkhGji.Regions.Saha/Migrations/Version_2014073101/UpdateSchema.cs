namespace Bars.GkhGji.Regions.Saha.Migrations.Version_2014073101
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Saha.Migrations.Version_2014073100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_DISP_SURVSUBJ",
                new Column("TYPE_SURVEY_SUBJ", DbType.Int32),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "DISP_SURVSUBJ", "GJI_DISPOSAL", "ID"));

            Database.RemoveColumn("GJI_SAHA_DISP_CON_MEASURE", "CONTROL_MEASURES_NAME");

            Database.AddRefColumn(
                "GJI_SAHA_DISP_CON_MEASURE", 
                new RefColumn("CONTROL_MEASURES_ID", ColumnProperty.Null, "DISP_CONTRMEASURES", "GJI_DICT_CON_ACTIVITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DISP_SURVSUBJ");

            Database.AddColumn("GJI_SAHA_DISP_CON_MEASURE", new Column("CONTROL_MEASURES_NAME", DbType.String, 2000));

            Database.RemoveColumn("GJI_SAHA_DISP_CON_MEASURE", "CONTROL_MEASURES_ID");
        }
    }
}