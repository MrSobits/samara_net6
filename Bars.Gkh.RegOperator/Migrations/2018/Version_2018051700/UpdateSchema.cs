namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018051700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018051700")]
   
    [MigrationDependsOn(typeof(Version_2018051500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_DEBTOR", "EXTRACT_EXISTS", DbType.Int32, ColumnProperty.Null);
          
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_DEBTOR", "EXTRACT_EXISTS");
        
        }
    }
}