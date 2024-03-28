namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061700
{
    using B4.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery("ALTER TABLE REGOP_LOC_CODE ALTER COLUMN FIAS_ID_L1 DROP NOT NULL");
            Database.ExecuteNonQuery("ALTER TABLE REGOP_LOC_CODE ALTER COLUMN FIAS_ID_L2 DROP NOT NULL");
            Database.ExecuteNonQuery("ALTER TABLE REGOP_LOC_CODE ALTER COLUMN FIAS_ID_L3 DROP NOT NULL");
        }

        public override void Down()
        {
            var recordCount_l1 = Database.ExecuteScalar("SELECT COUNT(ID) FROM REGOP_LOC_CODE WHERE FIAS_ID_L1 IS NULL").ToInt();
            var recordCount_l2 = Database.ExecuteScalar("SELECT COUNT(ID) FROM REGOP_LOC_CODE WHERE FIAS_ID_L2 IS NULL").ToInt();
            var recordCount_l3 = Database.ExecuteScalar("SELECT COUNT(ID) FROM REGOP_LOC_CODE WHERE FIAS_ID_L3 IS NULL").ToInt();

            if (recordCount_l1 == 0)
            {
                Database.ExecuteNonQuery("ALTER TABLE REGOP_LOC_CODE ALTER COLUMN FIAS_ID_L1 SET NOT NULL");
            }

            if (recordCount_l2 == 0)
            {
                Database.ExecuteNonQuery("ALTER TABLE REGOP_LOC_CODE ALTER COLUMN FIAS_ID_L2 SET NOT NULL");
            }

            if (recordCount_l3 == 0)
            {
                Database.ExecuteNonQuery("ALTER TABLE REGOP_LOC_CODE ALTER COLUMN FIAS_ID_L3 SET NOT NULL");
            }
        }
    }
}
