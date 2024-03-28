using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Regions.Voronezh.Migrations._2019.Version_2019011400
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Regions.Voronezh.Helpers;

    [Migration("2019011400")]
    [MigrationDependsOn(typeof(_2018.Version_2018122300.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
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