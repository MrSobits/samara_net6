namespace Bars.Gkh.Migrations._2023.Version_2023050117
{
    using System.Collections.Generic;
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050117")]

    [MigrationDependsOn(typeof(Version_2023050116.UpdateSchema))]

    /// Является Version_2019020500 из ядра
    public class UpdateSchema : Migration
    {
        private readonly List<KeyValuePair<string, string>> tables = new List<KeyValuePair<string, string>>
        {
            { new KeyValuePair<string, string>( "GIS_LOADED_FILE_REGISTER", "B4_FILE_INFO_ID") },
            { new KeyValuePair<string, string>("GIS_LOADED_FILE_REGISTER", "B4_LOG_INFO_ID") },
            { new KeyValuePair<string, string>("DI_DISINFO_COM_FACILS", "CONTRACT_FILE_ID") },
            { new KeyValuePair<string, string>("DI_DISINFO_COM_FACILS", "PROTOCOL_FILE_ID") },
            { new KeyValuePair<string, string>("GJI_INSPECTION_INSCHECK", "DOC_FILE_ID") },
            { new KeyValuePair<string, string>("CR_OBJ_TYPE_WORK_REMOVAL", "FILE_DOC_ID") },
            { new KeyValuePair<string, string>("CR_COMPETITION_PROTOCOL", "FILE_ID") },
            { new KeyValuePair<string, string>("GKH_OBJ_DIRECT_MANAG_CNRT", "SERV_FILE_ID") },
            { new KeyValuePair<string, string>("REGOP_PERS_ACC", "CONTRACT_FILE_ID") },
            { new KeyValuePair<string, string>("REGOP_PERIOD_CLS_CHCK_RES", "FULL_LOG_FILE_ID") },
            { new KeyValuePair<string, string>("RF_CONTRACT", "TERMINATION_CONTRACT_FILE_ID") }
        };

        public override void Up()
        {
            this.tables.ForEach(x =>
            {
                if (this.Database.ColumnExists(x.Key, x.Value))
                    this.Database.ExecuteNonQuery(@"
                        DO $$
                        BEGIN
                        IF NOT EXISTS(SELECT t.relname , attname
	                                    from pg_class t 
	                                    join pg_constraint c on c.conrelid = t.oid and c.confrelid = ('b4_file_info')::regclass
	                                    join pg_attribute a on a.attrelid = t.oid and a.attnum = any(c.conkey) 
	                                    where attname='" + x.Value.ToLower() + "' and t.relname='" + x.Key.ToLower() + @"' )  
                        THEN
                        UPDATE " + x.Key + " SET " + x.Value + " = NULL WHERE NOT EXISTS (SELECT NULL FROM B4_FILE_INFO f WHERE f.ID = " + x.Value +
                        ") AND " + x.Value + " IS NOT NULL;" +
                        "ALTER TABLE " + x.Key + " ADD CONSTRAINT fk_" + x.Key.ToLower() + "_" + x.Value.ToLower().Replace("_ID", "") + " FOREIGN KEY (" +
                        x.Value + @") REFERENCES public.B4_FILE_INFO (ID)
                        ON DELETE NO ACTION
                        ON UPDATE NO ACTION
                        NOT DEFERRABLE; 
                        END IF;
                        END$$;");
            });
        }

        public override void Down()
        {
            this.tables.ForEach(x =>
            {
                this.Database.ExecuteNonQuery("ALTER TABLE " + x.Key + " DROP CONSTRAINT IF EXISTS  fk_" + x.Key.ToLower() + "_" +
                    x.Value.ToLower().Replace("_ID", "") + ";");
            });
        }
    }
}