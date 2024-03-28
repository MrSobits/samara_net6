namespace Bars.Gkh.Migrations._2023.Version_2023050116
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050116")]

    [MigrationDependsOn(typeof(Version_2023050115.UpdateSchema))]

    /// Является Version_2019010900 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_ROOM", new Column("HAS_SEPARATE_ENTRANCE", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_ROOM", "HAS_SEPARATE_ENTRANCE");
        }
    }
}