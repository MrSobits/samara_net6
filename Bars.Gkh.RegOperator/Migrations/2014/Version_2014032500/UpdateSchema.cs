namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014032500
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014032100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Соответствующая сущность перенесена в модуль GkhCr. 
            // Необходимости в данной миграции нет, оставлена для целостности.
            // См. таблицу CR_ACTPAYMENT_DETAILS

            //Database.AddEntityTable("REGOP_ACTPAYMENT_DETAILS",
            //    new RefColumn("ACTPAYMENT_ID", ColumnProperty.NotNull, "REGOP_ACTPAYMENT_DETAILS_AP", "CR_OBJ_PER_ACT_PAYMENT", "ID"),
            //    new Column("SRC_FIN_TYPE", DbType.Int16, ColumnProperty.NotNull, 10),
            //    new Column("BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0),
            //    new Column("PAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            if (Database.TableExists("REGOP_ACTPAYMENT_DETAILS"))
            {
                Database.RemoveTable("REGOP_ACTPAYMENT_DETAILS");
            }
        }
    }
}
