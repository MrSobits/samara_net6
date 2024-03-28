namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018042300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018042300")]
   
    [MigrationDependsOn(typeof(Version_2018040800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "BIRTH_DATE", DbType.String);
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "BIRTH_PLACE", DbType.String);
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "LIVE_PLACE", DbType.String);
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "BIRTH_DATE");
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "BIRTH_PLACE");
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "LIVE_PLACE");
        }
    }
}