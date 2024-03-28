namespace Bars.Gkh.Migrations._2020.Version_2020032100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020032100")]
    
    [MigrationDependsOn(typeof(Version_2020032000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("CLW_LAWSUIT", new Column("REDIRECT_DATE", DbType.DateTime));
            this.Database.AddColumn("CLW_LAWSUIT",new Column("DUTY_DEBT_APPROVED", DbType.Decimal));
            this.Database.AddColumn("CLW_LAWSUIT",new Column("DISTANCE_DECISION_CANCEL_COMMENT", DbType.String,300));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT", "REDIRECT_DATE");
            this.Database.RemoveColumn("CLW_LAWSUIT", "DUTY_DEBT_APPROVED");
            this.Database.RemoveColumn("CLW_LAWSUIT", "DISTANCE_DECISION_CANCEL_COMMENT");
        }
    }
}