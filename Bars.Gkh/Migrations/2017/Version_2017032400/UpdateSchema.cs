namespace Bars.Gkh.Migrations._2017.Version_2017032400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <inheritdoc />
    [Migration("2017032400")]
    [MigrationDependsOn(typeof(Version_2017031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            if (!this.Database.ColumnExists("GKH_REALITY_OBJECT", "DATE_COMMISSIONING_LAST_SECTION"))
            {
                this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("DATE_COMMISSIONING_LAST_SECTION", DbType.Date));
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            if (this.Database.ColumnExists("GKH_REALITY_OBJECT", "DATE_COMMISSIONING_LAST_SECTION"))
            {
                this.Database.RemoveColumn("GKH_REALITY_OBJECT", "DATE_COMMISSIONING_LAST_SECTION");
            }
        }
    }
}
