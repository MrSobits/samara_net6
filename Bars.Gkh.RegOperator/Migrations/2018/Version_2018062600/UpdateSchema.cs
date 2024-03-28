namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018062600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018062600")]
   
    [MigrationDependsOn(typeof(Version_2018062000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        string table = "REGOP_LAWSUIT_OWNER_INFO";
        string column = "CLAIM_NUMBER";
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.RemoveColumn(table,column);
            this.Database.AddColumn(table, column, DbType.String, ColumnProperty.None);
        }
        public override void Down()
        {
            this.Database.RemoveColumn(table, column);
            this.Database.AddColumn(table, column, DbType.Int32, ColumnProperty.None);
        }
    }
}