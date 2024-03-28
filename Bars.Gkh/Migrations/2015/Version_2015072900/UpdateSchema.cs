namespace Bars.Gkh.Migrations.Version_2015072900
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015072700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"CREATE OR REPLACE FUNCTION table_locker() RETURNS TRIGGER AS $table_locker$
  BEGIN
    IF EXISTS(SELECT * FROM TABLE_LOCK WHERE LOWER(TABLE_NAME) = LOWER(TG_TABLE_NAME) AND LOWER(ACTION) = LOWER(TG_OP)) THEN
      RAISE EXCEPTION 'TABLE_LOCKED_EXCEPTION';
    END IF;
    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
      RETURN NEW;
    END IF;
    IF TG_OP = 'DELETE' THEN
      RETURN OLD;
    END IF;
  END;
$table_locker$ LANGUAGE plpgsql");
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"CREATE OR REPLACE PROCEDURE TABLE_LOCKER(
    TNAME IN VARCHAR2
    ACT IN VARCHAR2)
AS
  cnt              NUMBER(38, 0);
  TABLE_LOCKED_EXCEPTION EXCEPTION;
BEGIN
  SELECT COUNT(ID)
  INTO cnt
  FROM TABLE_LOCK
  WHERE LOWER(TABLE_NAME) = LOWER(TNAME) AND LOWER(ACTION) = LOWER(ACT);
  IF cnt                  > 0 THEN
    RAISE TABLE_LOCKED_EXCEPTION;
  END IF;
END TABLE_LOCKER;
");
            }
        }
    }
}