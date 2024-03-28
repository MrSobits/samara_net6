namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017041100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017041100")]
    [MigrationDependsOn(typeof(Version_2017032400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERS_ACC_BAN_RECALC", new Column("TYPE", DbType.Int32));
            this.Database.ExecuteNonQuery("UPDATE REGOP_PERS_ACC_BAN_RECALC SET TYPE = 3");
            this.Database.ChangeColumnNotNullable("REGOP_PERS_ACC_BAN_RECALC", "TYPE", true);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_BAN_RECALC", "TYPE");
        }
    }
}