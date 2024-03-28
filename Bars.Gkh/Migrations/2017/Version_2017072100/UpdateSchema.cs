namespace Bars.Gkh.Migrations._2017.Version_2017072100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017072100")]
    [MigrationDependsOn(typeof(Version_2017071300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017061900.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017062100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017070601.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017071000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("TERMINATION_DATE", DbType.DateTime));

            this.Database.ExecuteNonQuery(@"UPDATE GKH_MORG_CONTRACT b 
                SET TERMINATION_DATE = q.TERMINATION_DATE
                FROM GKH_MORG_JSKTSJ_CONTRACT q where q.id = b.id");

            this.Database.ExecuteNonQuery(@"UPDATE GKH_MORG_CONTRACT b 
                SET TERMINATION_DATE = q.TERMINATION_DATE
                FROM GKH_MORG_CONTRACT_JSKTSJ q where q.id = b.id");

            this.Database.ExecuteNonQuery(@"UPDATE GKH_MORG_CONTRACT b 
                SET TERMINATION_DATE = q.TERMINATION_DATE
                FROM GKH_MORG_CONTRACT_OWNERS q where q.id = b.id");

            this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "TERMINATION_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "TERMINATION_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "TERMINATION_DATE");
        }

        public override void Down()
        {
            this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("TERMINATION_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("TERMINATION_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("TERMINATION_DATE", DbType.DateTime));

            this.Database.ExecuteNonQuery(@"UPDATE GKH_MORG_JSKTSJ_CONTRACT b 
                SET TERMINATION_DATE = q.TERMINATION_DATE
                FROM GKH_MORG_CONTRACT q where q.id = b.id");

            this.Database.ExecuteNonQuery(@"UPDATE GKH_MORG_CONTRACT_JSKTSJ b 
                SET TERMINATION_DATE = q.TERMINATION_DATE
                FROM GKH_MORG_CONTRACT q where q.id = b.id");

            this.Database.ExecuteNonQuery(@"UPDATE GKH_MORG_CONTRACT_OWNERS b 
                SET TERMINATION_DATE = q.TERMINATION_DATE
                FROM GKH_MORG_CONTRACT q where q.id = b.id");

            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "TERMINATION_DATE");
        }
    }
}