namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018020900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2018020900")]
    [MigrationDependsOn(typeof(Version_2018020600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_CHES_IMPORT_FILE",
                new RefColumn("CHES_IMPORT_ID", ColumnProperty.NotNull, "REGOP_CHES_IMPORTFILE_ID", "REGOP_CHES_IMPORT", "ID"),
                new FileColumn("FILE_ID", "REGOP_CHES_IMPORT_FILEID"),
                new Column("FILE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("CHECK_STATE", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_IMPORTED", DbType.Boolean, ColumnProperty.NotNull, false));

            var sql =
                @"insert into REGOP_CHES_IMPORT_FILE (object_version,object_create_date,object_edit_date, ches_import_id,file_id,file_type,check_state,is_imported)
            select 0, now()::timestamp, now()::timestamp, a.id, null, 
            (case filetype::varchar when '""Account""' then 0
            when '""Calc""' then 1
            when '""CalcProt""' then 2
            when '""SaldoChange""' then 3
            when '""Recalc""' then 4 else -1 end),
            0,
            (case importfile::varchar when '""Account""' then true
            when '""Calc""' then true
            when '""CalcProt""' then true
            when '""SaldoChange""' then true
            when '""Recalc""' then true else false end)
            from(
                select json_array_elements(loaded_files::json) filetype, id
                from REGOP_CHES_IMPORT) a
                left join(select json_array_elements(imported_files::json) importfile, id
                from REGOP_CHES_IMPORT) b on a.id = b.id and a.filetype::varchar = b.importfile::varchar
            where filetype::varchar <> '""Pay""'";
            this.Database.ExecuteNonQuery(sql);

            sql = @"drop table if exists tmp_per;
                    create temp table tmp_per as
                    select id from regop_period where cstart>='2017-02-01';
                    
                    drop table if exists t_table_1;
                    create temp table t_table_1  as
                    SELECT table_name FROM information_schema.tables 
                    join tmp_per t on table_name like 'ches_%_'||t.id
                    where table_schema='import' and table_type='BASE TABLE' and table_name not like 'ches_pay_%'
                    and table_name not in (select distinct table_name FROM information_schema.COLUMNS  where column_name='id');
                    
                    do
                    $$
                    declare __sql varchar;
                      rec record;
                    begin
                    	for rec in select '
                    	ALTER TABLE import.'||table_name||' ADD COLUMN id serial NOT NULL;
                    	ALTER TABLE import.'||table_name||' ADD CONSTRAINT '||table_name||'_pkey PRIMARY KEY (id);
                    	ALTER TABLE import.'||table_name||' ADD COLUMN isvalid BOOLEAN;
                    	UPDATE import.'||table_name||' SET isvalid=TRUE;
                    	  ' scr from t_table_1
                    
                      LOOP
                        execute rec.scr;
                      end LOOP;
                     end;
                    $$ language plpgsql;";

            this.Database.ExecuteNonQuery(sql);
        }
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_CHES_IMPORT_FILE");
        }
    }
}