namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042604
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022042604")]
    [MigrationDependsOn(typeof(Version_2022042603.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_DECISION_INSPECTION_BASE";
        
        /// <summary>
        /// Создаём таблицу сущности DecisionInspectionBase
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(TableName,
                new RefColumn("INSP_BASE_TYPE_ID", ColumnProperty.NotNull, "GJI_DECISION_INSP_BASE_TYPE", "GJI_DICT_INSPECTION_BASE_TYPE", "ID"),
                new Column("OTHER_INSP_BASE_TYPE", DbType.String.WithSize(1000)),
                new Column("FOUNDATION_DATE", DbType.DateTime),
                new RefColumn("RISK_INDICATOR_ID", "GJI_DECISION_INSP_BASE_RISK_INDICATOR", "GJI_DICT_CONTROL_TYPE_RISK_INDICATORS", "ID"),
                new RefColumn("DECISION_ID", ColumnProperty.NotNull, "GJI_DECISION_INSP_BASE_TYPE_DISP_ID", "GJI_DECISION", "ID"),
                new Column("ERKNM_GUID", DbType.String.WithSize(36)));
        }

        public override void Down()
        {
            this.Database.RemoveTable(TableName);
        }
    }
}