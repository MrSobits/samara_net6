namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092395
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092395")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092394.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накат
        /// </summary>
        public override void Up()
        {
            if (this.Database.TableExists("B4_MUTEXES"))
            {
                return;
            }

            Database.AddPersistentObjectTable("B4_MUTEXES", new Column("NAME", DbType.String, 1000, ColumnProperty.NotNull));
            Database.AddIndex("IND_B4_MUTEXES_UNQ_NAME", true, "B4_MUTEXES", "NAME");

            Database.AddPersistentObjectTable(
                "B4_MUTEX_HISTORY",
                new Column("NAME", DbType.String, 1000, ColumnProperty.PrimaryKey | ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 4000, ColumnProperty.Null),
                new Column("USER_ID", DbType.Int64, ColumnProperty.Null),
                new Column("WAS_LOCKED", DbType.Boolean, ColumnProperty.NotNull),
                new Column("TIME_REQUESTED", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TIME_TAKEN", DbType.DateTime, ColumnProperty.Null),
                new Column("TIME_RELEASED", DbType.DateTime, ColumnProperty.Null));

            foreach (var sql in upSqlFunctions)
            {
                Database.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// Откат
        /// </summary>
        public override void Down()
        {
            if (!this.Database.TableExists("B4_MUTEXES"))
            {
                return;
            }

            Database.RemoveTable("B4_MUTEX_HISTORY");
            Database.RemoveIndex("IND_B4_MUTEXES_UNQ_NAME", "BP_MUTEXES");
            Database.RemoveTable("BP_MUTEXES");

            Database.ExecuteNonQuery("DROP FUNCTION b4_mutex_try_lock(character varying, bigint, character varying)");
            Database.ExecuteNonQuery("DROP FUNCTION b4_mutex_lock(character varying, bigint, character varying)");
            Database.ExecuteNonQuery("DROP FUNCTION b4_mutex_unlock(bigint)");
            Database.ExecuteNonQuery("DROP FUNCTION b4_mutex_create(character varying)");
        }

        private readonly string[] upSqlFunctions =
        {
@"CREATE OR REPLACE FUNCTION b4_mutex_create(mutex_name character varying)
  RETURNS boolean AS
$BODY$
begin
    begin
      insert into b4_mutexes (name) values (MUTEX_NAME);
exception
      when unique_violation then
    return false;
end;
return true;
end;
$BODY$
  LANGUAGE plpgsql;",

@"CREATE OR REPLACE FUNCTION b4_mutex_unlock(history_id bigint)
  RETURNS smallint AS
$BODY$
declare

is_lock boolean;

begin

  select pg_advisory_unlock(m.id)
  into is_lock
  from b4_mutexes m, b4_mutex_history h
  where h.id = history_id
  and h.name = m.name;
  
  update b4_mutex_history
  set time_released = now()
  where id = history_id;
  
  return 1;
end;
$BODY$
  LANGUAGE plpgsql;",

@"CREATE OR REPLACE FUNCTION b4_mutex_lock(mutex_name character varying, user_id bigint, description character varying)
  RETURNS bigint AS
$BODY$
declare
  row_name character varying(1000);
  time_requested timestamp without time zone := now();
  history_id bigint;
begin
  begin
    select name into row_name from b4_mutexes where name = mutex_name for update;
exception
    when no_data_found then raise exception 'Invariant violation: record in B4_MUTEX table was deleted';
end;
insert into b4_mutex_history(was_locked, name, description, user_id, time_requested, time_taken) values ('t', mutex_name, description, user_id, time_requested, now()) returning id into history_id;
return history_id;
end;
$BODY$
  LANGUAGE plpgsql;",

@"CREATE OR REPLACE FUNCTION b4_mutex_try_lock(mutex_name character varying, user_id bigint, description character varying)
  RETURNS bigint AS
$BODY$
declare
  history_id bigint;
is_lock boolean;
time_requested timestamp without time zone := now();
begin
  begin
    select pg_try_advisory_lock(id) into is_lock from b4_mutexes where name = mutex_name;
exception
    when no_data_found then raise exception 'Invariant violation: record in B4_MUTEX table was deleted';
end;
if (is_lock = false) then
      insert into b4_mutex_history(was_locked, name, description, user_id, time_requested) values ('f', mutex_name, description, user_id, time_requested);
return 0;
end if;
insert into b4_mutex_history(was_locked, name, description, user_id, time_requested, time_taken)
  values ('t', mutex_name, description, user_id, time_requested, now())
  returning id into history_id;
return history_id;
end;
$BODY$
  LANGUAGE plpgsql;"
        };
    }
}
