namespace Bars.Gkh.Migrations._2021.Version_2021061700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2021061700")]
    
    [MigrationDependsOn(typeof(Version_2021061101.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("IS_SOPR", DbType.Boolean, false));
            this.Database.ExecuteQuery("UPDATE GKH_CONTRAGENT SET IS_SOPR = IS_EDSE");
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "IS_SOPR");
        }
    }
}