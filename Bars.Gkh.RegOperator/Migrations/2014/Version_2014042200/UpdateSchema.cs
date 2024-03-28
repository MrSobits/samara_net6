using System.Data;
using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014042200
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014041601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_GKH_EN_LOG_LGHT", false, "GKH_ENTITY_LOG_LIGHT", "ENTITY_ID");
        }

        public override void Down()
        {
            Database.RemoveIndex("IND_GKH_EN_LOG_LGHT", "GKH_ENTITY_LOG_LIGHT");
        }
    }
}
