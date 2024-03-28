namespace Bars.GkhGji.Migration.Version_2014071501
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014071500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_PRESCRIPTION_VIOLAT", new Column("ACTION", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.ChangeColumn("GJI_PRESCRIPTION_VIOLAT", new Column("ACTION", DbType.String, 500));
        }
    }
}