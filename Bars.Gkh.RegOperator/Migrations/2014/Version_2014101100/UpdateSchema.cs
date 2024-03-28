namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014101100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014100300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddColumn("REGOP_WALLET", new Column("HAS_NEW_OPS", DbType.Boolean, ColumnProperty.NotNull, "FALSE"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("REGOP_WALLET", new Column("HAS_NEW_OPS", DbType.Boolean, ColumnProperty.NotNull, "FALSE"));
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_WALLET", "HAS_NEW_OPS");
        }
    }
}
