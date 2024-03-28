namespace Bars.Gkh.Migrations._2023.Version_2023050121
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050121")]

    [MigrationDependsOn(typeof(Version_2023050120.UpdateSchema))]

    /// Является Version_2019081500 из ядра
    public class UpdateSchema : Migration
    {
        private const string TableName = "b4_fias_address";
        private const string ColumnName = "house";

        /// <inheritdoc />
        public override void Up()
        {
            if (!this.Database.TableExists(UpdateSchema.TableName))
            {
                return;
            }

            this.Database.ChangeColumn(UpdateSchema.TableName, new Column(UpdateSchema.ColumnName, DbType.String, 20));
        }

        /// <inheritdoc />
        public override void Down()
        {
            if (!this.Database.TableExists(UpdateSchema.TableName))
            {
                return;
            }
            this.Database.ChangeColumn(UpdateSchema.TableName, new Column(UpdateSchema.ColumnName, DbType.String, 10));
        }
    }
}