namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014041600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014041500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_TRANSFER_OBJ",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "REGOP_TRANSFER_OBJ_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("REC_ID", ColumnProperty.NotNull, "REGOP_TRANSFER_OBJ_REC", "RF_TRANSFER_RECORD", "ID"),
                new Column("TRANSFERRED", DbType.Boolean),
                new Column("TRANSFERRED_SUM", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_TRANSFER_OBJ");
        }
    }
}
