using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2019.Version_2019073000
{
    [Migration("2019073000")]
    [MigrationDependsOn(typeof(Version_2019061300.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("CR_OBJ_CMP_ARCHIVE_MULTI_CONTR",
                new Column("VOLUME", DbType.Decimal),
                new Column("PERCENT", DbType.Decimal),
                new Column("COST_SUM", DbType.Decimal),
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "FK_CR_OBJ_TYPE_WORK", "CR_OBJ_TYPE_WORK", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "FK_GKH_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CR_OBJ_CMP_ARCHIVE_MULTI_CONTR");
        }
    }
}