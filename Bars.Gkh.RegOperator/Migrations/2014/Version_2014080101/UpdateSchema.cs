namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014080101
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014080100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_SERVICE_LOG",
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "SERVICE_LOG_FILE", "B4_FILE_INFO", "ID"),
                new Column("CASH_PAY_CENTER_NAME", DbType.String),
                new Column("STATUS", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DATE_EXECUTE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("FILE_DATE", DbType.DateTime),
                new Column("FILE_NUM", DbType.String),
                new Column("METHOD_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SERVICE_LOG");
        }
    }
}
