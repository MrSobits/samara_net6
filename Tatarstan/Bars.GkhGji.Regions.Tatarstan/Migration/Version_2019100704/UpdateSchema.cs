namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019100704
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2019100704")]
    [MigrationDependsOn(typeof(Version_2019100703.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string tableName = "GJI_TAT_DISPOSAL";

        public override void Up()
        {
            if (!this.Database.TableExists(UpdateSchema.tableName))
            {
                return;
            }

            this.Database.AddColumn(UpdateSchema.tableName, new Column("IS_SENT_TO_ERP", DbType.Boolean));
        }

        /// <inheritdoc />
        public override void Down()
        {
            const string columnName = "IS_SENT_TO_ERP";
            if (!this.Database.TableExists(UpdateSchema.tableName) || !this.Database.ColumnExists(UpdateSchema.tableName, columnName))
            {
                return;
            }

            this.Database.RemoveColumn(UpdateSchema.tableName, columnName);
        }
    }
}
