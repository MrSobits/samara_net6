namespace Bars.GkhGji.Migrations._2024.Version_2024030109
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030109")]
    [MigrationDependsOn(typeof(Version_2024030108.UpdateSchema))]
    /// Является Version_2019081900 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GJI_DICT_VIOLATION",
                new Column("ERP_GUID", DbType.String.WithSize(36), ColumnProperty.Null));

            this.Database.AddColumn("GJI_PRESCRIPTION",
                new Column("ERP_GUID", DbType.String.WithSize(36), ColumnProperty.Null));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DICT_VIOLATION", "ERP_GUID");
            this.Database.RemoveColumn("GJI_PRESCRIPTION", "ERP_GUID");
        }
    }
}