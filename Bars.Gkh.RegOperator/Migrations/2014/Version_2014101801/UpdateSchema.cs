namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014101801
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014101800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_UNACCEPT_PAY_PACKET", new Column("TYPE", DbType.Int32, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_PAY_PACKET", "TYPE");
        }
    }
}
