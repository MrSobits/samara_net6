namespace Bars.Gkh.Migrations.Version_2013091101
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013091100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OPERATOR", new Column("PKU_PR", DbType.Int64));
            Database.AddColumn("GKH_OPERATOR", new RefColumn("CONTRAGENT_ID", "OPER_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OPERATOR", "PKU_PR");
            Database.RemoveColumn("GKH_OPERATOR", "CONTRAGENT_ID");
        }
    }
}