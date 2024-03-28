namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022011200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022011200")]
    [MigrationDependsOn(typeof(Version_2022011000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName Table =
            new SchemaQualifiedObjectName { Name = "GJI_WARNING_DOC_VIOLATIONS", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.Table, new Column("TAKEN_MEASURES", DbType.String, 500));
            this.Database.AddColumn(this.Table, new Column("DATE_SOLUTION", DbType.Date));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.Table, "TAKEN_MEASURES");
            this.Database.RemoveColumn(this.Table, "DATE_SOLUTION");
        }
    }
}
