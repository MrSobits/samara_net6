namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023123102
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023123102")]

    [MigrationDependsOn(typeof(Version_2023123101.UpdateSchema))]
    // Является Version_2020052900 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var query = @"--все счета оплат, у которых нет RestructAmicableAgreementWallet
                        drop table if exists tmp_payment_acccount;
                        create temp table tmp_payment_acccount as
                          select id from REGOP_RO_PAYMENT_ACCOUNT where raa_wallet_id is null;
                        
                        do
                        $$
                        DECLARE
                           user_row record;
                         		result bigint;
                         	begin
                          for user_row in select id from tmp_payment_acccount
                            loop
                            insert into regop_wallet (object_version, object_create_date, object_edit_date, balance, wallet_guid, owner_type, wallet_type)
                            values (0,now(),now(),0, uuid_generate_v4(), 20, 7) returning id into result;
                        
                            update REGOP_RO_PAYMENT_ACCOUNT pay set raa_wallet_id = result where pay.id = user_row.id;
                          end loop;
                        	end;
                        $$
                        LANGUAGE 'plpgsql';";

            this.Database.ExecuteNonQuery(query);
        }    
    }
}
