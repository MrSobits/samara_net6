namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017020200
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2017020200")]
    [MigrationDependsOn(typeof(Version_2017011801.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_PAYMENT_OPERATION_BASE", new FileColumn("DOC_ID", ColumnProperty.Null, "REGOP_PAYMENT_DOC"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PAYMENT_OPERATION_BASE", "DOC_ID");
        }
    }
}
