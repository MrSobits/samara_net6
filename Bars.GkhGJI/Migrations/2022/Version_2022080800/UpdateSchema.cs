namespace Bars.GkhGji.Migrations._2022.Version_2022080800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022080800")]
    [MigrationDependsOn(typeof(Version_2022072100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName()
        {
            Name = "GJI_PROTOCOL_ARTLAW"
        };

        private readonly Column erknmGuidColumn = new Column("ERKNM_GUID", DbType.String.WithSize(36));
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, this.erknmGuidColumn);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, this.erknmGuidColumn.Name);
        }
    }
}