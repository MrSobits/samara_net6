using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.GkhCr.Migrations.Version_2014031900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014031200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CR_BLD_CONTR_TYPE_WRK",
                new RefColumn("BUILD_CONTRACT_ID", ColumnProperty.NotNull, "CR_BLD_CTR_TW_CTR", "CR_OBJ_BUILD_CONTRACT", "ID"),
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "CR_BLD_CTR_TW_WRK", "CR_OBJ_TYPE_WORK", "ID"),
                new Column("SUM", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveTable("CR_BLD_CONTR_TYPE_WRK");
        }
    }
}