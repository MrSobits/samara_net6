namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018032300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018032300")]
   
    [MigrationDependsOn(typeof(Version_2018020600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LAWSUIT_OWNER_INFO", new Column("UNDERAGE", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("REGOP_LAWSUIT_OWNER_INFO", new Column("SHARED_OWNERSHIP", DbType.Boolean, ColumnProperty.NotNull, false));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_OWNER_INFO", "UNDERAGE");
            this.Database.RemoveColumn("REGOP_LAWSUIT_OWNER_INFO", "SHARED_OWNERSHIP");
        }
    }
}