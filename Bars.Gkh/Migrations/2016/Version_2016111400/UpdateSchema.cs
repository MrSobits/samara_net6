namespace Bars.Gkh.Migrations._2016.Version_2016111400
{
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Миграция 2016111400
    /// </summary>
    [Migration("2016111400")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016110300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_CONSTRUCTOR_GROUP", new Column("DESCR_TYPE", DbType.Int32, ColumnProperty.NotNull));

            this.Database.AddEntityTable(
                "GKH_CONSTRUCTOR_DATA_META",
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 60, ColumnProperty.NotNull),
                new Column("WEIGHT", DbType.Decimal),
                new Column("FORMULA", DbType.String, 255),
                new Column("LEVEL", DbType.Int32, ColumnProperty.NotNull),
                new Column("TYPE", DbType.Int32, ColumnProperty.NotNull, (int)DataValueType.Number),
                new Column("MIN_LENGTH", DbType.Int32),
                new Column("MAX_LENGTH", DbType.Int32),
                new Column("DECIMALS", DbType.Int32),
                new Column("REQUIRED", DbType.Boolean, ColumnProperty.NotNull),
                new Column("FILLER_NAME", DbType.String),
                new RefColumn("GROUP_ID", "CONSTRUCTOR_GROUP_ID", "GKH_CONSTRUCTOR_GROUP", "ID"),
                new RefColumn("PARENT_ID", "CONSTRUCTOR_DATA_META_PARENT_ID", "GKH_CONSTRUCTOR_DATA_META", "ID"));

            this.Database.AddEntityTable(
                "GKH_CONSTRUCTOR_DATA_VALUE",
                new Column("VALUE", DbType.Binary),
                new RefColumn("META_INFO_ID", "DATA_META_INFO_ID", "GKH_CONSTRUCTOR_DATA_META", "ID"),
                new RefColumn("PARENT_ID", "DATA_VALUE_PARENT_ID", "GKH_CONSTRUCTOR_DATA_VALUE", "ID"));

            this.Database.AddEntityTable(
                "GKH_DICT_EF_PERIOD",
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.NotNull),
                new Column("GROUP_ID", DbType.Int64, ColumnProperty.NotNull));

            this.Database.AddIndex("DICT_EF_PERIOD_GROUP_ID", true, "GKH_DICT_EF_PERIOD", "GROUP_ID");
            this.Database.AddForeignKey("DICT_EF_PERIOD_GROUP_ID", "GKH_DICT_EF_PERIOD", "GROUP_ID", "GKH_CONSTRUCTOR_GROUP", "ID");

            this.Database.AddEntityTable(
                "GKH_EF_MANAGING_ORGANIZATION",
                new Column("DYNAMICS", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("RATING", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("MANAGING_ORG_ID", ColumnProperty.NotNull, "EF_MANAGING_ORGANIZATION_MANORG_ID", "GKH_MANAGING_ORGANIZATION", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "EF_MANAGING_ORGANIZATION_PERIOD_ID", "GKH_DICT_EF_PERIOD", "ID"));

            this.Database.AddTable(
                "GKH_EF_CONSTRUCTOR_MO_VALUE",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("DYNAMICS", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("EF_MANAGING_ORG_ID", ColumnProperty.NotNull, "CONSTRUCTOR_MANAGING_ORG_ID", "GKH_EF_MANAGING_ORGANIZATION", "ID"));

            this.Database.AddIndex("GKH_EF_CONSTRUCTOR_ID", true, "GKH_EF_CONSTRUCTOR_MO_VALUE", "ID");
            this.Database.AddForeignKey("FK_GKH_EF_CONSTRUCTOR_MO_ID", "GKH_EF_CONSTRUCTOR_MO_VALUE", "ID", "GKH_CONSTRUCTOR_DATA_VALUE", "ID");

            // Миграция по B4_MUTEX
            if (this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh.RegOperator" && x.Version == "2014092399"))
            {
                return;
            }

            this.Database.AddPersistentObjectTable("B4_MUTEXES", new Column("NAME", DbType.String, 1000, ColumnProperty.NotNull));
            this.Database.AddIndex("IND_B4_MUTEXES_UNQ_NAME", true, "B4_MUTEXES", "NAME");

            this.Database.AddPersistentObjectTable(
                "B4_MUTEX_HISTORY",
                new Column("NAME", DbType.String, 1000, ColumnProperty.PrimaryKey | ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 4000, ColumnProperty.Null),
                new Column("USER_ID", DbType.Int64, ColumnProperty.Null),
                new Column("WAS_LOCKED", DbType.Boolean, ColumnProperty.NotNull),
                new Column("TIME_REQUESTED", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TIME_TAKEN", DbType.DateTime, ColumnProperty.Null),
                new Column("TIME_RELEASED", DbType.DateTime, ColumnProperty.Null));

            foreach (var sql in this.upSqlFunctions())
            {
                this.Database.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_EF_CONSTRUCTOR_MO_VALUE");
            this.Database.RemoveTable("GKH_EF_MANAGING_ORGANIZATION");
            this.Database.RemoveTable("GKH_DICT_EF_PERIOD");

            this.Database.RemoveTable("GKH_CONSTRUCTOR_DATA_VALUE");
            this.Database.RemoveTable("GKH_CONSTRUCTOR_DATA_META");
            this.Database.RemoveTable("GKH_CONSTRUCTOR_GROUP");

            // Если есть миграция регопа, то здесь не трогаем
            if (this.Database.GetAppliedMigrations().All(x => x.ModuleId == "Bars.Gkh.RegOperator" && x.Version != "2014092399"))
            {
                this.Database.RemoveTable("B4_MUTEX_HISTORY");
                this.Database.RemoveIndex("IND_B4_MUTEXES_UNQ_NAME", "BP_MUTEXES");
                this.Database.RemoveTable("BP_MUTEXES");

                this.Database.ExecuteNonQuery("DROP FUNCTION b4_mutex_try_lock(character varying, bigint, character varying)");
                this.Database.ExecuteNonQuery("DROP FUNCTION b4_mutex_lock(character varying, bigint, character varying)");
                this.Database.ExecuteNonQuery("DROP FUNCTION b4_mutex_unlock(bigint)");
                this.Database.ExecuteNonQuery("DROP FUNCTION b4_mutex_create(character varying)");
            }
        }

        private string[] upSqlFunctions()
        {
            return new[] 
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
}