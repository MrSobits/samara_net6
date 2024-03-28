namespace Bars.Gkh.RegOperator.Migrations._2020.Version_2020091000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020091000")]
   
    [MigrationDependsOn(typeof(_2020.Version_2020082800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_UNCONFIRMED_PAYMENTS",
                new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.Null)
                );
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNCONFIRMED_PAYMENTS", "PAYMENT_DATE");
        }
    }
}