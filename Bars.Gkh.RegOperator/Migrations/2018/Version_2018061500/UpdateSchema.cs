namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018061500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018061500")]
   
    [MigrationDependsOn(typeof(Version_2018052400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LAWSUIT_OWNER_INFO", "CLAIM_NUMBER", DbType.Int32, ColumnProperty.None);
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_OWNER_INFO", "CLAIM_NUMBER");
        }
    }
}