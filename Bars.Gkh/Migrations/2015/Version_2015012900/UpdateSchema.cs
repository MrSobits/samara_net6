using System.Data;
using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Migrations.Version_2015012900
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015012800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_PERSON", new RefColumn("STATE_ID", "GKH_PERSON_ST", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PERSON", "STATE_ID");
        }
    }
}