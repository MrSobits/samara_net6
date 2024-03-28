namespace Bars.Gkh.Overhaul.Migration.Version_2019082700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2019082700")]
    [MigrationDependsOn(typeof(Version_2018051700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_CREDIT_ORG", new Column("GIS_GKH_ROOT_GUID", DbType.String, 36));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_CREDIT_ORG", "GIS_GKH_ROOT_GUID");
        }
    }
}