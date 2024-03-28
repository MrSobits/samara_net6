namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021121700
{
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021121700")]
    [MigrationDependsOn(typeof(Version_2021121600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly List<Column> listColumn = new List<Column>
        {
            new Column("DOCUMENT_TIME", DbType.DateTime),
            new Column("INTERACTION_PERSON_HOUR", DbType.Int16),
            new Column("INTERACTION_PERSON_MINUTES", DbType.Int16),
            new Column("SUSPENSION_INSPECTION_BASE", DbType.String, 500),
            new Column("SUSPENSION_DATE_FROM", DbType.DateTime),
            new Column("SUSPENSION_DATE_TO", DbType.DateTime),
            new Column("SUSPENSION_TIME_FROM", DbType.DateTime),
            new Column("SUSPENSION_TIME_TO", DbType.DateTime)
        };

        /// <inheritdoc />
        public override void Up()
        {
            foreach (var column in this.listColumn)
            {
                this.Database.AddColumn("GJI_TAT_DISPOSAL", column);
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            foreach (var column in this.listColumn)
            {
                this.Database.RemoveColumn("GJI_TAT_DISPOSAL", column.Name);
            }
        }
    }
}