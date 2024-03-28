namespace Bars.Gkh.RegOperator.Migrations._2020.Version_2020042800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020042800")]
   
    [MigrationDependsOn(typeof(_2020.Version_2020030400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_INDIVIDUAL_ACC_OWN", 
                new RefColumn("FACT_ADDR_DOC", ColumnProperty.Null, "FK_FILE_DOC_FACT_ADDR", "B4_FILE_INFO", "ID"));

        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "FACT_ADDR_DOC"); 
        }
    }
}