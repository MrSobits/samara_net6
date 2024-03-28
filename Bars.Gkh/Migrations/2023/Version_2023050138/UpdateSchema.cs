namespace Bars.Gkh.Migrations._2023.Version_2023050138
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050138")]

    [MigrationDependsOn(typeof(Version_2023050137.UpdateSchema))]

    /// Является Version_2020101300 из ядра
    public class UpdateSchema : Migration
    {
        private const string ContragentTable = "GKH_CONTRAGENT";

        private const string ErpGuidColumn = "ERP_GUID";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.ContragentTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.ContragentTable, UpdateSchema.ErpGuidColumn);
        }
    }
}