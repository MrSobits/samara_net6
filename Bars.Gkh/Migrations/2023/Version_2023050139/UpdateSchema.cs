namespace Bars.Gkh.Migrations._2023.Version_2023050139
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050139")]

    [MigrationDependsOn(typeof(Version_2023050138.UpdateSchema))]

    /// Является Version_2021052500 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"CREATE OR REPLACE FUNCTION public.ls_control_import_uo_prm()
											    RETURNS TABLE(id bigint, name character varying(255)) 
											    LANGUAGE 'plpgsql'
											    COST 100
											    VOLATILE PARALLEL UNSAFE
											    ROWS 2000

												AS $BODY$
												BEGIN
												/*
												dict_code
												1 - УО
												19 - ТСЖ
												20 - ЖК
												22 - ЖСК
												*/
												RETURN QUERY SELECT * FROM dblink('dbname=ris', 
																				  'SELECT DISTINCT c.contragent_id, c.full_name 
																				  FROM data.contragent c
																				  JOIN data.contragent_type ct ON c.contragent_id = ct.contragent_id AND ct.is_del = FALSE
                            	  												  JOIN nsi.nsi_contragent_type dct ON ct.contragent_type_id = dct.contragent_type_id
																				  WHERE dct.dict_code IN (''1'',''19'',''20'',''22'') AND c.is_del = FALSE') AS t1(contragent_id bigint, full_name character varying(255));
												END;
												$BODY$;");

            this.Database.ExecuteNonQuery(@"CREATE OR REPLACE FUNCTION public.report_control_import_contragent_prm()
											    RETURNS TABLE(id bigint, name character varying) 
											    LANGUAGE 'plpgsql'
											    COST 100
											    VOLATILE PARALLEL UNSAFE
											    ROWS 10000

												AS $BODY$
												BEGIN
												RETURN QUERY SELECT * FROM dblink('dbname=ris', 'SELECT contragent_id, full_name FROM data.contragent WHERE is_del = FALSE') AS t1(contragent_id bigint, full_name character varying(255));
												END;
												$BODY$;");
        }
    }
}