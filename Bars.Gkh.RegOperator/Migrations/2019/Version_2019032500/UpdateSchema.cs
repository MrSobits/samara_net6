namespace Bars.Gkh.RegOperator.Migrations._2019.Version_2019032500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019032500")]
   
    [MigrationDependsOn(typeof(Version_2019031200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
           Database.AddRefColumn("REGOP_LAWSUIT_OWNER_INFO", new RefColumn("JURINST_ID", ColumnProperty.None, "FK_REGOP_LAWSUIT_OWNER_JURINST", "CLW_JUR_INSTITUTION", "ID"));

        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_OWNER_INFO", "JURINST_ID");
        
        }
    }
}