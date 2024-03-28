namespace Bars.GkhGji.Migrations.Version_2014021500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014021300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("DESCRIPTION", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "DESCRIPTION");
        }
    }
}