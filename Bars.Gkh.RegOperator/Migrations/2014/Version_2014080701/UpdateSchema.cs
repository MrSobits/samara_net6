namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014080701
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014080700.UpdateSchema))]
    public sealed class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "DEC_CORE_DECISION",
                new[]
                {
                    new Column("DECISION_TYPE", DbType.Int32, ColumnProperty.NotNull),
                    new RefColumn("GOV_DECISION_ID", "GOV_DECISION", "DEC_GOV_DECISION", "ID"),
                    new RefColumn("ULTIMATE_DECISION_ID", "ULTIMATE_DECISION", "DEC_ULTIMATE_DECISION", "ID")
                });

            Database.ExecuteNonQuery(
                "insert into DEC_CORE_DECISION(object_version, object_create_date, object_edit_date, DECISION_TYPE, GOV_DECISION_ID, ULTIMATE_DECISION_ID) select 0, now(), now(), 10, null, Id from DEC_ULTIMATE_DECISION");
            Database.ExecuteNonQuery(
                "insert into DEC_CORE_DECISION(object_version, object_create_date, object_edit_date, DECISION_TYPE, ULTIMATE_DECISION_ID, GOV_DECISION_ID) select 0, now(), now(), 20, null, Id from DEC_GOV_DECISION");
        }

        public override void Down()
        {
            Database.RemoveTable("DEC_CORE_DECISION");
        }
    }
}
