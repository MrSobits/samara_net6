namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013100901
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013100900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "OVRHL_DICT_ACCTOPERATION",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.Int64, ColumnProperty.NotNull),
                new Column("TYPE", DbType.Int64, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_DICT_ACCTOPERATION");
        }
    }
}