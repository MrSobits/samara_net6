namespace Bars.Gkh.Migrations._2023.Version_2023050124
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050124")]

    [MigrationDependsOn(typeof(Version_2023050123.UpdateSchema))]

    /// Является Version_2019112200 из ядра
    public class UpdateSchema : Migration
    {
        private const string OutdoorTableName = "GKH_REALITY_OBJECT_OUTDOOR";
        private const string RealityObjTableName = "GKH_REALITY_OBJECT";
        private const string OutdoorColumnName = "OUTDOOR_ID";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.OutdoorTableName,
                new RefColumn("MUNICIPALITY_FIAS_OKTMO_ID", ColumnProperty.NotNull,
                    "REALITY_OBJECT_OUTDOOR_MUNICIPALITY_FIAS_OKTMO_ID", "GKH_MUNICIPALITY_FIAS_OKTMO", "ID"),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 255, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("AREA", DbType.Decimal),
                new Column("ASPHALT_AREA", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String, 500));

            this.Database.AddRefColumn(UpdateSchema.RealityObjTableName,
                new RefColumn(UpdateSchema.OutdoorColumnName, "REALITY_OBJECT_REALITY_OBJECT_OUTDOOR", UpdateSchema.OutdoorTableName, "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.RealityObjTableName, UpdateSchema.OutdoorColumnName);
            this.Database.RemoveTable(UpdateSchema.OutdoorTableName);
        }
    }
}