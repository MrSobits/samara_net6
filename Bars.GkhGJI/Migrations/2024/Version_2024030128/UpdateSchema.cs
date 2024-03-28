namespace Bars.GkhGji.Migrations._2024.Version_2024030128
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030128")]
    [MigrationDependsOn(typeof(Version_2024030127.UpdateSchema))]
    /// Является Version_2022052000 из ядра
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_DICT_SANCTION";
        private Column ErknmGuidColumn => new Column("ERKNM_GUID", DbType.String.WithSize(36));

        public override void Up()
        {
            this.Database.AddColumn(TableName, this.ErknmGuidColumn);
        }

        public override void Down()
        {
            this.Database.RemoveColumn(TableName, this.ErknmGuidColumn.Name);
        }
    }
}