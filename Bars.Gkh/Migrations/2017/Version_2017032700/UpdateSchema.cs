namespace Bars.Gkh.Migrations._2017.Version_2017032700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция 2017032700
    /// </summary>
    [Migration("2017032700")]
    [MigrationDependsOn(typeof(Version_2017031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_TECHNICAL_CUSTOMER",
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GKH_TECH_CUSTOMER_CONTR", "GKH_CONTRAGENT", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "GKH_TECH_CUSTOMER_PERIOD", "GKH_DICT_PERIOD", "ID"),
                new FileColumn("FILE_ID", "GKH_TECH_CUSTOMER_FILE"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_TECHNICAL_CUSTOMER");
        }
    }
}