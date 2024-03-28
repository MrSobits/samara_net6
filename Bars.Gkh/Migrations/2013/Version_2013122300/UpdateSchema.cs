namespace Bars.Gkh.Migrations.Version_Version_2013122300
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //удаляю интовое поле
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "HCS_NUMBER");

            //создаю строковое
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("HCS_NUMBER", DbType.String, 100));
        }
        
        public override void Down()
        {
            // удаляю строковое
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "HCS_NUMBER");

            //создаю интовое
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("HCS_NUMBER", DbType.Int64, 22));
        }
    }
}