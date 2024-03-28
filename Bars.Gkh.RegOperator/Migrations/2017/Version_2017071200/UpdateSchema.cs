namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017071200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017071200")]
    [MigrationDependsOn(typeof(Version_2017062300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017070400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_CHES_IMPORT", new Column("IMPORTED_FILES", DbType.String, 500));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_CHES_IMPORT", "IMPORTED_FILES");
        }
    }
}