namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2022052300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using System.Data;

    [Migration("2022052300")]
    [MigrationDependsOn(typeof(Version_2019122401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName Table => new SchemaQualifiedObjectName { Name = "GJI_TATARSTAN_RESOLUTION" };

        private readonly Column[] Columns = {
            new Column("SANCTIONS_DURATION", DbType.String),
            new Column("SNILS", DbType.String)
        };

        public override void Up()
        {
            Columns.ForEach(column => this.Database.AddColumn(this.Table, column));

        }

        public override void Down()
        {
            Columns.ForEach(column => this.Database.RemoveColumn(this.Table, column.Name));
        }
    }
}
