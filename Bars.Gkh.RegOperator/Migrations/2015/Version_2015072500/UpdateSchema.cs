namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015072500
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015072400.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "REGOP_CASHPAYM_CENTER_MAN_ORG",
                new RefColumn("CASHPAYM_CENTER_ID", "MAN_ORG_CPC", "REGOP_CASHPAYMENT_CENTER", "ID"),
                new RefColumn("MAN_ORG_ID", "CPC_MAN_ORG", "GKH_MANAGING_ORGANIZATION", "ID"),
                new Column("NUMBER_CONTRACT", DbType.String, 200),
                new Column("DATE_CONTRACT", DbType.DateTime),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime));

            Database.AddEntityTable(
                "REGOP_CASHPAYM_CENTER_MAN_ORG_RO",
                new RefColumn("CASHPAYM_CENTER_MAN_ORG_ID", "RO_CPC_MAN_ORG", "REGOP_CASHPAYM_CENTER_MAN_ORG", "ID"),
                new RefColumn("REAL_OBJ_ID", "RO_CPCMANORG_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_CASHPAYM_CENTER_MAN_ORG_RO");

            Database.RemoveTable("REGOP_CASHPAYM_CENTER_MAN_ORG");
        }
    }
}
