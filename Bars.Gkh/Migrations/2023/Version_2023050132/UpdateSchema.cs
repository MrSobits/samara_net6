namespace Bars.Gkh.Migrations._2023.Version_2023050132
{
    using System.Collections.Generic;
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050132")]

    [MigrationDependsOn(typeof(Version_2023050131.UpdateSchema))]

    /// Является Version_2020052000 из ядра
    public class UpdateSchema : Migration
    {
        private readonly List<KeyValuePair<string, string>> tables = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("CLW_EXECUTORY_PROCESS", "FILE_ID"),
            new KeyValuePair<string, string>("CR_SPECIAL_OBJ_TYPE_WORK_REMOVAL", "FILE_DOC_ID"),
            new KeyValuePair<string, string>("GKH_MORG_CONTRACT_OWNERS", "OWNERS_SIGNED_CONTRACT_FILE"),
            new KeyValuePair<string, string>("CLW_COURT_ORDER_CLAIM", "DOCUMENT_ID")
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