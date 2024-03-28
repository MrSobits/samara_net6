namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014070900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014070900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014070100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_UNACCEPT_C_PACKET", new Column("USER_NAME", DbType.String, 100, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_C_PACKET", "USER_NAME");
        }
    }
}
