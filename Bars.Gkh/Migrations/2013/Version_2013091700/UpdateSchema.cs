namespace Bars.Gkh.Migrations.Version_2013091700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013091200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_DICT_WORK", "DATE_END");
        }

        public override void Down()
        {
            Database.AddColumn("GKH_DICT_WORK", new Column("DATE_END", DbType.Date));
        }
    }
}