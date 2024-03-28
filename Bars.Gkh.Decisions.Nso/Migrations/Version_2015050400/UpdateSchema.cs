namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2015050400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014102700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddJoinedSubclassTable("DEC_PENALTY_DELAY",
                "DEC_ULTIMATE_DECISION",
                "DEC_PENALTY_DELAY_DEC",
                new Column("DECISION", DbType.String, ColumnProperty.NotNull, 2000));
        }

        public override void Down()
        {
            Database.RemoveTable("DEC_PENALTY_DELAY");
        }
    }
}