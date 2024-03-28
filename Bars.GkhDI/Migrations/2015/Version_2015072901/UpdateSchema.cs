namespace Bars.GkhDi.Migrations.Version_2015072901
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015072900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("RECEIVE_PRETENSION_CNT", DbType.Int32));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("APROVE_PRETENSION_CNT", DbType.Int32));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("NO_APROVE_PRETENSION_CNT", DbType.Int32));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("PRETENSION_RECALC_SUM", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("SENT_PRETENSION_CNT", DbType.Int32));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("SENT_PETITION_CNT", DbType.Int32));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("RECEIVE_SUM_BY_CLW", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "RECEIVE_SUM_BY_CLW");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "SENT_PETITION_CNT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "SENT_PRETENSION_CNT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "PRETENSION_RECALC_SUM");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "NO_APROVE_PRETENSION_CNT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "APROVE_PRETENSION_CNT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "RECEIVE_PRETENSION_CNT");
        }
    }
}