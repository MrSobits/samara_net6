namespace Bars.Gkh.Migrations._2022.Version_2022031000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022031000")]
    [MigrationDependsOn(typeof(Version_2022020200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName() { Name = "GKH_DICT_WORK" };
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, new Column("WITHIN_SHORT_PROGRAM", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, "WITHIN_SHORT_PROGRAM");
        }
    }
}