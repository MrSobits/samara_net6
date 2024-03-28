namespace Bars.Gkh.Migrations.Version_2013051600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013051600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013051400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("DATE_REG", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "DATE_REG");
        }
    }
}