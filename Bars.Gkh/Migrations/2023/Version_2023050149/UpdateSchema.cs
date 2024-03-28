namespace Bars.Gkh.Migrations._2023.Version_2023050149
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050149")]

    [MigrationDependsOn(typeof(Version_2023050148.UpdateSchema))]

    /// Является Version_2022072500 из ядра
    public class UpdateSchema : Migration
    {
        private readonly string tableName = "GKH_OPERATOR";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddRefColumn(this.tableName, new RefColumn("USER_PHOTO_FILE_ID", "GKH_USER_PHOTO_FILE_ID", "B4_FILE_INFO", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.tableName, "USER_PHOTO_FILE_ID");
        }
    }
}