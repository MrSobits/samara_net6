namespace Bars.Gkh.Migrations._2017.Version_2017011800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017011800
    /// </summary>
    [Migration("2017011800")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017011100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_TYPE_CUSTOMER", new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddTable("GKH_RSOCONTRACT_BUDGET_ORG", new Column("ID", DbType.Int64, ColumnProperty.NotNull));

            this.Database.AddForeignKey("FK_RSOCONTRACT_BUDGET_ORG_ID", "GKH_RSOCONTRACT_BUDGET_ORG", "ID", "GKH_RSOCONTRACT_BASE_PARTY", "ID");
            this.Database.AddIndex("IND_RSOCONTRACT_BUDGET_ORG_ID", true, "GKH_RSOCONTRACT_BUDGET_ORG", "ID");

            this.Database.AddRefColumn("GKH_RSOCONTRACT_BUDGET_ORG", new RefColumn("CONTRAGENT_ID", ColumnProperty.Null, "RSOCONTRACT_BUDGET_ORG_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));
            this.Database.AddRefColumn("GKH_RSOCONTRACT_BUDGET_ORG", new RefColumn("TYPE_CUSTOMER_ID", ColumnProperty.NotNull, "RSOCONTRACT_BUDGET_ORG_TYPE_CUSTOMER_ID", "GKH_DICT_TYPE_CUSTOMER", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_RSOCONTRACT_BUDGET_ORG");
            this.Database.RemoveTable("GKH_DICT_TYPE_CUSTOMER");
        }
    }
}