namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014031400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014031301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "REGOP_PARAMETER",
                new Column("KEY", DbType.String, ColumnProperty.NotNull),
                new Column("VALUE", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PARAMETER");
        }
    }
}
