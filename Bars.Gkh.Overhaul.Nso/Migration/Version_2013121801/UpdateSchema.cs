namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121801
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PRDEC_SVC_WORK_FACT",
                new Column("FACT_YEAR", DbType.Int32, ColumnProperty.Null),
                new RefColumn("DECISION_ID", ColumnProperty.NotNull, "O_PRDEC_SVC_WF_D", "OVRHL_PR_DEC_LIST_SERVICES", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "O_PRDEC_SVC_WF_WORK", "GKH_DICT_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PRDEC_SVC_WORK_FACT");
        }
    }
}