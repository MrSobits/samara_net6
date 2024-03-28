namespace Bars.Gkh.Migrations._2023.Version_2023050142
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050142")]

    [MigrationDependsOn(typeof(Version_2023050141.UpdateSchema))]

    /// Является Version_2021082300 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GKH_CONTRAGENT", new Column("RECEIVE_NOTIFICATIONS", DbType.Int32, ColumnProperty.None, (int)YesNo.No));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_CONTRAGENT", "RECEIVE_NOTIFICATIONS");
        }
    }
}