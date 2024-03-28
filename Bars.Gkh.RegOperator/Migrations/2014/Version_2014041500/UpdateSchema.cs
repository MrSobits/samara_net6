using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014041500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014040800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PAYMENT_DOC_INFO",
                new RefColumn("MUNICIPALITY_ID", "PAY_DOC_INF_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("SETTLEMENT_ID", "PAY_DOC_INF_SETTL_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("LOCALITY_ID", "PAY_DOC_INF_LOC_FIAS", "B4_FIAS", "ID"),
                new Column("DATE_START", DbType.Date, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.Date),
                new Column("INFORMATION", DbType.String, 2000),
                new RefColumn("RO_ID", "PAY_DOC_INF_REAL_OBJ", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PAYMENT_DOC_INFO");
        }
    }
}
