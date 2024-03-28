namespace Bars.Gkh.Migrations._2023.Version_2023050126
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050126")]

    [MigrationDependsOn(typeof(Version_2023050125.UpdateSchema))]

    /// Является Version_2019121400 из ядра
    public class UpdateSchema : Migration
    {
        private const string ContragentTableName = "GKH_CONTRAGENT";
        private const string RealityObjTableName = "GKH_REALITY_OBJECT";
        private const string TorIdColumnName = "TOR_ID";

        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.ContragentTableName, new GuidColumn(UpdateSchema.TorIdColumnName, ColumnProperty.Null));
            this.Database.AddColumn(UpdateSchema.RealityObjTableName, new GuidColumn(UpdateSchema.TorIdColumnName, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.ContragentTableName, UpdateSchema.TorIdColumnName);
            this.Database.RemoveColumn(UpdateSchema.RealityObjTableName, UpdateSchema.TorIdColumnName);
        }
    }
}