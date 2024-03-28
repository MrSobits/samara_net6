namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015072400
{
    using System.Data;
    using ECM7.Migrator.Framework;

    [Migration(2015072400)]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_LOAN", "DOCUMENT_NUMBER", DbType.Int64);
            var sql = @"UPDATE REGOP_RO_LOAN set DOCUMENT_NUMBER = v.rn
                        FROM  
                        (
                            SELECT row_number() over (order by id) AS rn, id
                            FROM REGOP_RO_LOAN
                        ) AS v
                        WHERE REGOP_RO_LOAN.id = v.id;";

            Database.ExecuteNonQuery(sql);

            Database.ChangeColumn("REGOP_RO_LOAN", new Column("DOCUMENT_NUMBER", DbType.Int64, ColumnProperty.NotNull, 1));
        }

        public override void Down()
        {
            //no down
        }
    }
}
