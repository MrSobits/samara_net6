namespace Bars.Gkh.Migrations._2023.Version_2023050130
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050130")]

    [MigrationDependsOn(typeof(Version_2023050129.UpdateSchema))]

    /// Является Version_2020032601 из ядра
    public class UpdateSchema : Migration
    {
        private const string OtherServiceTableName = "di_other_service";
        private const string UnitMeasureColumn = "unit_measure_id";
        private const string UnitMeasureConstraintName = "other_service_unit_measure_constraint";
        private const string UnitMeasureTableName = "gkh_dict_unitmeasure";


        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddRefColumn(UpdateSchema.OtherServiceTableName, new RefColumn(UpdateSchema.UnitMeasureColumn, ColumnProperty.Null,
                UpdateSchema.UnitMeasureConstraintName, UpdateSchema.UnitMeasureTableName, "id"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.OtherServiceTableName, UpdateSchema.UnitMeasureColumn);
        }
    }
}