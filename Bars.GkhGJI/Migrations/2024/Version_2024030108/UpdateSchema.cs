namespace Bars.GkhGji.Migrations._2024.Version_2024030108
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030108")]
    [MigrationDependsOn(typeof(Version_2024030107.UpdateSchema))]
    /// Является Version_2019060300 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_INSPECTION_RISK",
                new RefColumn("INSPECTION_ID", ColumnProperty.NotNull, "GJI_INSPECTION_RISK_INSPECTION", "GJI_INSPECTION", "ID"),
                new RefColumn("RISK_CATEGORY_ID", ColumnProperty.NotNull, "GJI_INSPECTION_RISK_RISKCAT", "GKH_DICT_RISK_CATEGORY", "ID"),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.Null));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_INSPECTION_RISK");
        }
    }
}