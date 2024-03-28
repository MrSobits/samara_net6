namespace Bars.GkhGji.Migrations._2022.Version_2022052300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [MigrationDependsOn(typeof(Version_2022051700.UpdateSchema))]
    [Migration("2022052300")]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName actCheckTable = new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK" };
        private readonly Column column = new Column("PLACE_ERKNM_GUID", DbType.String.WithSize(36));
        
        public override void Up()
        {
            this.Database.AddColumn(this.actCheckTable, this.column);
        }

        public override void Down()
        {
            this.Database.RemoveColumn(this.actCheckTable, this.column.Name);
        }
    }
}