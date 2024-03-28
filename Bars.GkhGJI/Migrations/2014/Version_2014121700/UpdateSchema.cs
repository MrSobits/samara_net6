namespace Bars.GkhGji.Migrations._2014.Version_2014121700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014121603.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_INSPECTION_ROBJECT", new Column("ROOM_NUMS", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_ROBJECT", "ROOM_NUMS");
        }
    }
}