namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018042500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018042500")]
    [MigrationDependsOn(typeof(Version_2018041900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "OVRHL_MAX_SUM_BY_YEAR",
               new RefColumn("MUNICIPALITY_ID", ColumnProperty.Null, "MUNICIPALITY_ID_GKH_DICT_MUNICIPALITY_MUNICIPALITY_ID_ID", "GKH_DICT_MUNICIPALITY", "ID"),
               new RefColumn("PROGRAM_ID", ColumnProperty.Null, "PROGRAM_ID_OVRHL_PRG_VERSION_PROGRAM_ID_ID", "OVRHL_PRG_VERSION", "ID"),
               new Column("YEAR", DbType.Int16, ColumnProperty.Null),
               new Column("SUM", DbType.Decimal, ColumnProperty.NotNull)
               );
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_MAX_SUM_BY_YEAR");
        }
    }
}