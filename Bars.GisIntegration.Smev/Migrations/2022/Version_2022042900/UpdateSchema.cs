namespace Bars.GisIntegration.Smev.Migrations._2022.Version_2022042900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022042900")]
    [MigrationDependsOn(typeof(Version_2022033000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ErknmTableName = "GI_ERKNM";
        private const string ErpTableName = "GI_ERP_GUID";
        private readonly Column erknmNewColumn = new Column("FIELD_NAME", DbType.String, ColumnProperty.NotNull, "'ErknmGuid'");
        private readonly Column erpNewColumn = new Column("FIELD_NAME", DbType.String, ColumnProperty.NotNull, "'ErpGuid'");
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.ErpTableName, this.erpNewColumn);
            this.Database.AddColumn(UpdateSchema.ErknmTableName, this.erknmNewColumn);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.ErpTableName, this.erpNewColumn.Name);
            this.Database.RemoveColumn(UpdateSchema.ErknmTableName, this.erknmNewColumn.Name);
        }
    }
}