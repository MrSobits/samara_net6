namespace Bars.Gkh.Migrations._2020.Version_2020022500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020022500")]
    
    [MigrationDependsOn(typeof(_2020.Version_2020012000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("IS_MOTIONLESS", DbType.Boolean));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_MOTIONLESS", DbType.DateTime));
            Database.AddColumn("CLW_LAWSUIT", new Column("IS_ERROR_FIX", DbType.Boolean));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_ERROR_FIX", DbType.DateTime));
            Database.AddColumn("CLW_LAWSUIT", new Column("IS_LIMITATION_OF_ACTIONS", DbType.Boolean));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_LIMITATION_OF_ACTIONS", DbType.DateTime));
            Database.AddColumn("CLW_LAWSUIT", new Column("IS_DISTANCE_DECISION_CANCEL", DbType.Boolean));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_DISTANCE_DECISION_CANCEL", DbType.DateTime));
           
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT","IS_MOTIONLESS");
            Database.RemoveColumn("CLW_LAWSUIT","DATE_MOTIONLESS");
            Database.RemoveColumn("CLW_LAWSUIT","IS_ERROR_FIX");
            Database.RemoveColumn("CLW_LAWSUIT","DATE_ERROR_FIX");
            Database.RemoveColumn("CLW_LAWSUIT","IS_LIMITATION_OF_ACTIONS");
            Database.RemoveColumn("CLW_LAWSUIT","DATE_LIMITATION_OF_ACTIONS");
            Database.RemoveColumn("CLW_LAWSUIT","IS_DISTANCE_DECISION_CANCEL");
            Database.RemoveColumn("CLW_LAWSUIT","DATE_DISTANCE_DECISION_CANCEL");
            
            
        }
    }
}