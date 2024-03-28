namespace Bars.GkhRf.Migrations._2023.Version_2023081000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2023081000")]
    [MigrationDependsOn(typeof(Version_2016052100.UpdateSchema))]
    public class UpdateSchema: Migration
    {
        private const string TableName = "RF_CONTRACT_OBJECT";
        private const string ColumnName = "NOTE";
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(TableName, new Column(ColumnName, DbType.String, 255));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(TableName, ColumnName);
        }
    }
}