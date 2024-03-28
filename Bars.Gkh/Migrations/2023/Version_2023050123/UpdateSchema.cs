namespace Bars.Gkh.Migrations._2023.Version_2023050123
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050123")]

    [MigrationDependsOn(typeof(Version_2023050122.UpdateSchema))]

    /// Является Version_2019100701 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_DICT_INSPECTOR",
                new Column("ERP_GUID", DbType.String.WithSize(36), ColumnProperty.Null));

            this.Database.AddColumn("GKH_REALITY_OBJECT",
                new Column("ERP_GUID", DbType.String.WithSize(36), ColumnProperty.Null));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_INSPECTOR", "ERP_GUID");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "ERP_GUID");
        }
    }
}