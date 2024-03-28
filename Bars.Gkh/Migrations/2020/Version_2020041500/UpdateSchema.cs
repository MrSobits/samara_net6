namespace Bars.Gkh.Migrations._2020.Version_2020041500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020041500")]
    
    [MigrationDependsOn(typeof(Version_2020032600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("CLW_LAWSUIT", new Column("DIRECTED_TO_DEBTOR", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT", "DIRECTED_TO_DEBTOR");
        }
    }
}