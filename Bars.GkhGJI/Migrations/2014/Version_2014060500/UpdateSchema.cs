namespace Bars.GkhGji.Migration.Version_2014060500
{
    using System.Data;

    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014052602.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_INSPECTION_VIOLATION", new Column("DESCRIPTION", DbType.String, 2000));
            Database.ChangeColumn("GJI_PRESCRIPTION_VIOLAT", new Column("DESCRIPTION", DbType.String, 2000));
        }

        public override void Down()
        {
            
        }
    }
}