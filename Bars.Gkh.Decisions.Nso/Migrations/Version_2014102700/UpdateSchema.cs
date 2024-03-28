namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014102700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014081200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("DATE_START", DbType.Date));

            Database.ExecuteNonQuery(@"update GKH_OBJ_D_PROTOCOL set date_start=protocol_date");

            Database.AlterColumnSetNullable("GKH_OBJ_D_PROTOCOL", "date_start", false);


            Database.AddColumn("DEC_GOV_DECISION", new Column("DATE_START", DbType.Date));

            Database.ExecuteNonQuery(@"update DEC_GOV_DECISION set date_start=protocol_date");

            Database.AlterColumnSetNullable("DEC_GOV_DECISION", "date_start", false);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "DATE_START");
            //Database.RemoveColumn("DEC_GOV_DECISION", "DATE_START");
        }
    }
}