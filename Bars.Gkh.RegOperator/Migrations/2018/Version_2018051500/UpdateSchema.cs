namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018051500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018051500")]
   
    [MigrationDependsOn(typeof(Version_2018042300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", "PAYMENT_DATE", DbType.String);
          
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", "PAYMENT_DATE");
        
        }
    }
}