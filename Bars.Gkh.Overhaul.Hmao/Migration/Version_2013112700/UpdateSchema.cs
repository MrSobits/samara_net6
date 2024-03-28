namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013112700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013112600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_LOADED_PROGRAM",
                new Column("INDEX_NUMBER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("LOCALITY", DbType.String, 500, ColumnProperty.NotNull),
                new Column("STREET", DbType.String, 255, ColumnProperty.NotNull),
                new Column("HOUSE", DbType.String, 16, ColumnProperty.NotNull),
                new Column("HOUSING", DbType.String, 16),
                new Column("ADDRESS", DbType.String, 500, ColumnProperty.NotNull),
                new Column("YEAR_COMMISSIONING", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("COMMON_ESTATE_OBJECT", DbType.String, 255, ColumnProperty.NotNull),
                new Column("WEAR", DbType.String, 255, ColumnProperty.NotNull),
                new Column("YEAR_LAST_OVERHAUL", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("YEAR_PLAN_OVERHAUL", DbType.Int16, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_LOADED_PROGRAM");
        }
    }
}