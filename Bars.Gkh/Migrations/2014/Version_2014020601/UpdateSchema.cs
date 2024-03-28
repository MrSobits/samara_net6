namespace Bars.Gkh.Migrations.Version_2014020601
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014020600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //перенесено в модуль Decisions

            /*Database.AddEntityTable("GKH_GENERIC_DECISION",
                new Column("DECISION_CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("IS_ACTUAL", DbType.Boolean, ColumnProperty.NotNull),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GEN_DECISION_PROT", "GKH_OBJ_PROTOCOL_MT", "ID"));*/
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("GKH_GENERIC_DECISION");
        }
    }
}