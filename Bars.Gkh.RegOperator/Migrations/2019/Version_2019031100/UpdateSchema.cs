namespace Bars.Gkh.RegOperator.Migrations._2019.Version_2019031100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019031100")]
   
    [MigrationDependsOn(typeof(_2018.Version_2018112100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("DEBTOR_STATE_HISTORY", DbType.Int32, ColumnProperty.None));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "DEBTOR_STATE_HISTORY");
        }
    }
}