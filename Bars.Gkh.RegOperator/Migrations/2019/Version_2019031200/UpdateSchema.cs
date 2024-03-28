namespace Bars.Gkh.RegOperator.Migrations._2019.Version_2019031200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019031200")]
   
    [MigrationDependsOn(typeof(Version_2019031100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_DEBTOR", "ROSREG_ACC_MATCHED", DbType.Int32, ColumnProperty.Null);
          
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_DEBTOR", "ROSREG_ACC_MATCHED");
        
        }
    }
}