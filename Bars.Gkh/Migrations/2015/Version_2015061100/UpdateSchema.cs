namespace Bars.Gkh.Migrations._2015.Version_2015061100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015061100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015060100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_LOG_OPERATION",
                new RefColumn("OPERATOR_ID", "GKH_LOG_OPER_OP", "GKH_OPERATOR", "ID"),
                new RefColumn("LOG_FILE_ID", "GKH_LOG_OPER_FI", "B4_FILE_INFO", "ID"),              
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("LOG_COMMENT", DbType.String, 2000),
                new Column("OPERATION_TYPE", DbType.Int32, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_LOG_OPERATION");
        }
    }
}