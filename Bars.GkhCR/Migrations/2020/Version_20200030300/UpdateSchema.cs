using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2020030300
{
    [Migration("2020030300")]
    [MigrationDependsOn(typeof(_2019.Version_2019091600.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("CR_OBJ_CMP_BUILD_CONTR",
                new Column("VOLUME", DbType.Decimal),
                new Column("PERCENT", DbType.Decimal),
                new Column("COST_SUM", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String, 1500),
                new Column("MONITORING_DATE", DbType.DateTime),
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "FK_CR_OBJ_TYPE_WORK_SK", "CR_OBJ_TYPE_WORK", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "FK_GKH_CONTRAGENT_SK", "GKH_CONTRAGENT", "ID"),
                new RefColumn("CONTRAGENT_SK_ID", ColumnProperty.NotNull, "FK_GKH_SK_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CR_OBJ_CMP_BUILD_CONTR");
        }
    }
}