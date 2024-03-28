namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014100300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014100201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddColumn("REGOP_TRANSFER", new Column("IS_INDIRECT", DbType.Boolean, ColumnProperty.NotNull, "false"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("REGOP_TRANSFER", new Column("IS_INDIRECT", DbType.Boolean, ColumnProperty.NotNull, "0"));
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_TRANSFER", "IS_INDIRECT");
        }
    }
}
