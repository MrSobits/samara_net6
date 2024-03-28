namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022031400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022031400")]
    [MigrationDependsOn(typeof(Version_2022022400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ControlTypeTableName = "GJI_DICT_CONTROL_TYPES";
        private const string IdentifierColumnName = "ERVK_IDENTIFIER";
        private const string VersionColumnName = "ERVK_VERSION";
        
        /// <summary>
        /// Добавляем в таблицу GJI_DICT_CONTROL_TYPES два новых столбца
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn(ControlTypeTableName, 
                new Column(IdentifierColumnName, DbType.String.WithSize(36)));
            
            this.Database.AddColumn(ControlTypeTableName, 
                new Column(VersionColumnName, DbType.String.WithSize(36)));
        }

        /// <summary>
        /// При откате миграций удаляем два новых столбца
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn(ControlTypeTableName, IdentifierColumnName);
            this.Database.RemoveColumn(ControlTypeTableName, VersionColumnName);
        }
    }
}