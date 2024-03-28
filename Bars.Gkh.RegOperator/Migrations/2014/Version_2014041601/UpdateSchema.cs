using System.Data;
using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014041601
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014041600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_SUSPACC_FIN_RES",
                new RefColumn("FIN_RESOURCE_ID", ColumnProperty.NotNull, "REGOP_SUSP_FIN_RES", "CR_OBJ_FIN_SOURCE_RES", "ID"),
                new RefColumn("SUSPACC_ID", ColumnProperty.NotNull, "REGOP_SUSP_FIN_RES_SUSP", "REG_OP_SUSPEN_ACCOUNT", "ID"),
                new Column("DISTRIB_TYPE", DbType.Int32, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SUSPACC_FIN_RES");
        }
    }
}
