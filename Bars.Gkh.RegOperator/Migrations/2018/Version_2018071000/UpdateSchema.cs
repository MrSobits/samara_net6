namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018071000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018071000")]
   
    [MigrationDependsOn(typeof(Version_2018062600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        string table = "REGOP_LAWSUIT_OWNER_INFO";
        string column = "SNILS";
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn(table, column, DbType.String, ColumnProperty.None);
        }
        public override void Down()
        {
            this.Database.RemoveColumn(table, column);
        }
    }
}