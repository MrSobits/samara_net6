namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2014112700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;


    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tatarstan.Migration.Version_2014091500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_TAT_GIS_GMP_PATTERN",
                new Column("PATTERN_CODE", DbType.String, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GJI_PATTERN_MUNICIPALITY",
                    "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TAT_GIS_GMP_PATTERN");
        }
    }
}