namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021122300
{
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021122300")]
    [MigrationDependsOn(typeof(Version_2021120200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private List<Column> listColumns = new List<Column>
        {
            new Column("ENTREPRENEUR_CREATE_DATE", DbType.DateTime),
            new Column("ENTREPRENEUR_DEBT_SUM", DbType.Decimal)
        };
        
        private readonly SchemaQualifiedObjectName litigationTable = new SchemaQualifiedObjectName
        {
            Schema = "PUBLIC",
            Name = "FSSP_LITIGATION"
        };

        public override void Up()
        {
            foreach (var column in listColumns)
            {
                this.Database.AddColumn(litigationTable, column);
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            foreach (var column in listColumns)
            {
                this.Database.RemoveColumn(litigationTable, column.Name);
            }
        }
    }
}