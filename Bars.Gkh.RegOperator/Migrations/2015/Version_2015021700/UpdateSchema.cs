namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015021700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015021700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015012200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "REGOP_PERS_ACC_PRIV_CAT",
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "REGOP_PERSAC_PRCA_PA", "REGOP_PERS_ACC", "ID"),
                new RefColumn("PRIV_CAT_ID", ColumnProperty.NotNull, "REGOP_PERSAC_PRCA_PC", "REGOP_PRIVILEGED_CATEGORY", "ID"),
                new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_TO", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERS_ACC_PRIV_CAT");
        }
    }
}
