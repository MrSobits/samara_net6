namespace Bars.Gkh.Regions.Voronezh.Migrations._2018.Version_2018122100
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Regions.Voronezh.Helpers;

    [Migration("2018122100")]
    [MigrationDependsOn(typeof(Version_2018110800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(EmbeddedResourceHelper.GetStringFromEmbedded("DebtorCleanup.sql"));
        }

        public override void Down()
        {
        }
    }
}