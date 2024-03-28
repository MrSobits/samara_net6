namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030502
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030502")]
    [MigrationDependsOn(typeof(Version_2024030501.UpdateSchema))]
    // Является Version_2020090700 из ядра
    public class UpdateSchema : Migration
    {
        private const string TableName = "OVRHL_DPKR_DOCUMENTS";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.TableName,
                new Column("DOC_KIND_ID", DbType.Int64),
                new Column("DOC_NAME", DbType.String, ColumnProperty.NotNull),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "DPKR_DOCUMENTS_FILE_ID",
                    "B4_FILE_INFO", "ID"),
                new Column("DOC_NUMBER", DbType.String),
                new Column("DOC_DATE", DbType.DateTime),
                new Column("DOC_DEPARTMENT", DbType.String),
                new Column("STATE", DbType.Int32, ColumnProperty.NotNull));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.TableName);
        }
    }
}