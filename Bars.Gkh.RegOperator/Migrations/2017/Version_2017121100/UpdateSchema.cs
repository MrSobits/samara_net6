namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017121100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017121100")]
    [MigrationDependsOn(typeof(Version_2017092600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017101001.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017101600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017113000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LAWSUIT_OWNER_INFO", new Column("AREA_SHARE_NUM", DbType.Byte, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("REGOP_LAWSUIT_OWNER_INFO", new Column("AREA_SHARE_DEN", DbType.Byte, ColumnProperty.NotNull, 1));
            this.Database.ExecuteNonQuery(@"
ALTER TABLE REGOP_LAWSUIT_OWNER_INFO ALTER COLUMN AREA_SHARE DROP NOT NULL;
CREATE OR REPLACE FUNCTION _area_share_gcd( a int2,  b int2)
RETURNS int2
IMMUTABLE
STRICT
LANGUAGE SQL
AS $$
WITH RECURSIVE t(a,b) AS (
    VALUES (abs($1)::int2, abs($2)::int2)
UNION ALL
    SELECT b, MOD(a,b) FROM t
    WHERE b > 0
)
SELECT a FROM t WHERE b = 0
$$;

CREATE OR REPLACE FUNCTION convert_area_share() RETURNS integer
AS $BODY$
DECLARE
    rec RECORD;
BEGIN
    DROP TABLE IF EXISTS _area_share_table;
    CREATE TEMP TABLE _area_share_table (id bigint, num int2, den int2);

    FOR rec IN (
    SELECT
        id,
        round((AREA_SHARE * 1000))::int2 as AREA_SHARE_NUM,
        (1000)::int2 as AREA_SHARE_DEN
    FROM REGOP_LAWSUIT_OWNER_INFO
        WHERE AREA_SHARE IS NOT NULL
    ) LOOP EXECUTE format(
$$

INSERT INTO _area_share_table
    SELECT %3$s as ID,
    (%1$s / _area_share_gcd(%1$s::int2, %2$s::int2)) as AREA_SHARE_NUM,
    (%2$s / _area_share_gcd(%1$s::int2, %2$s::int2)) as AREA_SHARE_DEN;

$$, rec.AREA_SHARE_NUM, rec.AREA_SHARE_DEN, rec.ID);
END LOOP;

UPDATE REGOP_LAWSUIT_OWNER_INFO as rlof
SET AREA_SHARE_NUM = ast.num,
    AREA_SHARE_DEN = ast.den
FROM _area_share_table as ast
WHERE rlof.id = ast.id;

RETURN 0;
END;
$BODY$ LANGUAGE plpgsql;

SELECT convert_area_share();

DROP FUNCTION IF EXISTS convert_area_share();
DROP FUNCTION IF EXISTS _area_share_gcd(int2, int2);
");
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_OWNER_INFO", "AREA_SHARE_NUM");
            this.Database.RemoveColumn("REGOP_LAWSUIT_OWNER_INFO", "AREA_SHARE_DEN");
        }
    }
}