namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014120301
{
    using System.Data;
    using B4.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014120300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERIOD", new Column("IS_CLOSING", DbType.Boolean, ColumnProperty.Null));

            var update = "update REGOP_PERIOD set is_closing = {0}";
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(update.FormatUsing("false"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(update.FormatUsing("f"));
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERIOD", "IS_CLOSING");
        }
    }
}
