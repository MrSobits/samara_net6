namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014060901
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014060900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_DEFFER_UNACTIV",
                new Column("UNACT_DATE", DbType.DateTime),
                new Column("PROCESSED", DbType.Boolean),

                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_DEF_UNAC_ACC", "REGOP_PERS_ACC", "ID"),
                new RefColumn("GOV_DECISION_ID", ColumnProperty.NotNull, "REGOP_DEF_UNAC_GOVDEC", "DEC_GOV_DECISION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_DEFFER_UNACTIV");
        }
    }
}
