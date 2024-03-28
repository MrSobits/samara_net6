namespace Bars.Gkh.Migrations._2017.Version_2017053000
{
    using B4.Modules.Ecm7.Framework;

    using Bars.Gkh.Utils;

    [Migration("2017053000")]
    [MigrationDependsOn(typeof(Version_2017052500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeDefaultValue("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_BEGIN_DATE", (byte)0);
            this.Database.ChangeDefaultValue("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_END_DATE", (byte)0);
            this.Database.ChangeDefaultValue("GKH_MORG_JSKTSJ_CONTRACT", "DRAWING_PD_DATE", (byte)0);
            this.Database.ChangeDefaultValue("GKH_MORG_JSKTSJ_CONTRACT", "COMP_REQIRED_PAY_AMOUNT", (decimal)0);

            this.Database.ExecuteNonQuery(@"UPDATE GKH_MORG_JSKTSJ_CONTRACT c
                    SET
                      INPUT_MDV_BEGIN_DATE    = coalesce(c.INPUT_MDV_BEGIN_DATE, 0),
                      INPUT_MDV_END_DATE      = coalesce(c.INPUT_MDV_END_DATE, 0),
                      DRAWING_PD_DATE         = coalesce(c.DRAWING_PD_DATE, 0),
                      COMP_REQIRED_PAY_AMOUNT = coalesce(c.COMP_REQIRED_PAY_AMOUNT, 0)
                    WHERE INPUT_MDV_BEGIN_DATE IS NULL
                          OR INPUT_MDV_END_DATE IS NULL
                          OR DRAWING_PD_DATE IS NULL
                          OR COMP_REQIRED_PAY_AMOUNT IS NULL;");

            this.Database.AlterColumnSetNullable("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_BEGIN_DATE", false);
            this.Database.AlterColumnSetNullable("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_END_DATE", false);
            this.Database.AlterColumnSetNullable("GKH_MORG_JSKTSJ_CONTRACT", "DRAWING_PD_DATE", false);
            this.Database.AlterColumnSetNullable("GKH_MORG_JSKTSJ_CONTRACT", "COMP_REQIRED_PAY_AMOUNT", false);
        }

        public override void Down()
        {
        }
    }
}
