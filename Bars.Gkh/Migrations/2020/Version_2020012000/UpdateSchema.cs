namespace Bars.Gkh.Migrations._2020.Version_2020012000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020012000")]
    
    [MigrationDependsOn(typeof(_2019.Version_2019121800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("EXPORT_TO_PORTAL", DbType.Boolean, true));
           
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "EXPORT_TO_PORTAL");
            
            
        }
    }
}