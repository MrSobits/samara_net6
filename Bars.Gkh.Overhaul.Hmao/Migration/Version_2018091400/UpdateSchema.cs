namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018091400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018091400")]
    [MigrationDependsOn(typeof(Version_2018070500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "OVRHL_KPKR_COST_LIMITS_OOI",
              new RefColumn("OOI_ID", ColumnProperty.Null, "OVRHL_KPKR_COST_LIMITS_OOI_OVRHL_COMMON_ESTATE_OBJECT_WORK_ID_ID", "OVRHL_COMMON_ESTATE_OBJECT", "ID"),
              new RefColumn("MUNICIPALITY_ID", ColumnProperty.Null, "OVRHL_KPKR_COST_LIMITS_OOI_GKH_DICT_MUNICIPALITY_MUNICIPALITY_ID_ID", "GKH_DICT_MUNICIPALITY", "ID"),
              new Column("COST", DbType.Decimal, ColumnProperty.NotNull),
              new Column("DATE_START", DbType.DateTime, ColumnProperty.Null),
              new Column("DATE_END", DbType.DateTime, ColumnProperty.Null),
              new Column("FLOOR_START", DbType.Int16, ColumnProperty.Null),
              new Column("FLOOR_END", DbType.Int16, ColumnProperty.Null)
              );
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_KPKR_COST_LIMITS_OOI");
        }
    }
}