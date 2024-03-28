namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_202206082022
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    [MigrationDependsOn(typeof(Version_2022051900.UpdateSchema))]
    [Migration("202206082022")]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName[] tables =
        {
            new SchemaQualifiedObjectName { Name = "GJI_TAT_DISPOSAL_ANNEX" },
            new SchemaQualifiedObjectName { Name = "GJI_KNM_REASON" }
        };
        private readonly Column column = new Column("ATTACHMENT_GUID", DbType.String.WithSize(36));

        public override void Up()
        {
            this.tables.ForEach(x => this.Database.RemoveColumn(x, this.column.Name));
        }

        public override void Down()
        {
            this.tables.ForEach(x => this.Database.AddColumn(x, this.column));
        }
    }
}