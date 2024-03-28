namespace Bars.GkhGji.Migrations._2022.Version_2022051700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022051700")]
    [MigrationDependsOn(typeof(Version_2022050500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName Table => new SchemaQualifiedObjectName { Name = "GJI_DOCUMENT" };
        private Column Column => new Column("QR_CODE_ACCESS_TOKEN", DbType.String.WithSize(36));

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.Table, this.Column);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.Table, this.Column.Name);
        }
    }
}