namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015062500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015062301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_IMPORTED_PAYMENT",
                new Column("EXTERNAL_TRANSACTION", DbType.String, 64, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "EXTERNAL_TRANSACTION");
        }
    }
}
