namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015072400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015071000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
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
