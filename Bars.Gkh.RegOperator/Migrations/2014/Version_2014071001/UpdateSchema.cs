namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014071001
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014070900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("PAYM_CR_SPECACC_NOTREGOP",
                new RefColumn("REALOBJ_ID", ColumnProperty.NotNull, "PAYM_CR_REALOBJ", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "PAYM_CR_PERIOD", "REGOP_PERIOD", "ID"),
                new RefColumn("FILE_ID", "PAYM_CR_FILEINFO", "B4_FILE_INFO", "ID"),
                new Column("INPUT_DATE", DbType.Date),
                new Column("AMOUNT_INCOME", DbType.Decimal),
                new Column("ENDYEAR_BALANCE", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveTable("PAYM_CR_SPECACC_NOTREGOP");
        }
    }
}
