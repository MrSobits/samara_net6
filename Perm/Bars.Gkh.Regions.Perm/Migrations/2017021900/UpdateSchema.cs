namespace Bars.Gkh.Regions.Perm.Migrations._2017021900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2017021900")]
    [MigrationDependsOn(typeof(Gkh.Migrations._2017.Version_2017021100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("GKH_MANORG_LIC_REQUEST", "CONTRAGENT_ID", true);
        }

        /// <inheritdoc />
        public override void Down()
        {
        }
    }
}