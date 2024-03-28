namespace Bars.Gkh.Migrations.Version_2014020600
{
    using System;
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014020402.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //перенесено в модуль Decisions
            /*Database.AddEntityTable(
                "GKH_OBJ_D_PROTOCOL",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_RO_D_PROT_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "GKH_RO_D_PROT_FILE", "B4_FILE_INFO", "ID"),
                new Column("DESCR", DbType.String, 500, ColumnProperty.Null),
                new Column("DOCUMENT_NAME", DbType.String, 300, ColumnProperty.Null),
                new Column("DOCUMENT_NUM", DbType.String, 50, ColumnProperty.Null),
                new Column("PROTOCOL_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("V_TOTAL_COUNT", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m),
                new Column("V_PART_COUNT", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m),
                new Column("PART_SHARE", DbType.Decimal.WithSize(3, 2), ColumnProperty.NotNull, 0m),
                new Column("HAS_QUORUM", DbType.Int16, ColumnProperty.NotNull, 20),
                new Column("POS_V_COUNT", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m),
                new Column("DECIDED_SHARE", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m));*/


        }

        public override void Down()
        {
            //Database.RemoveEntityTable("GKH_OBJ_D_PROTOCOL");
        }
    }
}