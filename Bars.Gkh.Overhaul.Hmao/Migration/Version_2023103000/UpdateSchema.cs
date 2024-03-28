namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2023103000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023103000")]
    [MigrationDependsOn(typeof(Version_2023091300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_DICT_CRITERIA_FOR_ACTUALIZE_VERSION",
               new Column("CRITERIA_TYPE", DbType.Int16),
               new Column("VALUE_FROM", DbType.Int32),
               new Column("VALUE_TO", DbType.Int32),
               new Column("POINTS", DbType.Int32),
               new Column("WEIGHT", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_DICT_CRITERIA_FOR_ACTUALIZE_VERSION");
        }
    }
}