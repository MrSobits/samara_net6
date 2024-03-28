namespace Bars.Gkh.Migrations._2017.Version_2017030600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция Gkh.2017030600
    /// </summary>
    [Migration("2017030600")]
    [MigrationDependsOn(typeof(Version_2017030200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_FORM_GOV_SERVICE",
                new Column("SERVICE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("YEAR", DbType.Int32, ColumnProperty.NotNull),
                new Column("QUARTER", DbType.Int32, ColumnProperty.NotNull));

            this.Database.AddEntityTable(
                "GKH_SERVICE_DETAIL_GROUP",
                new Column("NAME", DbType.String, 500),
                new Column("GROUP_NAME", DbType.String),
                new Column("ROW_NUMBER", DbType.Int32, ColumnProperty.NotNull),
                new Column("SECTION", DbType.Int32, ColumnProperty.NotNull));

            this.Database.AddEntityTable(
                "GKH_FORM_GOV_SERVICE_DETAIL",
                new Column("VALUE", DbType.Decimal),
                new RefColumn("GROUP_ID", ColumnProperty.NotNull, "FORM_GOV_SERVICE_GROUP_ID", "GKH_SERVICE_DETAIL_GROUP", "ID"),
                new RefColumn("GOV_SERVICE_ID", ColumnProperty.NotNull, "FORM_GOV_SERVICE_SERVICE_ID", "GKH_FORM_GOV_SERVICE", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_FORM_GOV_SERVICE_DETAIL");
            this.Database.RemoveTable("GKH_SERVICE_DETAIL_GROUP");
            this.Database.RemoveTable("GKH_FORM_GOV_SERVICE");
        }
    }
}