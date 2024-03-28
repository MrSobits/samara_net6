namespace Bars.Gkh.Migrations._2023.Version_2023050125
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050125")]

    [MigrationDependsOn(typeof(Version_2023050124.UpdateSchema))]

    /// Является Version_2019120600 из ядра
    public class UpdateSchema : Migration
    {
        private const string OutdoorTableName = "GKH_REALITY_OBJECT_OUTDOOR";
        private const string RepairYearColumnName = "REPAIR_PLAN_YEAR";

        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.OutdoorTableName, new Column(UpdateSchema.RepairYearColumnName, DbType.Int32, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.OutdoorTableName, UpdateSchema.RepairYearColumnName);
        }
    }
}