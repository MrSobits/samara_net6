using System.Data;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014050700
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014050700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014050500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddColumn("REGOP_DEL_AGENT_REAL_OBJ", new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull, "'01.01.0001'"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("REGOP_DEL_AGENT_REAL_OBJ", new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull));
            }
            Database.AddColumn("REGOP_DEL_AGENT_REAL_OBJ", new Column("DATE_END", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_DEL_AGENT_REAL_OBJ", "DATE_START");
            Database.RemoveColumn("REGOP_DEL_AGENT_REAL_OBJ", "DATE_END");
        }
    }
}
