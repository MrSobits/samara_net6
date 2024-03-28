namespace Bars.Gkh.Migrations._2023.Version_2023050105
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050105")]

    [MigrationDependsOn(typeof(Version_2023050104.UpdateSchema))]

    /// Является Version_2018040400 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GKH_FORMAT_DATA_EXPORT_TASK", new Column("BASE_PARAMS", DbType.Binary));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_FORMAT_DATA_EXPORT_TASK", "BASE_PARAMS");
        }
    }
}