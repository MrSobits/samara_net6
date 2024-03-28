namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021120200
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021120200")]
    [MigrationDependsOn(typeof(Version_2021110500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private List<Column> listColumns = new List<Column>
        {
            new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull, 0),
            new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull, "now()"),
            new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull, "now()")
        };

        private List<string> listTables = new List<string>
        {
            "FSSP_ADDRESS",
            "FSSP_LITIGATION",
            "PGMU_ADDRESSES"
        };

        /// <inheritdoc />
        public override void Up()
        {
            foreach (var table in listTables)
            {
                foreach (var column in listColumns)
                {
                    this.Database.AddColumn(table, column);
                }
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            foreach (var table in listTables)
            {
                foreach (var column in listColumns)
                {
                    this.Database.RemoveColumn(table, column.Name);
                }
            }
        }
    }
}
