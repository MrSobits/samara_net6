namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2017050300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017050300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2017011600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("LETTER_NUMBER", DbType.String));
            Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("LETTER_DATE", DbType.Date));

            Database.AddColumn("DEC_GOV_DECISION", new Column("LETTER_NUMBER", DbType.String));
            Database.AddColumn("DEC_GOV_DECISION", new Column("LETTER_DATE", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "LETTER_NUMBER");
            Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "LETTER_DATE");

            Database.RemoveColumn("DEC_GOV_DECISION", "LETTER_NUMBER");
            Database.RemoveColumn("DEC_GOV_DECISION", "LETTER_DATE");
        }
    }
}