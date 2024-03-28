namespace Bars.GkhGji.Regions.Stavropol.Migrations.Version_2023070700
{
	using Bars.B4.Modules.Ecm7.Framework;

	[Migration("2023070700")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
				CREATE OR REPLACE FUNCTION public.z_get_tariff_cr (V_OBJECT_ID integer, V_DATE date)
					RETURNS numeric
					LANGUAGE 'plpgsql'
					COST 100
					VOLATILE PARALLEL UNSAFE
				AS $code$
				DECLARE
					V_RESULT         numeric;
				    i_state_id       bigint;
				    i_max_date_start date;
				    i_type_id        text = 'gkh_real_obj_dec_protocol';
				BEGIN
				    SELECT bs.id 
				    INTO i_state_id
				    FROM public.b4_state bs
				    WHERE
				        bs.type_id = i_type_id
				        AND bs.final_state
				    LIMIT 1;
					
					SELECT MAX(godp.date_start) 
					INTO i_max_date_start
					FROM public.gkh_obj_d_protocol godp
					WHERE
						godp.ro_id = V_OBJECT_ID
						AND godp.state_id = i_state_id;
					
					SELECT  
						CASE
							WHEN (COALESCE(dmf.decision_value, '[]')::json -> 0 ->> 'Value')::numeric > COALESCE(oprr.dvalue, 0)
							THEN (dmf.decision_value::json -> 0 ->> 'Value')::numeric
							ELSE COALESCE(oprr.dvalue, opr.dvalue, 0)
						END AS tariff
					FROM public.gkh_reality_object gro
						LEFT JOIN (public.ovrhl_paysize_rec opr
								INNER JOIN public.ovrhl_paysize op
									ON op.id = opr.paysize_id
							) ON opr.mu_id = gro.municipality_id
							AND V_DATE BETWEEN op.date_start AND COALESCE(op.date_end, 'Infinity'::timestamp)
						LEFT JOIN (public.ovrhl_realestaterealityo etro
								INNER JOIN public.ovrhl_real_estate_type oret 
									ON oret.id = etro.ret_id 
									AND oret.code = '1'
								INNER JOIN public.ovrhl_paysize_rec_ret oprr 
									ON oprr.ret_id = oret.id
							) ON etro.ro_id = gro.id 
							AND oprr.record_id = opr.id
						LEFT JOIN (public.gkh_obj_d_protocol godp 
								INNER JOIN public.dec_ultimate_decision ud
									ON godp.id = ud.protocol_id 
									AND ud.is_checked
								INNER JOIN public.dec_monthly_fee dmf 
									ON dmf.id = ud.id 
							) ON godp.ro_id = gro.id 
							AND godp.date_start = i_max_date_start
					WHERE gro.id = V_OBJECT_ID
					INTO V_RESULT;
					
					RETURN V_RESULT;
				END;
				$code$;
			");
        }
    }
}