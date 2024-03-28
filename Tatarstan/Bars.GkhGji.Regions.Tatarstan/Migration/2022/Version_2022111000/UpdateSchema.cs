namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022111000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022111000")]
    [MigrationDependsOn(typeof(Version_2022110800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName appealResponseTable = new SchemaQualifiedObjectName { Name = "GJI_RAPID_RESPONSE_SYSTEM_APPEAL_RESPONSE" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.appealResponseTable.Name,
                new RefColumn("RAPID_RESPONSE_SYSTEM_APPEAL_DETAILS_ID", ColumnProperty.NotNull | ColumnProperty.Unique, $"{this.appealResponseTable.Name}_APPEAL_DETAILS", "GJI_RAPID_RESPONSE_SYSTEM_APPEAL_DETAILS", "ID"),
                new RefColumn("FILE_ID", $"{this.appealResponseTable.Name}_FILE", "B4_FILE_INFO", "ID"),
                new Column("RESPONSE", DbType.String.WithSize(8000)),
                new Column("THEME", DbType.String.WithSize(255)),
                new Column("CARRIED_OUT_WORK", DbType.String.WithSize(2000)),
                new Column("RESPONSE_DATE", DbType.Date));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.appealResponseTable);
        }
    }
}