namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015071000
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015071000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015070801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery(@"ALTER TABLE regop_payment_doc_snapshot ALTER COLUMN payer TYPE text;");
        }

        public override void Down()
        {
            //no down
        }
    }
}
