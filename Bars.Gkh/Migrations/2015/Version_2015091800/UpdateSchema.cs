namespace Bars.Gkh.Migrations._2015.Version_2015091800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015091800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015090701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"CREATE OR REPLACE PROCEDURE TABLE_LOCKER(
    TNAME IN VARCHAR2,
    ACT IN VARCHAR2)
AS
  cnt              NUMBER(38, 0);
  TABLE_LOCKED_EXCEPTION EXCEPTION;
BEGIN
  SELECT COUNT(TABLE_NAME)
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
