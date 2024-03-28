namespace Bars.Gkh.Migrations._2015.Version_2015081400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015081400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015080700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("REG_DATE_SOC_USE", DbType.DateTime));
            Database.AddColumn("GKH_CONTRAGENT", new Column("LICENSE_DATE_RECEIPT", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "REG_DATE_SOC_USE");
            Database.RemoveColumn("GKH_CONTRAGENT", "LICENSE_DATE_RECEIPT");
        }
    }
}