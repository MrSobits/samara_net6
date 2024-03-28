namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017122100
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("20171212100"), MigrationDependsOn(typeof(Version_2017121100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.ChangeColumnNotNullable("REGOP_PERS_ACC_OWNER_INFO", "END_DATE", false);
        }

        public override void Down()
        {
        }
    }
}