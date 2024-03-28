namespace Bars.Gkh.RegOperator.Migrations._2020.Version_2020082100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020082100")]
   
    [MigrationDependsOn(typeof(_2020.Version_2020052800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_UNCONFIRMED_PAYMENTS",
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "ACC_REF_ID", "REGOP_PERS_ACC", "ID"),
                new Column("PAYMENT_SUM", DbType.Decimal, ColumnProperty.Null),
                new Column("PGUID", DbType.String, ColumnProperty.Null),
                new Column("DESCRIPTION", DbType.String, ColumnProperty.Null),
                new Column("BANK_BIK", DbType.String, ColumnProperty.Null),
                new Column("BANK_NAME", DbType.String, ColumnProperty.Null),
                new Column("IS_CONFIRMED", DbType.Int32, ColumnProperty.Null),
                new RefColumn("FILE_ID", "FK_UNPAY_FILE", "B4_FILE_INFO", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_UNCONFIRMED_PAYMENTS");
        }
    }
}