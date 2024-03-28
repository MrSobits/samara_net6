namespace Bars.Gkh1468.Migrations.Version_2013111300
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013111300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013102900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"CREATE OR REPLACE FUNCTION updateregionmunicipality() RETURNS SETOF boolean AS 
                    $BODY$
                    DECLARE
                      recordCount int;
                      regionName varchar(120);
                    BEGIN
                      recordCount := count(*) from b4_fias where parentguid = '';

                      IF recordCount = 1 THEN
	                    regionName := offname from b4_fias where parentguid = '';

	                    UPDATE GKH_DICT_MUNICIPALITY
	                    SET region_name = regionName;
                      END IF;

                      RETURN;
                    END;
                    $BODY$
                    LANGUAGE plpgsql;
                    select updateregionmunicipality();
                    DROP FUNCTION updateregionmunicipality();");
            }
        }

        public override void Down()
        {
        }
    }
}