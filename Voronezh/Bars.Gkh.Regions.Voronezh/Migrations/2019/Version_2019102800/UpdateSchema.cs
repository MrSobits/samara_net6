using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Regions.Voronezh.Migrations._2019.Version_2019102800
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Regions.Voronezh.Helpers;
    using Bars.Gkh.Utils;

    [Migration("2019102800")]
    [MigrationDependsOn(typeof(_2019.Version_2019011700.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT_DEBT_WORK_SSP_DOC", new Column("NUMBER_DOC", DbType.String));
            this.Database.AlterColumnSetNullable("CLW_LAWSUIT_DEBT_WORK_SSP_DOC", "NUMBER", true);

        }

        public override void Down()
        {
            this.Database.AlterColumnSetNullable("CLW_LAWSUIT_DEBT_WORK_SSP_DOC", "NUMBER", false);
            Database.RemoveColumn("CLW_LAWSUIT_DEBT_WORK_SSP_DOC", "NUMBER_DOC");
        }
    }
}