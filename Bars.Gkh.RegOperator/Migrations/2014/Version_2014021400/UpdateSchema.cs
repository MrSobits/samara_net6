namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_SALDO_CHANGE",
                new Column("CURRENT_VAL", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("NEW_VAL", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("REASON", DbType.String, 200, ColumnProperty.Null),
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "ROP_SALDOCH_ACC", "REGOP_PERS_ACC", "ID"),
                new RefColumn("DOC_ID", ColumnProperty.Null, "ROP_SALDOCH_DOC", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SALDO_CHANGE");
        }
    }
}
