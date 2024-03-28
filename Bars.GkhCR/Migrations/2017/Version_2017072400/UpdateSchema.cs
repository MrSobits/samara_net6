namespace Bars.GkhCr.Migrations._2017.Version_2017072400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017072400")]
    [MigrationDependsOn(typeof(Version_2017062400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("CR_CONTR_CR_TYPE_WRK",
                new RefColumn("CONTRACT_CR_ID", ColumnProperty.NotNull, "CR_CONTR_CR_TYPE_WRK_CR_ID", "CR_OBJ_CONTRACT", "ID"),
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "CR_CONTR_CR_TYPE_WRK_ID", "CR_OBJ_TYPE_WORK", "ID"),
                new Column("SUM", DbType.Decimal));

            this.Database.AddRefColumn(
                "CR_OBJ_CONTRACT",
                new RefColumn("CUSTOMER_ID", "CR_OBJ_CONTRACT_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CR_CONTR_CR_TYPE_WRK");
            this.Database.RemoveColumn("CR_OBJ_CONTRACT", "CUTOMER");
        }
    }
}