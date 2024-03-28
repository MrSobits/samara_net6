namespace Bars.Gkh.Migrations._2017.Version_2017102600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017102600")]
    [MigrationDependsOn(typeof(Version_2017101500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_ROOM", new Column("CADASTRAL", DbType.String, 50, ColumnProperty.Null));
        }

        /// <inheritdoc />
        public override void Down()
        {
        }
    }
}