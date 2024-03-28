namespace Bars.Gkh.Migrations._2020.Version_2020031200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020031200")]
    
    [MigrationDependsOn(typeof(Version_2020022500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_INSPECTOR", new Column("IS_ACTIVE", DbType.Boolean, true));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_INSPECTOR", "IS_ACTIVE");
        }
    }
}