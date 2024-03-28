namespace Bars.Gkh.Migrations._2014.Version_2014042800
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014042500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_DELIVERY_AGENT",
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "DELIVERY_AG_CTRG", "GKH_CONTRAGENT", "ID"));

            Database.AddEntityTable("REGOP_DEL_AGENT_MU",
                new RefColumn("DEL_AGENT_ID", ColumnProperty.NotNull, "DELIVERY_AG_MU_DEL", "REGOP_DELIVERY_AGENT", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "DELIVERY_AG_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddEntityTable("REGOP_DEL_AGENT_REAL_OBJ",
                new RefColumn("DEL_AGENT_ID", ColumnProperty.NotNull, "DELIVERY_AG_RO_DEL", "REGOP_DELIVERY_AGENT", "ID"),
                new RefColumn("REAL_OBJ_ID", ColumnProperty.NotNull, "DELIVERY_AG_RO_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_DELIVERY_AGENT");
            Database.RemoveTable("REGOP_DEL_AGENT_MU");
            Database.RemoveTable("REGOP_DEL_AGENT_REAL_OBJ");
        }
    }
}
