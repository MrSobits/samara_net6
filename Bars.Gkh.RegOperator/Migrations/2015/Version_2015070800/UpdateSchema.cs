namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015070800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015070800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015070300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("regop_pers_paydoc_snap", new Column("room_address", DbType.String, 500));
        }

        public override void Down()
        {
            Database.ChangeColumn("regop_pers_paydoc_snap", new Column("room_address", DbType.String, 200));
        }
    }
}
