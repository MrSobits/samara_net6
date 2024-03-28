namespace Bars.Gkh.RegOperator.Migrations._2020.Version_2020082800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020082800")]
   
    [MigrationDependsOn(typeof(_2020.Version_2020082100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PAYMENT_DOC_SNAPSHOT",
                new Column("HAS_EMAIL", DbType.Int32, ColumnProperty.Null, defaultValue: 20)
                );
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "HAS_EMAIL");
        }
    }
}